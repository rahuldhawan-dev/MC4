using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SiteMap : ClassMap<Site>
    {
        public SiteMap()
        {
            Id(x => x.Id, "SiteId");

            Map(x => x.Name, "SiteName")
               .Length(Site.MAX_NAME_LENGTH)
               .Not.Nullable();

            References(x => x.Project).Not.Nullable();

            HasMany(x => x.Boards);
        }
    }
}
