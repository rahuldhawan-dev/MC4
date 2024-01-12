using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class WorkOrderRepository : MapCallSecuredRepositoryBase<WorkOrder>, IWorkOrderRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        // NOTE: DO NOT MAKE STATIC FIELDS THAT USE DATETIME.NOW!!!! -Ross 4/5/2016

        public static readonly ICriterion NotCompleted = Restrictions.IsNull("DateCompleted");

        public static readonly ICriterion MarkoutRequired =
            Restrictions.Not(Restrictions.Eq("MarkoutRequirement.Id", (int)MarkoutRequirementEnum.None));

        public static readonly ICriterion NotApproved =
            Restrictions.And(Restrictions.IsNull("ApprovedOn"), Restrictions.IsNull("ApprovedBy"));

        public static readonly ICriterion EmergencyPriority =
            Restrictions.Eq("Priority.Id", (int)WorkOrderPriorityEnum.Emergency);

        public static readonly ICriterion NotCancelled = Restrictions.IsNull("CancelledAt");

        /// <summary>
        /// This is used to filter out orders that haven't been submitted to SAP
        /// so that they don't appear until they have been fixed. This is old 271
        /// functionality.
        ///
        /// If the operating center does not have SAPWorkOrdersEnabled then it can be shown right away,
        /// we're otherwise waiting for the SAPWorkOrderNumber to come from whatever synchronization process
        /// we use with SAP. And contracted operations don't need to wait on SAP either apparently.
        ///
        /// If you change this logic, you must update the WorkOrder.IsSAPValid property as well.
        /// </summary>
        public static readonly ICriterion SAPValidCriteria = Restrictions.Or(
            Restrictions.Eq("OperatingCenter.SAPWorkOrdersEnabled", false),
            Restrictions.Or(
                Restrictions.Eq("OperatingCenter.IsContractedOperations", true), 
                Restrictions.IsNotNull("SAPWorkOrderNumber")));

        #endregion

        #region Fields

        protected readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        // We can schedule for markouts in the future
        public DetachedCriteria ValidMarkoutIds => 
            DetachedCriteria.For<Markout>("markout")
                            .SetProjection(Projections.Property("markout.WorkOrder"))
                            .Add(Property.ForName("markout.WorkOrder").EqProperty("workorder.Id"))
                            .Add(Restrictions.Ge("ExpirationDate", _dateTimeProvider.GetCurrentDate()));

        // We use this because we don't want to allow them to start work if it isn't current
        public DetachedCriteria CurrentValidMarkoutIds => 
            DetachedCriteria.For<Markout>("markout")
                            .SetProjection(Projections.Property("markout.WorkOrder"))
                            .Add(Property.ForName("markout.WorkOrder").EqProperty("workorder.Id"))
                            .Add(Restrictions.Ge("ExpirationDate", _dateTimeProvider.GetCurrentDate()))
                            .Add(Restrictions.Le("ReadyDate", _dateTimeProvider.GetCurrentDate()));

        public DetachedCriteria ValidPermitIds => 
            DetachedCriteria.For<StreetOpeningPermit>("permit")
                            .SetProjection(Projections.Property("permit.WorkOrder.Id"))
                            .Add(Property.ForName("permit.WorkOrder.Id").EqProperty("workorder.Id"))
                            .Add(Restrictions.IsNotNull("DateIssued"))
                            .Add(Restrictions.Ge("ExpirationDate", _dateTimeProvider.GetCurrentDate()));

        public DetachedCriteria ValidCrewAssignments => 
            DetachedCriteria.For<CrewAssignment>("crewassignment")
                            .SetProjection(Projections.Property("crewassignment.WorkOrder.Id"))
                            .Add(Property.ForName("crewassignment.WorkOrder.Id").EqProperty("workorder.Id"))
                            .Add(Restrictions.Or(Restrictions.Le("AssignedFor", _dateTimeProvider.GetCurrentDate()),
                                 Restrictions.IsNotNull("DateStarted")));

        public DetachedCriteria HasPermits =>
            DetachedCriteria.For<StreetOpeningPermit>("permit")
                            .SetProjection(Projections.Property("permit.WorkOrder.Id"))
                            .Add(Property.ForName("permit.WorkOrder.Id").EqProperty("workorder.Id"));

        public ICriterion StreetOpeningPermitRequired => Restrictions.Eq("StreetOpeningPermitRequired", true);

        public override RoleModules Role => ROLE;

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria;
                if (!CurrentUserCanAccessAllTheRecords)
                {
                    crit = crit.Add(Restrictions.In("OperatingCenter.Id", GetUserOperatingCenterIds()));
                }
                return crit;
            }
        }
        
        public ICriteria FinalizationOrders => 
            Criteria
               .CreateAlias("OperatingCenter", "OperatingCenter", JoinType.InnerJoin)
               .Add(GetFinalizationCriteria());

        public ICriteria SopProcessingOrders =>
            Criteria
               .CreateAlias("OperatingCenter", "OperatingCenter", JoinType.InnerJoin)
               .Add(GetSopProcessingCriteria());

        public ICriteria PrePlanningOrders => Criteria.Add(GetPrePlanningCriteria());

        public ICriteria PlanningOrders => Criteria.Add(GetPlanningCriteria());

        // We need to join on OperatingCenter because GetSchedulingCriteria
        // references OperatingCenter via SAPValidCriteria.
        // We should probably put this as part of the base Criteria, but
        // since we need to get this out ASAP, and I don't want to break
        // anything I don't have to break, we can see about doing this in
        // the other 271 -> MVC conversion tickets.
        public ICriteria SchedulingOrders => 
            Criteria
               .CreateAlias("OperatingCenter", "OperatingCenter", JoinType.InnerJoin)
               .Add(GetSchedulingCriteria());

        public ICriteria GeneralOrders => Criteria.Add(GetGeneralCriteria());
       
        private ICriteria SupervisorApprovalOrders
        {
            get
            {
                return Criteria.CreateAlias("OperatingCenter", "OperatingCenter", JoinType.InnerJoin)
                               .Add(GetSupervisorApprovalCriterion());
            }
        }

        private ICriteria StockToIssueOrders
        {
            get
            {
                return Criteria.CreateAlias("OperatingCenter", "OperatingCenter", JoinType.InnerJoin)
                               .Add(Restrictions.Conjunction()
                                                .Add(NotCancelled)
                                                .Add(SAPValidCriteria)
                                                .Add(Restrictions.IsNotNull("ApprovedBy"))
                                                .Add(Restrictions.Eq("HasMaterialsUsed", true)));
            }
        }
     
        #endregion

        #region Constructors

        public WorkOrderRepository(ISession session, IAuthenticationService<User> authenticationService,
            IContainer container,
            IRepository<AggregateRole> roleRepo, IDateTimeProvider dateTimeProvider) : base(session, container,
            authenticationService,
            roleRepo)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Private Methods

        private ICriterion GetPrePlanningCriteria()
        {
            return Restrictions.And(NotCompleted, NotCancelled);
        }

        private ICriterion GetPlanningCriteria()
        {
            return Restrictions
                  .Conjunction()
                  .Add(NotCancelled)
                  .Add(NotCompleted)
                  .Add(Restrictions.Or(
                       Restrictions.And(
                           MarkoutRequired,
                           Restrictions.Not(Subqueries.Exists(ValidMarkoutIds))
                       ),
                       Restrictions.And(
                           StreetOpeningPermitRequired,
                           Restrictions.Not(Subqueries.Exists(ValidPermitIds)))
                   ));
        }

        private ICriterion GetSchedulingCriteria()
        {
            return Restrictions.And(SAPValidCriteria, 
                Restrictions.And(
                    Restrictions.IsNull("AssignedContractor"),
                    Restrictions.And(NotCancelled,
                        Restrictions.And(NotCompleted,
                            Restrictions.Or(
                                Restrictions.Eq("MarkoutRequirement.Id", (int)MarkoutRequirement.Indices.EMERGENCY),
                                Restrictions.And(
                                    Restrictions.Or(
                                        Restrictions.Not(Restrictions.Eq("MarkoutRequirement.Id", (int)MarkoutRequirement.Indices.ROUTINE)),
                                        Subqueries.Exists(ValidMarkoutIds)),
                                    Restrictions.Or(
                                        Restrictions.Or(
                                            Restrictions.Not(Restrictions.Eq("StreetOpeningPermitRequired", true)), EmergencyPriority),
                                        Subqueries.Exists(ValidPermitIds))))))));
        }

        private ICriterion GetFinalizationCriteria()
        {
            return Restrictions.And(SAPValidCriteria,
                Restrictions.And(NotCancelled,
                    Restrictions.And(NotApproved,
                        Restrictions.Or(
                            Restrictions.Or(Subqueries.Exists(ValidCrewAssignments), EmergencyPriority),
                            Restrictions.IsNotNull("AssignedContractor")
                        ))));
        }

        private ICriterion GetSopProcessingCriteria()
        {
            return Restrictions.And(SAPValidCriteria,
                Restrictions.And(NotCancelled,
                    Restrictions.And(StreetOpeningPermitRequired, Subqueries.NotExists(HasPermits))));
        }

        private ICriterion GetGeneralCriteria()
        {
            return Restrictions.Conjunction();
        }

        private IQueryable<WorkOrder> GetBaseLinqCompletedWorkOrdersWithJobSiteCheckLists(
            ISearchCompletedWorkOrdersWithJobSiteCheckLists search)
        {
            var query = Linq.Where(wo => wo.CancelledAt == null);

            // TODO: This mapping is horrible and needs to be located in one spot somehow.
            switch (search.DateCompleted.Operator)
            {
                case RangeOperator.Between:
                    // NOTE: This includes the whole of the end day.
                    query = query.Where(x =>
                        search.DateCompleted.Start.Value.BeginningOfDay() <= x.DateCompleted &&
                        x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                    break;

                case RangeOperator.Equal:
                    query = query.Where(x =>
                        search.DateCompleted.End.Value.BeginningOfDay() <= x.DateCompleted &&
                        x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                    break;

                case RangeOperator.GreaterThan:
                    query = query.Where(x => x.DateCompleted > search.DateCompleted.End.Value.EndOfDay());
                    break;

                case RangeOperator.GreaterThanOrEqualTo:
                    query = query.Where(x => x.DateCompleted >= search.DateCompleted.End.Value.BeginningOfDay());
                    break;

                case RangeOperator.LessThan:
                    query = query.Where(x => x.DateCompleted < search.DateCompleted.End.Value.BeginningOfDay());
                    break;

                case RangeOperator.LessThanOrEqualTo:
                    query = query.Where(x => x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                    break;

                default:
                    throw new NotSupportedException(search.DateCompleted.Operator.ToString());
            }

            // NOTE: All of this is tested in the repository test. If additional filtering gets added,
            // make sure additional tests get created too.

            if (search.OperatingCenter != null && search.OperatingCenter.Any())
            {
                query = query.Where(x => search.OperatingCenter.Contains(x.OperatingCenter.Id));
            }

            if (search.State.HasValue)
            {
                query = query.Where(x => x.Town.County.State.Id == search.State.Value);
            }

            if (search.WorkDescription != null && search.WorkDescription.Any())
            {
                query = query.Where(x => search.WorkDescription.Contains(x.WorkDescription.Id));
            }

            if (search.IsAssignedContractor.HasValue)
            {
                if (search.IsAssignedContractor.Value)
                {
                    query = query.Where(x => x.AssignedContractor != null);
                }
                else
                {
                    query = query.Where(x => x.AssignedContractor == null);
                }
            }

            return query;
        }

        private List<CompletedWorkOrderWithJobSiteCheckListReportItem>
            GetFinalResultCompletedWorkOrdersWithJobSiteCheckLists(IQueryable<WorkOrder> query)
        {
            return (from wo in query
                    group wo by new {
                        State = wo.Town.County.State.Abbreviation,
                        OperatingCenter = wo.OperatingCenter.OperatingCenterCode,
                        OperatingCenterId = wo.OperatingCenter.Id,
                        WorkDescription = wo.WorkDescription.Description,
                        WorkDescriptionId = wo.WorkDescription.Id,
                    }
                    into groupedWo
                    select new CompletedWorkOrderWithJobSiteCheckListReportItem {
                        State = groupedWo.Key.State,
                        OperatingCenter = groupedWo.Key.OperatingCenter,
                        OperatingCenterId = groupedWo.Key.OperatingCenterId,
                        WorkDescription = groupedWo.Key.WorkDescription,
                        WorkDescriptionId = groupedWo.Key.WorkDescriptionId,
                        WorkOrderCount = groupedWo.Count(),
                    }).ToList();
        }

        private IQueryOver<WorkOrder, WorkOrder> GetMainBreaksAndServiceLineRepairsQueryOver()
        {
            WorkDescription description = null;

            var query = Session.QueryOver<WorkOrder>();

            query.JoinAlias(x => x.WorkDescription, () => description);

            query.Where(_ => description.Id == (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR ||
                             description.Id == (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE);

            return query;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<WorkOrder> GetLostWaterInPastDay()
        {
            return this.GetLostWaterInPastDayImpl(_dateTimeProvider);
        }

        public IEnumerable<WorkOrder> GetWorkOrdersWithSapRetryIssues()
        {
            return this.GetWorkOrdersWithSapRetryIssuesImpl();
        }

        public IEnumerable<WorkOrder> FindByPartialWorkOrderIDMatch(string partialWorkOrderID)
        {
            if (string.IsNullOrWhiteSpace(partialWorkOrderID))
            {
                return Enumerable.Empty<WorkOrder>();
            }

            return (from wo in Linq.Where(wo => wo.CancelledAt == null)
                    where wo.Id.ToString().StartsWith(partialWorkOrderID)
                    orderby wo.Id
                    select wo).Take(10);
        }

        public IEnumerable<WorkOrder> GetByTownIdForServices(int townId)
        {
            return (from wo in base.Linq
                    where wo.Town.Id == townId &&
                          (wo.AssetType.Id == AssetType.Indices.SERVICE ||
                           wo.AssetType.Id == AssetType.Indices.SEWER_LATERAL)
                    select new WorkOrder {Id = wo.Id}).OrderByDescending(x => x.Id);
        }

        public IEnumerable<WorkOrder> GetByTownIdForMainBreaks(int townId)
        {
            return (from wo in Linq.Where(wo => wo.CancelledAt == null)
                    where wo.Town.Id == townId
                          && WorkDescription.GetMainBreakWorkDescriptions().Contains(wo.WorkDescription.Id)
                    select new WorkOrder {Id = wo.Id}).OrderByDescending(x => x.Id);
        }

        public IEnumerable<WorkOrder> GetByTownId(int townId)
        {
            return (from wo in base.Linq
                    where wo.Town.Id == townId
                    select new WorkOrder {Id = wo.Id}).OrderByDescending(x => x.Id);
        }

        private IEnumerable<MainBreaksAndServiceLineRepairsViewModel> GetMainBreaksAndServiceLineRepairs(
            ISearchSet<WorkOrder> search)
        {
            var criteria = GenerateCriteriaForSearchSet(search, null);
            criteria = criteria
                      .Add(Restrictions.In("WorkDescription.Id", WorkOrder.ALL_MAIN_BREAKS))
                      .Add(Restrictions.IsNotNull("DateCompleted"));
            // ReSharper disable WrongIndentSize
            return criteria
                  .SetProjection(Projections.ProjectionList()
                                            .Add(Projections.Alias(Projections.GroupProperty("OperatingCenter"),
                                                 "OperatingCenter"))
                                            .Add(Projections.Alias(Projections.GroupProperty("WorkDescription"),
                                                 "WorkDescription"))
                                            .Add(Projections.Alias(Projections.GroupProperty("MonthCompleted"),
                                                 "MonthCompleted"))
                                            .Add(Projections.Alias(Projections.GroupProperty("YearCompleted"), "Year"))
                                            .Add(Projections.Alias(Projections.Count("MonthCompleted"), "Month"))
                   )
                  .SetResultTransformer(Transformers.AliasToBean<MainBreaksAndServiceLineRepairsViewModel>())
                  .List<MainBreaksAndServiceLineRepairsViewModel>();
            // ReSharper enable WrongIndentSize
        }

        public IEnumerable<MainBreaksAndServiceLineRepairsReportViewModel> SearchMainBreaksAndServiceLineRepairsReport(
            ISearchSet<WorkOrder> search)
        {
            // No paging for this report.
            search.EnablePaging = false;

            var results = GetMainBreaksAndServiceLineRepairs(search).ToList();
            var years = results.Select(x => x.Year).Distinct().OrderBy(x => x);
            var workDescriptions = results.Select(x => x.WorkDescription).Distinct().OrderBy(x => x.Description);
            var operatingCenters =
                results.Select(x => x.OperatingCenter).Distinct().OrderBy(x => x.OperatingCenterCode);
            var totalRecords = workDescriptions.Count() * years.Count() * operatingCenters.Count();
            var report = new List<MainBreaksAndServiceLineRepairsReportViewModel>();

            foreach (var year in years)
            {
                foreach (var operatingCenter in operatingCenters)
                {
                    foreach (var workDescription in workDescriptions)
                    {
                        var rowResult = results.Where(x =>
                            x.Year == year && x.OperatingCenter == operatingCenter &&
                            x.WorkDescription == workDescription).ToArray();

                        if (rowResult.Any())
                        {
                            Func<int, int> getCount = (month) => {
                                var first = rowResult.FirstOrDefault(x => x.MonthCompleted == month);
                                return (first != null) ? (int)first.GetPropertyValueByName("Month") : 0;
                            };

                            report.Add(new MainBreaksAndServiceLineRepairsReportViewModel {
                                OperatingCenter = operatingCenter,
                                WorkDescription = workDescription,
                                Year = year,
                                Jan = getCount(1),
                                Feb = getCount(2),
                                Mar = getCount(3),
                                Apr = getCount(4),
                                May = getCount(5),
                                Jun = getCount(6),
                                Jul = getCount(7),
                                Aug = getCount(8),
                                Sep = getCount(9),
                                Oct = getCount(10),
                                Nov = getCount(11),
                                Dec = getCount(12)
                            });
                        }
                    }
                }
            }

            return report;
        }

        public IEnumerable<int> GetDistinctYearsCompleted()
        {
            return
                (from wo in Linq.Where(wo => wo.CancelledAt == null)
                 where wo.DateCompleted.HasValue
                 select wo.DateCompleted.Value.Year
                ).Distinct();
        }

        public WorkOrder FindBySAPWorkOrderNumber(long sap)
        {
            // Because apparently multiple work orders can have the same SAP work order number.
            return Linq.Where(wo => wo.CancelledAt == null).FirstOrDefault(x => x.SAPWorkOrderNumber == sap);
        }

        public WorkOrder FindByOriginalWorkOrderNumber(int originalWorkOrderNumber)
        {
            return Linq.FirstOrDefault(x => x.OriginalOrderNumber.Id == originalWorkOrderNumber);
        }

        public ICriterion GetSAPNotificationsCriterion()
        {
            return Restrictions.Gt("SAPNotificationNumber", (Int64)0);
        }

        public IEnumerable<FieldCompletedBacklogQAReportItem> GetFieldCompletedBacklogQAReport(
            ISearchFieldCompletedBacklogQAReport search)
        {
            // Both fields of the search are required per bug 3398.

            var opCenters = Session.Query<OperatingCenter>().Where(x => search.OperatingCenter.Contains(x.Id)).ToList();
            var results = new List<FieldCompletedBacklogQAReportItem>();

            var dateLinq = Linq.Where(wo => wo.CancelledAt == null);

            switch (search.DateCompleted.Operator)
            {
                case RangeOperator.Between:
                    // NOTE: This includes the whole of the end day.
                    dateLinq = dateLinq.Where(x =>
                        search.DateCompleted.Start.Value.BeginningOfDay() <= x.DateCompleted &&
                        x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                    break;

                case RangeOperator.Equal:
                    dateLinq = dateLinq.Where(x =>
                        search.DateCompleted.End.Value.BeginningOfDay() <= x.DateCompleted &&
                        x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                    break;

                case RangeOperator.GreaterThan:
                    dateLinq = dateLinq.Where(x => x.DateCompleted > search.DateCompleted.End.Value.EndOfDay());
                    break;

                case RangeOperator.GreaterThanOrEqualTo:
                    dateLinq = dateLinq.Where(x => x.DateCompleted >= search.DateCompleted.End.Value.BeginningOfDay());
                    break;

                case RangeOperator.LessThan:
                    dateLinq = dateLinq.Where(x => x.DateCompleted < search.DateCompleted.End.Value.BeginningOfDay());
                    break;

                case RangeOperator.LessThanOrEqualTo:
                    dateLinq = dateLinq.Where(x => x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                    break;

                default:
                    throw new NotSupportedException(search.DateCompleted.Operator.ToString());
            }

            foreach (var opc in opCenters)
            {
                var resultItem = new FieldCompletedBacklogQAReportItem();
                resultItem.OperatingCenter = opc.OperatingCenterCode;

                var baseLinq = dateLinq.Where(x => x.OperatingCenter == opc);

                resultItem.AwaitingApprovalOrdersWithMaterials =
                    baseLinq.Count(x => x.ApprovedOn == null && x.MaterialsUsed.Any());
                resultItem.AwaitingApprovalOrdersWithoutMaterials =
                    baseLinq.Count(x => x.ApprovedOn == null && !x.MaterialsUsed.Any());
                resultItem.AwaitingStockToIssue = baseLinq.Count(x =>
                    x.ApprovedOn != null && x.MaterialsApprovedOn == null && x.MaterialsUsed.Any());

                results.Add(resultItem);
            }

            search.Results = results;
            search.Count = results.Count;

            return results;
        }

        public IEnumerable<WorkOrder> GetMainBreakRepairsForGIS(ISearchSet<WorkOrder> search)
        {
            var query = GetMainBreaksAndServiceLineRepairsQueryOver();

            return Search(search, query);
        }

        public IEnumerable<WorkOrder> GetMainBreakRepairsForGIS(ISearchMainBreakRepairsForGIS search)
        {
            var query = GetMainBreaksAndServiceLineRepairsQueryOver();

            if (search.RecentOrders.GetValueOrDefault())
            {
                CrewAssignment crewAssignment = null;

                query.JoinAlias(x => x.CrewAssignments, () => crewAssignment, JoinType.LeftOuterJoin)
                     .Where(Restrictions.Or(
                          Restrictions.Where(() => crewAssignment.DateStarted >= DateTime.Now.AddDays(-1)),
                          Restrictions.And(
                              Restrictions.Where<WorkOrder>((x) => x.DateCompleted <= search.DateCompleted.End),
                              Restrictions.Where<WorkOrder>((x) => x.DateCompleted >= search.DateCompleted.Start))))
                     .TransformUsing(Transformers.DistinctRootEntity);
            }
            else
            {
                query.Where(x =>
                    x.DateCompleted <= search.DateCompleted.End && x.DateCompleted >= search.DateCompleted.Start);
            }

            return Search(search, query);
        }

        public IEnumerable<MainBreakReport> GetMainBreakServiceLineReport(ISearchMainBreakReport search)
        {
            // get the data
            var report = new List<MainBreakReport>();
            var results = GetCompletedWorkOrdersForMainBreaksOnServices(search).ToList();
            var states = results.Select(x => x.State).Distinct().OrderBy(x => x).ToList();

            // used later
            var operatingCenterRepo = _container.GetInstance<IRepository<OperatingCenter>>();
            var opCenters = results.Select(x => x.OperatingCenterCode).ToList();

            // pivot the results

            var totalJan = 0;
            var totalFeb = 0;
            var totalMar = 0;
            var totalApr = 0;
            var totalMay = 0;
            var totalJun = 0;
            var totalJul = 0;
            var totalAug = 0;
            var totalSep = 0;
            var totalOct = 0;
            var totalNov = 0;
            var totalDec = 0;
            var stateTotalJan = 0;
            var stateTotalFeb = 0;
            var stateTotalMar = 0;
            var stateTotalApr = 0;
            var stateTotalMay = 0;
            var stateTotalJun = 0;
            var stateTotalJul = 0;
            var stateTotalAug = 0;
            var stateTotalSep = 0;
            var stateTotalOct = 0;
            var stateTotalNov = 0;
            var stateTotalDec = 0;
            var opCntrTotalJan = 0;
            var opCntrTotalFeb = 0;
            var opCntrTotalMar = 0;
            var opCntrTotalApr = 0;
            var opCntrTotalMay = 0;
            var opCntrTotalJun = 0;
            var opCntrTotalJul = 0;
            var opCntrTotalAug = 0;
            var opCntrTotalSep = 0;
            var opCntrTotalOct = 0;
            var opCntrTotalNov = 0;
            var opCntrTotalDec = 0;
            var workDescriptionTotalJan = 0;
            var workDescriptionTotalFeb = 0;
            var workDescriptionTotalMar = 0;
            var workDescriptionTotalApr = 0;
            var workDescriptionTotalMay = 0;
            var workDescriptionTotalJun = 0;
            var workDescriptionTotalJul = 0;
            var workDescriptionTotalAug = 0;
            var workDescriptionTotalSep = 0;
            var workDescriptionTotalOct = 0;
            var workDescriptionTotalNov = 0;
            var workDescriptionTotalDec = 0;

            foreach (var state in states)
            {
                stateTotalJan = 0;
                stateTotalFeb = 0;
                stateTotalMar = 0;
                stateTotalApr = 0;
                stateTotalMay = 0;
                stateTotalJun = 0;
                stateTotalJul = 0;
                stateTotalAug = 0;
                stateTotalSep = 0;
                stateTotalOct = 0;
                stateTotalNov = 0;
                stateTotalDec = 0;
                var operatingCenters = results.Where(x => x.State == state).Select(x => x.OperatingCenter).Distinct()
                                              .OrderBy(x => x).ToList();

                foreach (var oc in operatingCenters)
                {
                    opCntrTotalJan = 0;
                    opCntrTotalFeb = 0;
                    opCntrTotalMar = 0;
                    opCntrTotalApr = 0;
                    opCntrTotalMay = 0;
                    opCntrTotalJun = 0;
                    opCntrTotalJul = 0;
                    opCntrTotalAug = 0;
                    opCntrTotalSep = 0;
                    opCntrTotalOct = 0;
                    opCntrTotalNov = 0;
                    opCntrTotalDec = 0;
                    var workDescriptions = results.Where(x => x.OperatingCenter == oc && x.State == state)
                                                  .Select(x => x.WorkDescription).Distinct().OrderBy(x => x);

                    foreach (var wd in workDescriptions)
                    {
                        var rowResult = results.Where(x =>
                            x.OperatingCenter == oc && x.WorkDescription == wd && x.State == state);
                        if (rowResult.Any())
                        {
                            Func<int, int> getCount = (month) => {
                                var first = rowResult.FirstOrDefault(x => x.MonthCompleted == month);
                                return (first != null) ? (int)first.MonthTotal : 0;
                            };
                            workDescriptionTotalJan = getCount(1);
                            workDescriptionTotalFeb = getCount(2);
                            workDescriptionTotalMar = getCount(3);
                            workDescriptionTotalApr = getCount(4);
                            workDescriptionTotalMay = getCount(5);
                            workDescriptionTotalJun = getCount(6);
                            workDescriptionTotalJul = getCount(7);
                            workDescriptionTotalAug = getCount(8);
                            workDescriptionTotalSep = getCount(9);
                            workDescriptionTotalOct = getCount(10);
                            workDescriptionTotalNov = getCount(11);
                            workDescriptionTotalDec = getCount(12);

                            stateTotalJan += workDescriptionTotalJan;
                            stateTotalFeb += workDescriptionTotalFeb;
                            stateTotalMar += workDescriptionTotalMar;
                            stateTotalApr += workDescriptionTotalApr;
                            stateTotalMay += workDescriptionTotalMay;
                            stateTotalJun += workDescriptionTotalJun;
                            stateTotalJul += workDescriptionTotalJul;
                            stateTotalAug += workDescriptionTotalAug;
                            stateTotalSep += workDescriptionTotalSep;
                            stateTotalOct += workDescriptionTotalOct;
                            stateTotalNov += workDescriptionTotalNov;
                            stateTotalDec += workDescriptionTotalDec;

                            totalJan += workDescriptionTotalJan;
                            totalFeb += workDescriptionTotalFeb;
                            totalMar += workDescriptionTotalMar;
                            totalApr += workDescriptionTotalApr;
                            totalMay += workDescriptionTotalMay;
                            totalJun += workDescriptionTotalJun;
                            totalJul += workDescriptionTotalJul;
                            totalAug += workDescriptionTotalAug;
                            totalSep += workDescriptionTotalSep;
                            totalOct += workDescriptionTotalOct;
                            totalNov += workDescriptionTotalNov;
                            totalDec += workDescriptionTotalDec;

                            opCntrTotalJan += workDescriptionTotalJan;
                            opCntrTotalFeb += workDescriptionTotalFeb;
                            opCntrTotalMar += workDescriptionTotalMar;
                            opCntrTotalApr += workDescriptionTotalApr;
                            opCntrTotalMay += workDescriptionTotalMay;
                            opCntrTotalJun += workDescriptionTotalJun;
                            opCntrTotalJul += workDescriptionTotalJul;
                            opCntrTotalAug += workDescriptionTotalAug;
                            opCntrTotalSep += workDescriptionTotalSep;
                            opCntrTotalOct += workDescriptionTotalOct;
                            opCntrTotalNov += workDescriptionTotalNov;
                            opCntrTotalDec += workDescriptionTotalDec;

                            report.Add(new MainBreakReport {
                                OperatingCenter = oc,
                                WorkDescription = wd,
                                Year = search.Year.Value,
                                Jan = workDescriptionTotalJan,
                                Feb = workDescriptionTotalFeb,
                                Mar = workDescriptionTotalMar,
                                Apr = workDescriptionTotalApr,
                                May = workDescriptionTotalMay,
                                Jun = workDescriptionTotalJun,
                                Jul = workDescriptionTotalJul,
                                Aug = workDescriptionTotalAug,
                                Sep = workDescriptionTotalSep,
                                Oct = workDescriptionTotalOct,
                                Nov = workDescriptionTotalNov,
                                Dec = workDescriptionTotalDec,
                                State = state
                            });
                        }
                    }

                    //operating center row
                    report.Add(new MainBreakReport {
                        OperatingCenter = oc,
                        State = state,
                        Jan = opCntrTotalJan,
                        Feb = opCntrTotalFeb,
                        Mar = opCntrTotalMar,
                        Apr = opCntrTotalApr,
                        May = opCntrTotalMay,
                        Jun = opCntrTotalJun,
                        Jul = opCntrTotalJul,
                        Aug = opCntrTotalAug,
                        Sep = opCntrTotalSep,
                        Oct = opCntrTotalOct,
                        Nov = opCntrTotalNov,
                        Dec = opCntrTotalDec,
                    });
                }

                if (search.OperatingCenter == null || !search.OperatingCenter.Any())
                {
                    foreach (var opCenter in operatingCenterRepo
                                            .Where(oc =>
                                                 oc.State.Abbreviation == state &&
                                                 !opCenters.Contains(oc.OperatingCenterCode)).Select(oc =>
                                                 oc.OperatingCenterCode + " - " + oc.OperatingCenterName))
                    {
                        report.Add(new MainBreakReport {
                            OperatingCenter = opCenter,
                            State = state,
                            Jan = 0,
                            Feb = 0,
                            Mar = 0,
                            Apr = 0,
                            May = 0,
                            Jun = 0,
                            Jul = 0,
                            Aug = 0,
                            Sep = 0,
                            Oct = 0,
                            Nov = 0,
                            Dec = 0
                        });
                    }
                }

                // state row
                report.Add(new MainBreakReport {
                    State = state + " Total",
                    Jan = stateTotalJan,
                    Feb = stateTotalFeb,
                    Mar = stateTotalMar,
                    Apr = stateTotalApr,
                    May = stateTotalMay,
                    Jun = stateTotalJun,
                    Jul = stateTotalJul,
                    Aug = stateTotalAug,
                    Sep = stateTotalSep,
                    Oct = stateTotalOct,
                    Nov = stateTotalNov,
                    Dec = stateTotalDec
                });
            }

            report.Add(new MainBreakReport {
                State = "Total",
                Jan = totalJan,
                Feb = totalFeb,
                Mar = totalMar,
                Apr = totalApr,
                May = totalMay,
                Jun = totalJun,
                Jul = totalJul,
                Aug = totalAug,
                Sep = totalSep,
                Oct = totalOct,
                Nov = totalNov,
                Dec = totalDec
            });

            // return the results
            return report;
        }

        public IEnumerable<MainBreakReportItem> GetCompletedWorkOrdersForMainBreaksOnServices(
            ISearchMainBreakReport search)
        {
            var workDescriptions = WorkDescription.GetMainBreakWorkDescriptions();
            var qry = Where(wo => wo.YearCompleted == search.Year);
            qry = qry.Where(wo => workDescriptions.Contains(wo.WorkDescription.Id));
            qry = qry.Where(wo => wo.CancelledAt == null);
            qry = qry.Where(wo => wo.DateCompleted != null);
            if (search.IsContractedOperations.HasValue)
                qry = qry.Where(wo => wo.OperatingCenter.IsContractedOperations == search.IsContractedOperations);

            if (search.OperatingCenter != null && search.OperatingCenter.Any())
            {
                qry = qry.Where(wo => search.OperatingCenter.Contains(wo.OperatingCenter.Id));
            }

            if (search.State.HasValue)
            {
                qry = qry.Where(wo => search.State == wo.OperatingCenter.State.Id);
            }

            return qry.GroupBy(wo => new {
                wo.WorkDescription.Description,
                wo.OperatingCenter.OperatingCenterCode,
                wo.OperatingCenter.OperatingCenterName,
                wo.DateCompleted.Value.Month,
                wo.DateCompleted.Value.Year,
                wo.OperatingCenter.State.Abbreviation
            }).Select(group => new MainBreakReportItem {
                MonthCompleted = group.Key.Month,
                MonthTotal = group.Count(),
                WorkDescription = group.Key.Description,
                OperatingCenterCode = group.Key.OperatingCenterCode,
                OperatingCenterName = group.Key.OperatingCenterName,
                Year = group.Key.Year,
                State = group.Key.Abbreviation
            });
        }

        public IEnumerable<CompletedWorkOrderWithPreJobSafetyBriefReportItem>
            GetCompletedWorkOrderPreJobSafetyBriefCounts(ISearchCompletedWorkOrdersWithPreJobSafetyBriefs search)
        {
            // This needs to be split into two queries because there is no way to make it work
            // with one single query. Attempting to get both the total work order count and the
            // total count of work orders with job site check lists results in a query that doesn't
            // include the JSCL at all. Attempting to group by work orders JobSiteCheckLists.Any()
            // results in a "A recognition error occurred" because NHibernate doesn't know how
            // to do group bys with booleans.

            Func<IQueryable<WorkOrder>> getBaseLinq = () => {
                var query = Linq.Where(wo => wo.CancelledAt == null);

                // TODO: This mapping is horrible and needs to be located in one spot somehow.
                switch (search.DateCompleted.Operator)
                {
                    case RangeOperator.Between:
                        // NOTE: This includes the whole of the end day.
                        query = query.Where(x =>
                            search.DateCompleted.Start.Value.BeginningOfDay() <= x.DateCompleted &&
                            x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                        break;

                    case RangeOperator.Equal:
                        query = query.Where(x =>
                            search.DateCompleted.End.Value.BeginningOfDay() <= x.DateCompleted &&
                            x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                        break;

                    case RangeOperator.GreaterThan:
                        query = query.Where(x => x.DateCompleted > search.DateCompleted.End.Value.EndOfDay());
                        break;

                    case RangeOperator.GreaterThanOrEqualTo:
                        query = query.Where(x => x.DateCompleted >= search.DateCompleted.End.Value.BeginningOfDay());
                        break;

                    case RangeOperator.LessThan:
                        query = query.Where(x => x.DateCompleted < search.DateCompleted.End.Value.BeginningOfDay());
                        break;

                    case RangeOperator.LessThanOrEqualTo:
                        query = query.Where(x => x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                        break;

                    default:
                        throw new NotSupportedException(search.DateCompleted.Operator.ToString());
                }

                // NOTE: All of this is tested in the repository test. If additional filtering gets added,
                // make sure additional tests get created too.

                if (search.OperatingCenter != null && search.OperatingCenter.Any())
                {
                    query = query.Where(x => search.OperatingCenter.Contains(x.OperatingCenter.Id));
                }

                if (search.State.HasValue)
                {
                    query = query.Where(x => x.Town.County.State.Id == search.State.Value);
                }

                if (search.WorkDescription != null && search.WorkDescription.Any())
                {
                    query = query.Where(x => search.WorkDescription.Contains(x.WorkDescription.Id));
                }

                if (search.IsAssignedContractor.HasValue)
                {
                    if (search.IsAssignedContractor.Value)
                    {
                        query = query.Where(x => x.AssignedContractor != null);
                    }
                    else
                    {
                        query = query.Where(x => x.AssignedContractor == null);
                    }
                }

                return query;
            };

            Func<IQueryable<WorkOrder>, List<CompletedWorkOrderWithPreJobSafetyBriefReportItem>> getFinalResult =
                (query) => {
                    return (from wo in query
                            group wo by new {
                                State = wo.Town.County.State.Abbreviation,
                                OperatingCenter = wo.OperatingCenter.OperatingCenterCode,
                                OperatingCenterId = wo.OperatingCenter.Id,
                                WorkDescription = wo.WorkDescription.Description,
                                WorkDescriptionId = wo.WorkDescription.Id,
                                // This is not supported by NHibernate.
                                // HasJobSiteCheckLists = wo.JobSiteCheckLists.Any() 
                            }
                            into groupedWo
                            select new CompletedWorkOrderWithPreJobSafetyBriefReportItem {
                                State = groupedWo.Key.State,
                                OperatingCenter = groupedWo.Key.OperatingCenter,
                                OperatingCenterId = groupedWo.Key.OperatingCenterId,
                                WorkDescription = groupedWo.Key.WorkDescription,
                                WorkDescriptionId = groupedWo.Key.WorkDescriptionId,
                                WorkOrderCount = groupedWo.Count(),
                                // This generates the same query as above. It doesn't include the JSCL check. Useless.
                                //WorkOrdersWithJobSiteCheckListCount = groupedWo.Count(x => x.JobSiteCheckLists.Any()) 
                            }).ToList();
                };

            var resultsForAllWorkOrders = getFinalResult(getBaseLinq());
            var workOrdersWithJSCL = getFinalResult((from wo in getBaseLinq()
                                                     where wo.JobSiteCheckLists.Any(x =>
                                                         x.AnyPotentialWeatherHazards != null)
                                                     select wo));

            // Update the list of all work orders to include the JSCL count.
            foreach (var record in workOrdersWithJSCL)
            {
                var matchingReturnRecord = resultsForAllWorkOrders.Single(x =>
                    x.State == record.State && x.OperatingCenter == record.OperatingCenter &&
                    x.WorkDescription == record.WorkDescription);
                matchingReturnRecord.WorkOrdersWithPreJobSafetyBriefCount = record.WorkOrderCount;
            }

            // Users can't sort these results. It's been requested to use this default sorting.
            var orderedResults = resultsForAllWorkOrders.OrderBy(x => x.State)
                                                        .ThenBy(x => x.OperatingCenter)
                                                        .ThenBy(x => x.WorkDescription)
                                                        .ToList();

            search.Results = orderedResults;
            search.Count = orderedResults.Count;
            return search.Results;
        }

        public IEnumerable<CompletedWorkOrderWithJobSiteCheckListReportItem>
            GetCompletedWorkOrderJobSiteCheckListCounts(ISearchCompletedWorkOrdersWithJobSiteCheckLists search)
        {
            var query = GetBaseLinqCompletedWorkOrdersWithJobSiteCheckLists(search);
            var resultsForAllWorkOrders = GetFinalResultCompletedWorkOrdersWithJobSiteCheckLists(query);
            var workOrdersWithJSCL = GetFinalResultCompletedWorkOrdersWithJobSiteCheckLists(
                from wo in query where wo.JobSiteCheckLists.Any(x => x.HasExcavation != null) select wo);

            // Update the list of all work orders to include the JSCL count.
            foreach (var record in workOrdersWithJSCL)
            {
                var matchingReturnRecord = resultsForAllWorkOrders.Single(x =>
                    x.State == record.State && x.OperatingCenter == record.OperatingCenter &&
                    x.WorkDescription == record.WorkDescription);
                matchingReturnRecord.WorkOrdersWithJobSiteCheckListCount = record.WorkOrderCount;
            }

            // Users can't sort these results. It's been requested to use this default sorting.
            var orderedResults = resultsForAllWorkOrders.OrderBy(x => x.State)
                                                        .ThenBy(x => x.OperatingCenter)
                                                        .ThenBy(x => x.WorkDescription)
                                                        .ToList();

            search.Results = orderedResults;
            search.Count = orderedResults.Count;
            return search.Results;
        }

        public IEnumerable<CompletedWorkOrderWithMaterialReportItem> GetCompletedWorkOrderMaterialCounts(
            ISearchCompletedWorkOrdersWithMaterial search)
        {
            // This needs to be split into two queries because there is no way to make it work
            // with one single query. Attempting to get both the total work order count and the
            // total count of work orders with job site check lists results in a query that doesn't
            // include the JSCL at all. Attempting to group by work orders Material.Any()
            // results in a "A recognition error occurred" because NHibernate doesn't know how
            // to do group bys with booleans.

            Func<IQueryable<WorkOrder>> getBaseLinq = () => {
                var query = Linq.Where(wo => wo.CancelledAt == null);

                // TODO: This mapping is horrible and needs to be located in one spot somehow.
                switch (search.DateCompleted.Operator)
                {
                    case RangeOperator.Between:
                        // NOTE: This includes the whole of the end day.
                        query = query.Where(x =>
                            search.DateCompleted.Start.Value.BeginningOfDay() <= x.DateCompleted &&
                            x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                        break;

                    case RangeOperator.Equal:
                        query = query.Where(x =>
                            search.DateCompleted.End.Value.BeginningOfDay() <= x.DateCompleted &&
                            x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                        break;

                    case RangeOperator.GreaterThan:
                        query = query.Where(x => x.DateCompleted > search.DateCompleted.End.Value.EndOfDay());
                        break;

                    case RangeOperator.GreaterThanOrEqualTo:
                        query = query.Where(x => x.DateCompleted >= search.DateCompleted.End.Value.BeginningOfDay());
                        break;

                    case RangeOperator.LessThan:
                        query = query.Where(x => x.DateCompleted < search.DateCompleted.End.Value.BeginningOfDay());
                        break;

                    case RangeOperator.LessThanOrEqualTo:
                        query = query.Where(x => x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                        break;

                    default:
                        throw new NotSupportedException(search.DateCompleted.Operator.ToString());
                }

                if (search.OperatingCenter != null && search.OperatingCenter.Any())
                {
                    query = query.Where(x => search.OperatingCenter.Contains(x.OperatingCenter.Id));
                }

                if (search.State.HasValue)
                {
                    query = query.Where(x => x.Town.County.State.Id == search.State.Value);
                }

                if (search.WorkDescription != null && search.WorkDescription.Any())
                {
                    query = query.Where(x => search.WorkDescription.Contains(x.WorkDescription.Id));
                }

                if (search.IsAssignedContractor.HasValue)
                {
                    query = search.IsAssignedContractor.Value
                        ? query.Where(x => x.AssignedContractor != null)
                        : query.Where(x => x.AssignedContractor == null);
                }

                return query;
            };

            Func<IQueryable<WorkOrder>, List<CompletedWorkOrderWithMaterialReportItem>> getFinalResult = (query) => {
                return (from wo in query
                        group wo by new {
                            State = wo.Town.County.State.Abbreviation,
                            OperatingCenter = wo.OperatingCenter.OperatingCenterCode,
                            OperatingCenterId = wo.OperatingCenter.Id,
                            WorkDescriptionId = wo.WorkDescription.Id,
                            WorkDescription = wo.WorkDescription.Description,
                            // This is not supported by NHibernate.
                            // HasMaterial = wo.Material.Any() 
                        }
                        into groupedWo
                        select new CompletedWorkOrderWithMaterialReportItem {
                            State = groupedWo.Key.State,
                            OperatingCenter = groupedWo.Key.OperatingCenter,
                            OperatingCenterId = groupedWo.Key.OperatingCenterId,
                            WorkDescription = groupedWo.Key.WorkDescription,
                            WorkDescriptionId = groupedWo.Key.WorkDescriptionId,
                            WorkOrderCount = groupedWo.Count(),
                            // This generates the same query as above. It doesn't include the JSCL check. Useless.
                            //WorkOrdersWithMaterialCount = groupedWo.Count(x => x.Material.Any()) 
                        }).ToList();
            };

            var resultsForAllWorkOrders = getFinalResult(getBaseLinq());
            var workOrdersWithMaterial =
                getFinalResult((from wo in getBaseLinq() where wo.MaterialsUsed.Any() select wo));

            // Update the list of all work orders to include the JSCL count.
            foreach (var record in workOrdersWithMaterial)
            {
                var matchingReturnRecord = resultsForAllWorkOrders.Single(x =>
                    x.State == record.State && x.OperatingCenter == record.OperatingCenter &&
                    x.WorkDescription == record.WorkDescription);
                matchingReturnRecord.WorkOrdersWithMaterialCount = record.WorkOrderCount;
            }

            // Users can't sort these results. It's been requested to use this default sorting.
            var orderedResults = resultsForAllWorkOrders.OrderBy(x => x.State)
                                                        .ThenBy(x => x.OperatingCenter)
                                                        .ThenBy(x => x.WorkDescription)
                                                        .ToList();

            search.Results = orderedResults;
            search.Count = orderedResults.Count;
            return search.Results;
        }

        public IEnumerable<CompletedWorkOrderWithMarkoutReportItem> GetCompletedWorkOrderMarkoutCounts(
            ISearchCompletedWorkOrdersWithMarkout search)
        {
            Func<IQueryable<WorkOrder>> getBaseLinq = () => {
                var query = Linq.Where(wo => wo.CancelledAt == null);

                #region Search Criteria

                // TODO: This mapping is horrible and needs to be located in one spot somehow.
                switch (search.DateCompleted.Operator)
                {
                    case RangeOperator.Between:
                        // NOTE: This includes the whole of the end day.
                        query = query.Where(x =>
                            search.DateCompleted.Start.Value.BeginningOfDay() <= x.DateCompleted &&
                            x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                        break;

                    case RangeOperator.Equal:
                        query = query.Where(x =>
                            search.DateCompleted.End.Value.BeginningOfDay() <= x.DateCompleted &&
                            x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                        break;

                    case RangeOperator.GreaterThan:
                        query = query.Where(x => x.DateCompleted > search.DateCompleted.End.Value.EndOfDay());
                        break;

                    case RangeOperator.GreaterThanOrEqualTo:
                        query = query.Where(x => x.DateCompleted >= search.DateCompleted.End.Value.BeginningOfDay());
                        break;

                    case RangeOperator.LessThan:
                        query = query.Where(x => x.DateCompleted < search.DateCompleted.End.Value.BeginningOfDay());
                        break;

                    case RangeOperator.LessThanOrEqualTo:
                        query = query.Where(x => x.DateCompleted <= search.DateCompleted.End.Value.EndOfDay());
                        break;

                    default:
                        throw new NotSupportedException(search.DateCompleted.Operator.ToString());
                }

                if (search.OperatingCenter != null && search.OperatingCenter.Any())
                {
                    query = query.Where(x => search.OperatingCenter.Contains(x.OperatingCenter.Id));
                }

                if (search.State.HasValue)
                {
                    query = query.Where(x => x.Town.County.State.Id == search.State.Value);
                }

                if (search.WorkDescription != null && search.WorkDescription.Any())
                {
                    query = query.Where(x => search.WorkDescription.Contains(x.WorkDescription.Id));
                }

                if (search.IsAssignedContractor.HasValue)
                {
                    query = search.IsAssignedContractor.Value
                        ? query.Where(x => x.AssignedContractor != null)
                        : query.Where(x => x.AssignedContractor == null);
                }

                return query;
            };

            #endregion

            Func<IQueryable<WorkOrder>, List<CompletedWorkOrderWithMarkoutReportItem>> getFinalResult = (query) => {
                var result = (from wo in query
                              group wo by new {
                                  State = wo.Town.County.State.Abbreviation,
                                  OperatingCenter = wo.OperatingCenter.OperatingCenterCode,
                                  OperatingCenterId = wo.OperatingCenter.Id,
                                  WorkDescription = wo.WorkDescription.Description,
                                  WorkDescriptionId = wo.WorkDescription.Id,
                                  MarkoutRequirementId = wo.MarkoutRequirement.Id
                              }
                              into groupedWo
                              select new CompletedWorkOrderWithMarkoutReportItem {
                                  State = groupedWo.Key.State,
                                  OperatingCenter = groupedWo.Key.OperatingCenter,
                                  OperatingCenterId = groupedWo.Key.OperatingCenterId,
                                  WorkDescription = groupedWo.Key.WorkDescription,
                                  WorkDescriptionId = groupedWo.Key.WorkDescriptionId,
                                  WorkOrderCount = groupedWo.Count(),
                                  MarkoutRequirementId = groupedWo.Key.MarkoutRequirementId
                                  // This generates the same query as above. 
                                  //WorkOrdersWithMarkoutCount = groupedWo.Count(x => x.Markout.Any()) 
                              }).ToList();
                // This part needs to be done in-memory because Nhibernate refuses to do
                // do group.Count(x => x.Anything), it ignores the additional query.
                return (from wo in result
                        group wo by new {
                            State = wo.State,
                            OperatingCenter = wo.OperatingCenter,
                            OperatingCenterId = wo.OperatingCenterId,
                            WorkDescription = wo.WorkDescription,
                            WorkDescriptionId = wo.WorkDescriptionId,
                        }
                        into groupedWo
                        select new CompletedWorkOrderWithMarkoutReportItem {
                            State = groupedWo.Key.State,
                            OperatingCenter = groupedWo.Key.OperatingCenter,
                            OperatingCenterId = groupedWo.Key.OperatingCenterId,
                            WorkDescription = groupedWo.Key.WorkDescription,
                            WorkDescriptionId = groupedWo.Key.WorkDescriptionId,
                            WorkOrderCount = groupedWo.Sum(x => x.WorkOrderCount),
                            MarkoutNoneCount = groupedWo.Sum(x => x.MarkoutRequirementId == 1 ? x.WorkOrderCount : 0),
                            MarkoutRoutineCount =
                                groupedWo.Sum(x => x.MarkoutRequirementId == 2 ? x.WorkOrderCount : 0),
                            MarkoutEmergencyCount =
                                groupedWo.Sum(x => x.MarkoutRequirementId == 3 ? x.WorkOrderCount : 0),
                            // This generates the same query as above. It doesn't include the JSCL check. Useless.
                            //WorkOrdersWithMarkoutCount = groupedWo.Count(x => x.Markout.Any()) 
                        }).ToList();
            };
            // Update the list of all work orders to include the count.
            var resultsForAllWorkOrders = getFinalResult(getBaseLinq());

            // Users can't sort these results. It's been requested to use this default sorting.
            var orderedResults = resultsForAllWorkOrders.OrderBy(x => x.State)
                                                        .ThenBy(x => x.OperatingCenter)
                                                        .ThenBy(x => x.WorkDescription)
                                                        .ToList();

            search.Results = orderedResults;
            search.Count = orderedResults.Count;
            return search.Results;
        }

        public WorkOrder FindPrePlanningOrder(int id)
        {
            return PrePlanningOrders.Add(base.GetIdEqCriterion(id)).UniqueResult<WorkOrder>();
        }

        public IEnumerable<WorkOrder> GetPrePlanningWorkOrders(ISearchSet<WorkOrder> search)
        {
            return Search(search, PrePlanningOrders);
        }

        public WorkOrder FindPlanningOrder(int id)
        {
            return PlanningOrders.Add(base.GetIdEqCriterion(id)).UniqueResult<WorkOrder>();
        }

        public IEnumerable<WorkOrder> GetPlanningWorkOrders(ISearchSet<WorkOrder> search)
        {
            return Search(search, PlanningOrders);
        }

        private ICriterion GetSupervisorApprovalCriterion()
        {
            return Restrictions.Conjunction()
                               .Add(NotCancelled)
                               .Add(Restrictions.IsNotNull("DateCompleted"))
                               .Add(SAPValidCriteria);
        }
        
        public WorkOrder FindSchedulingOrder(int id)
        {
            return SchedulingOrders.Add(base.GetIdEqCriterion(id)).UniqueResult<WorkOrder>();
        }

        public IEnumerable<WorkOrder> GetSchedulingWorkOrders(ISearchSet<WorkOrder> search)
        {
            return Search(search, SchedulingOrders);
        }
        
        public WorkOrder FindSupervisorApprovalWorkOrder(int id)
        {
            return SupervisorApprovalOrders.Add(base.GetIdEqCriterion(id)).UniqueResult<WorkOrder>();
        }

        public IEnumerable<WorkOrder> GetFinalizingWorkOrders(ISearchSet<WorkOrder> search)
        {
            return Search(search, FinalizationOrders);
        }

        public IEnumerable<WorkOrder> GetSupervisorApprovalWorkOrders(ISearchSet<WorkOrder> search)
        {
            return Search(search, SupervisorApprovalOrders);
        }

        public WorkOrder FindStockToIssueWorkOrder(int id)
        {
            return StockToIssueOrders.Add(base.GetIdEqCriterion(id)).UniqueResult<WorkOrder>();
        }

        public IEnumerable<WorkOrder> GetStockToIssueWorkOrders(ISearchSet<WorkOrder> search)
        {
            return Search(search, StockToIssueOrders);
        }

        public WorkOrder FindFinalizationOrder(int id)
        {
            return FinalizationOrders.Add(base.GetIdEqCriterion(id)).UniqueResult<WorkOrder>();
        }

        public IEnumerable<WorkOrder> GetSopProcessingWorkOrders(ISearchSet<WorkOrder> search)
        {
            return Search(search, SopProcessingOrders);
        }

        public WorkOrder FindSopProcessingOrder(int id)
        {
            return SopProcessingOrders.Add(base.GetIdEqCriterion(id)).UniqueResult<WorkOrder>();
        }

        public IEnumerable<WorkOrder> GetIncompleteWorkOrdersByWorkDescriptionId(ISearchIncompleteWorkOrder search)
        {
            var query = Linq.Where(wo => wo.CancelledAt == null 
                                         && wo.DateCompleted == null);

            var resul = query.ToList();

            return search.WorkDescription.HasValue
                ? query.Where(x => search.WorkDescription != null 
                                   && search.WorkDescription == x.WorkDescription.Id).ToList()
                : query.ToList();
        }

        #endregion
    }

    public static class IWorkOrderRepositoryExtensions
    {
        #region Exposed Methods

        public static IQueryable<WorkOrder> GetLostWaterInPastDayImpl(this IRepository<WorkOrder> that,
            IDateTimeProvider dateTimeProvider)
        {
            var yesterday = dateTimeProvider.GetCurrentDate().AddDays(-1).Date;
            return
                that.Where(
                    wo => wo.DateCompleted.HasValue && wo.DateCompleted < yesterday.AddDays(1) &&
                          wo.DateCompleted >= yesterday &&
                          wo.LostWater.HasValue && wo.LostWater.Value > 0
                );
        }

        public static IEnumerable<WorkOrder> GetWorkOrdersWithSapRetryIssuesImpl(this IRepository<WorkOrder> that)
        {
            return
                that.Where(
                    x =>
                        x.SAPErrorCode != null && x.SAPErrorCode != "" && x.SAPErrorCode.StartsWith("RETRY") &&
                        x.SAPWorkOrderStep != null);
        }

        public static void GetWaterLossReport(this IRepository<WorkOrder> that, ISearchWaterLoss search)
        {
            search.EnablePaging = false;
            IQueryable<WorkOrder> query = that.Where(wo =>
                wo.DateCompleted != null &&
                wo.LostWater != null &&
                wo.LostWater > 0);

            if (search.OperatingCenter != null && search.OperatingCenter.Any())
            {
                query = query.Where(wo => search.OperatingCenter.Contains(wo.OperatingCenter.Id));
            }

            switch (search.Date.Operator)
            {
                case RangeOperator.Between:
                    query = query.Where(wo =>
                        wo.DateCompleted >= search.Date.Start.Value.BeginningOfDay() &&
                        wo.DateCompleted <= search.Date.End.Value.EndOfDay());
                    break;
                case RangeOperator.Equal:
                    query = query.Where(wo => wo.DateCompleted.Value.Date == search.Date.End.Value.Date);
                    break;
                case RangeOperator.GreaterThan:
                    query = query.Where(wo => wo.DateCompleted > search.Date.End);
                    break;
                case RangeOperator.GreaterThanOrEqualTo:
                    query = query.Where(wo => wo.DateCompleted >= search.Date.End);
                    break;
                case RangeOperator.LessThan:
                    query = query.Where(wo => wo.DateCompleted < search.Date.End);
                    break;
                case RangeOperator.LessThanOrEqualTo:
                    query = query.Where(wo => wo.DateCompleted <= search.Date.End);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            var results = query
                         .GroupBy(
                              wo => new {
                                  wo.OperatingCenter.OperatingCenterCode,
                                  wo.OperatingCenter.OperatingCenterName,
                                  wo.DateCompleted.Value.Year,
                                  wo.DateCompleted.Value.Month,
                                  wo.WorkDescription.Description,
                                  wo.BusinessUnit,
                                  OperatingCenter = wo.OperatingCenter.Id,
                                  WorkDescription = wo.WorkDescription.Id
                              },
                              (key, group) => new WaterLossSearchResultViewModel {
                                  OperatingCenterCodeName = key.OperatingCenterCode + " - " + key.OperatingCenterName,
                                  Year = key.Year,
                                  Month = key.Month,
                                  WorkDescriptionDescription = key.Description,
                                  BusinessUnit = key.BusinessUnit,
                                  TotalGallons = group.Sum(wo => wo.LostWater.Value),
                                  OperatingCenter = key.OperatingCenter,
                                  WorkDescription = key.WorkDescription,
                                  WorkOrderCount = group.Count()
                              })
                         .OrderBy(x => x.OperatingCenterCodeName)
                         .ThenBy(x => x.Year)
                         .ThenBy(x => x.Month)
                         .ThenBy(x => x.WorkDescriptionDescription)
                         .ToList();

            search.Count = results.Count();
            search.Results = results;
        }

        public static bool PlanningWorkOrderHasValidMarkoutForStartingCrewAssignment(this IWorkOrderRepositoryBase that,
            int workOrderId)
        {
            var woCount = that.PlanningOrders
                              .Add(Restrictions.Eq("Id", workOrderId))
                              .Add(Restrictions.And(
                                   Restrictions.Eq("MarkoutRequirement.Id", (int)MarkoutRequirement.Indices.ROUTINE),
                                   Restrictions.Not(Subqueries.Exists(that.CurrentValidMarkoutIds))))
                              .SetProjection(Projections.RowCount())
                              .UniqueResult<int>();

            return (woCount == 0);
        }

        public static bool PlanningWorkOrderHasValidStreetOpeningPermitForStartingCrewAssignment(
            this IWorkOrderRepositoryBase that, int workOrderId)
        {
            // Check that if a sop is required, it is currently valid
            var woCount = that.PlanningOrders
                              .Add(Restrictions.Eq("Id", workOrderId))
                              .Add(
                                   Restrictions.And(
                                       that.StreetOpeningPermitRequired,
                                       Restrictions.Not(Restrictions.Eq("Priority.Id",
                                           (int)WorkOrderPriorityEnum.Emergency))))
                              .Add(Restrictions.Not(Subqueries.Exists(that.ValidPermitIds)))
                              .SetProjection(Projections.RowCount())
                              .UniqueResult<int>();
            return (woCount == 0);
        }

        public static IEnumerable<WorkOrder> GetByServiceId(this IRepository<WorkOrder> that, int serviceId)
        {
            return
                that.Where(x => x.Service.Id == serviceId)
                    .Select(wo => new WorkOrder {
                         Id = wo.Id,
                         WorkDescription = new WorkDescription { Description = wo.WorkDescription.Description },
                         DateCompleted = wo.DateCompleted,
                         AssignedContractor = wo.AssignedContractor != null ? new Contractor { Id = wo.AssignedContractor.Id } : null
                     });
        }

        #endregion
    }

    public interface IWorkOrderRepository : IWorkOrderRepositoryBase
    {
        #region Abstract Methods

        IEnumerable<WorkOrder> FindByPartialWorkOrderIDMatch(string partialWorkOrderID);

        WorkOrder FindBySAPWorkOrderNumber(long sap);

        WorkOrder FindByOriginalWorkOrderNumber(int originalWorkOrderNumber);

        WorkOrder FindSchedulingOrder(int id);

        IEnumerable<WorkOrder> GetByTownId(int townId);

        IEnumerable<WorkOrder> GetByTownIdForServices(int townId);

        IEnumerable<WorkOrder> GetByTownIdForMainBreaks(int townId);

        IEnumerable<int> GetDistinctYearsCompleted();

        IEnumerable<WorkOrder> GetFinalizingWorkOrders(ISearchSet<WorkOrder> search);

        IEnumerable<WorkOrder> GetSopProcessingWorkOrders(ISearchSet<WorkOrder> search);

        IEnumerable<WorkOrder> GetLostWaterInPastDay();

        IEnumerable<WorkOrder> GetMainBreakRepairsForGIS(ISearchSet<WorkOrder> search);

        IEnumerable<WorkOrder> GetMainBreakRepairsForGIS(ISearchMainBreakRepairsForGIS search);

        IEnumerable<MainBreakReport> GetMainBreakServiceLineReport(ISearchMainBreakReport search);

        ICriterion GetSAPNotificationsCriterion();

        IEnumerable<WorkOrder> GetSchedulingWorkOrders(ISearchSet<WorkOrder> search);

        IEnumerable<MainBreaksAndServiceLineRepairsReportViewModel> SearchMainBreaksAndServiceLineRepairsReport(
            ISearchSet<WorkOrder> search);

        IEnumerable<FieldCompletedBacklogQAReportItem> GetFieldCompletedBacklogQAReport(
            ISearchFieldCompletedBacklogQAReport search);

        IEnumerable<CompletedWorkOrderWithPreJobSafetyBriefReportItem> GetCompletedWorkOrderPreJobSafetyBriefCounts(
            ISearchCompletedWorkOrdersWithPreJobSafetyBriefs search);

        IEnumerable<CompletedWorkOrderWithJobSiteCheckListReportItem> GetCompletedWorkOrderJobSiteCheckListCounts(
            ISearchCompletedWorkOrdersWithJobSiteCheckLists search);

        IEnumerable<CompletedWorkOrderWithMaterialReportItem> GetCompletedWorkOrderMaterialCounts(
            ISearchCompletedWorkOrdersWithMaterial search);

        IEnumerable<CompletedWorkOrderWithMarkoutReportItem> GetCompletedWorkOrderMarkoutCounts(
            ISearchCompletedWorkOrdersWithMarkout search);

        WorkOrder FindPrePlanningOrder(int id);

        IEnumerable<WorkOrder> GetPrePlanningWorkOrders(ISearchSet<WorkOrder> search);

        WorkOrder FindPlanningOrder(int id);

        IEnumerable<WorkOrder> GetPlanningWorkOrders(ISearchSet<WorkOrder> search);

        WorkOrder FindSupervisorApprovalWorkOrder(int id);

        IEnumerable<WorkOrder> GetSupervisorApprovalWorkOrders(ISearchSet<WorkOrder> search);

        WorkOrder FindStockToIssueWorkOrder(int id);

        IEnumerable<WorkOrder> GetStockToIssueWorkOrders(ISearchSet<WorkOrder> search);

        WorkOrder FindFinalizationOrder(int id);

        WorkOrder FindSopProcessingOrder(int id);

        IEnumerable<WorkOrder> GetIncompleteWorkOrdersByWorkDescriptionId(ISearchIncompleteWorkOrder search);

        #endregion
    }

    // in order to share functionality with Contractors
    public interface IWorkOrderRepositoryBase : IRepository<WorkOrder>
    {
        ICriterion StreetOpeningPermitRequired { get; }
        ICriteria PrePlanningOrders { get; }
        ICriteria PlanningOrders { get; }
        ICriteria SchedulingOrders { get; }
        DetachedCriteria CurrentValidMarkoutIds { get; }
        DetachedCriteria ValidPermitIds { get; }
    }
}
