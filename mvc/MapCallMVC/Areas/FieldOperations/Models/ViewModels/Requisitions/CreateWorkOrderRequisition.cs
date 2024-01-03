using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Requisitions
{
    public class CreateWorkOrderRequisition : WorkOrderRequisitionViewModelBase
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        #endregion

        #region Constructor

        public CreateWorkOrderRequisition(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override Requisition MapToEntity(Requisition entity)
        {
            base.MapToEntity(entity);

            entity.WorkOrder.Requisitions.Add(entity);

            return entity;
        }

        #endregion
    }
}