using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services
{
    public class AddServiceWorkOrder : ViewModel<Service>
    {
        #region Properties

        [DropDown]
        [EntityMap(MapDirections.None), EntityMustExist(typeof(WorkOrder)), Required]
        public int? WorkOrder { get; set; }

        #endregion

        #region Constructors

        public AddServiceWorkOrder(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override Service MapToEntity(Service entity)
        {
            var workOrder = _container.GetInstance<IWorkOrderRepository>().Find(WorkOrder.Value);
            if (workOrder.Service == null && (workOrder.AssetType.Id == AssetType.Indices.SERVICE || workOrder.AssetType.Id == AssetType.Indices.SEWER_LATERAL) &&
                !entity.WorkOrders.Contains(workOrder))
            {
                entity.WorkOrders.Add(workOrder);
                workOrder.ServiceNumber = entity.ServiceNumber.ToString();
                workOrder.PremiseNumber = entity.PremiseNumber;
            }
            return entity;
        }

        #endregion
    }
}
