using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentLinkMap : ClassMap<EquipmentLink>
    {
        public EquipmentLinkMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Equipment).Not.Nullable();
            References(x => x.LinkType).Not.Nullable();

            Map(x => x.Url).Not.Nullable();
        }
    }
}
