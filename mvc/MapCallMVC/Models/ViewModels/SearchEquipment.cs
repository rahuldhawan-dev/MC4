using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchEquipment : SearchSet<Equipment>, MapCall.Common.Model.ViewModels.ISearchEquipment
    {
        #region Properties

        public int? EntityId { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("OperatingCenter", "State.Id")]
        public int? State { get; set; }

        [DropDown("","OperatingCenter", "ByStateIdForProductionEquipment", DependsOn = "State"), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [SearchAlias("criteriaFacility.PlanningPlant", "Id")]
        [DropDown(Area = "", Controller = "PlanningPlant", Action = "ByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public virtual int? PlanningPlant { get; set; }

        [DropDown(Area = "", Controller = "Facility", Action = "ActiveByOperatingCenterOrPlanningPlant", DependsOn = "OperatingCenter,PlanningPlant", PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        public int? Facility { get; set; }

        //This field is needed for the search by Facility Area and Facility Sub Area to work as this alias is needed to join Facility Area and Facility Sub Area tables to Equipment table to filer the results
        [SearchAlias("FacilityFacilityArea", "ffa", "Id", Required = true)]
        public virtual int? FacilityFacilityArea { get; set; }

        //Please note that the Require = true should not be remove from below as it is needed for the sort by Facility Area to work in the results 
        [SearchAlias("ffa.FacilityArea", "area", "Id", Required = true)]
        [DropDown(Area = "", Controller = "FacilityFacilityArea", Action = "FacilityAreaByFacilityId", DependsOn = "Facility", PromptText = "Please select Facility above")]
        [View("Facility Area")]
        public virtual int? FacilityArea { get; set; }

        //Please note that the Require = true should not be remove from below as it is needed for the sort by Facility Sub Area to work in the results 
        [SearchAlias("ffa.FacilitySubArea", "subArea", "Id", Required = true)]
        [DropDown(Area = "", Controller = "FacilityFacilityArea", Action = "FacilitySubAreaByFacilityAreaId", DependsOn = "Facility,FacilityArea", PromptText = "Please select Facility area above")]
        [View("Facility Sub Area")]
        public virtual int? FacilitySubArea { get; set; }
        
        [DropDown, SearchAlias("criteriaFacility.Department", "Id")]
        public virtual int? Department { get; set; }

        [StringLength(Equipment.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        [MultiSelect, EntityMustExist(typeof(EquipmentGroup)), EntityMap]
        [SearchAlias("EquipmentType", "EquipmentGroup.Id")] 
        public int[] EquipmentGroup { get; set; }

        [MultiSelect("", "EquipmentType", "ByEquipmentGroupId", DependsOn = "EquipmentGroup", PromptText = "Please select an Equipment Group above")]
        public int[] EquipmentType { get; set; }
        [MultiSelect("", "EquipmentPurpose", "ByEquipmentTypeId", DependsOn = "EquipmentType", PromptText = "Please select an Equipment Type above")]
        public int[] EquipmentPurpose { get; set; }

        [DisplayName("Equipment Manufacturer"), EntityMustExist(typeof(EquipmentManufacturer)), EntityMap]
        [DropDown("", "EquipmentManufacturer", "ByEquipmentTypeId", DependsOn = "EquipmentType", PromptText = "Please select an Equipment Type above")]
        public virtual int? EquipmentManufacturer { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(EquipmentStatus))]
        public int[] EquipmentStatus { get; set; }

        public int? SAPEquipmentId { get; set; }

        [StringLength(Equipment.StringLengths.FUNCTIONAL_LOCATION)]
        [UIHint("FunctionalLocation")]
        public virtual string FunctionalLocation { get; set; }

        public bool? HasSensorAttached { get; set; }

        public bool? HasProcessSafetyManagement { get; set; }

        public bool? HasCompanyRequirement { get; set; }

        public bool? HasRegulatoryRequirement { get; set; }

        public bool? HasOshaRequirement { get; set; }

        public bool? OtherCompliance { get; set; }

        [View("Has Tank Inspection Form"), Search(ChecksExistenceOfChildCollection = true)]
        public bool? TankInspections { get; set; }

        public string WBSNumber { get; set; }

        public bool? HasNoSAPEquipmentId { get; set; }

        public bool? IsSignedOffByAssetControl { get; set; }

        public bool? Portable { get; set; }

        public decimal? ArcFlashHierarchy { get; set; }

        public string ArcFlashRating { get; set; }

        [Display(Name = "Created On")]
        public DateRange CreatedAt { get; set; }

        public DateRange UpdatedAt { get; set; } = null;

        [Search(CanMap = true)]
        public bool? HasOpenLockoutForms { get; set; }

        [CheckBox]
        public bool? HasOpenRedTagPermits { get; set; }

        [View("Has Well Tests"), Search(ChecksExistenceOfChildCollection = true)]
        public bool? WellTests { get; set; }

        // These are needed for the partial index search to wire up to 
        // a custom repository method.
        [Search(CanMap = false)]
        public int? OriginalEquipmentId { get; set; }

        [Search(CanMap = false)]
        public int? NotEqualEntityId { get; set; }

        #endregion
    }
}
