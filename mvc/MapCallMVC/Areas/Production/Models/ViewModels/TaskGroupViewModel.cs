using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class TaskGroupViewModel : ViewModel<TaskGroup>
    { 
        #region Properties
        [Required, StringLength(TaskGroup.StringLengths.TASK_GROUP_ID)]
        public string TaskGroupId { get; set; }

        [Required, StringLength(TaskGroup.StringLengths.TASK_GROUP_NAME)]
        public string TaskGroupName { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(TaskGroupCategory))]
        public int? TaskGroupCategory { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(MaintenancePlanTaskType))]
        public int? MaintenancePlanTaskType { get; set; }

        [Multiline]
        public string TaskDetails { get; set; }

        [Multiline, StringLength(TaskGroup.StringLengths.TASK_DETAILS_SUMMARY)]
        public string TaskDetailsSummary { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(EquipmentLifespan))]
        public int[] EquipmentLifespans { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(EquipmentPurpose))]
        public int[] EquipmentPurposes { get; set; }

        #endregion

        #region Constructors

        public TaskGroupViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchTaskGroup : SearchSet<TaskGroup>
    {
        public string TaskGroupId { get; set; }
        public string TaskGroupName { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MaintenancePlanTaskType))]
        public int? MaintenancePlanTaskType { get; set; }
    }
}