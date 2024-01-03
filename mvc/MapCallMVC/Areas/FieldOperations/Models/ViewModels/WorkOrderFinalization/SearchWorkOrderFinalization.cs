using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderFinalization
{
    public class SearchWorkOrderFinalization : SearchWorkOrder
    {
        [DropDown, RequiredWhen(nameof(Id), ComparisonType.EqualTo, null)]
        public override int? OperatingCenter
        {
            get => base.OperatingCenter; 
            set => base.OperatingCenter = value;
        }
    }
}
