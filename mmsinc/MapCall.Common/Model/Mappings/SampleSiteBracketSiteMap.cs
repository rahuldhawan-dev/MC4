using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteBracketSiteMap : ClassMap<SampleSiteBracketSite>
    {
        public const string TABLE_NAME = "SampleSitesBracketSites";

        public SampleSiteBracketSiteMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.BracketSampleSite, "BracketSiteSampleSiteId").Not.Nullable();
            References(x => x.SampleSite).Not.Nullable();
            References(x => x.BracketSiteLocationType, "SampleSiteBracketSiteLocationTypeId").Not.Nullable();
        }
    }
}
