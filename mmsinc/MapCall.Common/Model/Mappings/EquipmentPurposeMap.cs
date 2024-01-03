using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentPurposeMap : ClassMap<EquipmentPurpose>
    {
        #region Constructors

        public EquipmentPurposeMap()
        {
            Id(x => x.Id, "EquipmentPurposeID");

            References(x => x.EquipmentCategory).Column("CategoryID").Fetch.Join();
            References(x => x.EquipmentLifespan).Column("EquipmentLifespanId");
            References(x => x.EquipmentSubCategory).Column("SubCategoryID").Fetch.Join();
            References(x => x.EquipmentType);

            Map(x => x.Description).Not.Nullable();
            Map(x => x.Abbreviation).Not.Nullable();

            Map(x => x.HasNoEquipmentType)
               .Formula("(CASE WHEN EquipmentTypeId IS NULL THEN 1 ELSE 0 END)").ReadOnly();

            HasMany(x => x.Equipment).KeyColumn("TypeID");

            HasManyToMany(x => x.TaskGroups)
               .Table("TaskGroupsEquipmentPurposes")
               .ParentKeyColumn("EquipmentPurposeId")
               .ChildKeyColumn("TaskGroupId")
               .Cascade.None();
        }

        #endregion
    }
}
