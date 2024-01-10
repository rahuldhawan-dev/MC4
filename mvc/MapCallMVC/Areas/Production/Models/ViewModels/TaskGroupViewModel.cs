using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Data.NHibernate;

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

        [MultiSelect, EntityMustExist(typeof(EquipmentGroup)), EntityMap(MapDirections.None)]
        public int[] EquipmentGroup { get; set; }

        [MultiSelect("", "EquipmentType", "ByEquipmentGroupId", DependsOn = "EquipmentGroup", PromptText = "Please select an Equipment Group above")]
        [EntityMap, EntityMustExist(typeof(EquipmentType))]
        public int[] EquipmentTypes { get; set; }

        [MultiSelect("", "EquipmentPurpose", "ByEquipmentTypeId", DependsOn = "EquipmentTypes", PromptText = "Please select an Equipment Type above")]
        [EntityMap, EntityMustExist(typeof(EquipmentPurpose))]
        public int[] EquipmentPurposes { get; set; }
        
        #endregion

        #region Exposed Methods

        public override void Map(TaskGroup entity)
        {
            base.Map(entity);
            //Get the Equipment Group from Equipment Types as Equipment groups are not persisted
            if (entity.EquipmentTypes.Count > 0)
            {
                int[] equipmentTypesIds = entity.EquipmentTypes.Select(x => x.Id).ToArray();
                EquipmentGroup = _container.GetInstance<IRepository<EquipmentType>>().Where(x => equipmentTypesIds.Contains(x.Id)).Select(t => t.EquipmentGroup.Id).Distinct().ToArray();
            }
        }

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