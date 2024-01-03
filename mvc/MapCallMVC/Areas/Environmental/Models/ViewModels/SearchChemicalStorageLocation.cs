using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class SearchChemicalStorageLocation : SearchSet<ChemicalStorageLocation>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State)), Search(CanMap = false)]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ByStateIdForEnvironmentalChemicalData", DependsOn = "State", PromptText = "Please select a state above")]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [RequiredWhen("State", ComparisonType.NotEqualTo, null, ErrorMessage = "Required with State")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "PlanningPlant", "ByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? PlanningPlant { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ChemicalWarehouseNumber))]
        public int? ChemicalWarehouseNumber { get; set; }

        public SearchString StorageLocationNumber { get; set; }

        public SearchString StorageLocationDescription { get; set; }

        public bool? IsActive { get; set; }

        #endregion
    }
}
