using System;
using MapCall.Common.Model.Entities.Users;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using System.Collections.Generic;
using System.Linq;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ProductionWorkOrder : IEntity, ISAPEntity, IThingWithCoordinate, IThingWithNotes, IThingWithDocuments,
        IHasWorkOrderStatus, IThingWithOperatingCenter
    {
        #region Constants

        public struct DisplayNames
        {
            public const string NEEDS_RED_TAG_PERMIT_AUTHORIZATION =
                                    "Is any part of the system being compromised or impaired?",
                                NEEDS_RED_TAG_PERMIT_AUTHORIZED_ON = "Authorized On",
                                NEEDS_RED_TAG_PERMIT_AUTHORIZED_BY = "Authorized By",
                                DATE_RECEIVED = "Date Created",
                                ESTIMATED_COMPLETION_HOURS = "Estimated Completion Time (Hrs)",
                                ACTUAL_COMPLETION_HOURS = "Actual Completion Time (Hrs)",
                                SCHEDULED_START = "Scheduled Start Date",
                                DAYS_OVERDUE = "# Days Overdue",
                                PLANNING_PLANT = "District",
                                PUBLIC_WATER_SUPPLY = "PWSID",
                                PUBLIC_WATER_SUPPLY_PRESSURE_ZONE = "PWSID Pressure Zone",
                                WORK_DESCRIPTION = "Work Description",
                                ORDER_NOTES = "Notes",
                                SAP_ERROR_CODE = "SAP Status",
                                HAS_AT_LEAST_ONE_WELL_TEST = "Has Well Test",
                                STATUS = "Order Status",
                                AUTO_CREATED_CORRECTIVE_WORK_ORDER = "Identified from PM",
                                TASK_GROUP_NAME = "Task Group Name";
        }

        public struct StringLengths
        {
            public const int FUNCTIONAL_LOCATION = 30,
                             NOTES = 255,
                             SAP_WORK_ORDER = 50,
                             WBS_ELEMENT = 30,
                             LOCAL_TASK_DESCRIPTION = 75;
        }

        // MC-1131 says only capital/corrective orders deal with supervisor approval. Other orders do not.
        private static IEnumerable<int> SUPERVISOR_APPROVABLE_ORDERTYPES = new[]
            {OrderType.Indices.CORRECTIVE_ACTION_20, OrderType.Indices.RP_CAPITAL_40};

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        
        [View(DisplayNames.PLANNING_PLANT)]
        public virtual PlanningPlant PlanningPlant { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual FacilityFacilityArea FacilityFacilityArea { get; set; }

        public virtual string FunctionalLocation { get; set; }

        public virtual EquipmentType EquipmentType { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual ProductionWorkOrderPriority Priority { get; set; }

        //It was requested that this field be added to the screen even though it doesn't have anything backing it at the moment.
        //There is ticket MC-5290 to populate it in the future.
        public virtual string AccountType { get; set; }

        public virtual bool IsOpen { get; }

        public virtual bool IsLockoutFormStillOpen { get; }

        public virtual bool IsRedTagPermitStillOpen { get; }

        public virtual string LocalTaskDescription { get; set; }

        [View(DisplayNames.WORK_DESCRIPTION)]
        public virtual ProductionWorkDescription ProductionWorkDescription { get; set; }

        public virtual ProductionWorkOrder CapitalizedFrom { get; set; }

        public virtual ISet<EmployeeAssignment> CurrentAssignments { get; set; }

        public virtual Employee RequestedBy { get; set; }

        [View(DisplayNames.ORDER_NOTES)]
        public virtual string OrderNotes { get; set; }

        [View(DisplayName = DisplayNames.DATE_RECEIVED, DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? DateReceived { get; set; }

        public virtual bool? BreakdownIndicator { get; set; }
        public virtual string SAPWorkOrder { get; set; }

        [View(DisplayNames.SAP_ERROR_CODE)]
        public virtual string SAPErrorCode { get; set; }

        public virtual long? SAPNotificationNumber { get; set; }
        public virtual string WBSElement { get; set; }

        public virtual string CapitalizationReason { get; set; }
        
        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE, ApplyFormatInEditMode = false)]
        public virtual DateTime? DateCompleted { get; set; }
        
        public virtual User CompletedBy { get; set; }
        public virtual User CancelledBy { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE, ApplyFormatInEditMode = false)]
        public virtual DateTime? ApprovedOn { get; set; }

        public virtual User ApprovedBy { get; set; }
        public virtual DateTime? MaterialsApprovedOn { get; set; }
        public virtual DateTime? MaterialsPlannedOn { get; set; }
        public virtual User MaterialsApprovedBy { get; set; }

        [View(DisplayNames.NEEDS_RED_TAG_PERMIT_AUTHORIZATION)]
        public virtual bool? NeedsRedTagPermitAuthorization { get; set; }

        [View(DisplayNames.NEEDS_RED_TAG_PERMIT_AUTHORIZED_ON)]
        public virtual DateTime? NeedsRedTagPermitAuthorizedOn { get; set; }

        [View(DisplayNames.NEEDS_RED_TAG_PERMIT_AUTHORIZED_BY)]
        public virtual Employee NeedsRedTagPermitAuthorizedBy { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? BasicStart { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? BasicFinish { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE, ApplyFormatInEditMode = false)]
        public virtual DateTime? DateCancelled { get; set; }

        public virtual ProductionWorkOrderCancellationReason CancellationReason { get; set; }

        public virtual PlantMaintenanceActivityType PlantMaintenanceActivityTypeOverride { get; set; }

        public virtual CorrectiveOrderProblemCode CorrectiveOrderProblemCode { get; set; }

        public virtual string OtherProblemNotes { get; set; }
        public virtual bool? HasAssignmentsOnNonCancelledWorkOrder { get; set; }
        public virtual ProductionWorkOrderActionCode ActionCode { get; set; }
        public virtual ProductionWorkOrderFailureCode FailureCode { get; set; }
        public virtual ProductionWorkOrderCauseCode CauseCode { get; set; }

        public virtual ProductionWorkOrderRequiresSupervisorApproval ProductionWorkOrderRequiresSupervisorApproval
        {
            get;
            set;
        }

        public virtual long? SAPMaintenancePlanId { get; set; }

        public virtual MaintenancePlan MaintenancePlan { get; set; }

        public virtual RedTagPermit RedTagPermit { get; set; }

        public virtual IList<ProductionWorkOrderProductionPrerequisite> ProductionWorkOrderProductionPrerequisites
        {
            get;
            set;
        }

        public virtual ISet<EmployeeAssignment> EmployeeAssignments { get; set; }

        public virtual ISet<ProductionWorkOrderMaterialUsed> ProductionWorkOrderMaterialUsed { get; set; }

        public virtual IList<JobObservation> JobObservations { get; set; }

        //TODO: This needs a better name.
        public virtual ISet<ProductionWorkOrderEquipment> Equipments { get; set; }

        public virtual IList<ProductionWorkOrderMeasurementPointValue> ProductionWorkOrderMeasurementPointValues
        {
            get;
            set;
        }

        public virtual IList<LockoutForm> LockoutForms { get; set; }
        public virtual IList<TankInspection> TankInspections { get; set; }
        public virtual IList<LockoutDevice> LockoutDevices { get; set; }

        //added by Apurva, this is required to push material to SAP
        //these don't actually exist in mapcall
        public virtual IList<ProductionWorkOrderChildNotification> ProductionWorkOrderChildNotification { get; set; }
        public virtual IList<ProductionWorkOrderDependencies> ProductionWorkOrderDependencies { get; set; }
        public virtual IList<ProductionWorkOrderActions> ProductionWorkOrderActions { get; set; }
        public virtual IList<ProductionWorkOrderMeasuringPoints> ProductionWorkOrderMeasuringPoints { get; set; }

        public virtual ISet<ConfinedSpaceForm> ConfinedSpaceForms { get; set; }
        public virtual ISet<ProductionPreJobSafetyBrief> ProductionPreJobSafetyBriefs { get; set; }
        public virtual ISet<AssetReliability> AssetReliabilities { get; set; }

        [DoesNotExport]
        public virtual bool HasPreJobSafetyBrief => ProductionPreJobSafetyBriefs.Any();

        public virtual ISet<WellTest> WellTests { get; set; } = new HashSet<WellTest>();
        
        [View(DisplayNames.ESTIMATED_COMPLETION_HOURS)]
        public virtual decimal EstimatedCompletionHours { get; set; }

        [View(DisplayName = DisplayNames.SCHEDULED_START, DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? StartDate { get; set; }
       
        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? DueDate { get; set; }

        [View(DisplayNames.DAYS_OVERDUE)]
        public virtual int DaysOverdue { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? AssignedOnDate { get; set; }

        [View(DisplayName = DisplayNames.AUTO_CREATED_CORRECTIVE_WORK_ORDER)]
        public virtual bool AutoCreatedCorrectiveWorkOrder { get; set; }

        #endregion

        #region Logical Properties

        public virtual ProductionWorkOrderFrequency ProductionWorkOrderFrequency => MaintenancePlan?.ProductionWorkOrderFrequency;

        public virtual Equipment Equipment => Equipments.Where(x => x.IsParent.GetValueOrDefault())
                                                        .Select(x => x.Equipment).FirstOrDefault();

        [View(DisplayNames.HAS_AT_LEAST_ONE_WELL_TEST)]
        public virtual bool HasAtLeastOneWellTest => WellTests.Any();

        public virtual string EmployeesAssigned => string.Join(", ", EmployeeAssignments.Select(x => x.AssignedTo));

        [DoesNotExport]
        public virtual bool HasTankInspections => TankInspections.Any();
        [DoesNotExport]
        public virtual bool RequiresTankInspection => EquipmentIsTank &&
                                                      ((Priority.Id == (int)ProductionWorkOrderPriority.Indices.ROUTINE) || Priority.Id == (int)ProductionWorkOrderPriority.Indices.ROUTINE_OFF_SCHEDULED)
                                                      && (MaintenancePlan?.TaskGroup?.TaskGroupId == TaskGroup.TaskGroupIds.OPERATIONS_SITE_OBSERVATION_TASK_GROUP_ID);
        [DoesNotExport]
        public virtual bool EquipmentIsTank => EquipmentType?.Abbreviation?.Equals(EquipmentType.ComparisonValue.POTABLE_WATER_TANK) ?? false;
 
        public virtual bool MaterialsApproved => MaterialsApprovedOn.HasValue && MaterialsApprovedBy != null;

        [DoesNotExport]
        public virtual bool LinkedToEnvironmentalPermit => (Equipment?.EnvironmentalPermits.Any() ?? false) ||
                                                           Equipments.Any(e => e.Equipment.EnvironmentalPermits.Any());

        public virtual WorkOrderStatus Status
        {
            get
            {
                if (DateCancelled.HasValue)
                {
                    return WorkOrderStatus.Cancelled;
                }

                if (DateCompleted.HasValue)
                {
                    if (CanBeSupervisorApproved)
                    {
                        return WorkOrderStatus.RequiresSupervisorApproval;
                    }

                    return WorkOrderStatus.Completed;
                }

                if (CurrentAssignments.Any())
                {
                    if (CurrentAssignments.Any(ca => ca.AssignedFor < DateTime.Today))
                    {
                        return WorkOrderStatus.ScheduledPreviously;
                    }

                    return WorkOrderStatus.ScheduledCurrently;
                }

                if (Equipment != null && (Equipment.HasProcessSafetyManagement || Equipment.HasCompanyRequirement ||
                                          Equipment.HasRegulatoryRequirement || Equipment.HasOshaRequirement ||
                                          Equipment.OtherCompliance))
                {
                    return WorkOrderStatus.WithCompliance;
                }

                return WorkOrderStatus.Other;
            }
        }

        [View("Order Status")]
        public virtual string WorkOrderStatusDisplayText => WorkOrder.WORK_ORDER_STATUS_DISPLAY_NAMES[(int)Status];

        // MC-1556 - Adding OrderType Prop so exporter can include order type in production work order export
        public virtual OrderType OrderType => ProductionWorkDescription.OrderType;

        [DoesNotExport]
        public virtual string TableName => nameof(ProductionWorkOrder) + "s";

        public virtual IList<ProductionWorkOrderDocument> ProductionWorkOrderDocuments { get; set; }
        public virtual IList<ProductionWorkOrderNote> ProductionWorkOrderNotes { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => ProductionWorkOrderDocuments.Map(e => (IDocumentLink)e);
        public virtual IList<Document> Documents => ProductionWorkOrderDocuments.Map(e => e.Document);
        public virtual IList<INoteLink> LinkedNotes => ProductionWorkOrderNotes.Map(e => (INoteLink)e);
        public virtual IList<Note> Notes => ProductionWorkOrderNotes.Map(e => e.Note);
        public virtual MapIcon Icon => Coordinate?.Icon;

        [DoesNotExport]
        [BoolFormat("Yes", "No")]
        public virtual bool HasSupervisorApprovableOrderType =>
            SUPERVISOR_APPROVABLE_ORDERTYPES.Contains(ProductionWorkDescription.OrderType.Id);

        public virtual bool HasComplianceOrderType => ProductionWorkDescription != null && OrderType.COMPLIANCE_ORDER_TYPES.Contains(ProductionWorkDescription.OrderType.Id);

        /// <summary>
        /// This is going to appear in controller actions to determine if we actually send to SAP or not. 
        /// E.g. ProductionWorkOrderController.ProgressSAP/FinalizeSAP
        /// </summary>
        public virtual bool SendToSAP => OperatingCenter.CanSyncWithSAP;

        public virtual bool CanBeSupervisorApproved => (HasSupervisorApprovableOrderType && !ApprovedOn.HasValue &&
                                                        !DateCancelled.HasValue && DateCompleted.HasValue);

        public virtual bool CanBeMaterialApproved
        {
            get
            {
                if (DateCancelled.HasValue || MaterialsApprovedOn.HasValue || HasComplianceOrderType || !ApprovedOn.HasValue || !DateCompleted.HasValue)
                {
                    return false;
                }

                return ProductionWorkOrderMaterialUsed.Any();
            }
        }

        public virtual bool CanBeCompleted => !DateCompleted.HasValue && !DateCancelled.HasValue &&
                                              EmployeeAssignments.Any() &&
                                              EmployeeAssignments.All(x => x.DateEnded != null);

        public virtual bool CanBeCancelled =>
            !MaterialsApprovedOn.HasValue && !DateCancelled.HasValue && !DateCompleted.HasValue;

        public virtual bool CanBeMaterialPlanned => ProductionWorkOrderMaterialUsed.Any() && !DateCompleted.HasValue &&
                                                    !DateCancelled.HasValue && !MaterialsPlannedOn.HasValue;

        public virtual bool CapitalizationCancelsOrder => !EmployeeAssignments.Any(x => x.DateEnded.HasValue);

        public virtual bool LockoutFormRequired => ProductionWorkOrderProductionPrerequisites.Any(x =>
            x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT &&
            !x.SkipRequirement);

        public virtual bool ConfinedSpaceFormRequired => ProductionWorkOrderProductionPrerequisites.Any(x =>
            x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.IS_CONFINED_SPACE && !x.SkipRequirement);

        public virtual bool IsEligibleForRedTagPermit => ProductionWorkOrderProductionPrerequisites.Any(x =>
            x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.RED_TAG_PERMIT &&
            !x.SkipRequirement);

        public virtual bool PreJobSafetyFormRequired => ProductionWorkOrderProductionPrerequisites.Any(x =>
            x.ProductionPrerequisite.Id == ProductionPrerequisite.Indices.PRE_JOB_SAFETY_BRIEF && !x.SkipRequirement);
        
        [View(DisplayNames.ACTUAL_COMPLETION_HOURS)]
        public virtual decimal ActualCompletionHours
        {
            get
            {
                return EmployeeAssignments.Sum(e => e.HoursWorked);
            }
        }

        #endregion

        #region NonDatabaseFields

        [DoesNotExport]
        public virtual bool CompleteMeasurementPoints { get; set; }

        public virtual string CurrentlyAssignedEmployee => CurrentAssignments.LastOrDefault()?.AssignedTo?.ToString();

        // Used in notification templates
        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        #endregion

        #endregion

        public override string ToString()
        {
            return Id.ToString();
        }

        #region Constructors

        public ProductionWorkOrder()
        {
            AssetReliabilities = new HashSet<AssetReliability>();
            ConfinedSpaceForms = new HashSet<ConfinedSpaceForm>();
            CurrentAssignments = new HashSet<EmployeeAssignment>();
            ProductionPreJobSafetyBriefs = new HashSet<ProductionPreJobSafetyBrief>();
            ProductionWorkOrderDocuments = new List<ProductionWorkOrderDocument>();
            ProductionWorkOrderNotes = new List<ProductionWorkOrderNote>();
            //ProductionPrerequisites = new List<ProductionPrerequisite>();
            EmployeeAssignments = new HashSet<EmployeeAssignment>();
            ProductionWorkOrderMaterialUsed = new HashSet<ProductionWorkOrderMaterialUsed>();
            ProductionWorkOrderProductionPrerequisites = new List<ProductionWorkOrderProductionPrerequisite>();
            //AuditLogEntries = new List<AuditLogEntry<ProductionWorkOrder>>();
            JobObservations = new List<JobObservation>();
            Equipments = new HashSet<ProductionWorkOrderEquipment>();
            ProductionWorkOrderMeasurementPointValues = new List<ProductionWorkOrderMeasurementPointValue>();
            ProductionWorkOrderChildNotification = new List<ProductionWorkOrderChildNotification>();
            LockoutForms = new List<LockoutForm>();
            TankInspections = new List<TankInspection>();
            LockoutDevices = new List<LockoutDevice>();
        }

        #endregion
    }
}
