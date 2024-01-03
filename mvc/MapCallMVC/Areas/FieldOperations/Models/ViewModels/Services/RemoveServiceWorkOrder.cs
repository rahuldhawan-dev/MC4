using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services
{
    public class RemoveServiceWorkOrder : ViewModel<Service>
    {
        #region Properties

        [DoesNotAutoMap]
        [Required, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        #endregion

        #region Constructors

        public RemoveServiceWorkOrder(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override Service MapToEntity(Service entity)
        {
            var workOrder = entity.WorkOrders.SingleOrDefault(x => x.Id == WorkOrder.Value);
            entity.WorkOrders.Remove(workOrder);
            return entity;
        }

        #endregion
    }
}
