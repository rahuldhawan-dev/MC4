using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System;

namespace MapCall.Common.Model.ViewModels
{
    public class ProductionWorkOrderPerformanceResultViewModel
    {
        private decimal PercentageOfCreated(int count)
        {
            return (decimal)count / NumberCreated;
        }

        public string OrderType { get; set; }
        public int OrderTypeId { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }
        public string OperatingCenter { get; set; }
        public int? OperatingCenterId { get; set; }
        public string PlanningPlant { get; set; }
        public int? PlanningPlantId { get; set; }
        public string Facility { get; set; }
        public int? FacilityId { get; set; }

        [View("# of WO's Created")]
        public int NumberCreated { get; set; }

        [View("# of WO's Unscheduled")]
        public int NumberUnscheduled { get; set; }

        [View("# of WO's Scheduled")]
        public int NumberScheduled { get; set; }

        [View("# of WO's Incomplete")]
        public int NumberIncomplete { get; set; }

        [View("# of WO's Canceled")]
        public int NumberCanceled { get; set; }

        [View("# of WO's Completed")]
        public int NumberCompleted { get; set; }

        [View("# of WO's Not Approved")]
        public int NumberNotApproved { get; set; }

        [View("% of WO's Unscheduled", FormatStyle.Percentage)]
        public decimal PercentUnscheduled => PercentageOfCreated(NumberUnscheduled);

        [View("% of WO's Canceled", FormatStyle.Percentage)]
        public decimal PercentCanceled => PercentageOfCreated(NumberCanceled);

        [View("% of WO's Completed", FormatStyle.Percentage)]
        public decimal PercentCompleted => PercentageOfCreated(NumberCompleted);
    }

    public class ProductionWorkOrderNotification
    {
        public ProductionWorkOrder ProductionWorkOrder { get; set; }
        public string RecordUrl { get; set; }
    }
    
    public class ProductionWorkOrderExcelItem
    {
        public string Id { get; set; }
        public string OperatingCenter { get; set; }
        public string PlanningPlant { get; set; }
        public string Facility { get; set; }
        public string FacilityArea { get; set; }
        public string FunctionalLocation { get; set; }
        public string EquipmentType { get; set; }
        public string Equipment { get; set; }
        public string Coordinate { get; set; }
        public string Priority { get; set; }
        public string IsOpen { get; set; }
        public string WorkDescription { get; set; }
        public string AirPermit { get; set; }
        public string HasLockoutRequirement { get; set; }
        public string IsEligibleForRedTagPermit { get; set; }
        public string HotWork { get; set; }
        public string IsConfinedSpace { get; set; }
        public string JobSafetyChecklist { get; set; }
        public string CapitalizedFrom { get; set; }
        public string RequestedBy { get; set; }
        public string Notes { get; set; }
        public string DateReceived { get; set; }
        public string BreakdownIndicator { get; set; }
        public string SAPWorkOrder { get; set; }
        public string SAPStatus { get; set; }
        public string SAPNotificationNumber { get; set; }
        public string WBSElement { get; set; }
        public string CapitalizationReason { get; set; }
        public string DateCompleted { get; set; }
        public string CompletedBy { get; set; }
        public string ApprovedOn { get; set; }
        public string ApprovedBy { get; set; }
        public string MaterialsApprovedOn { get; set; }
        public string MaterialsPlannedOn { get; set; }
        public string MaterialsApprovedBy { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public string BasicStart { get; set; }

        public string DateCancelled { get; set; }
        public string CancellationReason { get; set; }

        public string PlantMaintenanceActivityTypeOverride { get; set; }

        public string CorrectiveOrderProblemCode { get; set; }

        public string OtherProblemNotes { get; set; }
        public string ActionCode { get; set; }
        public string FailureCode { get; set; }
        public string CauseCode { get; set; }
        public string ProductionWorkOrderRequiresSupervisorApproval { get; set; }
        public string MaterialsApproved { get; set; }
        public string Status { get; set; }
        public string OrderType { get; set; }
        public string SendToSap { get; set; }
        public string CanBeSupervisorApproved { get; set; }
        public string CanBeMaterialApproved { get; set; }
        public string CanBeCompleted { get; set; }
        public string CanBeCancelled { get; set; }
        public string CanBeMaterialPlanned { get; set; }
        public string CapitalizationCancelsOrder { get; set; }
        public string CurrentlyAssignedEmployee { get; set; }
        public string LockoutFormCreated { get; set; }
        public string ConfinedSpaceFormCreated { get; set; }
        public string LockoutForms { get; set; }
        public string RedTagPermit { get; set; }
        public string LockoutDevices { get; set; }
        public string RedTagPermitCreated { get; set; }
        public string EstimatedCompletionHours { get; set; }
        public string ActualCompletionHours { get; set; }
        public string TankInspections { get; set; }
        public string PlanNumber { get; set; }
        public string LocalTaskDescription { get; set; }
        public string AutoCreatedCorrectiveWorkOrder { get; set; }
    }

    public interface ISearchProductionWorkOrderPerformance : ISearchSet<ProductionWorkOrderPerformanceResultViewModel>
    {
        int[] State { get; set; }
        int[] OperatingCenter { get; set; }
        int[] PlanningPlant { get; set; }
        int[] Facility { get; set; }
        RequiredDateRange DateReceived { get; set; }
        int[] OrderType { get; set; }

        string[] SelectedOrderTypes { get; set; }
        string[] SelectedFacilities { get; set; }
        string[] SelectedOperatingCenters { get; set; }
        string[] SelectedPlanningPlants { get; set; }
        string[] SelectedStates { get; set; }
    }

    public interface ISearchProductionWorkOrder : ISearchSet<ProductionWorkOrder>
    {
        [Search(CanMap = false)]
        int? Equipment { get; set; }
        [Search(CanMap = false)]
        int? SAPEquipmentId { get; set; }
        [Search(CanMap = false)]
        bool? HasProcessSafetyManagement { get; set; }
        [Search(CanMap = false)]
        bool? HasCompanyRequirement { get; set; }
        [Search(CanMap = false)]
        bool? HasRegulatoryRequirement { get; set; }
        [Search(CanMap = false)]
        bool? HasOshaRequirement { get; set; }
        [Search(CanMap = false)]
        bool? OtherCompliance { get; set; }
    }

    public interface ISearchProductionWorkOrderHistory : ISearchSet<ProductionWorkOrder>
    {
        [Search(CanMap = false)]
        int? OperatingCenterId { get; set; }

        [Search(CanMap = false)]
        int? Equipment { get; set; }
    }
}
