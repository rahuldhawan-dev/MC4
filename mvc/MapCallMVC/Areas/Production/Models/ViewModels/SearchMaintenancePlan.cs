using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SearchMaintenancePlan : SearchSet<MaintenancePlan>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown(Area = "", Controller = "OperatingCenter", Action = "ByStateIds", DependsOn = nameof(State), PromptText = "Please select a state above.")]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "PlanningPlant", "ByOperatingCenter", DependsOn = nameof(OperatingCenter), PromptText = "Please select an operating center above")]
        [EntityMap, EntityMustExist(typeof(PlanningPlant))]
        [View(MaintenancePlan.DisplayNames.PLANNING_PLANT)]
        public int? PlanningPlant { get; set; }

        [DropDown(Area = "", Controller = "Facility", Action = "ByOperatingCenterOrPlanningPlant", DependsOn = nameof(OperatingCenter) + "," + nameof(PlanningPlant), PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        [EntityMap, EntityMustExist(typeof(Facility)), SearchAlias("Facility", "Id")]
        public int? Facility { get; set; }

        [EntityMap, DropDown, EntityMustExist(typeof(MaintenancePlanTaskType))]
        [View(MaintenancePlan.DisplayNames.PLAN_TYPE), SearchAlias("TaskGroup", "MaintenancePlanTaskType.Id")]
        public int? MaintenancePlanTaskType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ProductionWorkOrderFrequency))]
        [View(MaintenancePlan.DisplayNames.PRODUCTION_WORK_ORDER_FREQUENCY)]
        public int? ProductionWorkOrderFrequency { get; set; }

        [DropDown, EntityMustExist(typeof(EquipmentGroup)), EntityMap]
        [SearchAlias("EquipmentTypes", "EquipmentGroup.Id")]
        public int? EquipmentGroup { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(EquipmentType))]
        [SearchAlias("EquipmentTypes", "Id")]
        public int[] EquipmentTypes { get; set; }

        [EntityMap, EntityMustExist(typeof(EquipmentPurpose))]
        [MultiSelect("", "EquipmentPurpose", "ByEquipmentTypeId", DependsOn = "EquipmentTypes", PromptText = "Please select an Equipment Type above")]
        [SearchAlias("EquipmentPurposes", "Id")]
        public int[] EquipmentPurposes { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(TaskGroupCategory))]
        public int[] TaskGroupCategory { get; set; }

        [EntityMap, EntityMustExist(typeof(TaskGroup))]
        [MultiSelect(Area = "Production", Controller = "TaskGroup", Action = "ByTaskGroupCategoryIdOrAll",
            DependsOn = nameof(TaskGroupCategory), DependentsRequired = DependentRequirement.None)]
        public int[] TaskGroup { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsPlanPaused { get; set; }
        public string PlanNumber { get; set; }
        public string LocalTaskDescription { get; set; }
        public bool? HasCompanyRequirement { get; set; }
        public bool? HasOshaRequirement { get; set; }
        public bool? HasPsmRequirement { get; set; }
        public bool? HasRegulatoryRequirement { get; set; }
        public bool? HasOtherCompliance { get; set; }

        #endregion
    }
}
