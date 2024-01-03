using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ConfinedSpaceFormAtmosphericTestMap : ClassMap<ConfinedSpaceFormAtmosphericTest>
    {
        public ConfinedSpaceFormAtmosphericTestMap()
        {
            Id(x => x.Id).Not.Nullable();

            Map(x => x.CarbonMonoxidePartsPerMillionBottom).Not.Nullable();
            Map(x => x.CarbonMonoxidePartsPerMillionMiddle).Not.Nullable();
            Map(x => x.CarbonMonoxidePartsPerMillionTop).Not.Nullable();
            Map(x => x.HydrogenSulfidePartsPerMillionBottom).Not.Nullable();
            Map(x => x.HydrogenSulfidePartsPerMillionMiddle).Not.Nullable();
            Map(x => x.HydrogenSulfidePartsPerMillionTop).Not.Nullable();

            // These percentages can only go to 100.00%
            Map(x => x.LowerExplosiveLimitPercentageBottom).Precision(5).Scale(2).Not.Nullable();
            Map(x => x.LowerExplosiveLimitPercentageMiddle).Precision(5).Scale(2).Not.Nullable();
            Map(x => x.LowerExplosiveLimitPercentageTop).Precision(5).Scale(2).Not.Nullable();
            Map(x => x.OxygenPercentageBottom).Precision(5).Scale(2).Not.Nullable();
            Map(x => x.OxygenPercentageMiddle).Precision(5).Scale(2).Not.Nullable();
            Map(x => x.OxygenPercentageTop).Precision(5).Scale(2).Not.Nullable();
            Map(x => x.TestedAt).Not.Nullable();

            References(x => x.ConfinedSpaceForm).Not.Nullable();
            References(x => x.TestedBy, "TestedByEmployeeId").Not.Nullable();
            References(x => x.ConfinedSpaceFormReadingCaptureTime).Not.Nullable();
        }
    }
}
