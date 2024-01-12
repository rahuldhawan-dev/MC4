using System.Diagnostics;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.ViewModels
{
    public class FieldCompletedBacklogQAReportItem
    {
        public string OperatingCenter { get; set; }
        public int AwaitingApprovalOrdersWithMaterials { get; set; }
        public int AwaitingApprovalOrdersWithoutMaterials { get; set; }
        public int AwaitingStockToIssue { get; set; }

        public int AwaitingApprovalTotalOrders => AwaitingApprovalOrdersWithMaterials + AwaitingApprovalOrdersWithoutMaterials; 
    }

    public interface ISearchFieldCompletedBacklogQAReport : ISearchSet<FieldCompletedBacklogQAReportItem>
    {
        int[] OperatingCenter { get; set; }
        RequiredDateRange DateCompleted { get; set; }
    }

    [DebuggerDisplay(
        "State: {State}, OperatingCenter: {OperatingCenter}, WorkDescription: {WorkDescription}, WorkOrderCount: {WorkOrderCount}, WorkOrdersWithPreJobSafetyBriefCount: {WorkOrdersWithPreJobSafetyBriefCount}")]
    public class CompletedWorkOrderWithPreJobSafetyBriefReportItem
    {
        public string State { get; set; }
        public string OperatingCenter { get; set; }
        public int OperatingCenterId { get; set; }
        public string WorkDescription { get; set; }
        public int WorkDescriptionId { get; set; }
        public int WorkOrderCount { get; set; }
        public int WorkOrdersWithPreJobSafetyBriefCount { get; set; }

        public int WorkOrdersWithoutPreJobSafetyBriefCount => WorkOrderCount - WorkOrdersWithPreJobSafetyBriefCount;

        [View(FormatStyle.Percentage)]
        public decimal PercentageWithSafetyBrief => (decimal)WorkOrdersWithPreJobSafetyBriefCount / WorkOrderCount;
    }

    [DebuggerDisplay(
        "State: {State}, OperatingCenter: {OperatingCenter}, WorkDescription: {WorkDescription}, WorkOrderCount: {WorkOrderCount}, WorkOrdersWithJobSiteCheckListCount: {WorkOrdersWithJobSiteCheckListCount}")]
    public class CompletedWorkOrderWithJobSiteCheckListReportItem
    {
        public string State { get; set; }
        public string OperatingCenter { get; set; }
        public int OperatingCenterId { get; set; }
        public string WorkDescription { get; set; }
        public int WorkDescriptionId { get; set; }
        public int WorkOrderCount { get; set; }
        public int WorkOrdersWithJobSiteCheckListCount { get; set; }

        public int WorkOrdersWithoutJobSiteCheckListCount => WorkOrderCount - WorkOrdersWithJobSiteCheckListCount;

        [View(FormatStyle.Percentage)]
        public decimal PercentageWithJobSiteCheckList => (decimal)WorkOrdersWithJobSiteCheckListCount / WorkOrderCount; 
    }

    public class CompletedWorkOrderWithMaterialReportItem
    {
        public string State { get; set; }
        public string OperatingCenter { get; set; }
        public int OperatingCenterId { get; set; }
        public string WorkDescription { get; set; }
        public int WorkDescriptionId { get; set; }
        public int WorkOrderCount { get; set; }
        public int WorkOrdersWithMaterialCount { get; set; }
        public int WorkOrdersWithoutMaterialCount => (WorkOrderCount - WorkOrdersWithMaterialCount);

        [View(FormatStyle.Percentage)]
        public decimal PercentageWithMaterial => (decimal)WorkOrdersWithMaterialCount / WorkOrderCount;
    }

    public class WaterLossSearchResultViewModel
    {
        public string OperatingCenterCodeName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string BusinessUnit { get; set; }
        public string WorkDescriptionDescription { get; set; }
        public long TotalGallons { get; set; }
        public int WorkOrderCount { get; set; }
        public int? OperatingCenter { get; set; }
        public int? WorkDescription { get; set; }
    }

    public interface ISearchWaterLoss : ISearchSet<WaterLossSearchResultViewModel>
    {
        RequiredDateRange Date { get; set; }
        int[] OperatingCenter { get; set; }
    }

    public interface
        ISearchCompletedWorkOrdersWithPreJobSafetyBriefs : ISearchSet<CompletedWorkOrderWithPreJobSafetyBriefReportItem>
    {
        int? State { get; set; }
        int[] OperatingCenter { get; set; }
        int[] WorkDescription { get; set; }
        RequiredDateRange DateCompleted { get; set; }
        bool? IsAssignedContractor { get; set; }
    }

    public interface
        ISearchCompletedWorkOrdersWithJobSiteCheckLists : ISearchSet<CompletedWorkOrderWithJobSiteCheckListReportItem>
    {
        int? State { get; set; }
        int[] OperatingCenter { get; set; }
        int[] WorkDescription { get; set; }
        RequiredDateRange DateCompleted { get; set; }
        bool? IsAssignedContractor { get; set; }
    }

    public interface ISearchCompletedWorkOrdersWithMaterial : ISearchSet<CompletedWorkOrderWithMaterialReportItem>
    {
        int? State { get; set; }
        int[] OperatingCenter { get; set; }
        int[] WorkDescription { get; set; }
        RequiredDateRange DateCompleted { get; set; }
        bool? IsAssignedContractor { get; set; }
    }

    public interface ISearchCompletedWorkOrdersWithMarkout : ISearchSet<CompletedWorkOrderWithMarkoutReportItem>
    {
        int? State { get; set; }
        int[] OperatingCenter { get; set; }
        int[] WorkDescription { get; set; }
        RequiredDateRange DateCompleted { get; set; }
        bool? IsAssignedContractor { get; set; }
    }

    public class CompletedWorkOrderWithMarkoutReportItem
    {
        public string State { get; set; }
        public string OperatingCenter { get; set; }
        public int OperatingCenterId { get; set; }
        public string WorkDescription { get; set; }
        public int WorkDescriptionId { get; set; }
        public int WorkOrderCount { get; set; }

        [DoesNotExport]
        public int MarkoutRequirementId { get; set; }

        [DoesNotExport]
        public string MarkoutRequirementDescription { get; set; }

        public int MarkoutRequirementCount { get; set; }
        public int MarkoutNoneCount { get; set; }
        public int MarkoutRoutineCount { get; set; }
        public int MarkoutEmergencyCount { get; set; }

        [View(FormatStyle.Percentage)]
        public decimal PercentageNone => (decimal)MarkoutNoneCount / WorkOrderCount;

        [View(FormatStyle.Percentage)]
        public decimal PercentageRoutine => (decimal)MarkoutRoutineCount / WorkOrderCount;

        [View(FormatStyle.Percentage)]
        public decimal PercentageEmergency => (decimal)MarkoutEmergencyCount / WorkOrderCount;
    }

    public interface ISearchMainBreakRepairsForGIS : ISearchSet<WorkOrder>
    {
        DateRange DateCompleted { get; set; }

        bool? RecentOrders { get; set; }
    }

    public interface ISearchIncompleteWorkOrder : ISearchSet<WorkOrder>
    {
        int? WorkDescription { get; set; }
    }
}
