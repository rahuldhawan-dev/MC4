using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ConfinedSpaceFormHazardMap : ClassMap<ConfinedSpaceFormHazard>
    {
        public ConfinedSpaceFormHazardMap()
        {
            Id(x => x.Id);
            Map(x => x.Notes).Length(ConfinedSpaceFormHazard.StringLengths.NOTES).Not.Nullable();
            References(x => x.ConfinedSpaceForm).Not.Nullable();
            References(x => x.HazardType, "ConfinedSpaceFormHazardTypeId").Not.Nullable();
        }
    }
}
