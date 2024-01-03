using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentLifespanMap : ClassMap<EquipmentLifespan>
    {
        #region Constructors

        public EquipmentLifespanMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Description).Not.Nullable();
            Map(x => x.ExtendedLifeMajor).Nullable();
            Map(x => x.ExtendedLifeMinor).Nullable();
            Map(x => x.EstimatedLifespan).Nullable();
            Map(x => x.IsActive).Nullable();

            HasMany(x => x.EquipmentPurposes).KeyColumn("EquipmentLifespanId");

            HasManyToMany(x => x.TaskGroups)
               .Table("TaskGroupsEquipmentLifespans")
               .ParentKeyColumn("EquipmentLifespanId")
               .ChildKeyColumn("TaskGroupId")
               .Cascade.None();
        }

        #endregion
    }
}
