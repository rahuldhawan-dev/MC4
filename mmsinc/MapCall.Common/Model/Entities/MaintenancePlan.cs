using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities.Excel;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MaintenancePlan : IEntity, IThingWithNotes, IThingWithDocuments
    {
        #region Private Members

        private MaintenancePlanDisplayItem _display;

        #endregion

        #region Constants

        public const string PLAN_NUMBER_PREFIX = "9";

        public struct StringLengths
        {
            public const int PAUSED_PLAN_NOTES = 250,
                             ADDITIONAL_TASK_DETAILS = 500,
                             OTHER_COMPLIANCE_REASON = 255,
                             LOCAL_TASK_DESCRIPTION = 75,
                             WORK_DESCRIPTION = 50,
                             DEACTIVATION_REASON = 150;
        }

        public struct DisplayNames
        {
            public const string PLANNING_PLANT = "District",
                                PLAN_NAME = "Plan Name",
                                PLAN_TYPE = "Plan Type",
                                PRODUCTION_WORK_ORDER_FREQUENCY = "Plan Frequency",
                                TASK_GROUP = "Task Group Name",
                                COMPANY_REQUIREMENT = "Company Requirement",
                                OSHA_REQUIREMENT = "OSHA Requirement",
                                PSM_REQUIREMENT = "Process Safety Management",
                                REGULATORY_REQUIREMENT = "Environmental / Water Quality Regulatory Requirement",
                                OTHER_COMPLIANCE = "Other",
                                OTHER_COMPLIANCE_REASON = "Other Compliance Reason",
                                HAS_COMPLIANCE_REQUIREMENT = "Compliance Plan",
                                HAS_COMPLETION_REQUIREMENT = "Auto Cancel Requirement",
                                RESOURCES = "# of Resources",
                                CONTRACTOR_COST = "Estimated Contractor Cost",
                                SKILL_SET = "Required Skill Set",
                                EQUIPMENT_GROUP = "Equipment Groups";
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual State State { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        [View(DisplayNames.PLANNING_PLANT)]
        public virtual PlanningPlant PlanningPlant { get; set; }

        [View(DisplayNames.TASK_GROUP)]
        public virtual TaskGroup TaskGroup { get; set; }

        public virtual TaskGroupCategory TaskGroupCategory { get; set; }

        [DoesNotExport]
        public virtual ProductionWorkDescription WorkDescription { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime Start { get; set; }

        public virtual bool IsActive { get; set; }

        [View(DisplayNames.PRODUCTION_WORK_ORDER_FREQUENCY)]
        public virtual ProductionWorkOrderFrequency ProductionWorkOrderFrequency { get; set; }

        [View(DisplayNames.HAS_COMPLETION_REQUIREMENT)]
        public virtual bool HasACompletionRequirement { get; set; }

        public virtual decimal ForecastPeriodMultiplier { get; set; }

        public virtual bool IsPlanPaused { get; set; }
        public virtual string PausedPlanNotes { get; set; }

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? PausedPlanResumeDate { get; set; }

        public virtual int TaskPlanId { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string AdditionalTaskDetails { get; set; }

        public virtual Facility Facility { get; set; }

        [View(DisplayNames.COMPANY_REQUIREMENT)]
        public virtual bool HasCompanyRequirement { get; set; }

        [View(DisplayNames.OSHA_REQUIREMENT)]
        public virtual bool HasOshaRequirement { get; set; }

        [View(DisplayNames.PSM_REQUIREMENT)]
        public virtual bool HasPsmRequirement { get; set; }

        [View(DisplayNames.REGULATORY_REQUIREMENT)]
        public virtual bool HasRegulatoryRequirement { get; set; }

        [View(DisplayNames.OTHER_COMPLIANCE)]
        public virtual bool HasOtherCompliance { get; set; }

        [View(DisplayNames.OTHER_COMPLIANCE_REASON), StringLength(StringLengths.OTHER_COMPLIANCE_REASON)]
        public virtual string OtherComplianceReason { get; set; }

        public virtual string LocalTaskDescription { get; set; }

        public virtual string PlanNumber { get; set; }

        [View(DisplayName = DisplayNames.HAS_COMPLIANCE_REQUIREMENT)]
        public virtual bool HasComplianceRequirement { get; set; }

        public virtual CountEquipmentMaintenancePlansByMaintenancePlan CountEquipmentMaintenancePlansByMaintenancePlan { get; set; }

        [View(DisplayNames.RESOURCES)]
        public virtual decimal? Resources { get; set; }
        
        public virtual decimal? EstimatedHours { get; set; }

        [View(DisplayNames.CONTRACTOR_COST), DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public virtual decimal? ContractorCost { get; set; }

        [View(DisplayNames.SKILL_SET)]
        public virtual SkillSet SkillSet { get; set; }

        [DoesNotExport]
        public virtual string DeactivationReason { get; set; }

        [DoesNotExport]
        public virtual Employee DeactivationEmployee { get; set; }

        [DoesNotExport, View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? DeactivationDate { get; set; }

        public virtual IList<FacilityFacilityArea> FacilityAreas { get; set; } = new List<FacilityFacilityArea>();
        public virtual IList<EquipmentType> EquipmentTypes { get; set; } = new List<EquipmentType>();
        public virtual ISet<EquipmentPurpose> EquipmentPurposes { get; set; } = new HashSet<EquipmentPurpose>();
        public virtual IList<Equipment> Equipment { get; set; } = new List<Equipment>();
        public virtual IList<ProductionWorkOrder> ProductionWorkOrders { get; set; } = new List<ProductionWorkOrder>();
        public virtual IList<MaintenancePlanDocument> Documents { get; set; } = new List<MaintenancePlanDocument>();
        public virtual IList<MaintenancePlanNote> Notes { get; set; } = new List<MaintenancePlanNote>();
        public virtual ISet<ScheduledAssignment> ScheduledAssignments { get; set; } = new HashSet<ScheduledAssignment>();

        #region Logical Properties

        [View(DisplayNames.PLAN_NAME)]
        public virtual string Name
        {
            get
            {
                var facilityPortion = string.IsNullOrEmpty(Facility.FacilityName)
                    ? $"Facility {Facility.Id}"
                    : Facility.FacilityName;
                return $"{facilityPortion} : {TaskGroup.TaskGroupName} : {ProductionWorkOrderFrequency.ToString().ToUpper()}";
            }
        }

        public virtual MaintenancePlanTaskType PlanType => TaskGroup.MaintenancePlanTaskType;
        public virtual string TaskDetails => TaskGroup.TaskDetails;
        public virtual string TaskDetailsSummary => TaskGroup.TaskDetailsSummary;
        public virtual string Description => (_display ?? (_display = new MaintenancePlanDisplayItem {
            Id = Id,
            Facility = Facility,
            TaskGroup = TaskGroup,
            ProductionWorkOrderFrequency = ProductionWorkOrderFrequency
        })).Display;

        [DoesNotExport]
        public virtual string TableName => nameof(MaintenancePlan) + "s";

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Map(e => (IDocumentLink)e);
        public virtual IList<INoteLink> LinkedNotes => Notes.Map(e => (INoteLink)e);

        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? LastWorkOrderCompleted => ProductionWorkOrders.Max(x => x.DateCompleted);

        // NEVER run this without FirstOrDefault or it will break and/or result in a large number of dates
        [View(DisplayFormat = CommonStringFormats.DATE)]
        public virtual DateTime? NextWorkOrderDueDate => ProductionWorkOrderFrequency.GetFrequencyDates(DateTime.Today, DateTime.Today.AddYears(16)).FirstOrDefault();

        public virtual string TaskDescription
        {
            get
            {
                var abbrevPortion = TaskGroup.MaintenancePlanTaskType?.Abbreviation ?? ProductionWorkOrderFrequency.Name.ToUpper();
                return $"{abbrevPortion} : {TaskGroup.TaskGroupName}";
            }
        }

        #endregion

        #endregion

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }

    [Serializable]
    public class MaintenancePlanDisplayItem : DisplayItem<MaintenancePlan>
    {
        public Facility Facility { get; set; }
        public TaskGroup TaskGroup { get; set; }
        public ProductionWorkOrderFrequency ProductionWorkOrderFrequency { get; set; }
        public override string Display 
        {
            get
            {
                var facilityPortion = string.IsNullOrEmpty(Facility.FacilityName)
                    ? $"Facility {Facility.Id}"
                    : Facility.FacilityName;
                return $"{facilityPortion} : {TaskGroup.TaskGroupName} : {ProductionWorkOrderFrequency.ToString().ToUpper()}";
            }
        }
    }
}