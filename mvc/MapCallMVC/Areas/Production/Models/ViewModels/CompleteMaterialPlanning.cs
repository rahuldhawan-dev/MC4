using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels {
    public class CompleteProductionMaterialPlanning : ViewModel<ProductionWorkOrder>
    {
        public CompleteProductionMaterialPlanning(IContainer container) : base(container) { }

        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            entity.MaterialsPlannedOn = _container.GetInstance<DateTimeProvider>().GetCurrentDate();
            return entity;
        }
    }
}