using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200408113432182), Tags("Production")]
    public class MC1850AddNewAttributesRenameAttributesForFacility : Migration
    {
        public override void Up()
        {
            Alter.Table("tblFacilities").AddColumn("SwmStation").AsBoolean().WithDefaultValue(false).NotNullable();
            Alter.Table("tblFacilities").AddColumn("WellProd").AsBoolean().WithDefaultValue(false).NotNullable();
            Alter.Table("tblFacilities").AddColumn("WellMonitoring").AsBoolean().WithDefaultValue(false).NotNullable();
            Alter.Table("tblFacilities").AddColumn("ClearWell").AsBoolean().WithDefaultValue(false).NotNullable();
            Alter.Table("tblFacilities").AddColumn("RawWaterIntake").AsBoolean().WithDefaultValue(false).NotNullable();
            Rename.Column("SewerTreatment").OnTable("tblFacilities").To("WasteWaterTreatmentFacility");
            Rename.Column("SewerLift").OnTable("tblFacilities").To("SewerLiftStation");
            Rename.Column("Booster_Pumping").OnTable("tblFacilities").To("BoosterStation");
        }

        public override void Down()
        {
            Delete.Column("SWMStation").FromTable("tblFacilities");
            Delete.Column("WellProd").FromTable("tblFacilities");
            Delete.Column("WellMonitoring").FromTable("tblFacilities");
            Delete.Column("ClearWell").FromTable("tblFacilities");
            Delete.Column("RawWaterIntake").FromTable("tblFacilities");
            Rename.Column("WasteWaterTreatmentFacility").OnTable("tblFacilities").To("SewerTreatment");
            Rename.Column("SewerLiftStation").OnTable("tblFacilities").To("SewerLift");
            Rename.Column("BoosterStation").OnTable("tblFacilities").To("Booster_Pumping");
        }
    }
}
