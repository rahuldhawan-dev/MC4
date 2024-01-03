using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;
using MMSINC.Metadata;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CancelWorkOrder : ViewModel<WorkOrder>
    {
        public CancelWorkOrder(IContainer container) : base(container) { }

        [Required, DropDown, EntityMustExist(typeof(WorkOrderCancellationReason)), EntityMap,
         View(WorkOrder.DisplayNames.WORK_ORDER_CANCELLATION_REASON)]
        public int? WorkOrderCancellationReason { get; set; }

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            base.MapToEntity(entity);
            entity.CancelledAt = _container.GetInstance<DateTimeProvider>().GetCurrentDate();
            entity.AssignedToContractorOn = null;
            entity.AssignedContractor = null;
            return entity;
        }
    }
}
