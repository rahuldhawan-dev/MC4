using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IIncidentRepository : IRepository<Incident>
    {
        IEnumerable<Incident> SearchOSHA(ISearchIncidentOSHARecordableSummary search);
        IQueryable<Incident> GetByOperatingCenter(int operatingCenter);
        IEnumerable<Incident> GetByEmployeeId(int empId);
    }

    public class IncidentRepository : MapCallSecuredRepositoryBase<Incident>, IIncidentRepository
    {
        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                // admins can see it all
                if (_authenticationSerice.CurrentUser.IsAdmin)
                {
                    return base.Criteria;
                }

                var opCenterIds = CurrentUser.GetQueryableMatchingRoles(_roleRepo, Role, RoleActions.UserAdministrator)
                                             .OperatingCenters;
                // if the role doesn't have an operating center then only filter by reports to
                var crit = base.Criteria
                               .CreateAlias("Employee", "criteriaEmployee", JoinType.LeftOuterJoin)
                               .CreateAlias("criteriaEmployee.ReportsTo", "criteriaSupervisor", JoinType.LeftOuterJoin)
                               .CreateAlias("criteriaSupervisor.ReportsTo", "criteriaSupervisorManager", JoinType.LeftOuterJoin)
                               .CreateAlias("criteriaSupervisorManager.ReportsTo", "criteriaSupervisorManagerSupervisor", JoinType.LeftOuterJoin)
                               .CreateAlias("ContractorObservedBy", "criteriaContractorObservedBy", JoinType.LeftOuterJoin)
                               .CreateAlias("criteriaContractorObservedBy.ReportsTo", "criteriaContractorObservedByReportsTo", JoinType.LeftOuterJoin);
                // Not all users are linked to Employees. If that's the case, we don't
                // want to return any results to them. Unlike Linq, we can't completely
                // skip the query since we need to return an ICriteria object.
                var empId = CurrentUser.Employee?.Id ?? 0;

                if (opCenterIds.Length > 0)
                {
                    return crit
                       .Add(Restrictions.Or(
                            Restrictions.In("OperatingCenter.Id", opCenterIds),
                            Restrictions.Or(
                                Restrictions.Eq("criteriaSupervisor.Id", empId),
                                Restrictions.Or(
                                    Restrictions.Eq("criteriaContractorObservedBy.Id", empId),
                                    Restrictions.Or(
                                        Restrictions.Eq("criteriaContractorObservedByReportsTo.Id", empId),
                                        Restrictions.Or(
                                            Restrictions.Eq("criteriaSupervisorManager.Id", empId),
                                            Restrictions.Eq("criteriaSupervisorManagerSupervisor.Id", empId)))))));
                }

                return crit.Add(Restrictions.Or(
                    Restrictions.Eq("criteriaSupervisor.Id", empId),
                    Restrictions.Or(
                        Restrictions.Eq("criteriaContractorObservedBy.Id", empId),
                        Restrictions.Or(
                            Restrictions.Eq("criteriaContractorObservedByReportsTo.Id", empId),
                            Restrictions.Or(
                                Restrictions.Eq("criteriaSupervisorManager.Id", empId),
                                Restrictions.Eq("criteriaSupervisorManagerSupervisor.Id", empId))))));
            }
        }

        public override IQueryable<Incident> Linq
        {
            get
            {
                // admins can see it all
                if (_authenticationSerice.CurrentUser.IsAdmin)
                {
                    return base.Linq;
                }

                // Not all users are linked to Employees. If that's the case, we don't
                // want to return any results to them. And if we don't want to return results,
                // let's just skip the query entirely.
                if (CurrentUser.Employee == null)
                {
                    return Enumerable.Empty<Incident>().AsQueryable();
                }

                var opCenterIds = CurrentUser.GetQueryableMatchingRoles(_roleRepo, Role, RoleActions.UserAdministrator)
                                             .OperatingCenters;

                var empId = CurrentUser.Employee.Id;

                // user has UserAdmin roles for some operating centers.  return
                // incidents for those operating centers, as well as for their
                // direct and next to direct reports
                if (opCenterIds.Length > 0)
                {
                    return base.Linq.Where(x =>
                        opCenterIds.Contains(x.OperatingCenter.Id) ||
                        x.Employee.ReportsTo.Id == empId ||
                        x.Employee.ReportsTo.ReportsTo.Id == empId ||
                        x.Employee.ReportsTo.ReportsTo.ReportsTo.Id == empId ||
                        x.ContractorObservedBy.Id == empId || 
                        x.ContractorObservedBy.ReportsTo.Id == empId
                    );
                }

                // user has no UserAdmin roles for any operating centers. return
                // incidents only for their direct and next to direct reports
                return base.Linq.Where(x =>
                    x.Employee.ReportsTo.Id == empId ||
                    x.Employee.ReportsTo.ReportsTo.Id == empId ||
                    x.Employee.ReportsTo.ReportsTo.ReportsTo.Id == empId ||
                    x.ContractorObservedBy.Id == empId ||
                    x.ContractorObservedBy.ReportsTo.Id == empId
                );
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.OperationsIncidents; }
        }

        #endregion

        #region Fields

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public IncidentRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IDateTimeProvider dateTimeProvider) : base(session,
            container,
            authenticationService, roleRepo)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Public Methods

        public IQueryable<Incident> GetByOperatingCenter(int opcId)
        {
            return (from fd in Linq where fd.OperatingCenter.Id == opcId select fd);
        }

        public IEnumerable<Incident> SearchOSHA(ISearchIncidentOSHARecordableSummary search)
        {
            var opc = search.OperatingCenter ?? new int[0];
            var searchBegin = search.IncidentDate.Start ?? Convert.ToDateTime("01/01/0001");
            var searchEnd = search.IncidentDate.End.Value;
            var endDateForUnsetValues = DateTime.Now;
            IQueryable<Incident> results = null;

            switch (search.IncidentDate.Operator)
            {
                case RangeOperator.Between:
                    searchEnd = searchEnd.EndOfDay();
                    searchBegin = search.IncidentDate.Start.Value.BeginningOfDay();
                    results = (from i in Linq
                               where
                                   (searchBegin <= i.IncidentDate && i.IncidentDate <= searchEnd)
                                   || i.EmployeeAvailabilities.Any(x =>
                                       searchBegin.BeginningOfDay() <= (x.EndDate ?? endDateForUnsetValues) &&
                                       x.StartDate <= searchEnd.EndOfDay())
                               select i);
                    break;
                case RangeOperator.Equal:
                    results = (from i in Linq
                               where
                                   (searchEnd.Date == i.IncidentDate.Date)
                                   || i.EmployeeAvailabilities.Any(x =>
                                       searchEnd.BeginningOfDay() <= (x.EndDate ?? endDateForUnsetValues) &&
                                       x.StartDate <= searchEnd.EndOfDay())
                               select i);
                    break;
                case RangeOperator.GreaterThan:
                    searchBegin = search.IncidentDate.End.Value.EndOfDay();
                    searchEnd = endDateForUnsetValues.EndOfDay();
                    results = (from i in Linq
                               where
                                   (i.IncidentDate > searchBegin)
                                   || i.EmployeeAvailabilities.Any(x => x.StartDate > searchBegin)
                                   || i.EmployeeAvailabilities.Any(x => x.EndDate <= searchEnd)
                               select i);
                    break;
                case RangeOperator.GreaterThanOrEqualTo:
                    searchBegin = search.IncidentDate.End.Value.BeginningOfDay();
                    searchEnd = endDateForUnsetValues.EndOfDay();
                    results = (from i in Linq
                               where
                                   (i.IncidentDate >= searchBegin)
                                   || i.EmployeeAvailabilities.Any(x => x.StartDate >= searchBegin)
                                   || i.EmployeeAvailabilities.Any(x => x.EndDate <= searchEnd)
                               select i);
                    break;
                case RangeOperator.LessThan:
                    searchEnd = searchEnd.BeginningOfDay();
                    results = (from i in Linq
                               where
                                   (i.IncidentDate < searchEnd)
                                   || i.EmployeeAvailabilities.Any(x => x.EndDate < searchEnd)
                               select i);
                    break;
                case RangeOperator.LessThanOrEqualTo:
                    searchEnd = searchEnd.EndOfDay();
                    results = (from i in Linq
                               where
                                   (i.IncidentDate <= searchEnd)
                                   || i.EmployeeAvailabilities.Any(x => x.EndDate <= searchEnd)
                               select i);
                    break;
            }

            results = results.Where(x => x.IsOSHARecordable == true);

            if (opc.Any())
            {
                results = results.Where(x => opc.Contains(x.OperatingCenter.Id));
            }

            search.Results = results.ToList();
            return search.Results;
        }

        #endregion

        public override void Delete(Incident entity)
        {
            // Action items need to be deleted with the record. Reports based on action items
            // start throwing NHibernate errors when you need to reference the ActionItem.Entity property 
            // and the entity no longer exists. We need to find a better way of dealing with this, if possible.
            // Otherwise we need to implement this deletion for everything that uses action items. -Ross 8/7/2020
            this.DeleteAllActionItems(Session, entity);

            base.Delete(entity);
        }

        public IEnumerable<Incident> GetByEmployeeId(int employeeId)
        {
            return (from wo in base.Linq
                    where wo.Employee.Id == employeeId
                    select new Incident { Id = wo.Id }).OrderByDescending(x => x.Id);
        }
    }
}
