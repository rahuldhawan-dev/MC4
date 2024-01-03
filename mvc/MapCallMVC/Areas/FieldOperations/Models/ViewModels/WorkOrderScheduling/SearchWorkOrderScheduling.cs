using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderScheduling
{
    public class SearchWorkOrderScheduling : SearchWorkOrder
    {
        [DropDown, RequiredWhen(nameof(Id), ComparisonType.EqualTo, null)]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter; 
            set => base.OperatingCenter = value;
        }

        public bool? TrafficControlRequired { get; set; }

        [Search(CanMap = false)]
        public int? MarkoutExpirationDays { get; set; }

        [SearchAlias("Markouts", "ExpirationDate")]
        public DateRange ExpirationDate { get; set; }

        [SearchAlias("WorkDescription", "TimeToComplete")]
        public NumericRange TimeToComplete { get; set; }

        [View(WorkOrder.DisplayNames.HAS_PENDING_ASSIGNMENTS), BoolFormat("True", "False")]
        public bool? HasPendingAssignments { get; set; }
    }
}