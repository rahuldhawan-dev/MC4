using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161006090802071), Tags("Production")]
    public class AddTablesForMeterChangesOutsForBug3015 : Migration
    {
        public struct StringLengths
        {
            public const int
                DESCRIPTION = 50,
                PREMISE_NUMBER = 15,
                ACCOUNT_NUMBER = 50;
        }

        public override void Up()
        {
            Delete.Table("MeterChangeOuts");

            // METER CHANGEOUT STATUSES
            this.CreateLookupTableWithValues("MeterChangeOutStatuses", "Already Changed", "AW to Complete", "Changed",
                "LVM", "Markout Required", "Problem", "Problem Created SO", "Problem-IP", "Saturday Work", "Scheduled",
                "Shutoff Done", "Shutoff Pending", "Shutoff Scheduled", "Vault");

            // METER LOCATIONS
            this.CreateLookupTableWithValues("SmallMeterLocations", "Cellar / Basement", "Curb", "Utility room",
                "Bathroom", "Closet", "Crawl Space", "Sidewalk", "Driveway", "Fence", "Property Line", "Alley",
                "Parking Lot", "Lawn / Field", "Street", "Wall", "Garage", "Shop", "Main Building", "Vault");

            this.CreateLookupTableWithValues("CustomerMeterLocations", "Basement", "Crawl Space", "Fire System",
                "Garage", "Inside", "Outside", "Unknown", "Vault");

            this.CreateLookupTableWithValues("MeterDirections", "Front", "Left Side", "Rear", "Right Side");

            this.CreateLookupTableWithValues("MeterSupplementalLocations", "Inside", "Outside", "Secure Access");

            this.CreateLookupTableWithValues("MeterChangeOutWorkScopes", "MMSI SHUTOFF", "NJAW SHUTOFF", "ACTIVE SITE",
                "CHANGE METER", "INACTIVE SITE", "RETURN VISIT", "R900 REQUIRED");

            this.CreateLookupTableWithValues("WaterServiceStatuses", "ON", "OFF", "UTD");

            // Contractor Meter Crews
            // Contractor, Name, TimeFrame, # of Meters per timeframe
            Create.Table("ContractorMeterCrews")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ContractorId", "Contractors", "ContractorId").NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION).NotNullable()
                  .WithColumn("AMMeters").AsInt32().NotNullable()
                  .WithColumn("PMMeters").AsInt32().NotNullable()
                  .WithColumn("AMLargeMeters").AsInt32().NotNullable()
                  .WithColumn("PMLargeMeters").AsInt32().NotNullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable();

            this.CreateLookupTableWithValues("MeterScheduleTimes", "AM 8-12", "AM @ 6:00", "AM @ 6:30", "AM @ 7:00",
                "AM @ 8:00",
                "PM 12-4", "PM 4-7", "PM Last", "PM @ 1:00", "PM @ 12:00");
            Alter.Table("MeterScheduleTimes").AddColumn("AM").AsBoolean().NotNullable().WithDefaultValue(false);
            Execute.Sql("UPDATE MeterScheduleTimes SET AM = 1 WHERE Description like 'AM%'");

            Create.Table("MeterChangeOutContracts")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(255).NotNullable()
                  .WithForeignKeyColumn("ContractorId", "Contractors", "ContractorID").NotNullable()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID").NotNullable()
                  .WithColumn("DateCreated").AsDateTime().NotNullable();

            Create.Table("MeterChangeOuts")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("MeterChangeOutContractId", "MeterChangeOutContracts").NotNullable()
                  .WithColumn("PremiseNumber").AsAnsiString(StringLengths.PREMISE_NUMBER).Nullable()
                  .WithColumn("AccountNumber").AsAnsiString(StringLengths.ACCOUNT_NUMBER).Nullable()
                  .WithColumn("CustomerName").AsAnsiString(60).NotNullable()
                  .WithColumn("ServiceStreetNumber").AsAnsiString(10).NotNullable()
                  .WithColumn("ServiceStreet").AsAnsiString(60).NotNullable()
                  .WithForeignKeyColumn("ServiceCityId", "Towns", "TownID").Nullable() // Impedance Mismatch/Sync??
                  .WithColumn("ServiceZip").AsAnsiString(10).Nullable()
                  .WithColumn("ServicePhone").AsAnsiString(20).Nullable()
                  .WithColumn("ServicePhoneExtension").AsAnsiString(10).Nullable()
                  .WithColumn("ServicePhone2").AsAnsiString(20).Nullable()
                  .WithColumn("ServicePhone2Extension").AsAnsiString(10).Nullable()
                  .WithForeignKeyColumn("OwnerCustomerMeterLocationId", "CustomerMeterLocations").Nullable()
                  .WithColumn("OwnerLocationOther").AsAnsiString(50).Nullable() // ShutoffBy - Contractor or NJAW
                  .WithColumn("FieldNotes").AsCustom("text").Nullable()
                  .WithColumn("DateScheduled").AsDateTime().Nullable()
                  .WithForeignKeyColumn("MeterSizeId", "ServiceSizes")
                   //.WithColumn("MeterType") /// ALL BLANK // NO
                  .WithForeignKeyColumn("ScheduledTimeId", "MeterScheduleTimes")
                  .WithForeignKeyColumn("AssignedContractorMeterCrewId", "ContractorMeterCrews").Nullable()
                  .WithForeignKeyColumn("MeterChangeOutStatusId", "MeterChangeOutStatuses").Nullable()
                  .WithColumn("Email").AsAnsiString(255).Nullable() // Prefix??
                  .WithForeignKeyColumn("CalledInByContractorMeterCrewId", "ContractorMeterCrews").Nullable()
                  .WithColumn("RemovedSerialNumber").AsAnsiString(20).Nullable() // Serial Number in access
                  .WithColumn("OutRead").AsInt32().Nullable()
                  .WithColumn("NewSerialNumber").AsAnsiString(20).Nullable() // NewSN
                  .WithColumn("NewRFNumber").AsAnsiString(20).Nullable() // NewRF
                  .WithColumn("StartRead").AsInt32().Nullable()
                  .WithForeignKeyColumn("MeterLocationId", "SmallMeterLocations") // FK
                  .WithForeignKeyColumn("MeterSupplementalLocationId", "MeterSupplementalLocations")
                  .WithForeignKeyColumn("MeterDirectionId", "MeterDirections")
                  .WithForeignKeyColumn("RFLocationId", "SmallMeterLocations") // FK
                  .WithForeignKeyColumn("RFSupplementalLocationId", "MeterSupplementalLocations")
                  .WithForeignKeyColumn("RFDirectionId", "MeterDirections")
                  .WithForeignKeyColumn("MeterChangeOutWorkScopeId", "MeterChangeOutWorkScopes")
                  .WithColumn("IsNeptuneRFOnly").AsBoolean().Nullable()
                  .WithColumn("IsHotRodRFOnly").AsBoolean().Nullable()
                  .WithColumn("OperatedPointOfControlAtAnyValve").AsBoolean().Nullable()
                  .WithColumn("IsMuellerMeter").AsBoolean().Nullable()
                  .WithForeignKeyColumn("StartStatus", "WaterServiceStatuses").Nullable()
                  .WithForeignKeyColumn("EndStatus", "WaterServiceStatuses").Nullable()
                  .WithColumn("TurnedOffWater").AsBoolean().Nullable()
                  .WithColumn("MovedMeterToPit").AsBoolean().Nullable()
                  .WithColumn("RanNewWire").AsBoolean().Nullable()
                  .WithColumn("InstalledJumperBar").AsBoolean().Nullable()
                  .WithColumn("ContractorDrilledLid").AsBoolean().Nullable()
                  .WithColumn("Canvassed").AsBoolean().Nullable()
                  .WithColumn("ClickAdvantexUpdated").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("YearInstalled").AsInt32().Nullable()
                  .WithColumn("SAPOrderNumber").AsString(9).Nullable()
                  .WithColumn("NumberOfDials").AsInt32().Nullable()
                  .WithColumn("EquipmentID").AsAnsiString(20).Nullable()
                  .WithColumn("RouteNumber").AsAnsiString(10).Nullable()
                  .WithColumn("MeterReadCode1").AsAnsiString(10).Nullable()
                  .WithColumn("MeterReadComment1").AsAnsiString(50).Nullable()
                  .WithColumn("MeterReadCode2").AsAnsiString(10).Nullable()
                  .WithColumn("MeterReadComment2").AsAnsiString(50).Nullable()
                  .WithColumn("MeterReadCode3").AsAnsiString(10).Nullable()
                  .WithColumn("MeterReadComment3").AsAnsiString(50).Nullable()
                  .WithColumn("MeterReadCode4").AsAnsiString(10).Nullable()
                  .WithColumn("MeterReadComment4").AsAnsiString(50).Nullable()
                  .WithColumn("MeterCommentsPremise").AsAnsiString(255).Nullable()
                  .WithColumn("DateStatusChanged").AsDateTime().Nullable()
                ;
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_MeterChangeOuts_MeterSupplementalLocations_MeterSupplementalLocationId")
                  .OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_SmallMeterLocations_MeterLocationId").OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_MeterDirections_MeterDirectionId").OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_MeterSupplementalLocations_RFSupplementalLocationId")
                  .OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_SmallMeterLocations_RFLocationId").OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_MeterDirections_RFDirectionId").OnTable("MeterChangeOuts");

            Delete.ForeignKey("FK_MeterChangeOuts_MeterChangeOutWorkScopes_MeterChangeOutWorkScopeId")
                  .OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_MeterChangeOutStatuses_MeterChangeOutStatusId")
                  .OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_ContractorMeterCrews_AssignedContractorMeterCrewId")
                  .OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_ContractorMeterCrews_CalledInByContractorMeterCrewId")
                  .OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_MeterChangeOutContracts_MeterChangeOutContractId")
                  .OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_CustomerMeterLocations_OwnerCustomerMeterLocationId")
                  .OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_MeterScheduleTimes_ScheduledTimeId").OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_ServiceSizes_MeterSizeId").OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOuts_Towns_ServiceCityId").OnTable("MeterChangeOuts");
            Delete.ForeignKey("FK_MeterChangeOutContracts_Contractors_ContractorId").OnTable("MeterChangeOutContracts");
            Delete.ForeignKey("FK_MeterChangeOutContracts_OperatingCenters_OperatingCenterId")
                  .OnTable("MeterChangeOutContracts");
            Delete.Table("MeterChangeoutStatuses");
            Delete.Table("MeterChangeOutWorkScopes");
            Delete.Table("SmallMeterLocations");
            Delete.Table("CustomerMeterLocations");
            Delete.Table("MeterDirections");
            Delete.Table("MeterSupplementalLocations");
            Delete.Table("ContractorMeterCrews");
            Delete.Table("MeterScheduleTimes");
            Delete.Table("MeterChangeOuts");
            Delete.Table("MeterChangeOutContracts");
            Delete.Table("WaterServiceStatuses");

            Create.Table("MeterChangeOuts")
                  .WithIdentityColumn("MeterChangeOutId")
                  .WithColumn("ServiceOrderNumber").AsAnsiString(50).Nullable()
                  .WithColumn("Address").AsAnsiString(100).Nullable()
                  .WithForeignKeyColumn("Town", "Towns", "TownId")
                  .WithForeignKeyColumn("State", "States", "StateID")
                  .WithColumn("Zip").AsAnsiString(10).Nullable()
                  .WithColumn("BillingClass").AsAnsiString(50).Nullable()
                  .WithColumn("PremiseNumber").AsAnsiString(10).Nullable()
                  .WithColumn("OldReading").AsInt32().Nullable()
                  .WithColumn("MeterChangeOutDate").AsDateTime().Nullable()
                  .WithColumn("NewMeterNumber").AsAnsiString(50).Nullable();
        }
    }
}
