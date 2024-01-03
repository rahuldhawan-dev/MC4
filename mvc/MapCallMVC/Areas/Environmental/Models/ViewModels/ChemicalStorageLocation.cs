using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class ChemicalStorageLocationViewModel : ViewModel<ChemicalStorageLocation>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(State))]
        public virtual int? State { get; set; }

        [Required, DropDown("", "OperatingCenter", "ByStateIdForEnvironmentalChemicalData", DependsOn = "State", PromptText = "Please select a state above")]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [DropDown("", "PlanningPlant", "ByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMap, EntityMustExist(typeof(PlanningPlant))]
        public virtual int? PlanningPlant { get; set; }

        [DropDown("Environmental", "ChemicalWarehouseNumber", "ByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMap, EntityMustExist(typeof(ChemicalWarehouseNumber))]
        public virtual int? ChemicalWarehouseNumber { get; set; }

        [Required, StringLength(ChemicalStorageLocation.StringLengths.STORAGE_LOCATION_NUMBER)]
        public string StorageLocationNumber { get; set; }

        [Required, StringLength(ChemicalStorageLocation.StringLengths.STORAGE_LOCATION_DESCRIPTION)]
        public string StorageLocationDescription { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        #endregion

        #region Constructors

        public ChemicalStorageLocationViewModel(IContainer container) : base(container) { }

        #endregion
    }
}
