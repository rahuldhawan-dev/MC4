using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class LocalityMap : ClassMap<Locality>
    {
        public LocalityMap()
        {
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code).Length(4).Not.Nullable();
            Map(x => x.Description).Length(30).Not.Nullable();
        }
    }
}
