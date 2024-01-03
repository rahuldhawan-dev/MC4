using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SamplePlanMap : ClassMap<SamplePlan>
    {
        public SamplePlanMap()
        {
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.PWSID).Column("PublicWaterSupplyId").Not.Nullable();
            References(x => x.ContactPerson).Not.Nullable();

            Map(x => x.Name).Nullable();
            Map(x => x.Cws).Column("CWS").Not.Nullable();
            Map(x => x.Ntnc).Column("NTNC").Not.Nullable();
            Map(x => x.MonitoringPeriodFrom).Not.Nullable();
            Map(x => x.MonitoringPeriodTo).Not.Nullable();
            Map(x => x.Standard).Not.Nullable();
            Map(x => x.Reduced).Not.Nullable();
            Map(x => x.MinimumSamplesRequired).Not.Nullable().Precision(10);
            Map(x => x.NameOfCertifiedLaboratory).Not.Nullable().Length(50);
            Map(x => x.SameAsPreviousPeriod).Not.Nullable();
            Map(x => x.AllSamplesTier1).Not.Nullable();
            Map(x => x.Tier2Sites).Not.Nullable();
            Map(x => x.Tier3Sites).Not.Nullable();
            Map(x => x.Tier1SitesVerified).Not.Nullable();
            Map(x => x.LeadServiceLines).Not.Nullable();
            Map(x => x.LeadLinesVerified).Not.Nullable();
            Map(x => x.FiftyPercent).Not.Nullable();
            Map(x => x.Comments).Nullable();
            Map(x => x.HasActiveSampleSites)
               .Formula(
                    "(CASE WHEN (SELECT COUNT(1) FROM SamplePlansSampleSites ssss where ssss.samplePlanId = Id) > 0 THEN 1 ELSE 0 END)");

            HasMany(x => x.PlanDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.PlanNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();

            HasManyToMany(x => x.SampleSites)
               .Table("SamplePlansSampleSites")
               .ParentKeyColumn("SamplePlanId")
               .ChildKeyColumn("SampleSiteId");
        }
    }
}
