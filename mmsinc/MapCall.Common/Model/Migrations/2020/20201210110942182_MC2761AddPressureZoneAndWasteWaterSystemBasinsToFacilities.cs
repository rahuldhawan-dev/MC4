using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201210110942182), Tags("Production")]
    public class MC2761_AddPressureZoneAndWasteWaterSystemBasinsToFacilities : Migration
    {
        public override void Up()
        {
            Alter
               .Table("tblFacilities")
               .AddForeignKeyColumn("PublicWaterSupplyPressureZoneId", "PublicWaterSupplyPressureZones")
               .AddForeignKeyColumn("WasteWaterSystemBasinId", "WasteWaterSystemBasins");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblFacilities", "PublicWaterSupplyPressureZoneId",
                "PublicWaterSupplyPressureZones");
            Delete.ForeignKeyColumn("tblFacilities", "WasteWaterSystemBasinId", "WasteWaterSystemBasins");
        }
    }
}
