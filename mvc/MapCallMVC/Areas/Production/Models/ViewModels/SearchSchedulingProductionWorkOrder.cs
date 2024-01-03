using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SearchSchedulingProductionWorkOrder : SearchProductionWorkOrderBase
    {
        #region Properties

        // Greg - 1/14/20 - Needed to over ride the drop down cascade from SearchProductionWorkOrder
        // so we need to initialize an empty dropdown
        [RequiredWhen("PlanNumber", ComparisonType.EqualTo, null)]
        [DropDown("", "OperatingCenter", "ByStateIdForProductionWorkManagement", DependsOn = "State", PromptText = "Please select a state above")]
        public virtual int? OperatingCenter { get; set; }

        [MultiSelect("Environmental", "WasteWaterSystem", "ByOperatingCenter", DependsOn = nameof(OperatingCenter), PromptText = "Please select an operating center")]
        [EntityMap, EntityMustExist(typeof(WasteWaterSystem))]
        [SearchAlias("Facility", "wws", "WasteWaterSystem.Id")]
        public virtual int[] WWSID { get; set; }

        [EntityMap, EntityMustExist(typeof(WasteWaterSystemBasin))]
        [DropDown("Environmental", "WasteWaterSystemBasin", "ByWasteWaterSystem", DependsOn = "WWSID", PromptText = "Please select a WWSID above")]
        [SearchAlias("Facility", "WasteWaterSystemBasin.Id")]
        public virtual int? WWSIDBasin { get; set; }

        [EntityMap, EntityMustExist(typeof(ProductionWorkDescription))]
        public virtual int[] ProductionWorkDescription { get; set; }

        [EntityMap, EntityMustExist(typeof(MaintenancePlan))]
        [SearchAlias("MaintenancePlan", "Id", Required = true)]
        public override int? MaintenancePlan { get; set; }

        [EntityMap]
        [MultiSelect("Production", "TaskGroup", "ByTaskTypesForTaskGroupNames", DependsOn = "TaskType", PromptText = "Please select a task type")]
        [SearchAlias("MaintenancePlan.TaskGroup", "TaskGroupName")]
        public virtual string[] TaskGroupName { get; set; }

        [EntityMap, EntityMustExist(typeof(TaskGroup))]
        [SearchAlias("MaintenancePlan.TaskGroup", "Id", Required = true)]
        public virtual int? TaskGroup { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(MaintenancePlanTaskType))]
        [SearchAlias("MaintenancePlan.TaskGroup", "MaintenancePlanTaskType.Id")]
        public virtual int[] TaskType { get; set; }

        [SearchAlias("MaintenancePlan", "PlanNumber")]
        public override string PlanNumber { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an Operating Center above")]
        [EntityMap]
        [EntityMustExist(typeof(Facility))]
        public override int? Facility { get; set; }

        #endregion

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            mapper.MappedProperties[nameof(DateCancelled)].Value = SearchMapperSpecialValues.IsNull;
            mapper.MappedProperties[nameof(DateCompleted)].Value = SearchMapperSpecialValues.IsNull;
        }

        #endregion
    }
}