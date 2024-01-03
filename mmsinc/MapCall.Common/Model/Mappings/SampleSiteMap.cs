using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteMap : ClassMap<SampleSite>
    {
        public SampleSiteMap()
        {
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.State).Nullable();
            References(x => x.OperatingCenter).Nullable();
            References(x => x.PublicWaterSupply).Column("PWSID").Nullable();
            References(x => x.Availability).Column("Availability").Nullable();
            References(x => x.Status).Column("SiteStatusId").Nullable();
            References(x => x.County).Nullable();
            References(x => x.Facility).Column("facilityID").Nullable();
            References(x => x.Valve).Nullable();
            References(x => x.Hydrant).Nullable();
            References(x => x.Gradient).Nullable();
            References(x => x.Coordinate).Nullable();
            References(x => x.Town).Nullable();
            References(x => x.TownSection).Nullable();
            References(x => x.Street).Nullable();
            References(x => x.CrossStreet).Nullable();
            References(x => x.SampleSiteInactivationReason).Nullable();
            References(x => x.CertifiedBy, "CertifiedByUserId").Nullable();
            References(x => x.CollectionType).Nullable();
            References(x => x.SubCollectionType).Nullable();
            References(x => x.LocationType).Nullable();
            References(x => x.ParentSite, "ParentSampleSiteId").Nullable();
            References(x => x.LeadCopperTierClassification).Nullable();
            References(x => x.LeadCopperValidationMethod).Nullable();
            References(x => x.LeadCopperTierSampleCategory).Nullable();
            References(x => x.CustomerPlumbingMaterial).Nullable();
            References(x => x.SampleSitePointOfUseTreatmentType).Nullable();
            References(x => x.ValidatedBy).Nullable();
            References(x => x.SampleSiteAddressLocationType).Nullable();
            References(x => x.SampleSiteValidationStatus).Nullable();
            References(x => x.SampleSiteProfile).Nullable();
            References(x => x.CustomerContactMethod).Nullable();
            References(x => x.Premise).Nullable();
            
            Map(x => x.PointOfUseTreatmentTypeOtherReason).Nullable().Length(SampleSite.StringLengths.POINT_OF_USE_TREATMENT_TYPE_OTHER_REASON);
            Map(x => x.LeadCopperTierThreeExplanation).Nullable().Length(SampleSite.StringLengths.LEAD_COPPER_TIER_THREE_EXPLANATION);
            Map(x => x.LimsFacilityId).Nullable().Length(SampleSite.StringLengths.LIMS_FACILITY_ID);
            Map(x => x.LimsSiteId).Nullable().Length(SampleSite.StringLengths.LIMS_SITE_ID);
            Map(x => x.LimsPrimaryStationCode).Nullable().Length(SampleSite.StringLengths.LIMS_PRIMARY_STATION_CODE);
            Map(x => x.LimsSequenceNumber).Nullable();
            Map(x => x.CertifiedDate).Nullable();
            Map(x => x.IsCertified)
               .Formula("(case when CertifiedDate is not null then 1 else 0 end)")
               .ReadOnly();

            Map(x => x.BactiSite).Not.Nullable();
            Map(x => x.StreetNumber).Length(50);
            Map(x => x.TownText).Column("Town").Length(50);
            Map(x => x.ZipCode).Nullable().Length(SampleSite.StringLengths.ZIP_CODE);
            Map(x => x.OutOfServiceArea);
            Map(x => x.LeadCopperSite).Nullable();
            Map(x => x.AgencyId);
            Map(x => x.IsAlternateSite).Not.Nullable().Default("false");
            Map(x => x.ResourceDistributionMaps).Nullable();
            Map(x => x.ResourceCapitalImprovement).Nullable();
            Map(x => x.ResourceUtilityRecords).Nullable();
            Map(x => x.ResourceSamplingResults).Nullable();
            Map(x => x.ResourceInterviewsPersonnel).Nullable();
            Map(x => x.ResourceCommunitySurvey).Nullable();
            Map(x => x.ResourceCountyAppraisal).Nullable();
            Map(x => x.ResourceContacts).Nullable();
            Map(x => x.ResourceSurveyResults).Nullable();
            Map(x => x.ResourceInterviewsResidents).Nullable();
            Map(x => x.ResourceInterviewsContractors).Nullable();
            Map(x => x.ResourceLeadCheckSwabs).Nullable();
            Map(x => x.OtherSources).Nullable();
            Map(x => x.PreviousMonitoringPeriod).Nullable();
            Map(x => x.CustomerParticipationConfirmed).Nullable();
            Map(x => x.CustomerName).Nullable();
            Map(x => x.CustomerHomePhone).Nullable();
            Map(x => x.CustomerAltPhone).Nullable();
            Map(x => x.CustomerEmail).Nullable();
            Map(x => x.FieldVerifiedServiceMaterial).Nullable();
            Map(x => x.FieldVerifiedServiceCustomerSideMaterial).Nullable();
            Map(x => x.FieldVerifiedCustomerPlumbingMaterial).Nullable();
            Map(x => x.ValidatedAt).Nullable();
            Map(x => x.IsComplianceSampleSite).Nullable();
            Map(x => x.IsProcessSampleSite).Nullable();
            Map(x => x.IsResearchSampleSite).Nullable();
            Map(x => x.CommonSiteName).Nullable().Length(SampleSite.StringLengths.COMMON_SITE_NAME);
            Map(x => x.LocationNameDescription).Nullable().Length(SampleSite.StringLengths.LOCATION_NAME_DESCRIPTION);
            Map(x => x.IsLimsLocation).Nullable();
            Map(x => x.NeedsToSync).Not.Nullable();
            Map(x => x.LastSyncedAt).Nullable();
            Map(x => x.LeadServiceLineReplacementSite).Nullable();

            HasMany(x => x.SampleSiteDocuments).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.SampleSiteNotes).KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.SampleIdMatrices).KeyColumn("SampleSiteId").ReadOnly().LazyLoad();
            HasMany(x => x.BracketSites).KeyColumn("SampleSiteId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.ChildSites).KeyColumn("ParentSampleSiteId").LazyLoad();

            HasManyToMany(x => x.SamplePlans)
               .Table("SamplePlansSampleSites")
               .ParentKeyColumn("SampleSiteId")
               .ChildKeyColumn("SamplePlanId");
        }
    }
}
