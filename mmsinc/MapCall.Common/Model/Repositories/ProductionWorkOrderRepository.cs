using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.BooleanExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Utilities;
using NHibernate.Linq;
using static MapCall.Common.Model.Entities.WorkOrder;

namespace MapCall.Common.Model.Repositories
{
    public class ProductionWorkOrderRepository : OperatingCenterSecuredRepositoryBase<ProductionWorkOrder>,
        IProductionWorkOrderRepository
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        private ProductionWorkOrderCancellationReason CancelledOrderCancellationReason { get; } =
            new ProductionWorkOrderCancellationReason { Id = ProductionWorkOrderCancellationReason.Indices.ORDER_PAST_EXPIRATION_DATE };
        
        private ProductionWorkOrderPriority RoutinePriority { get; } = new ProductionWorkOrderPriority {
            Id = (int)ProductionWorkOrderPriority.Indices.ROUTINE
        };

        #endregion

        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        #endregion

        #region Logical Properties

        public override RoleModules Role => ROLE;
        private DateTime Today => _dateTimeProvider.GetCurrentDate().Date;

        #endregion

        #region Constructors

        public ProductionWorkOrderRepository(ISession session,
            IContainer container,
            IAuthenticationService<User> authenticationService,
            IRepository<AggregateRole> roleRepo,
            IDateTimeProvider dateTimeProvider) : base(session, container, authenticationService, roleRepo)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<ProductionWorkOrder> GetByFacilityIdForLockoutForms(int facilityId)
        {
            return (from pwo in base.Linq
                    where pwo.Facility.Id == facilityId
                    select new ProductionWorkOrder { Id = pwo.Id }).OrderByDescending(x => x.Id);
        }

        public IEnumerable<ProductionWorkOrder> GetCorrectiveProductionWorkOrdersForReplacedEquipments(int equipmentId)
        {
            return (from pwo in base.Linq
                    where pwo.Equipments.Any(y => y.Equipment.Id == equipmentId)
                    where pwo.ProductionWorkDescription.OrderType.Id == OrderType.Indices.CORRECTIVE_ACTION_20
                    select new ProductionWorkOrder { Id = pwo.Id }).OrderByDescending(x => x.Id);
        }

        private void SetFilters(ISearchProductionWorkOrder search, IQueryOver<ProductionWorkOrder, ProductionWorkOrder> query)
        {
            if (search.HasCompanyRequirement.HasValue || search.HasOshaRequirement.HasValue ||
                search.HasProcessSafetyManagement.HasValue || search.HasRegulatoryRequirement.HasValue ||
                search.OtherCompliance.HasValue || search.Equipment != null || search.SAPEquipmentId != null)
            {
                Equipment e = null;
                ProductionWorkOrder pwo = null;
                var subquery = QueryOver.Of<ProductionWorkOrderEquipment>()
                                        .JoinAlias(x => x.Equipment, () => e)
                                        .JoinAlias(y => y.ProductionWorkOrder, () => pwo);

                if (search.HasProcessSafetyManagement.GetValueOrDefault())
                {
                    subquery.Where(_ => e.HasProcessSafetyManagement);
                }

                if (search.HasCompanyRequirement.GetValueOrDefault())
                {
                    subquery.Where(_ => e.HasCompanyRequirement);
                }

                if (search.HasRegulatoryRequirement.GetValueOrDefault())
                {
                    subquery.Where(_ => e.HasRegulatoryRequirement);
                }

                if (search.HasOshaRequirement.GetValueOrDefault())
                {
                    subquery.Where(_ => e.HasOshaRequirement);
                }

                if (search.OtherCompliance.GetValueOrDefault())
                {
                    subquery.Where(_ => e.OtherCompliance);
                }

                if (search.Equipment.HasValue)
                {
                    subquery.Where(_ => e.Id == search.Equipment.Value);
                }

                if (search.SAPEquipmentId.HasValue)
                {
                    subquery.Where(x => e.SAPEquipmentId == search.SAPEquipmentId);
                }

                query.WithSubquery.WhereProperty(x => x.Id).In(subquery.Select(x => x.ProductionWorkOrder));
            }

            if (!CurrentUserCanAccessAllTheRecords)
            {
                query.Where(Restrictions.On<ProductionWorkOrder>(x => x.OperatingCenter.Id)
                                        .IsIn(UserOperatingCenterIds.ToArray()));
            }
        }

        #region SearchForExcelItem

        public IEnumerable<ProductionWorkOrder> SearchForDistinct(ISearchProductionWorkOrder search)
        {
            var query = Session.QueryOver<ProductionWorkOrder>();

            SetFilters(search, query);

            return Search(search, query);
        }

        public IEnumerable<ProductionWorkOrder> SearchForProductionWorkOrderHistory(ISearchProductionWorkOrderHistory search)
        {
            var query = Session.QueryOver<ProductionWorkOrder>();

            if (search.OperatingCenterId.HasValue ||
                search.Equipment.HasValue)
            {
                Equipment e = null;
                ProductionWorkOrder pwo = null;
                var subquery = QueryOver.Of<ProductionWorkOrderEquipment>()
                                        .JoinAlias(x => x.Equipment, () => e)
                                        .JoinAlias(y => y.ProductionWorkOrder, () => pwo);

                if (search.Equipment.HasValue)
                {
                    subquery.Where(_ => e.Id == search.Equipment.Value);
                }

                query.WithSubquery.WhereProperty(x => x.Id).In(subquery.Select(x => x.ProductionWorkOrder));

                if (search.OperatingCenterId.HasValue)
                {
                    query.Where(x => x.OperatingCenter.Id == search.OperatingCenterId);
                }
            }

            if (!CurrentUserCanAccessAllTheRecords)
            {
                query.Where(Restrictions.On<ProductionWorkOrder>(x => x.OperatingCenter.Id).IsIn(UserOperatingCenterIds.ToArray()));
            }

            query.Where(x => x.DateCancelled == null && x.DateCompleted == null);

            return Search(search, query);
        }

        public IEnumerable<ProductionWorkOrderExcelItem> SearchForExcel(ISearchSet<ProductionWorkOrder> search)
        {
            var boolFormatter = _container.GetInstance<BooleanFormatProvider>();

            var query = Session.QueryOver<ProductionWorkOrder>();

            SetFilters(search as ISearchProductionWorkOrder, query);

            // the majority of these joins were determined by running the site against profiler and then adding an explicit
            // join for anything that fired off an n+1 query.  failing to do this for newly-added references and such will
            // slow this repo method down (not that the big nasty query isn't slow on its own)
            ProductionWorkDescription pwd = null;
            query = query.JoinAlias(x => x.ProductionWorkDescription, () => pwd, JoinType.LeftOuterJoin);
            ProductionWorkOrderEquipment pwoe = null;
            query = query.JoinAlias(x => x.Equipments, () => pwoe, JoinType.LeftOuterJoin);
            ProductionWorkOrderProductionPrerequisite ppre = null;
            query = query.JoinAlias(x => x.ProductionWorkOrderProductionPrerequisites, () => ppre,
                JoinType.LeftOuterJoin);
            EmployeeAssignment currentAssignment = null;
            query = query.JoinAlias(x => x.CurrentAssignments, () => currentAssignment,
                JoinType.LeftOuterJoin);
            Employee currentAssignedTo = null;
            query = query.JoinAlias(_ => currentAssignment.AssignedTo, () => currentAssignedTo, JoinType.LeftOuterJoin);
            User currentAssignedToUser = null;
            query = query.JoinAlias(_ => currentAssignedTo.User, () => currentAssignedToUser, JoinType.LeftOuterJoin);
            Employee currentAssignedBy = null;
            query = query.JoinAlias(_ => currentAssignment.AssignedBy, () => currentAssignedBy, JoinType.LeftOuterJoin);
            User currentAssignedByUser = null;
            query = query.JoinAlias(_ => currentAssignedBy.User, () => currentAssignedByUser, JoinType.LeftOuterJoin);
            EmployeeAssignment assignment = null;
            query = query.JoinAlias(x => x.EmployeeAssignments, () => assignment,
                JoinType.LeftOuterJoin);
            Employee assignedTo = null;
            query = query.JoinAlias(_ => assignment.AssignedTo, () => assignedTo, JoinType.LeftOuterJoin);
            User assignedToUser = null;
            query = query.JoinAlias(_ => assignedTo.User, () => assignedToUser, JoinType.LeftOuterJoin);
            Employee assignedBy = null;
            query = query.JoinAlias(_ => assignment.AssignedBy, () => assignedBy, JoinType.LeftOuterJoin);
            User assignedByUser = null;
            query = query.JoinAlias(_ => assignedBy.User, () => assignedByUser, JoinType.LeftOuterJoin);
            Employee requestedBy = null;
            query = query.JoinAlias(x => x.RequestedBy, () => requestedBy, JoinType.LeftOuterJoin);
            User requestedByUser = null;
            query = query.JoinAlias(_ => requestedBy.User, () => requestedByUser, JoinType.LeftOuterJoin);
            ProductionWorkOrderMaterialUsed pwoMaterialUsed = null;
            query = query.JoinAlias(x => x.ProductionWorkOrderMaterialUsed, () => pwoMaterialUsed,
                JoinType.LeftOuterJoin);
            OperatingCenter oc = null;
            query = query.JoinAlias(x => x.OperatingCenter, () => oc, JoinType.LeftOuterJoin);
            PlanningPlant pp = null;
            query = query.JoinAlias(x => x.PlanningPlant, () => pp, JoinType.LeftOuterJoin);
            Facility fac = null;
            query = query.JoinAlias(x => x.Facility, () => fac, JoinType.LeftOuterJoin);
            Coordinate coord = null;
            query = query.JoinAlias(x => x.Coordinate, () => coord, JoinType.LeftOuterJoin);
            ProductionWorkOrderPriority pwop = null;
            query = query.JoinAlias(x => x.Priority, () => pwop, JoinType.LeftOuterJoin);
            EquipmentType eqt = null;
            query = query.JoinAlias(x => x.EquipmentType, () => eqt, JoinType.LeftOuterJoin);
            CorrectiveOrderProblemCode copc = null;
            query = query.JoinAlias(x => x.CorrectiveOrderProblemCode, () => copc, JoinType.LeftOuterJoin);
            ProductionWorkOrderActionCode pwoac = null;
            query = query.JoinAlias(x => x.ActionCode, () => pwoac, JoinType.LeftOuterJoin);
            ProductionWorkOrderFailureCode pwofc = null;
            query = query.JoinAlias(x => x.FailureCode, () => pwofc, JoinType.LeftOuterJoin);
            ProductionWorkOrderCauseCode pwocc = null;
            query = query.JoinAlias(x => x.CauseCode, () => pwocc, JoinType.LeftOuterJoin);
            ProductionWorkOrderCancellationReason pwocr = null;
            query = query.JoinAlias(x => x.CancellationReason, () => pwocr, JoinType.LeftOuterJoin);
            OrderType orderType = null;
            query = query.JoinAlias(_ => pwd.OrderType, () => orderType, JoinType.LeftOuterJoin);
            MapIcon mapIcon = null;
            query = query.JoinAlias(_ => coord.Icon, () => mapIcon, JoinType.LeftOuterJoin);
            MapIcon mapIconOffset = null;
            query = query.JoinAlias(_ => mapIcon.Offset, () => mapIconOffset, JoinType.LeftOuterJoin);

            var results = Search(search, query).Distinct().ToList();

            string HasPrerequisite(ProductionWorkOrder productionWorkOrder, int productionPreqrequisiteId) =>
                productionWorkOrder.ProductionWorkOrderProductionPrerequisites.Any(x =>
                    x.ProductionPrerequisite.Id == productionPreqrequisiteId
                ).ToString(boolFormatter);

            foreach (var workOrder in results.OrderBy(x => x.Id))
            {
                List<string> lockouts = new List<string>();

                foreach (var lockout in workOrder.LockoutForms)
                {
                    lockouts.Add(lockout.Id.ToString());
                }

                yield return new ProductionWorkOrderExcelItem {
                    Id = workOrder.Id.ToString(),
                    OperatingCenter = workOrder.OperatingCenter?.ToString(),
                    PlanningPlant = workOrder.PlanningPlant?.ToString(),
                    Facility = workOrder.Facility?.ToString(),
                    FacilityArea = workOrder.FacilityFacilityArea?.FacilityArea?.Description,
                    FunctionalLocation = workOrder.FunctionalLocation,
                    EquipmentType = workOrder.EquipmentType?.ToString(),
                    Equipment = workOrder.Equipment?.ToString(),
                    Coordinate = workOrder.Coordinate?.ToString(),
                    Priority = workOrder.Priority?.ToString(),
                    IsOpen = workOrder.IsOpen.ToString(),
                    WorkDescription = workOrder.ProductionWorkDescription?.ToString(),
                    AirPermit = HasPrerequisite(workOrder, ProductionPrerequisite.Indices.AIR_PERMIT),
                    IsEligibleForRedTagPermit = HasPrerequisite(workOrder, ProductionPrerequisite.Indices.RED_TAG_PERMIT),
                    HasLockoutRequirement =
                        HasPrerequisite(workOrder, ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT),
                    HotWork = HasPrerequisite(workOrder, ProductionPrerequisite.Indices.HOT_WORK),
                    IsConfinedSpace = HasPrerequisite(workOrder, ProductionPrerequisite.Indices.IS_CONFINED_SPACE),
                    JobSafetyChecklist =
                        HasPrerequisite(workOrder, ProductionPrerequisite.Indices.JOB_SAFETY_CHECKLIST),
                    CapitalizedFrom = workOrder.CapitalizedFrom?.ToString(),
                    RequestedBy = workOrder.RequestedBy?.ToString(),
                    Notes = workOrder.OrderNotes,
                    DateReceived = workOrder.DateReceived?.ToString(),
                    BreakdownIndicator = workOrder.BreakdownIndicator?.ToString(),
                    SAPWorkOrder = workOrder.SAPWorkOrder,
                    SAPStatus = workOrder.SAPErrorCode,
                    SAPNotificationNumber = workOrder.SAPNotificationNumber?.ToString(),
                    WBSElement = workOrder.WBSElement,
                    CapitalizationReason = workOrder.CapitalizationReason,
                    DateCompleted = workOrder.DateCompleted?.ToString(),
                    CompletedBy = workOrder.CompletedBy?.ToString(),
                    ApprovedOn = workOrder.ApprovedOn?.ToString(),
                    ApprovedBy = workOrder.ApprovedBy?.ToString(),
                    MaterialsApprovedOn = workOrder.MaterialsApprovedOn?.ToString(),
                    MaterialsPlannedOn = workOrder.MaterialsPlannedOn?.ToString(),
                    MaterialsApprovedBy = workOrder.MaterialsApprovedBy?.ToString(),
                    BasicStart = workOrder.BasicStart?.ToString(),
                    DateCancelled = workOrder.DateCancelled?.ToString(),
                    CancellationReason = workOrder.CancellationReason?.ToString(),
                    PlantMaintenanceActivityTypeOverride = workOrder.PlantMaintenanceActivityTypeOverride?.ToString(),
                    CorrectiveOrderProblemCode = workOrder.CorrectiveOrderProblemCode?.ToString(),
                    OtherProblemNotes = workOrder.OtherProblemNotes,
                    ActionCode = workOrder.ActionCode?.ToString(),
                    FailureCode = workOrder.FailureCode?.ToString(),
                    CauseCode = workOrder.CauseCode?.ToString(),
                    ProductionWorkOrderRequiresSupervisorApproval =
                        workOrder.ProductionWorkOrderRequiresSupervisorApproval?.RequiresSupervisorApproval.ToString(),
                    MaterialsApproved = workOrder.MaterialsApproved.ToString(),
                    Status = workOrder.Status.ToString(),
                    OrderType = workOrder.OrderType?.ToString(),
                    SendToSap = workOrder.SendToSAP.ToString(),
                    CanBeSupervisorApproved = workOrder.CanBeSupervisorApproved.ToString(),
                    CanBeMaterialApproved = workOrder.CanBeMaterialApproved.ToString(),
                    CanBeCompleted = workOrder.CanBeCompleted.ToString(),
                    CanBeCancelled = workOrder.CanBeCancelled.ToString(),
                    CanBeMaterialPlanned = workOrder.CanBeMaterialPlanned.ToString(),
                    CapitalizationCancelsOrder = workOrder.CapitalizationCancelsOrder.ToString(),
                    CurrentlyAssignedEmployee = workOrder.CurrentlyAssignedEmployee,
                    LockoutFormCreated = workOrder.LockoutForms.Any().ToString(boolFormatter),
                    LockoutForms = string.Join(", ", lockouts),
                    LockoutDevices = string.Join(", ", workOrder.LockoutDevices),
                    ConfinedSpaceFormCreated = workOrder.ConfinedSpaceForms.Any().ToString(boolFormatter),
                    RedTagPermitCreated = (workOrder.RedTagPermit != null).ToString(boolFormatter),
                    TankInspections = string.Join(", ", workOrder.TankInspections),
                    RedTagPermit = workOrder.RedTagPermit?.Id.ToString(),
                    EstimatedCompletionHours = workOrder.EstimatedCompletionHours.ToString(),
                    ActualCompletionHours = workOrder.ActualCompletionHours.ToString()
                };
            }
        }

        #endregion

        #region RoutineProductionWorkOrder

        /// <summary>
        /// Get the Ids of all ProductionWorkOrders that meet the requirements for cancellation,
        /// and whose parent MaintenancePlan has AutoCancel (HasACompletionRequirement == true). Only ProductionWorkOrders
        /// whose parent MaintenancePlan is contained inside <paramref name="scheduledPlans"/> will be considered for cancellation.
        /// </summary>
        /// <remarks>This method will enumerate <paramref name="scheduledPlans"/> so be mindful of multiple enumeration.</remarks>
        /// <param name="scheduledPlans">An enumerable of plans that are scheduled to create new ProductionWorkOrders</param>
        /// <returns>An enumerable of all the Ids that should be cancelled via CancelOrders(). Only enumerate this once, as it will contact the DB.</returns>
        public IEnumerable<int> GetAutoCancelRoutineProductionWorkOrders(IEnumerable<ScheduledMaintenancePlan> scheduledPlans)
        {
            var scheduledPlanIds = scheduledPlans.Select(x => x.MaintenancePlanId);
            return Where(x =>
                       scheduledPlanIds.Contains(x.MaintenancePlan.Id)
                       && x.MaintenancePlan.HasACompletionRequirement
                       && x.MaterialsApprovedOn == null
                       && x.DateCancelled == null
                       && x.DateCompleted == null
                       && (x.EmployeeAssignments.Any() == false ||
                           x.EmployeeAssignments.All(y =>
                               y.DateStarted == null &&
                               y.DateEnded == null)))
                  .Select(x => x.Id)
                  .AsEnumerable();
        }

        /// <summary>
        /// Cancels all the ProductionWorkOrders whose Ids are contained within the provided IEnumerable. This method does not check
        /// whether or not a PWO should be cancelled, so do not call this until you have already checked the PWO meets the company requirements
        /// for cancellation.
        /// </summary>
        /// <remarks>Invoking this method persists changes to the underlying data store.</remarks>
        /// <param name="orderIds">An enumerable of ProductionWorkOrder Ids to be cancelled.</param>
        public void CancelOrders(IEnumerable<int> orderIds)
        {
            Session.Query<ProductionWorkOrder>()
                   .Where(x => orderIds.Contains(x.Id))
                   .Update(x => new {
                        DateCancelled = Today,
                        CancelledBy = _authenticationSerice.CurrentUser,
                        CancellationReason = CancelledOrderCancellationReason,
                    });
        }

        /// <summary>
        /// Enumerating the result of this method will return a new ProductionWorkOrder for each provided ScheduledMaintenancePlan.
        /// </summary>
        /// <remarks>This method will enumerate <paramref name="plans"/> so be mindful of multiple enumeration.</remarks>
        /// <param name="plans"></param>
        /// <returns>An enumeration of new ProductionWorkOrders whose Id is 0, which can then be Save()'d</returns>
        public IEnumerable<ProductionWorkOrder> BuildRoutineProductionWorkOrdersFromScheduledPlans(IEnumerable<ScheduledMaintenancePlan> plans)
        {
            foreach (var plan in plans)
            {
                var pwo = new ProductionWorkOrder {
                    MaintenancePlan = new MaintenancePlan { Id = plan.MaintenancePlanId },
                    OperatingCenter = new OperatingCenter { Id = plan.OperatingCenterId },
                    PlanningPlant = new PlanningPlant { Id = plan.PlanningPlantId },
                    EquipmentType = plan.EquipmentType,
                    ProductionWorkDescription = plan.WorkDescription,
                    LocalTaskDescription = plan.LocalTaskDescription,
                    Facility = plan.Facility,
                    Priority = RoutinePriority,
                    //WOs created from plan should have Breakdown Indicator false
                    BreakdownIndicator = false,
                    StartDate = Today,
                    DateReceived = Today,
                    BasicStart = Today,
                    DueDate = ProductionWorkOrderFrequency.GetFrequencyNextEndDate(plan.ProductionWorkOrderFrequencyId, Today)
                };

                var assignments =
                    plan.Assignments.Where(x => x.ScheduledDate.Date == Today.Date)
                        .Select(assignment => new EmployeeAssignment {
                             ProductionWorkOrder = pwo,
                             AssignedFor = assignment.AssignedFor,
                             AssignedBy = _authenticationSerice.CurrentUser.Employee,
                             AssignedTo = assignment.AssignedTo,
                             AssignedOn = Today
                         })
                        .ToList();

                pwo.EmployeeAssignments.AddRange(assignments);

                pwo.Equipments = plan.Equipment.Select(x => new ProductionWorkOrderEquipment {
                    Equipment = x,
                    ProductionWorkOrder = pwo,
                    IsParent = true,
                }).ToHashSet();

                yield return pwo;
            }
        }

        /// <summary>
        /// This is a modified version of the RepositoryBase's Save(IEnumerable) method. This method saves all the provided ProductionWorkOrders
        /// and then returns all the EmployeeAssignments in those work orders. There's a use-case where we need to save a bunch of new PWO records and then send
        /// notifications for all the EmployeeAssignments created on the new PWOs. This prevents us from having to enumerate over the PWOs multiple times
        /// (one for saving then another for getting the EmployeeAssignments)
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public IEnumerable<EmployeeAssignment> SaveAllAndGetAssignmentsForNotifications(IEnumerable<ProductionWorkOrder> entities)
        {
            var assignments = new List<EmployeeAssignment>();
            foreach (var entity in entities)
            {
                Session.Save(entity);
                assignments.AddRange(entity.EmployeeAssignments);
            }

            Session.Flush();
            return assignments;
        }

        #endregion

        #endregion
    }

    public static class ProductionWorkOrderRepositoryExtensions
    {
        private static ProductionWorkOrderPerformanceResultViewModel CreatePerformanceResultModel(
            IQueryable<ProductionWorkOrder> sub,
            string orderType, int orderTypeId, string state, int stateId, string operatingCenter = "",
            int? operatingCenterId = null, string planningPlant = "", int? planningPlantId = null, string facility = "",
            int? facilityId = null)
        {
            // NOTE: Some of the logic here and below has to be duplicated in the SearchProductionWorkOrder model
            // in order to generate links correctly. If you make a change here, you probably need to make a change there.
            var data = sub.Select(wo => new {
                IsCanceled = wo.DateCancelled != null,
                IsCompleted = wo.DateCompleted != null,
                HasAssignments = wo.EmployeeAssignments.Any(),
                HasUnstartedAssignments = wo.EmployeeAssignments.Any(a => a.DateStarted == null),
                HasStartedAssignments = wo.EmployeeAssignments.Any(a => a.DateStarted != null),
                IsApproved = wo.ApprovedOn != null
            });

            // NOTE: When creating this object, the database is hit with seven queries(one for each Count).
            return new ProductionWorkOrderPerformanceResultViewModel {
                OrderType = orderType,
                OrderTypeId = orderTypeId,
                State = state,
                StateId = stateId,
                OperatingCenter = operatingCenter,
                OperatingCenterId = operatingCenterId,
                PlanningPlant = planningPlant,
                PlanningPlantId = planningPlantId,
                Facility = facility,
                FacilityId = facilityId,
                NumberCreated = data.Count(),
                NumberUnscheduled = data.Count(wo => !wo.IsCanceled && !wo.IsCompleted && !wo.HasAssignments),
                NumberScheduled = data.Count(wo => !wo.IsCanceled && !wo.IsCompleted && wo.HasUnstartedAssignments),
                // MC-1653 requested that incomplete "include assignments that are finished but the work order is not complete" 
                // so we're ignoring the DateEnded check since its value no longer matters.
                NumberIncomplete = data.Count(wo => !wo.IsCanceled && !wo.IsCompleted && wo.HasStartedAssignments),
                NumberCanceled = data.Count(wo => wo.IsCanceled),
                NumberCompleted = data.Count(wo => wo.IsCompleted),
                NumberNotApproved = data.Count(wo => wo.IsCompleted && !wo.IsApproved)
            };
        }

        public static void GetPerformanceReport(this IRepository<ProductionWorkOrder> that,
            ISearchProductionWorkOrderPerformance search)
        {
            var query = that.Where(_ => true);
            IQueryable<ProductionWorkOrderPerformanceResultViewModel> result;

            if (search.OrderType.Any())
            {
                query = query.Where(wo => search.OrderType.Contains(wo.ProductionWorkDescription.OrderType.Id));
                search.SelectedOrderTypes = query.Select(wo => wo.ProductionWorkDescription.OrderType.Description)
                                                 .Distinct().ToArray();
            }

            switch (search.DateReceived.Operator)
            {
                case RangeOperator.Between:
                    query = query.Where(wo =>
                        wo.DateReceived >= search.DateReceived.Start.Value.BeginningOfDay() &&
                        wo.DateReceived <= search.DateReceived.End.Value.EndOfDay());
                    break;
                case RangeOperator.Equal:
                    query = query.Where(wo => wo.DateReceived.Value.Date == search.DateReceived.End.Value.Date);
                    break;
                case RangeOperator.GreaterThan:
                    query = query.Where(wo => wo.DateReceived > search.DateReceived.End);
                    break;
                case RangeOperator.GreaterThanOrEqualTo:
                    query = query.Where(wo => wo.DateReceived >= search.DateReceived.End);
                    break;
                case RangeOperator.LessThan:
                    query = query.Where(wo => wo.DateReceived < search.DateReceived.End);
                    break;
                case RangeOperator.LessThanOrEqualTo:
                    query = query.Where(wo => wo.DateReceived <= search.DateReceived.End);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            // NOTE: This whole if/else section is based on the order the fields cascade off
            // of one another.
            if (search.Facility.Any())
            {
                var facilityQuery = search.Facility == null
                    ? query.Where(wo => search.PlanningPlant.Contains(wo.PlanningPlant.Id))
                    : query.Where(wo => search.Facility.Contains(wo.Facility.Id));

                var facilities = facilityQuery.Select(wo => new {
                                                   OrderType = wo.ProductionWorkDescription.OrderType.Description,
                                                   OrderTypeId = wo.ProductionWorkDescription.OrderType.Id,
                                                   State = wo.OperatingCenter.State.Abbreviation,
                                                   StateId = wo.OperatingCenter.State.Id,
                                                   OperatingCenter = wo.OperatingCenter.OperatingCenterCode + " - " +
                                                                     wo.OperatingCenter.OperatingCenterName,
                                                   OperatingCenterId = wo.OperatingCenter.Id,
                                                   PlanningPlantCode = wo.PlanningPlant.Code,
                                                   PlanningPlantDescription = wo.PlanningPlant.Description,
                                                   PlanningPlantId = wo.PlanningPlant.Id,
                                                   wo.Facility.FacilityName,
                                                   FacilityId = (int?)wo.Facility.Id
                                               }).Distinct().ToList()
                                              .OrderBy(f => f.State)
                                              .ThenBy(f => f.OperatingCenter)
                                              .ThenBy(f => f.PlanningPlantCode)
                                              .ThenBy(f => f.FacilityName)
                                              .ThenBy(f => f.OrderType);

                search.SelectedStates = facilities.Select(f => f.State).Distinct().ToArray();
                search.SelectedOperatingCenters = facilities.Select(f => f.OperatingCenter).Distinct().ToArray();
                search.SelectedPlanningPlants = facilities
                                               .Select(f =>
                                                    string.IsNullOrWhiteSpace(f.PlanningPlantCode)
                                                        ? string.Empty
                                                        : $"{f.PlanningPlantCode} - {f.PlanningPlantDescription}")
                                               .Distinct()
                                               .ToArray();
                search.SelectedFacilities = facilities.Select(f => f.FacilityName).Distinct().ToArray();

                search.Count = facilities.Count();
                search.Results = facilities.Select(f => {
                    return CreatePerformanceResultModel(query.Where(wo =>
                            wo.ProductionWorkDescription.OrderType.Id == f.OrderTypeId &&
                            ((f.FacilityId == null && wo.Facility == null) || wo.Facility.Id == f.FacilityId)),
                        f.OrderType, f.OrderTypeId, f.State, f.StateId, f.OperatingCenter, f.OperatingCenterId,
                        string.IsNullOrWhiteSpace(f.PlanningPlantCode)
                            ? string.Empty
                            : $"{f.PlanningPlantCode} - {f.PlanningPlantDescription}", f.PlanningPlantId,
                        f.FacilityName, f.FacilityId);
                });
            }
            else if (search.PlanningPlant.Any() || search.OperatingCenter.Any())
            {
                var planningPlantQuery = search.PlanningPlant.Any()
                    ? query.Where(wo => search.PlanningPlant.Contains(wo.PlanningPlant.Id))
                    : query.Where(wo => search.OperatingCenter.Contains(wo.OperatingCenter.Id));
                var planningPlants = planningPlantQuery.Select(wo => new {
                                                            OrderType = wo
                                                                       .ProductionWorkDescription.OrderType.Description,
                                                            OrderTypeId = wo.ProductionWorkDescription.OrderType.Id,
                                                            State = wo.OperatingCenter.State.Abbreviation,
                                                            StateId = wo.OperatingCenter.State.Id,
                                                            OperatingCenter =
                                                                wo.OperatingCenter.OperatingCenterCode + " - " +
                                                                wo.OperatingCenter.OperatingCenterName,
                                                            OperatingCenterId = wo.OperatingCenter.Id,
                                                            PlanningPlantCode = wo.PlanningPlant.Code,
                                                            PlanningPlantDescription = wo.PlanningPlant.Description,
                                                            PlanningPlantId = (int?)wo.PlanningPlant.Id
                                                        })
                                                       .Distinct()
                                                       .ToList()
                                                       .OrderBy(pp => pp.State)
                                                       .ThenBy(pp => pp.OperatingCenter)
                                                       .ThenBy(pp => pp.PlanningPlantCode)
                                                       .ThenBy(pp => pp.OrderType);

                search.SelectedStates = planningPlants.Select(f => f.State).Distinct().ToArray();
                search.SelectedOperatingCenters = planningPlants.Select(f => f.OperatingCenter).Distinct().ToArray();

                if (search.PlanningPlant.Any())
                {
                    search.SelectedPlanningPlants = planningPlants
                                                   .Select(f =>
                                                        string.IsNullOrWhiteSpace(f.PlanningPlantCode)
                                                            ? string.Empty
                                                            : $"{f.PlanningPlantCode} - {f.PlanningPlantDescription}")
                                                   .Distinct()
                                                   .ToArray();
                }

                search.Count = planningPlants.Count();
                search.Results = planningPlants.Select(pp => {
                    return CreatePerformanceResultModel(query.Where(wo =>
                            wo.ProductionWorkDescription.OrderType.Id == pp.OrderTypeId &&
                            wo.OperatingCenter.Id == pp.OperatingCenterId &&
                            ((pp.PlanningPlantId == null && wo.PlanningPlant == null) ||
                             wo.PlanningPlant.Id == pp.PlanningPlantId)), pp.OrderType, pp.OrderTypeId, pp.State,
                        pp.StateId, pp.OperatingCenter, pp.OperatingCenterId,
                        string.IsNullOrWhiteSpace(pp.PlanningPlantCode)
                            ? string.Empty
                            : $"{pp.PlanningPlantCode} - {pp.PlanningPlantDescription}", pp.PlanningPlantId);
                });
            }
            else if (search.State.Any())
            {
                var operatingCenters = query.Where(wo => search.State.Contains(wo.OperatingCenter.State.Id)).Select(
                                                 wo => new {
                                                     OrderType = wo.ProductionWorkDescription.OrderType.Description,
                                                     OrderTypeId = wo.ProductionWorkDescription.OrderType.Id,
                                                     State = wo.OperatingCenter.State.Abbreviation,
                                                     StateId = wo.OperatingCenter.State.Id,
                                                     OperatingCenterId = wo.OperatingCenter.Id,
                                                     OperatingCenter =
                                                         wo.OperatingCenter.OperatingCenterCode + " - " +
                                                         wo.OperatingCenter.OperatingCenterName
                                                 }).Distinct().ToList()
                                            .OrderBy(f => f.State)
                                            .ThenBy(f => f.OrderType);

                search.SelectedStates = operatingCenters.Select(f => f.State).Distinct().ToArray();

                search.Count = operatingCenters.Count();
                search.Results = operatingCenters.Select(opCenter => {
                    return CreatePerformanceResultModel(query.Where(wo =>
                            wo.ProductionWorkDescription.OrderType.Id == opCenter.OrderTypeId &&
                            wo.OperatingCenter.Id == opCenter.OperatingCenterId), opCenter.OrderType,
                        opCenter.OrderTypeId,
                        opCenter.State, opCenter.StateId, opCenter.OperatingCenter, opCenter.OperatingCenterId);
                });
            }
            else
            {
                var states = query.Select(wo => new {
                    OrderType = wo.ProductionWorkDescription.OrderType.Description,
                    OrderTypeId = wo.ProductionWorkDescription.OrderType.Id,
                    StateId = wo.OperatingCenter.State.Id,
                    State = wo.OperatingCenter.State.Abbreviation,
                }).Distinct().ToList();

                search.Count = states.Count();
                search.Results = states.Select(state => {
                    return CreatePerformanceResultModel(query.Where(wo =>
                            wo.ProductionWorkDescription.OrderType.Id == state.OrderTypeId &&
                            wo.OperatingCenter.State.Id == state.StateId), state.OrderType, state.OrderTypeId,
                        state.State,
                        state.StateId);
                });
            }
        }
    }

    public interface IProductionWorkOrderRepository : IRepository<ProductionWorkOrder>
    {
        IEnumerable<ProductionWorkOrder> GetByFacilityIdForLockoutForms(int facilityId);
        IEnumerable<ProductionWorkOrder> GetCorrectiveProductionWorkOrdersForReplacedEquipments(int equipmentId);
        IEnumerable<ProductionWorkOrderExcelItem> SearchForExcel(ISearchSet<ProductionWorkOrder> search);
        IEnumerable<ProductionWorkOrder> SearchForProductionWorkOrderHistory(ISearchProductionWorkOrderHistory search);
        IEnumerable<ProductionWorkOrder> SearchForDistinct(ISearchProductionWorkOrder search);
        IEnumerable<int> GetAutoCancelRoutineProductionWorkOrders(IEnumerable<ScheduledMaintenancePlan> scheduledPlans);
        void CancelOrders(IEnumerable<int> orderIds);
        IEnumerable<ProductionWorkOrder> BuildRoutineProductionWorkOrdersFromScheduledPlans(IEnumerable<ScheduledMaintenancePlan> scheduledPlans);
        IEnumerable<EmployeeAssignment> SaveAllAndGetAssignmentsForNotifications(IEnumerable<ProductionWorkOrder> entities);
    }
}
