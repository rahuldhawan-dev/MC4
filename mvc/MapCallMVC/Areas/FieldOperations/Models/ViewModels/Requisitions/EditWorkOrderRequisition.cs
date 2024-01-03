using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Requisitions
{
    public class EditWorkOrderRequisition : WorkOrderRequisitionViewModelBase
    {
        #region Constructor

        public EditWorkOrderRequisition(IContainer container) : base(container) { }

        #endregion
    }
}