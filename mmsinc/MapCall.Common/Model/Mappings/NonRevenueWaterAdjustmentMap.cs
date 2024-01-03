using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class NonRevenueWaterAdjustmentMap : ClassMap<NonRevenueWaterAdjustment>
    {
        public NonRevenueWaterAdjustmentMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.TotalGallons).Not.Nullable();
            Map(x => x.BusinessUnit).Not.Nullable();
            Map(x => x.Comments).Not.Nullable();

            References(x => x.NonRevenueWaterEntry).Not.Nullable();
        }
    }
}
