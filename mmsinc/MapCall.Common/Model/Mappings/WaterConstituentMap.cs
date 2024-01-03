using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WaterConstituentMap : ClassMap<WaterConstituent>
    {
        public WaterConstituentMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Column("WaterConstituent").Length(255).Not.Nullable();
            Map(x => x.Min).Precision(53).Nullable();
            Map(x => x.Max).Precision(53).Nullable();
            Map(x => x.Mcl).Column("MCL").Precision(53).Nullable();
            Map(x => x.Mclg).Column("MCLG").Precision(53).Nullable();
            Map(x => x.Smcl).Column("SMCL").Precision(53).Nullable();
            Map(x => x.ActionLimit).Length(255).Nullable();
            Map(x => x.Regulation).Length(255).Nullable();
            Map(x => x.SamplingFrequency).Length(255);
            Map(x => x.SamplingMethod).Length(255);
            Map(x => x.SampleContainerSizeMl).Column("SampleContainerSize").Length(255);
            Map(x => x.HoldingTimeHrs).Column("HoldingTime").Length(255);
            Map(x => x.PreservativeQuenchingAgent).Length(255);
            Map(x => x.AnalyticalMethod).Length(255);
            Map(x => x.TatBellvileDays).Column("TATBellvilleDays").Length(255);

            Map(x => x.NoEPALimits)
               .Formula(
                    "(CASE WHEN Min IS NULL AND Max IS NULL AND MCL IS NULL AND MCLG IS NULL AND SMCL IS NULL AND ActionLimit IS NULL AND Regulation IS NULL THEN 1 ELSE 0 END)");

            References(x => x.UnitOfMeasure).Nullable();
            References(x => x.DrinkingWaterContaminantCategory).Nullable();
            References(x => x.WasteWaterContaminantCategory).Nullable();

            HasMany(x => x.StateLimits).KeyColumn("WaterConstituentId").Inverse().Cascade.AllDeleteOrphan();

            HasMany(x => x.WaterConstituentDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.WaterConstituentNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
