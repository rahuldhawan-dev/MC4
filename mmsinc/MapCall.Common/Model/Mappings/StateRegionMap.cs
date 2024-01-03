using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StateRegionMap : ClassMap<StateRegion>
    {
        public StateRegionMap()
        {
            Table("StateRegions");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Region).Not.Nullable();

            References(x => x.State).Not.Nullable();
        }
    }
}
