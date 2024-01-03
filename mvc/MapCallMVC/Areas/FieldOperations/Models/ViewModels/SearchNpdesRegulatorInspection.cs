using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchNpdesRegulatorInspection : SearchSet<NpdesRegulatorInspection>
    {
        #region Properties

        [Required, DropDown, EntityMustExist(typeof(OperatingCenter)), EntityMap]
        [SearchAlias("sm.OperatingCenter", "oc", "Id")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [SearchAlias("sm.Town", "town", "Id")]
        public int? Town { get; set; }

        [DropDown("", "SewerOpening", "ByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [EntityMustExist(typeof(SewerOpening)), EntityMap]
        public int? SewerOpening { get; set; }

        [DropDown("", "BodyOfWater", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [EntityMustExist(typeof(BodyOfWater)), EntityMap]
        [SearchAlias("sm.BodyOfWater", "bow", "Id")]
        public int? BodyOfWater { get; set; }

        [DropDown, EntityMustExist(typeof(NpdesRegulatorInspectionType)), EntityMap]
        public int? NpdesRegulatorInspectionType { get; set; }

        public DateRange DepartureDateTime { get; set; }

        [SearchAlias("SewerOpening", "sm", "OutfallNumber")]
        public string OutfallNumber { get; set; }

        [SearchAlias("SewerOpening", "sm", "LocationDescription")]
        public string LocationDescription { get; set; }

        public bool? IsDischargePresent { get; set; }

        #endregion
    }
}