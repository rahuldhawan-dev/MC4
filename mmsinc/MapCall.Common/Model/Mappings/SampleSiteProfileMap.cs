using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteProfileMap : ClassMap<SampleSiteProfile>
    {
        public SampleSiteProfileMap()
        {
            Id(x => x.Id);

            Map(x => x.Name).Nullable();
            Map(x => x.Number).Not.Nullable();

            References(x => x.SampleSiteProfileAnalysisType).Not.Nullable();
            References(x => x.PublicWaterSupply).Not.Nullable();
        }
    }
}
