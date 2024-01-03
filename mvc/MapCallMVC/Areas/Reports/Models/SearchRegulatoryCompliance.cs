using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchRegulatoryCompliance : SearchSet<RegulatoryCompliance>, ISearchRegulatoryCompliance
    {
        #region Properties

        [Search(CanMap = false)] // This is used by the controller action to determine pdf rendering.
        public bool? NoPdf { get; set; }

        [MultiSelect]
        [EntityMap, EntityMustExist(typeof(State))]
        [Search(CanMap = false)]
        public int[] State { get; set; } = new int[0];

        [EntityMap("Facility.OperatingCenter", MapDirections.ToPrimary)]
        [MultiSelect("", "OperatingCenter", "ByStateIds", DependsOn = "State", PromptText = "Select a state above")]
        [Search(CanMap = false)]
        public int[] OperatingCenter { get; set; } = new int[0];

        [MultiSelect("", "PlanningPlant", "ByOperatingCenters", DependsOn = "OperatingCenter")]
        [EntityMustExist(typeof(PlanningPlant))]
        [Search(CanMap = false)]
        public int[] PlanningPlant { get; set; } = new int[0];

        [MultiSelect("", "Facility", "ByPlanningPlants", DependsOn = "PlanningPlant"),
         EntityMustExist(typeof(Facility))]
        [Search(CanMap = false)]
        public int[] Facility { get; set; } = new int[0];

        [MultiSelect]
        [EntityMap, EntityMustExist(typeof(EquipmentType))]
        [SearchAlias("equipment.EquipmentType", "Id")]
        public int[] EquipmentType { get; set; } = new int[0];

        [MultiSelect("", "EquipmentPurpose", "ByEquipmentTypeId", DependsOn = "EquipmentType", PromptText = "Please select an Equipment Type above")]
        [SearchAlias("equipment.EquipmentPurpose", "Id")]
        public int[] EquipmentPurpose { get; set; } = new int[0];
        [Search(CanMap = false)]
        public bool? HasProcessSafetyManagement { get; set; }
        [Search(CanMap = false)]
        public bool? HasCompanyRequirement { get; set; }
        [Search(CanMap = false)]
        public bool? HasRegulatoryRequirement { get; set; }
        [Search(CanMap = false)]
        public bool? HasOshaRequirement { get; set; }
        [Search(CanMap = false)]
        public bool? OtherCompliance { get; set; }

        [StringLength(Equipment.StringLengths.DESCRIPTION)]
        [Search(CanMap = false)]
        public string Description { get; set; }

        [Required]
        [Search(CanMap = false)]
        public RequiredDateRange DateReceived { get; set; }
        [UIHint("StringArray"), Search(CanMap = false)]
        public string[] SelectedFacilities { get; set; }
        [UIHint("StringArray"), Search(CanMap = false)]
        public string[] SelectedEquipmentTypes { get; set; }
        [UIHint("StringArray"), Search(CanMap = false)]
        public string[] SelectedEquipmentPurposes { get; set; }
        [UIHint("StringArray"), Search(CanMap = false)]
        public string[] SelectedStates { get; set; }
        [UIHint("StringArray"), Search(CanMap = false)]
        public string[] SelectedOperatingCenters { get; set; }
        [UIHint("StringArray"), Search(CanMap = false)]
        public string[] SelectedPlanningPlants { get; set; }

        #endregion
    }
}