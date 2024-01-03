using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191203140238646), Tags("Production")]
    public class MC1738AddCRTKFieldsToFacilities : Migration
    {
        private const string TABLE_FACILITIES = "tblFacilities";

        public override void Up()
        {
            Alter.Table(TABLE_FACILITIES)
                 .AddColumn("CommunityRightToKnowFacilityId").AsString(50).Nullable()
                 .AddColumn("CoMu").AsString(50).Nullable()
                 .AddColumn("NorthAmericanIndustryClassificationSystem").AsString(50).Nullable()
                 .AddColumn("NumberOfEmployees").AsInt32().Nullable()
                 .AddColumn("FacilityEmergencyContactName").AsString(255).Nullable()
                 .AddColumn("FacilityEmergencyContactTitle").AsString(255).Nullable()
                 .AddColumn("FacilityEmergencyContactPhoneNumber").AsString(20).Nullable()
                 .AddColumn("FacilityEmergencyContactEmergencyPhoneNumber").AsString(20).Nullable()
                 .AddColumn("AuthorizedRepresentativeName").AsString(255).Nullable()
                 .AddColumn("AuthorizedRepresentativeTitle").AsString(255).Nullable()
                 .AddColumn("AuthorizedRepresentativeEmailAddress").AsString(255).Nullable()
                 .AddColumn("AuthorizedRepresentativePhoneNumber").AsString(20).Nullable()
                 .AddColumn("UnionRepresentativeUnionName").AsString(255).Nullable()
                 .AddColumn("UnionRepresentativeName").AsString(255).Nullable()
                 .AddColumn("UnionRepresentativeEmailAddress").AsString(255).Nullable()
                 .AddColumn("UnionRepresentativePhoneNumber").AsString(20).Nullable()
                 .AddColumn("FacilityHasNJCRTKHazardousSubstancesInAnyQuantity").AsBoolean().Nullable()
                 .AddColumn("FacilityHasNJCRTKHazardousSubstancesAboveThresholds").AsBoolean().Nullable()
                 .AddColumn("ToxinsReleaseInventoryFacilityId").AsString(50).Nullable()
                 .AddColumn("RDExemptionApprovalNumber").AsString(50).Nullable()
                 .AddColumn("RiskManagementPlanFacilityId").AsString(50).Nullable()
                 .AddColumn("MaximumNumberOfOccupants").AsInt32().Nullable()
                 .AddColumn("LocationIsManned").AsBoolean().Nullable()
                 .AddColumn("IsSubjectToChemicalAccidentPrevention").AsBoolean().Nullable()
                 .AddColumn("IsSubjectToEmergencyPlanning").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("CommunityRightToKnowFacilityId").FromTable(TABLE_FACILITIES);
            Delete.Column("CoMu").FromTable(TABLE_FACILITIES);
            Delete.Column("NorthAmericanIndustryClassificationSystem").FromTable(TABLE_FACILITIES);
            Delete.Column("NumberOfEmployees").FromTable(TABLE_FACILITIES);
            Delete.Column("FacilityEmergencyContactName").FromTable(TABLE_FACILITIES);
            Delete.Column("FacilityEmergencyContactTitle").FromTable(TABLE_FACILITIES);
            Delete.Column("FacilityEmergencyContactPhoneNumber").FromTable(TABLE_FACILITIES);
            Delete.Column("FacilityEmergencyContactEmergencyPhoneNumber").FromTable(TABLE_FACILITIES);
            Delete.Column("AuthorizedRepresentativeName").FromTable(TABLE_FACILITIES);
            Delete.Column("AuthorizedRepresentativeTitle").FromTable(TABLE_FACILITIES);
            Delete.Column("AuthorizedRepresentativeEmailAddress").FromTable(TABLE_FACILITIES);
            Delete.Column("AuthorizedRepresentativePhoneNumber").FromTable(TABLE_FACILITIES);
            Delete.Column("UnionRepresentativeUnionName").FromTable(TABLE_FACILITIES);
            Delete.Column("UnionRepresentativeName").FromTable(TABLE_FACILITIES);
            Delete.Column("UnionRepresentativeEmailAddress").FromTable(TABLE_FACILITIES);
            Delete.Column("UnionRepresentativePhoneNumber").FromTable(TABLE_FACILITIES);
            Delete.Column("FacilityHasNJCRTKHazardousSubstancesInAnyQuantity").FromTable(TABLE_FACILITIES);
            Delete.Column("FacilityHasNJCRTKHazardousSubstancesAboveThresholds").FromTable(TABLE_FACILITIES);
            Delete.Column("ToxinsReleaseInventoryFacilityId").FromTable(TABLE_FACILITIES);
            Delete.Column("RDExemptionApprovalNumber").FromTable(TABLE_FACILITIES);
            Delete.Column("RiskManagementPlanFacilityId").FromTable(TABLE_FACILITIES);
            Delete.Column("MaximumNumberOfOccupants").FromTable(TABLE_FACILITIES);
            Delete.Column("LocationIsManned").FromTable(TABLE_FACILITIES);
            Delete.Column("IsSubjectToChemicalAccidentPrevention").FromTable(TABLE_FACILITIES);
            Delete.Column("IsSubjectToEmergencyPlanning").FromTable(TABLE_FACILITIES);
        }
    }
}
