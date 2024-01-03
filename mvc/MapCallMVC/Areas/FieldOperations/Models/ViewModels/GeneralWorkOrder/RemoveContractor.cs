using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    public class RemoveContractor : ViewModel<WorkOrder>
    {
        #region Constructors

        public RemoveContractor(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            base.MapToEntity(entity);
            entity.AssignedContractor = null;

            return entity;
        }

        #endregion
    }
}