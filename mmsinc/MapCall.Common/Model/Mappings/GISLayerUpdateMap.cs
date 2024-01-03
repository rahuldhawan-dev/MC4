using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class GISLayerUpdateMap : ClassMap<GISLayerUpdate>
    {
        public const int MAP_ID_LENGTH = 32;

        public GISLayerUpdateMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.CreatedBy).Not.Nullable();

            Map(x => x.Updated).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.MapId).Length(MAP_ID_LENGTH).Not.Nullable();
        }
    }
}
