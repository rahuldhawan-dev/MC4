using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class CommunityRightToKnowMap : ClassMap<CommunityRightToKnow>
    {
        public const string TABLE_NAME = "CommunityRightToKnows";

        public CommunityRightToKnowMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id).GeneratedBy.Identity();

            LazyLoad();

            References(x => x.Facility).Not.Nullable();

            Map(x => x.CommunityRightToKnowFacilityId).Nullable().Length(Facility.StringLengths.COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID);
            Map(x => x.CoMu).Nullable().Length(Facility.StringLengths.COMU);
            Map(x => x.FacilityEmergencyContactEmergencyPhoneNumber).Nullable().Length(Facility.StringLengths.FACILITY_EMERGENCY_CONTACT_EMERGENCY_PHONE_NUMBER);
            Map(x => x.FacilityEmergencyContactName).Nullable().Length(Facility.StringLengths.FACILITY_EMERGENCY_CONTACT_NAME);
            Map(x => x.FacilityEmergencyContactPhoneNumber).Nullable().Length(Facility.StringLengths.FACILITY_EMERGENCY_CONTACT_PHONE_NUMBER);
            Map(x => x.FacilityEmergencyContactTitle).Nullable().Length(Facility.StringLengths.FACILITY_EMERGENCY_CONTACT_TITLE);
            Map(x => x.FacilityHasNJCRTKHazardousSubstancesAboveThresholds).Nullable();
            Map(x => x.FacilityHasNJCRTKHazardousSubstancesInAnyQuantity).Nullable();
            Map(x => x.IsSubjectToChemicalAccidentPrevention).Nullable();
            Map(x => x.IsSubjectToEmergencyPlanning).Nullable();
            Map(x => x.LocationIsManned).Nullable();
            Map(x => x.MaximumNumberOfOccupants).Nullable();
            Map(x => x.NorthAmericanIndustryClassificationSystem).Nullable().Length(Facility.StringLengths.NORTH_AMERICAN_INDUSTRY_CLASSIFICATION_SYSTEM);
            Map(x => x.RDExemptionApprovalNumber).Nullable().Length(Facility.StringLengths.RD_EXEMPTION_APPROVAL_NUMBER);
            Map(x => x.NumberOfEmployees).Nullable();
            Map(x => x.RiskManagementPlanFacilityId).Nullable().Length(Facility.StringLengths.RISK_MANAGEMENT_PLAN_FACILITY_ID);
            Map(x => x.ToxinsReleaseInventoryFacilityId).Nullable().Length(Facility.StringLengths.TOXINS_RELEASE_INVENTORY_FACILITY_ID);
            Map(x => x.SubmissionDate).Nullable();
            Map(x => x.ExpirationDate).Nullable();
            Map(x => x.Expired).DbSpecificFormula(
                "(CASE WHEN (ExpirationDate <= cast(getdate() as date)) THEN 1 ELSE 0 END)",
                "(CASE WHEN (ExpirationDate <= date('now', 'localtime')) THEN 1 ELSE 0 END)");

            HasMany(x => x.CommunityRightToKnowDocuments).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.CommunityRightToKnowNotes).KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}
