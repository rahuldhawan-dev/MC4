using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TaskGroupMap : ClassMap<TaskGroup>
    {
        public TaskGroupMap()
        {
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.TaskGroupId).Length(TaskGroup.StringLengths.TASK_GROUP_ID).Nullable();
            Map(x => x.TaskGroupName).Length(TaskGroup.StringLengths.TASK_GROUP_NAME).Not.Nullable();
            Map(x => x.TaskDetails).Length(int.MaxValue).Nullable();
            Map(x => x.TaskDetailsSummary).Length(TaskGroup.StringLengths.TASK_DETAILS_SUMMARY).Nullable();
            References(x => x.TaskGroupCategory).Nullable();
            References(x => x.MaintenancePlanTaskType).Nullable();

            HasManyToMany(x => x.EquipmentTypes)
               .Table("TaskGroupsEquipmentTypes")
               .ParentKeyColumn("TaskGroupId")
               .ChildKeyColumn("EquipmentTypeId")
               .Cascade.None();

            HasManyToMany(x => x.EquipmentLifespans)
               .Table("TaskGroupsEquipmentLifespans")
               .ParentKeyColumn("TaskGroupId")
               .ChildKeyColumn("EquipmentLifespanId")
               .Cascade.None();

            HasManyToMany(x => x.EquipmentPurposes)
               .Table("TaskGroupsEquipmentPurposes")
               .ParentKeyColumn("TaskGroupId")
               .ChildKeyColumn("EquipmentPurposeId")
               .Cascade.None();
        }
    }
}
