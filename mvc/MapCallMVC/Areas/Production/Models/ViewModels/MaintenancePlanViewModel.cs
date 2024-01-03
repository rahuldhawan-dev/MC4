using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class MaintenancePlanViewModel : ViewModel<MaintenancePlan>
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown(Area = "", Controller = "OperatingCenter", Action = "ByStateId", DependsOn = nameof(State),
            PromptText = "Please select a state above")]
        [Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        [DropDown("", "PlanningPlant", "ByOperatingCenter", DependsOn = nameof(OperatingCenter),
            PromptText = "Please select an operating center above")]
        [Required, EntityMap, EntityMustExist(typeof(PlanningPlant))]
        public int? PlanningPlant { get; set; }

        [DropDown(Area = "", Controller = "Facility", Action = "ByOperatingCenterOrPlanningPlant",
            DependsOn = nameof(OperatingCenter) + "," + nameof(PlanningPlant),
            PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        [Required, EntityMap, EntityMustExist(typeof(Facility))]
        public int? Facility { get; set; }

        [MultiSelect(Area = "", Controller = "FacilityFacilityArea", Action = "ByFacilityIds",
            DependsOn = nameof(Facility), PromptText = "Please select a facility above")]
        [EntityMap, EntityMustExist(typeof(FacilityFacilityArea))]
        public int[] FacilityAreas { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(EquipmentType))]
        [MultiSelect(Area = "", Controller = "EquipmentType", Action = "ByFacilityIds", DependsOn = nameof(Facility),
            PromptText = "Please select a facility in the previous section")]
        public int[] EquipmentTypes { get; set; }

        [EntityMap, EntityMustExist(typeof(EquipmentPurpose))]
        [MultiSelect("", "EquipmentPurpose", "ByEquipmentTypeId", DependsOn = "EquipmentTypes",
            PromptText = "Please select an Equipment Type above")]
        public int[] EquipmentPurposes { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(TaskGroupCategory))]
        public int? TaskGroupCategory { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(TaskGroup))]
        [DropDown(Area = "Production", Controller = "TaskGroup", Action = "ByTaskGroupCategoryIdOrAll",
            DependsOn = nameof(TaskGroupCategory), PromptText = "Please select a task group category above")]
        public int? TaskGroup { get; set; }

        [Required]
        public DateTime? Start { get; set; }

        [Required]
        public bool? IsActive { get; set; }

        [Required]
        public bool? HasACompletionRequirement { get; set; }

        public bool? IsPlanPaused { get; set; }

        [DoesNotAutoMap]
        public string TaskDetails { get; set; }

        [DoesNotAutoMap]
        public string TaskDetailsSummary { get; set; }

        public decimal? Resources { get; set; }

        public decimal? EstimatedHours { get; set; }

        public decimal? ContractorCost { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SkillSet))]
        public int? SkillSet { get; set; }

        [DoesNotAutoMap]
        public string PlanType { get; set; }

        [Multiline, StringLength(MaintenancePlan.StringLengths.ADDITIONAL_TASK_DETAILS)]
        public string AdditionalTaskDetails { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(ProductionWorkOrderFrequency))]
        public int? ProductionWorkOrderFrequency { get; set; }

        public bool HasCompanyRequirement { get; set; }
        public bool HasOshaRequirement { get; set; }
        public bool HasPsmRequirement { get; set; }
        public bool HasRegulatoryRequirement { get; set; }
        public bool HasOtherCompliance { get; set; }

        [RequiredWhen(nameof(HasOtherCompliance), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public string OtherComplianceReason { get; set; }

        [Required, StringLength(MaintenancePlan.StringLengths.LOCAL_TASK_DESCRIPTION)]
        public string LocalTaskDescription { get; set; }

        [DoesNotAutoMap]
        public string Name { get; set; }

        [StringLength(MaintenancePlan.StringLengths.DEACTIVATION_REASON)]
        [RequiredWhen(nameof(IsActive), ComparisonType.EqualTo, false, FieldOnlyVisibleWhenRequired = true)]
        public string DeactivationReason { get; set; }

        [EntityMap, EntityMustExist(typeof(Employee))]
        public int? DeactivationEmployee { get; set; }

        public DateTime? DeactivationDate { get; set; }

        #endregion

        #region Constructors

        public MaintenancePlanViewModel(IContainer container) : base(container) { }

        #endregion
    }
}