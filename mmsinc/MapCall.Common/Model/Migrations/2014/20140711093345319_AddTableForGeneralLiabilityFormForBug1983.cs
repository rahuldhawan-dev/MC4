using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140711093345319), Tags("Production")]
    public class AddTableForGeneralLiabilityFormForBug1983 : Migration
    {
        #region Constants

        public struct Sql
        {
            public const string CREATE_NOTIFICATION_PURPOSE = @"
                                    declare @moduleId int
                                    set @moduleId = (select ModuleID from [Modules] where Name = 'Health and Safety')
                                    insert into [NotificationPurposes] ([ModuleID], [Purpose]) VALUES(@moduleId, 'General Liability Claim')",
                                REMOVE_NOTIFICATION_PURPOSE = @"
                                    declare @purposeId int
                                    set @purposeId = (select top 1 NotificationPurposeID from [NotificationPurposes] where [Purpose] = 'General Liability Claim')
                
                                    delete from [NotificationConfigurations] where [NotificationPurposeID] = @purposeId
                                    delete from [NotificationPurposes] where [NotificationPurposeID] = @purposeId";
        }

        public struct StringLengths
        {
            public const int NAME = 50,
                             CLAIM_NUMBER = 20,
                             PHONE_NUMBER = 20,
                             EMAIL = 50,
                             LOCATION = 50,
                             DESCRIPTION = 50,
                             LICENSE_NUMBER = 20;
        }

        public struct TableNames
        {
            public const string GENERAL_LIABILITY_CLAIMS = "GeneralLiabilityClaims",
                                LIABILITY_TYPES = "LiabilityTypes",
                                CLAIMS_REPRESENTATIVES = "ClaimsRepresentatives";
        }

        #endregion

        #region Private Methods

        private void CreateLookupTable(string table, params string[] descriptions)
        {
            Create.Table(table)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(50).Unique().NotNullable();

            foreach (var d in descriptions)
            {
                Insert.IntoTable(table).Row(new {Description = d});
            }
        }

        #endregion

        public override void Up()
        {
            // TABLES
            CreateLookupTable(TableNames.LIABILITY_TYPES, "General Liability", "General Liability - Customer",
                "Personal Injury", "Vehicle");
            CreateLookupTable(TableNames.CLAIMS_REPRESENTATIVES, "Sabetha Alvey", "Brandy Hill");

            Create.Table(TableNames.GENERAL_LIABILITY_CLAIMS)
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("OperatingCenterId").AsInt32()
                  .ForeignKey("FK_GeneralLiabilityClaims_OperatingCenters_OperatingCenterId", "OperatingCenters",
                       "OperatingCenterId").NotNullable()
                  .WithColumn("CompanyContactId").AsInt32()
                  .ForeignKey("FK_GeneralLiabilityClaims_tblEmployee_CompanyContactId", "tblEmployee", "tblEmployeeId")
                  .NotNullable()
                  .WithColumn("ClaimsRepresentativeId").AsInt32()
                  .ForeignKey("FK_GeneralLiabilityClaims_ClaimsRepresentatives_ClaimsRepresentativeId",
                       "ClaimsRepresentatives", "Id").NotNullable()
                  .WithColumn("ClaimNumber").AsAnsiString(StringLengths.CLAIM_NUMBER).NotNullable()
                  .WithColumn("LiabilityTypeId").AsInt32()
                  .ForeignKey("FK_GeneralLiabilityClaims_LiabilityTypes_LiabilityTypeId", TableNames.LIABILITY_TYPES,
                       "Id").NotNullable()
                  .WithColumn("MeterBox").AsBoolean().Nullable()
                  .WithColumn("CurbValveBox").AsBoolean().Nullable()
                  .WithColumn("Excavation").AsBoolean().Nullable()
                  .WithColumn("Barricades").AsBoolean().Nullable()
                  .WithColumn("Vehicle").AsBoolean().Nullable()
                  .WithColumn("WaterMeter").AsBoolean().Nullable()
                  .WithColumn("FireHydrant").AsBoolean().Nullable()
                  .WithColumn("Backhoe").AsBoolean().Nullable()
                  .WithColumn("WaterQuality").AsBoolean().Nullable()
                  .WithColumn("WaterPressure").AsBoolean().Nullable()
                  .WithColumn("WaterMain").AsBoolean().Nullable()
                  .WithColumn("ServiceLine").AsBoolean().Nullable()
                  .WithColumn("Description").AsCustom("text").NotNullable()
                  .WithColumn("Name").AsAnsiString(StringLengths.NAME).Nullable()
                  .WithColumn("PhoneNumber").AsAnsiString(StringLengths.PHONE_NUMBER).Nullable()
                  .WithColumn("Address").AsCustom("text").Nullable()
                  .WithColumn("Email").AsAnsiString(StringLengths.EMAIL).Nullable()
                  .WithColumn("DriverName").AsAnsiString(StringLengths.NAME).Nullable()
                  .WithColumn("DriverPhone").AsAnsiString(StringLengths.PHONE_NUMBER).Nullable()
                  .WithColumn("PHHContacted").AsBoolean().Nullable()
                  .WithColumn("OtherDriver").AsAnsiString(StringLengths.NAME).Nullable()
                  .WithColumn("OtherDriverPhone").AsAnsiString(StringLengths.PHONE_NUMBER).Nullable()
                  .WithColumn("OtherDriverAddress").AsCustom("text").Nullable()
                  .WithColumn("LocationOfIncident").AsAnsiString(StringLengths.LOCATION).Nullable()
                  .WithColumn("CoordinateId").AsInt32().ForeignKey("FK_GeneralLiabilityClaims_Coordinates_CoordinateId",
                       "Coordinates", "CoordinateId").NotNullable()
                  .WithColumn("IncidentDateTime").AsDateTime().NotNullable()
                  .WithColumn("VehicleYear").AsInt32().Nullable()
                  .WithColumn("VehicleMake").AsAnsiString(StringLengths.NAME).Nullable()
                  .WithColumn("VehicleVIN").AsAnsiString(StringLengths.NAME).Nullable()
                  .WithColumn("LicenseNumber").AsAnsiString(StringLengths.LICENSE_NUMBER).Nullable()
                  .WithColumn("VehiclePhoneNumber").AsAnsiString(StringLengths.PHONE_NUMBER).Nullable()
                  .WithColumn("PoliceCalled").AsBoolean().Nullable()
                  .WithColumn("PoliceDepartment").AsAnsiString(StringLengths.DESCRIPTION).Nullable()
                  .WithColumn("PoliceCaseNumber").AsAnsiString(StringLengths.CLAIM_NUMBER).Nullable()
                  .WithColumn("Witness").AsAnsiString(StringLengths.NAME).Nullable()
                  .WithColumn("WitnessPhone").AsAnsiString(StringLengths.PHONE_NUMBER).Nullable()
                  .WithColumn("AnyInjuries").AsBoolean().Nullable()
                  .WithColumn("ReportedBy").AsAnsiString(StringLengths.NAME).NotNullable()
                  .WithColumn("ReportedByPhone").AsAnsiString(StringLengths.PHONE_NUMBER).Nullable()
                  .WithColumn("IncidentNotificationDate").AsDateTime().NotNullable()
                  .WithColumn("IncidentReportedDate").AsDateTime().Nullable()
                  .WithColumn("CompletedDate").AsDateTime().Nullable()
                  .WithColumn("CreatedBy").AsAnsiString(StringLengths.DESCRIPTION).NotNullable()
                  .WithColumn("CreatedOn").AsDateTime().NotNullable();

            // NOTIFICATIONS
            Execute.Sql(Sql.CREATE_NOTIFICATION_PURPOSE);
        }

        public override void Down()
        {
            // NOTIFICATIONS
            Execute.Sql(Sql.REMOVE_NOTIFICATION_PURPOSE);

            // FOREIGN KEYS
            Delete.ForeignKey("FK_GeneralLiabilityClaims_ClaimsRepresentatives_ClaimsRepresentativeId")
                  .OnTable(TableNames.GENERAL_LIABILITY_CLAIMS);
            Delete.ForeignKey("FK_GeneralLiabilityClaims_tblEmployee_CompanyContactId")
                  .OnTable(TableNames.GENERAL_LIABILITY_CLAIMS);
            Delete.ForeignKey("FK_GeneralLiabilityClaims_OperatingCenters_OperatingCenterId")
                  .OnTable(TableNames.GENERAL_LIABILITY_CLAIMS);
            Delete.ForeignKey("FK_GeneralLiabilityClaims_Coordinates_CoordinateId")
                  .OnTable(TableNames.GENERAL_LIABILITY_CLAIMS);
            Delete.ForeignKey("FK_GeneralLiabilityClaims_LiabilityTypes_LiabilityTypeId")
                  .OnTable(TableNames.GENERAL_LIABILITY_CLAIMS);

            // TABLES
            Delete.Table(TableNames.GENERAL_LIABILITY_CLAIMS);
            Delete.Table(TableNames.CLAIMS_REPRESENTATIVES);
            Delete.Table(TableNames.LIABILITY_TYPES);
        }
    }
}
