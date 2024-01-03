using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class ProductionWorkDescriptionViewModel : ViewModel<ProductionWorkDescription>
    {
        #region Properties

        [Required, StringLength(ProductionWorkDescription.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(EquipmentType))]
        public int? EquipmentType { get; set; }

        [DropDown]
        [Required, EntityMap, EntityMustExist(typeof(OrderType))]
        public int? OrderType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PlantMaintenanceActivityType))]
        public int? PlantMaintenanceActivityType { get; set; }

        [Required]
        public bool? BreakdownIndicator { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ProductionSkillSet))]
        public int? ProductionSkillSet { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MaintenancePlanTaskType))]
        public int? MaintenancePlanTaskType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(TaskGroup))]
        public int? TaskGroup { get; set; }

        #endregion

        #region Constructors

        public ProductionWorkDescriptionViewModel(IContainer container) : base(container) { }

        #endregion
    }
}