using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Controllers;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerOverflows
{
    public class SearchSewerOverflow : SearchSet<SewerOverflow>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State)), SearchAlias("OperatingCenter", "State.Id")]
        public int? State { get; set; }

        [MultiSelect("", "OperatingCenter", nameof(OperatingCenterController.ByStateIdOrAll), DependsOn = nameof(State), DependentsRequired = DependentRequirement.None)]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }

        [DropDown("Environmental", "WasteWaterSystem", nameof(WasteWaterSystemController.ByOperatingCenters), DependsOn = nameof(OperatingCenter), PromptText = "Select an operating center above")]
        public int? WasteWaterSystem { get; set; }

        [DropDown("", "Town", nameof(TownController.ByOperatingCenterIds), DependsOn = nameof(OperatingCenter), PromptText = "Please select an operating center")]
        public int? Town { get; set; }

        [DropDown("", "Street", nameof(StreetController.ByTownId), DependsOn = nameof(Town), PromptText = "Please select a town")]
        public int? Street { get; set; }

        public DateRange IncidentDate { get; set; }
        public int? Id { get; set; }

        #endregion
    }
}