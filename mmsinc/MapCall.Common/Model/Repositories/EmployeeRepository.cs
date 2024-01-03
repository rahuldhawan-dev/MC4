using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        public override IQueryable<Employee> Linq
        {
            get
            {
                return
                    base.Linq.OrderBy(e => e.OperatingCenter.OperatingCenterCode)
                        .ThenBy(e => e.LastName)
                        .ThenBy(e => e.FirstName);
            }
        }

        #endregion

        #region Constructors

        public EmployeeRepository(ISession session, IContainer container, IDateTimeProvider dateTimeProvider) : base(
            session, container)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Private Methods

        private DateTime GetNow()
        {
            return _dateTimeProvider.GetCurrentDate().Date;
        }

        #endregion

        #region Exposed Methods

        #region Cascades

        public IQueryable<Employee> GetActiveEmployeesByOperatingCenterId(params int[] operatingCenterId)
        {
            return GetActiveEmployeesWhere(x => operatingCenterId.Contains(x.OperatingCenter.Id));
        }

        public IQueryable<Employee> GetEmployeesByOperatingCenters(params int[] operatingCenterIds)
        {
            return
                Linq.Where(x => operatingCenterIds.Contains(x.OperatingCenter.Id));
        }

        public IQueryable<Employee> GetActiveEmployeesWhere(Expression<Func<Employee, bool>> fn)
        {
            return
                Linq.Where(x => x.Status != null && x.Status.Id == EmployeeStatus.Indices.ACTIVE).Where(fn);
        }

        public IQueryable<Employee> GetByOperatingCenterId(int[] operatingCenterIds)
        {
            return Linq.Where(x => operatingCenterIds.Contains(x.OperatingCenter.Id));
        }

        public IQueryable<Employee> GetForSelect()
        {
            return (from e in Linq
                    select new Employee {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        MiddleName = e.MiddleName,
                        OperatingCenter = e.OperatingCenter,
                        EmployeeId = e.EmployeeId,
                        Status = e.Status
                    });
        }

        public IQueryable<Employee> GetActiveForSelect()
        {
            return GetForSelect().Where(x => x.IsActive);
        }

        public IQueryable<Employee> GetByOperatingCenterIdAndStatusId(int operatingCenterId, int statusId)
        {
            return Where(x => operatingCenterId == x.OperatingCenter.Id && statusId == x.Status.Id);
        }

        public IQueryable<Employee> GetByStateIdOrOperatingCenterIdAndStatusId(int? stateId, int? operatingCenterId, int? statusId)
        {
            if (stateId < 1)
            {
                return Enumerable.Empty<Employee>().AsQueryable();
            }

            var results = Where(x => stateId == x.OperatingCenter.State.Id);
            
            if (operatingCenterId != null)
            {
                results = results.Where(x => operatingCenterId == x.OperatingCenter.Id);
            }

            if (statusId != null)
            {
                results = results.Where(x => statusId == x.Status.Id);
            }
            
            return results;
        }

        #endregion

        #region Reports

        public IEnumerable<Employee> GetEmployeesWithCommercialDriversLicenseRenewalsDueInTwoMonths(
            int? operatingCenterId = null)
        {
            var now = GetNow();
            var twoMonthsFromNow = now.AddDays(60);

            return from e in Linq
                   where
                       ((operatingCenterId == null || e.OperatingCenter.Id == operatingCenterId))
                       && e.DriversLicenses.Any(x =>
                           DriversLicenseClass.CommercialDriversLicenseIndices.Contains(x.DriversLicenseClass.Id))
                       && e.DriversLicenses.Max(x => x.RenewalDate) >= twoMonthsFromNow.BeginningOfDay()
                       && e.DriversLicenses.Max(x => x.RenewalDate) < twoMonthsFromNow.BeginningOfDay().AddDays(1)
                       && e.CommercialDriversLicenseProgramStatus.Id ==
                       CommercialDriversLicenseProgramStatus.Indices.IN_PROGRAM
                       && e.Status != null && e.Status.Description == "Active"
                   select e;
        }

        /// <summary>
        /// Not even once.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<EmployeeTrainingReportItem> GetRecentTraining(ISearchEmployeeTraining search)
        {
            var now = _dateTimeProvider.GetCurrentDate();

            EmployeeTrainingReportItem result = null;
            Employee employee = null;
            OperatingCenter opCntr = null;
            PositionGroup positionGroup = null;
            PositionGroupCommonName positionGroupCommonName = null;
            TrainingRequirement trainingRequirement = null;
            TrainingModule trainingModule = null;
            TrainingRecordAttendedEmployee attendedTrainingRecords = null;
            TrainingRecordScheduledEmployee scheduledTrainingRecords = null;
            TrainingRecord attendedTrainingRecord = null, scheduledTrainingRecord = null;
            TrainingModule attendedTrainingModule = null,
                           scheduledTrainingModule = null,
                           activeInitial = null,
                           activeInitialAndRecurring = null,
                           activeRecurring = null;

            // We're querying employees, lets set that up and all the joins. buckle up.
            var query = Session.QueryOver(() => employee);

            #region Joins

            // We want the employee's (default) operating center
            query.JoinAlias(x => x.OperatingCenter, () => opCntr);
            // We want the position group they belong to.
            query.JoinAlias(x => x.PositionGroup, () => positionGroup, JoinType.LeftOuterJoin);
            // And the position group common name
            query.JoinAlias(_ => positionGroup.CommonName, () => positionGroupCommonName, JoinType.LeftOuterJoin);
            // then lets get the training requirements that are linked to this position group common name
            // still with us? this is a one to many relationship
            query.JoinAlias(_ => positionGroupCommonName.TrainingRequirements, () => trainingRequirement,
                JoinType.LeftOuterJoin);
            query.JoinAlias(_ => trainingRequirement.ActiveInitialTrainingModule, () => activeInitial,
                JoinType.LeftOuterJoin);
            query.JoinAlias(_ => trainingRequirement.ActiveInitialAndRecurringTrainingModule,
                () => activeInitialAndRecurring, JoinType.LeftOuterJoin);
            query.JoinAlias(_ => trainingRequirement.ActiveRecurringTrainingModule, () => activeRecurring,
                JoinType.LeftOuterJoin);
            // next up the training requirement's training modules - another one to many
            // this is the one that hurts, somehow this is what we want to display though
            query.JoinAlias(_ => trainingRequirement.TrainingModules, () => trainingModule, JoinType.LeftOuterJoin);

            #endregion

            #region SubQueries

            #region The most recent training date for the employee

            var recentTrainingDateSubQuery = QueryOver.Of(() => attendedTrainingRecord)
                                                      .JoinAlias(_ => attendedTrainingRecord.EmployeesAttended,
                                                           () => attendedTrainingRecords)
                                                      .JoinAlias(_ => attendedTrainingRecord.TrainingModule,
                                                           () => attendedTrainingModule)
                                                      .Where(trr =>
                                                           attendedTrainingModule.TrainingRequirement.Id ==
                                                           trainingRequirement.Id
                                                           &&
                                                           employee.Id == attendedTrainingRecords.Employee.Id)
                                                      .OrderBy(_ => attendedTrainingRecord.HeldOn).Desc
                                                      .Select(_ => attendedTrainingRecord.HeldOn);
            // if the search is querying this date, add the where
            // and make sure we aren't pulling back records with nulls
            if (search.RecentTrainingDate != null && search.RecentTrainingDate.End.HasValue)
            {
                recentTrainingDateSubQuery.Where(
                    search.RecentTrainingDate.GetRestriction("attendedTrainingRecord.HeldOn"));
                // we don't want nulls, and if we just checked the subquery for nulls, it doesn't stop the parent query
                // from retrieving them, so this tells the parent query to not bring back nulls for this subquery
                query.WithSubquery.WhereExists(recentTrainingDateSubQuery);
            }

            #endregion

            #region The employees next scheduled training date

            var nextScheduledDateSubQuery = QueryOver.Of(() => scheduledTrainingRecord)
                                                     .JoinAlias(_ => scheduledTrainingRecord.EmployeesScheduled,
                                                          () => scheduledTrainingRecords)
                                                     .JoinAlias(_ => scheduledTrainingRecord.TrainingModule,
                                                          () => scheduledTrainingModule)
                                                     .Where(trr =>
                                                          scheduledTrainingModule.TrainingRequirement.Id ==
                                                          trainingRequirement.Id
                                                          && employee.Id == scheduledTrainingRecords.Employee.Id
                                                          && scheduledTrainingRecord.ScheduledDate > now)
                                                     .OrderBy(_ => scheduledTrainingRecord.ScheduledDate).Desc
                                                     .Select(_ => scheduledTrainingRecord.ScheduledDate);
            // if the search is querying this date, add the where
            // and make sure we aren't pulling back records with nulls
            if (search.NextScheduledDate != null && search.NextScheduledDate.End.HasValue)
            {
                nextScheduledDateSubQuery.Where(
                    search.NextScheduledDate.GetRestriction("scheduledTrainingRecord.ScheduledDate"));
                // we don't want nulls, and if we just checked the subquery for nulls, it doesn't stop the parent query
                // from retrieving them, so this tells the parent query to not bring back nulls for this subquery
                query.WithSubquery.WhereExists(nextScheduledDateSubQuery);
            }

            #endregion

            #region The employee's next due by date

            var nextDueByDateSubQuery = QueryOver.Of(() => attendedTrainingRecord)
                                                 .JoinAlias(_ => attendedTrainingRecord.EmployeesAttended,
                                                      () => attendedTrainingRecords)
                                                 .JoinAlias(_ => attendedTrainingRecord.TrainingModule,
                                                      () => attendedTrainingModule)
                                                 .Where(trr =>
                                                      attendedTrainingModule.TrainingRequirement.Id ==
                                                      trainingRequirement.Id
                                                      &&
                                                      employee.Id == attendedTrainingRecords.Employee.Id)
                                                 .Select(
                                                      Projections.SqlFunction(
                                                          new SQLFunctionTemplate(NHibernateUtil.DateTime,
                                                              FluentNHibernateExtensions.IsSqlite()
                                                                  ? "dateaddplus(?1, ?2, ?3)"
                                                                  : "dbo.dateaddplus(?1, ?2, ?3)"),
                                                          NHibernateUtil.DateTime,
                                                          Projections.Property(() =>
                                                              trainingRequirement.TrainingFrequencyUnit),
                                                          Projections.Property(() =>
                                                              trainingRequirement.TrainingFrequency.Value),
                                                          Projections.SqlFunction("max", NHibernateUtil.DateTime,
                                                              Projections.Property<TrainingRecord>(x => x.HeldOn))
                                                      )
                                                  );

            if (search.NextDueByDate != null && search.NextDueByDate.End.HasValue)
            {
                query.Where(search.NextDueByDate.GetRestriction(Projections.SubQuery(nextDueByDateSubQuery)));
            }

            #endregion

            #endregion

            #region Actual select with groups and counts

            query.SelectList(x => x
                                 .SelectGroup(_ => opCntr.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(_ => trainingRequirement.IsOSHARequirement)
                                 .WithAlias(() => result.OSHARequirement)
                                 .SelectGroup(_ => trainingRequirement.Description)
                                 .WithAlias(() => result.TrainingRequirement)
                                 .SelectGroup(_ => activeInitial.Title)
                                 .WithAlias(() => result.ActiveInitialTrainingModule)
                                 .SelectGroup(_ => activeInitialAndRecurring.Title)
                                 .WithAlias(() => result.ActiveInitialAndRecurringTrainingModule)
                                 .SelectGroup(_ => activeRecurring.Title)
                                 .WithAlias(() => result.ActiveRecurringTrainingModule)
                                 .SelectGroup(_ => activeInitial.Id)
                                 .WithAlias(() => result.ActiveInitialTrainingModuleId)
                                 .SelectGroup(_ => activeInitialAndRecurring.Id)
                                 .WithAlias(() => result.ActiveInitialAndRecurringTrainingModuleId)
                                 .SelectGroup(_ => activeRecurring.Id)
                                 .WithAlias(() => result.ActiveRecurringTrainingModuleId)
                                 .SelectGroup(_ => positionGroupCommonName.Description)
                                 .WithAlias(() => result.CommonName)
                                 .SelectGroup(_ => positionGroupCommonName.Id)
                                 .WithAlias(() => result.PositionGroupCommonNameId)
                                 .SelectGroup(_ => positionGroup.PositionDescription).WithAlias(() => result.Position)
                                 .SelectGroup(_ => positionGroup.Group).WithAlias(() => result.PositionGroup)
                                 .SelectGroup(_ => positionGroup.Id).WithAlias(() => result.PositionGroupId)
                                 .SelectGroup(e => e.LastName).WithAlias(() => result.LastName)
                                 .SelectGroup(e => e.FirstName).WithAlias(() => result.FirstName)
                                 .SelectGroup(_ => trainingModule.TotalHours).WithAlias(() => result.TotalHours)
                                 .SelectGroup(_ => trainingRequirement.Id).WithAlias(() => result.TrainingRequirementId)
                                 .SelectGroup(_ => trainingRequirement.TrainingFrequency)
                                 .SelectGroup(_ => trainingRequirement.TrainingFrequencyUnit)
                                 .SelectGroup(e => e.Id).WithAlias(() => result.EmployeeId)
                                 .SelectSubQuery(recentTrainingDateSubQuery.Take(1))
                                 .WithAlias(() => result.RecentTrainingDate)
                                 .SelectSubQuery(nextScheduledDateSubQuery.Take(1))
                                 .WithAlias(() => result.NextScheduledDate)
                                 .SelectSubQuery(nextDueByDateSubQuery).WithAlias(() => result.NextDueByDate)
            );

            #endregion

            // conditionals
            query.Where(x => x.Status != null && x.Status.Id == EmployeeStatus.Indices.ACTIVE);
            query.Where(_ => trainingRequirement.IsActive);
            query.Where(_ => trainingModule.IsActive.Value);

            query.TransformUsing(Transformers.AliasToBean<EmployeeTrainingReportItem>());

            return Search(search, query);
        }

        public IEnumerable<EmployeeTrainingByDateReportItem> GetTrainingByDate(ISearchEmployeeTrainingByDate search)
        {
            // This query is similar to GetRecentTraining except that
            //      - It doesn't use PositionGroupCommonName to get an employee's requirements. ie: This displays all training and not just required training for their PositionGroup.
            //      - It doesn't group together training. This displays every individual training record.

            EmployeeTrainingByDateReportItem result = null;
            Employee employee = null;
            OperatingCenter opCntr = null;
            PositionGroup positionGroup = null;
            PositionGroupCommonName positionGroupCommonName = null;
            TrainingRequirement trainingRequirement = null;
            TrainingModule trainingModule = null;
            TrainingRecordAttendedEmployee attendedTrainingRecords = null;
            TrainingRecordScheduledEmployee scheduledTrainingRecords = null;
            TrainingRecord attendedTrainingRecord = null, scheduledTrainingRecord = null;
            TrainingModule attendedTrainingModule = null,
                           scheduledTrainingModule = null,
                           activeInitial = null,
                           activeInitialAndRecurring = null,
                           activeRecurring = null;

            // We're querying employees, lets set that up and all the joins. buckle up.
            var query = Session.QueryOver(() => employee);

            #region Joins

            // We want the employee's (default) operating center
            query.JoinAlias(x => x.OperatingCenter, () => opCntr);
            // We want the position group they belong to. 
            query.JoinAlias(x => x.PositionGroup, () => positionGroup, JoinType.LeftOuterJoin);
            // And the position group common name
            query.JoinAlias(_ => positionGroup.CommonName, () => positionGroupCommonName, JoinType.LeftOuterJoin);

            query.JoinAlias(() => employee.AttendedTrainingRecords, () => attendedTrainingRecords);
            query.JoinAlias(() => attendedTrainingRecords.TrainingRecord, () => attendedTrainingRecord);
            query.JoinAlias(() => attendedTrainingRecord.TrainingModule, () => trainingModule);
            query.JoinAlias(() => trainingModule.TrainingRequirement, () => trainingRequirement,
                JoinType.LeftOuterJoin); // Not all TrainingModules have TrainingRequirements.
            query.JoinAlias(() => trainingRequirement.ActiveInitialTrainingModule, () => activeInitial,
                JoinType.LeftOuterJoin);
            query.JoinAlias(() => trainingRequirement.ActiveInitialAndRecurringTrainingModule,
                () => activeInitialAndRecurring, JoinType.LeftOuterJoin);
            query.JoinAlias(() => trainingRequirement.ActiveRecurringTrainingModule, () => activeRecurring,
                JoinType.LeftOuterJoin);

            #endregion

            #region SubQueries

            #region The most recent training date for the employee

            var recentTrainingDateSubQuery = QueryOver.Of(() => attendedTrainingRecord)
                                                      .JoinAlias(() => attendedTrainingRecord.EmployeesAttended,
                                                           () => attendedTrainingRecords)
                                                      .JoinAlias(() => attendedTrainingRecord.TrainingModule,
                                                           () => attendedTrainingModule)
                                                      .Where(() =>
                                                           attendedTrainingModule.TrainingRequirement.Id ==
                                                           trainingRequirement.Id
                                                           &&
                                                           employee.Id == attendedTrainingRecords.Employee.Id)
                                                      .OrderBy(_ => attendedTrainingRecord.HeldOn).Desc
                                                      .Select(_ => attendedTrainingRecord.HeldOn);
            // if the search is querying this date, add the where
            // and make sure we aren't pulling back records with nulls
            if (search.RecentTrainingDate != null && search.RecentTrainingDate.End.HasValue)
            {
                recentTrainingDateSubQuery.Where(
                    search.RecentTrainingDate.GetRestriction("attendedTrainingRecord.HeldOn"));
                // we don't want nulls, and if we just checked the subquery for nulls, it doesn't stop the parent query
                // from retrieving them, so this tells the parent query to not bring back nulls for this subquery
                query.WithSubquery.WhereExists(recentTrainingDateSubQuery);
            }

            #endregion

            #region The employees next scheduled training date

            var now = _dateTimeProvider.GetCurrentDate();

            var nextScheduledDateSubQuery = QueryOver.Of(() => scheduledTrainingRecord)
                                                     .JoinAlias(_ => scheduledTrainingRecord.EmployeesScheduled,
                                                          () => scheduledTrainingRecords)
                                                     .JoinAlias(_ => scheduledTrainingRecord.TrainingModule,
                                                          () => scheduledTrainingModule)
                                                     .Where(trr =>
                                                          scheduledTrainingModule.TrainingRequirement.Id ==
                                                          trainingRequirement.Id
                                                          && employee.Id == scheduledTrainingRecords.Employee.Id
                                                          && scheduledTrainingRecord.ScheduledDate > now)
                                                     .OrderBy(_ => scheduledTrainingRecord.ScheduledDate).Desc
                                                     .Select(_ => scheduledTrainingRecord.ScheduledDate);
            // if the search is querying this date, add the where
            // and make sure we aren't pulling back records with nulls
            if (search.NextScheduledDate != null && search.NextScheduledDate.End.HasValue)
            {
                nextScheduledDateSubQuery.Where(
                    search.NextScheduledDate.GetRestriction("scheduledTrainingRecord.ScheduledDate"));
                // we don't want nulls, and if we just checked the subquery for nulls, it doesn't stop the parent query
                // from retrieving them, so this tells the parent query to not bring back nulls for this subquery
                query.WithSubquery.WhereExists(nextScheduledDateSubQuery);
            }

            #endregion

            #region The employee's next due by date

            var nextDueByDateSubQuery = QueryOver.Of(() => attendedTrainingRecord)
                                                 .JoinAlias(_ => attendedTrainingRecord.EmployeesAttended,
                                                      () => attendedTrainingRecords)
                                                 .JoinAlias(_ => attendedTrainingRecord.TrainingModule,
                                                      () => attendedTrainingModule)
                                                 .Where(trr =>
                                                      attendedTrainingModule.TrainingRequirement.Id ==
                                                      trainingRequirement.Id
                                                      &&
                                                      employee.Id == attendedTrainingRecords.Employee.Id)
                                                 .Select(
                                                      Projections.SqlFunction(
                                                          new SQLFunctionTemplate(NHibernateUtil.DateTime,
                                                              FluentNHibernateExtensions.IsSqlite()
                                                                  ? "dateaddplus(?1, ?2, ?3)"
                                                                  : "dbo.dateaddplus(?1, ?2, ?3)"),
                                                          NHibernateUtil.DateTime,
                                                          Projections.Property(() =>
                                                              trainingRequirement.TrainingFrequencyUnit),
                                                          Projections.Property(() =>
                                                              trainingRequirement.TrainingFrequency.Value),
                                                          Projections.SqlFunction("max", NHibernateUtil.DateTime,
                                                              Projections.Property<TrainingRecord>(x => x.HeldOn))
                                                      )
                                                  );

            if (search.NextDueByDate != null && search.NextDueByDate.End.HasValue)
            {
                query.Where(search.NextDueByDate.GetRestriction(Projections.SubQuery(nextDueByDateSubQuery)));
            }

            #endregion

            #endregion

            #region Actual select with groups and counts

            query.SelectList(x => x
                                 .Select(_ => opCntr.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                 .Select(_ => trainingRequirement.IsOSHARequirement)
                                 .WithAlias(() => result.OSHARequirement)
                                 .Select(_ => trainingRequirement.Description)
                                 .WithAlias(() => result.TrainingRequirement)
                                 .Select(_ => activeInitial.Title).WithAlias(() => result.ActiveInitialTrainingModule)
                                 .Select(_ => activeInitialAndRecurring.Title)
                                 .WithAlias(() => result.ActiveInitialAndRecurringTrainingModule)
                                 .Select(_ => activeRecurring.Title)
                                 .WithAlias(() => result.ActiveRecurringTrainingModule)
                                 .Select(_ => activeInitial.Id).WithAlias(() => result.ActiveInitialTrainingModuleId)
                                 .Select(_ => activeInitialAndRecurring.Id)
                                 .WithAlias(() => result.ActiveInitialAndRecurringTrainingModuleId)
                                 .Select(_ => activeRecurring.Id)
                                 .WithAlias(() => result.ActiveRecurringTrainingModuleId)
                                 .Select(_ => positionGroupCommonName.Description).WithAlias(() => result.CommonName)
                                 .Select(_ => positionGroupCommonName.Id)
                                 .WithAlias(() => result.PositionGroupCommonNameId)
                                 .Select(_ => positionGroup.PositionDescription).WithAlias(() => result.Position)
                                 .Select(_ => positionGroup.Group).WithAlias(() => result.PositionGroup)
                                 .Select(_ => positionGroup.Id).WithAlias(() => result.PositionGroupId)
                                 .Select(e => e.LastName).WithAlias(() => result.LastName)
                                 .Select(e => e.FirstName).WithAlias(() => result.FirstName)
                                 .Select(_ => trainingModule.TotalHours).WithAlias(() => result.TotalHours)
                                 .Select(_ => trainingRequirement.Id).WithAlias(() => result.TrainingRequirementId)
                                 .Select(_ => trainingRequirement.TrainingFrequency)
                                 .Select(_ => trainingRequirement.TrainingFrequencyUnit)
                                 .Select(e => e.Id).WithAlias(() => result.EmployeeId)
                                 .Select(() => attendedTrainingRecord.HeldOn).WithAlias(() => result.DateAttended)
                                 .SelectSubQuery(recentTrainingDateSubQuery.Take(1))
                                 .WithAlias(() => result.RecentTrainingDate)
                                 .SelectSubQuery(nextScheduledDateSubQuery.Take(1))
                                 .WithAlias(() => result.NextScheduledDate)
                                 .SelectSubQuery(nextDueByDateSubQuery).WithAlias(() => result.NextDueByDate)
            );

            #endregion

            // conditionals
            query.Where(x => x.Status != null && x.Status.Id == EmployeeStatus.Indices.ACTIVE);

            if (search.DateAttended != null && search.DateAttended.End.HasValue)
            {
                query.Where(search.DateAttended.GetRestriction("attendedTrainingRecord.HeldOn"));
            }

            // Don't include this as it would hide the training they did that may have become inactive
            // query.Where(_ => trainingRequirement.IsActive);
            // query.Where(_ => trainingModule.IsActive.Value);

            query.TransformUsing(Transformers.AliasToBean<EmployeeTrainingByDateReportItem>());

            return Search(search, query);
        }

        public IEnumerable<EmployeeUserReconciliationReportItem> GetMismatchedUsers(
            ISearchEmployeeUserReconciliation search)
        {
            EmployeeUserReconciliationReportItem result = null;
            User user = null;
            OperatingCenter eOpCntr = null, uOpCntr = null;

            var query = Session.QueryOver<Employee>();

            query.JoinAlias(e => e.User, () => user);
            query.JoinAlias(e => e.OperatingCenter, () => eOpCntr);
            query.JoinAlias(_ => user.DefaultOperatingCenter, () => uOpCntr);

            query.SelectList(x => x
                                 .Select(e => e.FirstName).WithAlias(() => result.EmployeeFirstName)
                                 .Select(e => e.LastName).WithAlias(() => result.EmployeeLastName)
                                 .Select(e => e.Id).WithAlias(() => result.EmployeeId)
                                 .Select(_ => user.Id).WithAlias(() => result.UserId)
                                 .Select(e => e.EmployeeId).WithAlias(() => result.EmployeeNumber)
                                 .Select(_ => eOpCntr.OperatingCenterCode)
                                 .WithAlias(() => result.EmployeeOperatingCenter)
                                 .Select(_ => uOpCntr.OperatingCenterCode).WithAlias(() => result.UserOperatingCenter)
            );

            query.Where(_ => eOpCntr.Id != uOpCntr.Id);

            query.TransformUsing(Transformers.AliasToBean<EmployeeUserReconciliationReportItem>());

            return Search(search, query);
        }

        public IEnumerable<EmployeeTrainingRecordExportItem> GetEmployeeTrainingRecordExport(
            SearchSet<EmployeeTrainingRecordExportItem> search)
        {
            EmployeeTrainingRecordExportItem result = null;
            TrainingRecordAttendedEmployee trainingRecordAttendedEmployee = null;
            Employee employee = null, instructor = null;
            TrainingModule trainingModule = null;
            TrainingRecord trainingRecord = null;
            TrainingSession trainingSession = null;
            LEARNItemType itemType = null;
            List<TrainingSession> trainingSessions = null;
            List<TrainingRecordNote> trainingRecordNotes = null;

            var query = Session.QueryOver(() => trainingRecordAttendedEmployee);

            query.JoinAlias(x => x.Employee, () => employee);
            query.JoinAlias(x => x.TrainingRecord, () => trainingRecord);
            query.JoinAlias(x => trainingRecord.TrainingModule, () => trainingModule);
            query.JoinAlias(x => trainingModule.LEARNItemType, () => itemType);

            var lastTrainingSessionSubQuery = QueryOver.Of(() => trainingSession)
                                                       .JoinAlias(_ => trainingSession.TrainingRecord,
                                                            () => trainingSessions)
                                                       .Where(tr => tr.TrainingRecord.Id == trainingRecord.Id)
                                                       .OrderBy(_ => trainingSession.EndDateTime).Desc
                                                       .Select(_ => trainingSession.EndDateTime);
            query.WithSubquery.WhereExists(lastTrainingSessionSubQuery);

            query.SelectList(x => x
                                 .Select(k => employee.EmployeeId).WithAlias(() => result.EmployeeId)
                                 .Select(k => trainingModule.AmericanWaterCourseNumber).WithAlias(() => result.ItemID)
                                 .Select(k => EmployeeTrainingRecordExportItem.COMPLETION_STATUS)
                                 .WithAlias(() => result.CompletionStatus)
                                 .SelectSubQuery(lastTrainingSessionSubQuery.Take(1))
                                 .WithAlias(() => result.CompletionDateTime)
                                 .Select(k => trainingModule.TCHCreditValue).WithAlias(() => result.ContactHours)
                                 .Select(k => trainingModule.TotalHours).WithAlias(() => result.TotalHours)
                                 .Select(k => trainingRecord.Instructor).WithAlias(() => result.Instructor)
                                 .Select(k => trainingRecord.OutsideInstructor)
                                 .WithAlias(() => result.OutsideInstructor)
                                 .Select(k => trainingRecord.AttendeesExportedDate)
                                 .WithAlias(() => result.AttendeesExportedDate)
                                 .Select(k => trainingRecord.Exported).WithAlias(() => result.Exported)
                                 .Select(k => trainingRecordAttendedEmployee.TrainingRecord)
                                 .WithAlias(() => result.TrainingRecord)
                                 .Select(k => trainingRecord.HeldOn).WithAlias(() => result.HeldOn)
                                 .Select(k => trainingRecord.ScheduledDate).WithAlias(() => result.ScheduledDate)
                                 .Select(k => itemType.Abbreviation).WithAlias(() => result.ItemType)
            );

            query.Where(_ =>
                trainingModule.AmericanWaterCourseNumber != null && trainingModule.AmericanWaterCourseNumber != "");
            //query.Where(_ => trainingRecord.AttendeesExportedDate == null);

            query.TransformUsing(Transformers.AliasToBean<EmployeeTrainingRecordExportItem>());

            return Search(search, query);
        }

        private IEnumerable<Employee> GetEmployeesWithMedicalCertificatesDue(DateTime date)
        {
            date = date.BeginningOfDay();
            var nextDate = date.AddDays(1);

            return from e in Linq
                   where
                       e.Status.Description == "Active" && e.MedicalCertificateExpirationDate >= date &&
                       e.MedicalCertificateExpirationDate < nextDate
                   select e;
        }

        public IEnumerable<Employee> GetEmployeesWithMedicalCertificatesDueInOneMonth()
        {
            return GetEmployeesWithMedicalCertificatesDue(GetNow().AddDays(28));
        }

        public IEnumerable<Employee> GetEmployeesWithMedicalCertificatesDueInTwoMonths()
        {
            return GetEmployeesWithMedicalCertificatesDue(GetNow().AddDays(28 * 2));
        }

        public IEnumerable<Employee> GetEmployeesWithMedicalCertificatesDueInTwoWeeks()
        {
            return GetEmployeesWithMedicalCertificatesDue(GetNow().AddDays(14));
        }

        public IEnumerable<Employee> GetEmployeesWithMedicalCertificatesOverdue()
        {
            return GetEmployeesWithMedicalCertificatesDue(GetNow().AddDays(-1));
        }

        public IEnumerable<Employee> SearchForApi(ISearchSet<Employee> search)
        {
            PositionGroup pg = null;
            Employee employee = null;

            var query = Session.QueryOver<Employee>();
            query.JoinAlias(x => x.PositionGroup, () => pg, JoinType.LeftOuterJoin);
            query.Select(Projections.Property<Employee>(x => x.Id).WithAlias(nameof(employee.Id)),
                Projections.Property<Employee>(x => x.Status).WithAlias(nameof(employee.Status)),
                Projections.Property<Employee>(x => x.PositionGroup).WithAlias(nameof(employee.PositionGroup)),
                Projections.Property<Employee>(x => x.FirstName).WithAlias(nameof(employee.FirstName)),
                Projections.Property<Employee>(x => x.LastName).WithAlias(nameof(employee.LastName)),
                Projections.Property<Employee>(x => x.EmployeeId).WithAlias(nameof(employee.EmployeeId)),
                Projections.Property<Employee>(x => x.OperatingCenter).WithAlias(nameof(employee.OperatingCenter)),
                Projections.Property<Employee>(x => x.EmailAddress).WithAlias(nameof(employee.EmailAddress)),
                Projections.Property<Employee>(x => x.ReportsTo).WithAlias(nameof(employee.ReportsTo)));

            query.TransformUsing(Transformers.AliasToBean<Employee>());

            return Search(search, query);
        }

        #endregion

        #endregion
    }

    public static class EmployeeRepositoryExtensions
    {
        public static Employee GetByEmployeeId(this IRepository<Employee> that, string employeeId)
        {
            // This is using SingleOrDefault because EmployeeId is a unique constraint.
            return (from e in that.GetAll() where e.EmployeeId == employeeId select e).SingleOrDefault();
        }

        public static IQueryable<Employee> GetActiveEmployeesForSelect(this IRepository<Employee> that)
        {
            return
                that.Where(x => x.Status != null && x.Status.Id == EmployeeStatus.Indices.ACTIVE);
        }

        public static IQueryable<Employee> GetActiveEmployeesForSelectWhere(this IRepository<Employee> that,
            Expression<Func<Employee, bool>> fn)
        {
            return that.GetActiveEmployeesForSelect().Where(fn);
        }

        /// <summary>
        /// Returns employees with user accounts that have access to role a module
        /// regardless of which role action they have. This returns users that have
        /// any sort of read access for the role(as any action counts as read).
        /// </summary>
        /// <param name="that"></param>
        /// <param name="operatingCenterId"></param>
        /// <param name="roleModule"></param>
        /// <returns></returns>
        public static IQueryable<Employee> GetEmployeesByUserRole(this IRepository<Employee> that,
            int operatingCenterId, RoleModules roleModule)
        {
            return that.Where(e => e.User != null && e.User.AggregateRoles.Any(r =>
                (r.OperatingCenter == null ||
                 r.OperatingCenter.Id == operatingCenterId) &&
                r.Module.Id == (int)roleModule));
        }

        public static IQueryable<Employee> GetEmployeesByUserRole(this IRepository<Employee> that,
            int operatingCenterId, RoleModules roleModule, params RoleActions[] actions)
        {
            var iActions = actions.Select(a => (int)a).ToArray();

            return that.Where(e => e.User != null && e.User.AggregateRoles.Any(r =>
                (r.OperatingCenter == null ||
                 r.OperatingCenter.Id == operatingCenterId) &&
                r.Module.Id == (int)roleModule && iActions.Contains(r.Action.Id)));
        }

        /// <summary>
        /// Returns active employees with user accounts that have access to role a module 
        /// regardless of which role action they have. This returns users that have
        /// any sort of read access for the role(as any action counts as read).
        /// </summary>
        /// <param name="that"></param>
        /// <param name="operatingCenterId"></param>
        /// <param name="roleModule"></param>
        /// <returns></returns>
        public static IQueryable<Employee> GetActiveEmployeesByUserRole(this IRepository<Employee> that,
            int operatingCenterId, RoleModules roleModule)
        {
            return that.GetEmployeesByUserRole(operatingCenterId, roleModule)
                       .Where(e => e.Status != null && e.Status.Id == EmployeeStatus.Indices.ACTIVE);
        }

        public static IQueryable<Employee> GetActiveEmployeesByUserRole(this IRepository<Employee> that,
            int operatingCenterId, RoleModules roleModule, params RoleActions[] actions)
        {
            return that.GetEmployeesByUserRole(operatingCenterId, roleModule, actions)
                       .Where(e => e.Status != null && e.Status.Id == EmployeeStatus.Indices.ACTIVE);
        }

        public static IQueryable<Employee> GetEmployeesByOperatingCenters(this IRepository<Employee> that,
            IEnumerable<int> operatingCenterIds)
        {
            return that.Where(e => operatingCenterIds.Contains(e.OperatingCenter.Id));
        }

        public static IQueryable<Employee> GetActiveEmployeesByOperatingCentersForRole(this IRepository<Employee> that,
            User curUser, RoleModules role)
        {
            if (curUser.IsAdmin)
            {
                return that.GetAll();
            }

            var roles = curUser.Roles.Where(r => r.Module.Id == (int)role);
            // a role where OperatingCenter is null is a wildcard
            // for all operating centers, so just return all
            if (roles.Any(r => r.OperatingCenter == null))
            {
                return that.GetAll();
            }

            var opCenterIds = roles.Select(r => r.OperatingCenter.Id).Distinct().ToArray();
            return that.Where(e => opCenterIds.Contains(e.OperatingCenter.Id));
        }
    }

    public interface IEmployeeRepository : IRepository<Employee>
    {
        #region Abstract Methods

        IQueryable<Employee> GetActiveEmployeesWhere(Expression<Func<Employee, bool>> fn);
        IQueryable<Employee> GetActiveEmployeesByOperatingCenterId(params int[] operatingCenterId);
        IQueryable<Employee> GetEmployeesByOperatingCenters(params int[] operatingCenterIds);
        IQueryable<Employee> GetByOperatingCenterId(int[] operatingCenterIds);
        IQueryable<Employee> GetForSelect();
        IQueryable<Employee> GetActiveForSelect();

        IEnumerable<Employee> GetEmployeesWithCommercialDriversLicenseRenewalsDueInTwoMonths(
            int? operatingCenterId = null);

        IEnumerable<EmployeeTrainingReportItem> GetRecentTraining(ISearchEmployeeTraining search);
        IEnumerable<EmployeeTrainingByDateReportItem> GetTrainingByDate(ISearchEmployeeTrainingByDate search);
        IEnumerable<EmployeeUserReconciliationReportItem> GetMismatchedUsers(ISearchEmployeeUserReconciliation search);

        IEnumerable<EmployeeTrainingRecordExportItem> GetEmployeeTrainingRecordExport(
            SearchSet<EmployeeTrainingRecordExportItem> search);

        #endregion

        IEnumerable<Employee> GetEmployeesWithMedicalCertificatesDueInTwoWeeks();
        IEnumerable<Employee> GetEmployeesWithMedicalCertificatesDueInOneMonth();
        IEnumerable<Employee> GetEmployeesWithMedicalCertificatesDueInTwoMonths();
        IEnumerable<Employee> GetEmployeesWithMedicalCertificatesOverdue();
        IEnumerable<Employee> SearchForApi(ISearchSet<Employee> search);
        IQueryable<Employee> GetByOperatingCenterIdAndStatusId(int operatingCenterId, int statusId);
        IQueryable<Employee> GetByStateIdOrOperatingCenterIdAndStatusId(int? stateId, int? operatingCenterId, int? statusId);
    }
}
