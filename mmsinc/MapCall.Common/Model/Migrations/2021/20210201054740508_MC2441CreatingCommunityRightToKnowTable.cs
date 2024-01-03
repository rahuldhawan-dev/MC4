using FluentMigrator;
using MapCall.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210201054740508), Tags("Production")]
    public class MC2441CreatingCommunityRightToKnowTable : Migration
    {
        public struct ColumnNames
        {
            public const string ID = "Id",
                                FACILITY_ID = "FacilityId",
                                COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID = "CommunityRightToKnowFacilityId",
                                COMU = "CoMu",
                                FACILITY_EMERGENCY_CONTACT_NAME = "FacilityEmergencyContactName",
                                FACILITY_EMERGENCY_CONTACT_TITLE = "FacilityEmergencyContactTitle",
                                FACILITY_EMERGENCY_CONTACT_PHONENUMBER = "FacilityEmergencyContactPhoneNumber",
                                FACILITY_EMERGENCY_CONTACT_EMERGENCYCONTACTPHONENUMBER = "FacilityEmergencyContactEmergencyPhoneNumber",
                                HAS_HAZARDOUS_SUBSTANCE_IN_ANY_QUANTITY = "FacilityHasNJCRTKHazardousSubstancesInAnyQuantity",
                                HAS_HAZARDOUS_SUBSTANCE_ABOVE_THRESHOLDS = "FacilityHasNJCRTKHazardousSubstancesAboveThresholds",
                                IS_SUBJECT_TO_CHEMICAL_ACCIDENT_PREVENTION = "IsSubjectToChemicalAccidentPrevention",
                                IS_SUBJECT_TO_EMERGENCY_PLANNING = "IsSubjectToEmergencyPlanning",
                                LOCATION_IS_MANNED = "LocationIsManned",
                                MAXIMUM_NUMBER_OF_OCCUPANTS = "MaximumNumberOfOccupants",
                                NORTH_AMERICAN_INDUSTRY_CLASSIFICATION_SYSTEM = "NorthAmericanIndustryClassificationSystem",
                                NUMBER_OF_EMPLOYEES = "NumberOfEmployees",
                                RD_EXEMPTION_APPROVAL_NUMBER = "RDExemptionApprovalNumber",
                                RISK_MANAGEMENT_PLAN_FACILITY_ID = "RiskManagementPlanFacilityId",
                                TOXINS_RELEASE_INVENTORY_FACILITY_ID = "ToxinsReleaseInventoryFacilityId",
                                SUBMISSION_DATE = "SubmissionDate",
                                EXPIRATION_DATE = "ExpirationDate";
        }

        private const string FOREIGN_KEY = "FK_CommunityRightToKnows_tblFacilities_RecordId";
        private const string CRTK_TABLE_NAME = "CommunityRightToKnows";
        private const string FACILITIES_TABLE_NAME = "tblFacilities";
        private const string MIGRATE_DATA_QUERY = "INSERT INTO [CommunityRightToKnows] ( " +
                                                  "[FacilityId]," +
                                                  "[CommunityRightToKnowFacilityId]," +
                                                  "[CoMu]," +
                                                  "[FacilityEmergencyContactName]," +
                                                  "[FacilityEmergencyContactTitle]," +
                                                  "[FacilityEmergencyContactPhoneNumber]," +
                                                  "[FacilityEmergencyContactEmergencyPhoneNumber]," +
                                                  "[FacilityHasNJCRTKHazardousSubstancesInAnyQuantity]," +
                                                  "[FacilityHasNJCRTKHazardousSubstancesAboveThresholds]," +
                                                  "[IsSubjectToChemicalAccidentPrevention]," +
                                                  "[IsSubjectToEmergencyPlanning]," +
                                                  "[LocationIsManned]," +
                                                  "[MaximumNumberOfOccupants]," +
                                                  "[NorthAmericanIndustryClassificationSystem]," +
                                                  "[NumberOfEmployees]," +
                                                  "[RDExemptionApprovalNumber]," +
                                                  "[RiskManagementPlanFacilityId]," +
                                                  "[ToxinsReleaseInventoryFacilityId]" +
                                                  ")" +
                                                  "SELECT" +
                                                  "[RecordId]," +
                                                  "[CommunityRightToKnowFacilityId]," +
                                                  "[CoMu]," +
                                                  "[FacilityEmergencyContactName]," +
                                                  "[FacilityEmergencyContactTitle]," +
                                                  "[FacilityEmergencyContactPhoneNumber]," +
                                                  "[FacilityEmergencyContactEmergencyPhoneNumber]," +
                                                  "[FacilityHasNJCRTKHazardousSubstancesInAnyQuantity]," +
                                                  "[FacilityHasNJCRTKHazardousSubstancesAboveThresholds]," +
                                                  "[IsSubjectToChemicalAccidentPrevention]," +
                                                  "[IsSubjectToEmergencyPlanning]," +
                                                  "[LocationIsManned]," +
                                                  "[MaximumNumberOfOccupants]," +
                                                  "[NorthAmericanIndustryClassificationSystem]," +
                                                  "[NumberOfEmployees]," +
                                                  "[RDExemptionApprovalNumber]," +
                                                  "[RiskManagementPlanFacilityId]," +
                                                  "[ToxinsReleaseInventoryFacilityId]" +
                                                  "FROM [tblFacilities]";

        public override void Up()
        {
            Create.Table(CRTK_TABLE_NAME)
                  .WithColumn(ColumnNames.ID).AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn(ColumnNames.FACILITY_ID).AsInt32().NotNullable().ForeignKey(FOREIGN_KEY, "tblFacilities", "RecordId")
                  .WithColumn(ColumnNames.COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID).AsString(50).Nullable()
                  .WithColumn(ColumnNames.COMU).AsString(50).Nullable()
                  .WithColumn(ColumnNames.FACILITY_EMERGENCY_CONTACT_NAME).AsString(255).Nullable()
                  .WithColumn(ColumnNames.FACILITY_EMERGENCY_CONTACT_TITLE).AsString(255).Nullable()
                  .WithColumn(ColumnNames.FACILITY_EMERGENCY_CONTACT_PHONENUMBER).AsString(20).Nullable()
                  .WithColumn(ColumnNames.FACILITY_EMERGENCY_CONTACT_EMERGENCYCONTACTPHONENUMBER).AsString(20).Nullable()
                  .WithColumn(ColumnNames.HAS_HAZARDOUS_SUBSTANCE_IN_ANY_QUANTITY).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.HAS_HAZARDOUS_SUBSTANCE_ABOVE_THRESHOLDS).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.IS_SUBJECT_TO_CHEMICAL_ACCIDENT_PREVENTION).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.IS_SUBJECT_TO_EMERGENCY_PLANNING).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.LOCATION_IS_MANNED).AsBoolean().Nullable()
                  .WithColumn(ColumnNames.MAXIMUM_NUMBER_OF_OCCUPANTS).AsInt32().Nullable()
                  .WithColumn(ColumnNames.NORTH_AMERICAN_INDUSTRY_CLASSIFICATION_SYSTEM).AsString(50).Nullable()
                  .WithColumn(ColumnNames.NUMBER_OF_EMPLOYEES).AsInt32().Nullable()
                  .WithColumn(ColumnNames.RD_EXEMPTION_APPROVAL_NUMBER).AsString(50).Nullable()
                  .WithColumn(ColumnNames.RISK_MANAGEMENT_PLAN_FACILITY_ID).AsString(50).Nullable()
                  .WithColumn(ColumnNames.TOXINS_RELEASE_INVENTORY_FACILITY_ID).AsString(50).Nullable()
                  .WithColumn(ColumnNames.SUBMISSION_DATE).AsDateTime().Nullable()
                  .WithColumn(ColumnNames.EXPIRATION_DATE).AsDateTime().Nullable();

            Execute.Sql(MIGRATE_DATA_QUERY);

            Delete.Column(ColumnNames.COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.COMU).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.FACILITY_EMERGENCY_CONTACT_NAME).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.FACILITY_EMERGENCY_CONTACT_TITLE).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.FACILITY_EMERGENCY_CONTACT_PHONENUMBER).FromTable(FACILITIES_TABLE_NAME);
            
            Delete.Column(ColumnNames.FACILITY_EMERGENCY_CONTACT_EMERGENCYCONTACTPHONENUMBER).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.HAS_HAZARDOUS_SUBSTANCE_IN_ANY_QUANTITY).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.HAS_HAZARDOUS_SUBSTANCE_ABOVE_THRESHOLDS).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.IS_SUBJECT_TO_CHEMICAL_ACCIDENT_PREVENTION).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.IS_SUBJECT_TO_EMERGENCY_PLANNING).FromTable(FACILITIES_TABLE_NAME);
            
            Delete.Column(ColumnNames.LOCATION_IS_MANNED).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.MAXIMUM_NUMBER_OF_OCCUPANTS).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.NORTH_AMERICAN_INDUSTRY_CLASSIFICATION_SYSTEM).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.NUMBER_OF_EMPLOYEES).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.RD_EXEMPTION_APPROVAL_NUMBER).FromTable(FACILITIES_TABLE_NAME);
            
            Delete.Column(ColumnNames.RISK_MANAGEMENT_PLAN_FACILITY_ID).FromTable(FACILITIES_TABLE_NAME);
            Delete.Column(ColumnNames.TOXINS_RELEASE_INVENTORY_FACILITY_ID).FromTable(FACILITIES_TABLE_NAME);

            this.CreateDocumentType(CRTK_TABLE_NAME, "Community Right To Know", "Community Right To Know");
        }

        public override void Down()
        {
            Delete.ForeignKey(FOREIGN_KEY).OnTable(CRTK_TABLE_NAME);
            Delete.Table(CRTK_TABLE_NAME);

            Alter.Table(FACILITIES_TABLE_NAME)
                 .AddColumn(ColumnNames.COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID).AsString(50).Nullable()
                 .AddColumn(ColumnNames.COMU).AsString(50).Nullable()
                 .AddColumn(ColumnNames.FACILITY_EMERGENCY_CONTACT_NAME).AsString(255).Nullable()
                 .AddColumn(ColumnNames.FACILITY_EMERGENCY_CONTACT_TITLE).AsString(255).Nullable()
                 .AddColumn(ColumnNames.FACILITY_EMERGENCY_CONTACT_PHONENUMBER).AsString(20).Nullable()
                 .AddColumn(ColumnNames.FACILITY_EMERGENCY_CONTACT_EMERGENCYCONTACTPHONENUMBER).AsString(20).Nullable()
                 .AddColumn(ColumnNames.HAS_HAZARDOUS_SUBSTANCE_IN_ANY_QUANTITY).AsBoolean().Nullable()
                 .AddColumn(ColumnNames.HAS_HAZARDOUS_SUBSTANCE_ABOVE_THRESHOLDS).AsBoolean().Nullable()
                 .AddColumn(ColumnNames.IS_SUBJECT_TO_CHEMICAL_ACCIDENT_PREVENTION).AsBoolean().Nullable()
                 .AddColumn(ColumnNames.IS_SUBJECT_TO_EMERGENCY_PLANNING).AsBoolean().Nullable()
                 .AddColumn(ColumnNames.LOCATION_IS_MANNED).AsBoolean().Nullable()
                 .AddColumn(ColumnNames.MAXIMUM_NUMBER_OF_OCCUPANTS).AsInt32().Nullable()
                 .AddColumn(ColumnNames.NORTH_AMERICAN_INDUSTRY_CLASSIFICATION_SYSTEM).AsString(50).Nullable()
                 .AddColumn(ColumnNames.NUMBER_OF_EMPLOYEES).AsInt32().Nullable()
                 .AddColumn(ColumnNames.RD_EXEMPTION_APPROVAL_NUMBER).AsString(50).Nullable()
                 .AddColumn(ColumnNames.RISK_MANAGEMENT_PLAN_FACILITY_ID).AsString(50).Nullable()
                 .AddColumn(ColumnNames.TOXINS_RELEASE_INVENTORY_FACILITY_ID).AsString(50).Nullable();

            this.DeleteDataType(CRTK_TABLE_NAME);
        }
    }
}
