using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class NonRevenueWaterDetailMap : ClassMap<NonRevenueWaterDetail>
    {
        public NonRevenueWaterDetailMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Month).Not.Nullable();
            Map(x => x.Year).Not.Nullable();
            Map(x => x.BusinessUnit).Not.Nullable();
            Map(x => x.WorkDescription).Not.Nullable();
            Map(x => x.TotalGallons).Not.Nullable();

            References(x => x.NonRevenueWaterEntry).Not.Nullable();
        }
    }
}
