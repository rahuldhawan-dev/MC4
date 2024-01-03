using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class RejectProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        public RejectProductionWorkOrder(IContainer container) : base(container) { }

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            entity.DateCompleted = null;
            entity.CompletedBy = null;
            return base.MapToEntity(entity);
        }
    }
}