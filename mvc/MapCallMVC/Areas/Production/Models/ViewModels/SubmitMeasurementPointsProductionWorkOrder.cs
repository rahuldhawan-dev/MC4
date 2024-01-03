using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SubmitMeasurementPointsProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        [DoesNotAutoMap("Done in controller")]
        public string SelectedEquipmentIds { get; set; }

        public SubmitMeasurementPointsProductionWorkOrder(IContainer container) : base(container) { }
    }
}