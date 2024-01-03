using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchFacilityRiskCharacteristicData : SearchSet<Facility>
    {
        [View("State"), EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("Town", "T", "State.Id")]
        [MultiSelect]
        public int[] TownState { get; set; }

        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [MultiSelect("", "OperatingCenter", "ByStateIds", DependsOn = "TownState", PromptText = "Select a state above")]
        public int[] OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(PlanningPlant))]
        [MultiSelect("", "PlanningPlant", "ByOperatingCenters", DependsOn = "OperatingCenter",
            PromptText = "Select a operating center above")]
        public int[] PlanningPlant { get; set; }

        [View("Facility"),
         MultiSelect("", "Facility", "ByPlanningPlants", DependsOn = "PlanningPlant",
             PromptText = "Select a planning plant above"), EntityMustExist(typeof(Facility))]
        public int[] Id { get; set; }
    }
}