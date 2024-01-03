using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels {
    public class ProductionWorkOrderMeasurementPointValueViewModel : ViewModel<ProductionWorkOrderMeasurementPointValue>
    {
        public ProductionWorkOrderMeasurementPointValueViewModel(IContainer container) : base(container) { }

        [Required, EntityMap,EntityMustExist(typeof(ProductionWorkOrder))]
        public int? ProductionWorkOrder { get; set; }

        [Required, DropDown, EntityMap,EntityMustExist(typeof(MeasurementPointEquipmentType))]
        public int? MeasurementPointEquipmentType { get; set; }

        [Required,DropDown,EntityMap, EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }

        [Required, StringLength(100)]
        public string Value { get; set; }
    }

    public class CreateProductionWorkOrderMeasurementPointValue : ProductionWorkOrderMeasurementPointValueViewModel
    {
        public CreateProductionWorkOrderMeasurementPointValue(IContainer container) : base(container) { }
    }

    public class EditProductionWorkOrderMeasurementPointValue : ProductionWorkOrderMeasurementPointValueViewModel
    {
        public EditProductionWorkOrderMeasurementPointValue(IContainer container) : base(container) { }
    }
}