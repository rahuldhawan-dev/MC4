using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170320155418030), Tags("Production")]
    public class RedoPremisesForBug3648 : Migration
    {
        public override void Up()
        {
            Delete.Table("MeterRecorderChangeOrders");
            Delete.Table("MeterRecorderHistory");
            Delete.Table("Premises");

            this.CreateLookupTableWithValues("PremiseUtilityTypes", "Fire Service", "Water Service", "Waste Water");

            Alter.Table("ServiceUtilityTypes").AlterColumn("Type").AsAnsiString(4).NotNullable();
            Execute.Sql("delete from ServiceUtilityTypes;" +
                        "dbcc checkident('ServiceUtilityTypes', reseed, 1);" +
                        "insert into ServiceUtilityTypes Values('FPUB', 'Public Fire Service');" +
                        "insert into ServiceUtilityTypes Values('FPVT', 'Private Fire Service');" +
                        "insert into ServiceUtilityTypes Values('IRRG', 'Irrigation');" +
                        "insert into ServiceUtilityTypes Values('WATR', 'Domestic Water');" +
                        "insert into ServiceUtilityTypes Values('WWTR', 'Domestic Wastewater');" +
                        "insert into MeterLocations Values('0001', 'Inside-Appointment required')" +
                        "insert into MeterLocations Values('0002', 'Outside-Appointment not required')" +
                        "insert into MeterLocations Values('0003', 'Unknown')");

            Insert.IntoTable("ServiceUtilityTypes").Rows(
                new {Type = "BULK", Description = "Bulk Water"},
                new {Type = "BUMA", Description = "Bulk Water Master"},
                new {Type = "DISC", Description = "Discharge Water"},
                new {Type = "DVST", Description = "Divestiture Service/Sold"},
                new {Type = "FLAT", Description = "Flat Rate"},
                new {Type = "FREE", Description = "Free Water"},
                new {Type = "FSDC", Description = "Fire Service1 Detector Check"},
                new {Type = "GREY", Description = "Grey Water"},
                new {Type = "MB1C", Description = "Master/Battery w/Primary Charg"},
                new {Type = "MBAC", Description = "Master/Battery w/all Charges"},
                new {Type = "NON", Description = "Not-Potable"},
                new {Type = "RUSE", Description = "Reuse/recycled water"},
                new {Type = "TEMP", Description = "Temporary Hydrant Meter"},
                new {Type = "USER", Description = "User Agreement"},
                new {Type = "WWDT", Description = "Wastewater w/Deduct Service"}
            );

            Create.Table("PremiseDistricts")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithColumn("SAPCode").AsAnsiString(8).NotNullable();
            Create.Table("PremiseAreaCodes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithColumn("SAPCode").AsAnsiString(8).NotNullable();
            Create.Table("RegionCodes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithColumn("SAPCode").AsAnsiString(8).NotNullable();
            Create.Table("MeterReadingRoutes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithColumn("SAPCode").AsAnsiString(8).NotNullable();
            Create.Table("MeterReadingRouteReadingDates")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("MeterReadingRouteId", "MeterReadingRoutes").NotNullable()
                  .WithColumn("ReadingDate").AsDateTime().NotNullable();

            Create.Table("Premises")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID")
                  .WithColumn("PremiseNumber").AsAnsiString(10).NotNullable()
                  .WithColumn("ConnectionObject").AsAnsiString(30).Nullable()
                  .WithColumn("DeviceCategory").AsAnsiString(18).Nullable()
                  .WithColumn("DeviceLocation").AsAnsiString(30).Nullable()
                  .WithColumn("Equipment").AsAnsiString(18).Nullable()
                  .WithColumn("NextMeterReadingDate").AsDateTime().Nullable()
                  .WithColumn("DeviceSerialNumber").AsAnsiString(18).Nullable()
                  .WithForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID").Nullable()
                  .WithColumn("ServiceAddressHouseNumber").AsAnsiString(10).Nullable()
                  .WithColumn("ServiceAddressFraction").AsAnsiString(10).Nullable()
                  .WithColumn("ServiceAddressApartment").AsAnsiString(10).Nullable()
                  .WithColumn("ServiceAddressStreet").AsAnsiString(60).Nullable()
                  .WithForeignKeyColumn("ServiceCityId", "Towns", "TownID").Nullable()
                  .WithForeignKeyColumn("ServiceStateId", "States", "StateID").Nullable()
                  .WithColumn("ServiceZip").AsAnsiString(10).Nullable()
                  .WithForeignKeyColumn("ServiceDistrictId", "PremiseDistricts").Nullable()
                  .WithForeignKeyColumn("AreaCodeId", "PremiseAreaCodes").Nullable()
                  .WithForeignKeyColumn("RegionCodeId", "RegionCodes").Nullable()
                  .WithForeignKeyColumn("RouteNumberId", "MeterReadingRoutes").Nullable()
                  .WithColumn("StatusCode").AsAnsiString(10).Nullable()
                  .WithForeignKeyColumn("ServiceUtilityTypeId", "ServiceUtilityTypes", "ServiceUtilityTypeID")
                  .Nullable()
                  .WithForeignKeyColumn("MeterSizeId", "ServiceSizes").Nullable()
                  .WithColumn("MeterLocationId").AsAnsiString(10).Nullable()
                  .WithColumn("MeterLocationFreeText").AsCustom("text").Nullable()
                  .WithColumn("Installation").AsAnsiString(10).Nullable()
                  .WithColumn("MeterSerialNumber").AsAnsiString(30).Nullable();

            Alter.Table("ServiceSizes").AddColumn("SAPCode").AsAnsiString(4).Nullable();
            Execute.Sql("update ServiceSizes set SAPCode = '0001' where ServiceSizeDescription = '5/8';" +
                        "update ServiceSizes set SAPCode = '0002' where ServiceSizeDescription = '3/4';" +
                        "update ServiceSizes set SAPCode = '0003' where ServiceSizeDescription = '1';" +
                        "update ServiceSizes set SAPCode = '0004' where ServiceSizeDescription = '1 1/2';" +
                        "update ServiceSizes set SAPCode = '0005' where ServiceSizeDescription = '2';" +
                        "update ServiceSizes set SAPCode = '0006' where ServiceSizeDescription = '3';" +
                        "update ServiceSizes set SAPCode = '0007' where ServiceSizeDescription = '4';" +
                        "update ServiceSizes set SAPCode = '0008' where ServiceSizeDescription = '6';" +
                        "update ServiceSizes set SAPCode = '0009' where ServiceSizeDescription = '8';" +
                        "update ServiceSizes set SAPCode = '0010' where ServiceSizeDescription = '10';" +
                        "update ServiceSizes set SAPCode = '0011' where ServiceSizeDescription = '12';" +
                        "update ServiceSizes set SAPCode = '0012' where ServiceSizeDescription = '1/2';" +
                        "update ServiceSizes set SAPCode = '0013' where ServiceSizeDescription = '14';" +
                        "update ServiceSizes set SAPCode = '0014' where ServiceSizeDescription = '16';" +
                        "update ServiceSizes set SAPCode = '0015' where ServiceSizeDescription = '5';");
        }

        public override void Down()
        {
            Delete.Table("Premises");
            Delete.Table("PremiseDistricts");
            Delete.Table("PremiseAreaCodes");
            Delete.Table("RegionCodes");
            Delete.Table("MeterReadingRouteReadingDates");
            Delete.Table("MeterReadingRoutes");
            Delete.Table("PremiseUtilityTypes");
            Create.Table("Premises").WithIdentityColumn();
            Create.Table("MeterRecorderChangeOrders").WithIdentityColumn();
            Create.Table("MeterRecorderHistory").WithIdentityColumn();
            Delete.Column("SapCode").FromTable("ServiceSizes");
            Execute.Sql("delete from ServiceUtilityTypes;" +
                        "delete from MeterLocations");
            Alter.Table("ServiceUtilityTypes").AlterColumn("Type").AsAnsiString(1).NotNullable();
        }
    }
}
