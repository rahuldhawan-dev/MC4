using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class NonRevenueWaterEntryMap : ClassMap<NonRevenueWaterEntry>
    {
        public NonRevenueWaterEntryMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.HasBeenReportedToHyperion).Not.Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
            Map(x => x.UpdatedAt).Not.Nullable();
            Map(x => x.Year).Nullable();
            Map(x => x.Month).Nullable();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.CreatedBy).Not.Nullable();
            References(x => x.UpdatedBy).Nullable();

            HasMany(x => x.NonRevenueWaterDetails).KeyColumn("NonRevenueWaterEntryId").Cascade.AllDeleteOrphan().Inverse();
            HasMany(x => x.NonRevenueWaterAdjustments)
               .KeyColumn("NonRevenueWaterEntryId")
               .Cascade.AllDeleteOrphan()
               .Inverse();
        }
    }
}
