using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchEquipmentRiskCharacteristicData : SearchSet<Equipment>
    {
        [MultiSelect]
        [EntityMap, EntityMustExist(typeof(State))]
        [SearchAlias("OperatingCenter", "State.Id")]
        public int[] State { get; set; }

        [EntityMap("criteriaFacility.OperatingCenter", MapDirections.ToPrimary)]
        [MultiSelect("", "OperatingCenter", "ByStateIds", DependsOn = "State", PromptText = "Select a state above")]
        public int[] OperatingCenter { get; set; }

        [SearchAlias("criteriaFacility.PlanningPlant", "Id")]
        [EntityMap, EntityMustExist(typeof(PlanningPlant))]
        [MultiSelect("", "PlanningPlant", "ByOperatingCenters", DependsOn = "OperatingCenter",
            PromptText = "Select a operating center above")]
        public int[] PlanningPlant { get; set; }

        [View("Facility"),
         MultiSelect("", "Facility", "ByPlanningPlants", DependsOn = "PlanningPlant",
             PromptText = "Select a planning plant above"), EntityMustExist(typeof(Facility))]
        public int[] Facility { get; set; }

        [EntityMap, EntityMustExist(typeof(EquipmentType))]
        [MultiSelect]
        public virtual int[] EquipmentType { get; set; }

        [StringLength(Equipment.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }
        
        [View(Equipment.DisplayNames.RISK_CHARACTERISTICS_LAST_UPDATED_ON)]
        public DateRange RiskCharacteristicsLastUpdatedOn { get; set; }
    }
}
