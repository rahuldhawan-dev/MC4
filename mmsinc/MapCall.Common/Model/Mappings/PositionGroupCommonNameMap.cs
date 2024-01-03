using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PositionGroupCommonNameMap : ClassMap<PositionGroupCommonName>
    {
        public PositionGroupCommonNameMap()
        {
            Id(x => x.Id).Not.Nullable().GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable().Length(50);

            HasMany(x => x.PositionGroups).KeyColumn("PositionGroupCommonNameId");

            HasManyToMany(x => x.TrainingRequirements)
               .Table("PositionGroupCommonNamesTrainingRequirements")
               .ParentKeyColumn("PositionGroupCommonNameId")
               .ChildKeyColumn("TrainingRequirementID");
        }
    }
}
