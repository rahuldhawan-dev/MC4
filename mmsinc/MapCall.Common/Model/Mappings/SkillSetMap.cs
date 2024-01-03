using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SkillSetMap : ClassMap<SkillSet>
    {
        public SkillSetMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Name).Length(SkillSet.StringLengths.NAME).Not.Nullable();
            Map(x => x.Abbreviation).Length(SkillSet.StringLengths.ABBREVIATION).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.Description).Length(SkillSet.StringLengths.DESCRIPTION).Not.Nullable();
        }
    }
}