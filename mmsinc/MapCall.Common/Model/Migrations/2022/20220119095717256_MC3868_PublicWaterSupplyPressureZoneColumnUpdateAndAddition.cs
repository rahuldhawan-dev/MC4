using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220119095717256), Tags("Production")]
    public class MC3868_PublicWaterSupplyPressureZoneColumnUpdateAndAddition : Migration
    {
        public override void Up()
        {
            Rename.Column("Name").OnTable("PublicWaterSupplyPressureZones").To("HydraulicModelName");
            Alter.Table("PublicWaterSupplyPressureZones").AddColumn("CommonName").AsString(50).Nullable();
            Alter.Column("PressureMin").OnTable("PublicWaterSupplyPressureZones").AsInt32().Nullable();
            Alter.Column("PressureMax").OnTable("PublicWaterSupplyPressureZones").AsInt32().Nullable();
        }

        public override void Down()
        {
            Rename.Column("HydraulicModelName").OnTable("PublicWaterSupplyPressureZones").To("Name");
            Delete.Column("CommonName").FromTable("PublicWaterSupplyPressureZones");

            // If data was entered after a migration but before a rollback occurs, we need to set a default value for any records
            // where PressureMin/PressureMax is null before setting them back to not nullable.
            Execute.Sql("UPDATE PublicWaterSupplyPressureZones SET PressureMin = 0 WHERE PressureMin IS NULL");
            Execute.Sql("UPDATE PublicWaterSupplyPressureZones SET PressureMax = 0 WHERE PressureMax IS NULL");

            // Now it's safe to set these back to not nullable.
            Alter.Column("PressureMin").OnTable("PublicWaterSupplyPressureZones").AsInt32().NotNullable();
            Alter.Column("PressureMax").OnTable("PublicWaterSupplyPressureZones").AsInt32().NotNullable();
        }
    }
}