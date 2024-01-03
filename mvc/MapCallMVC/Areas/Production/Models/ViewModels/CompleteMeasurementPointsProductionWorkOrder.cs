using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CompleteMeasurementPointsProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        [DoesNotAutoMap]
        public string SelectedCompletionEquipmentIds { get; set; }

        public CompleteMeasurementPointsProductionWorkOrder(IContainer container) : base(container) { }
        
        public override ProductionWorkOrder MapToEntity(ProductionWorkOrder entity)
        {
            entity = base.MapToEntity(entity);
            entity.CompleteMeasurementPoints = true;
            
            return entity;
        }
    }
}