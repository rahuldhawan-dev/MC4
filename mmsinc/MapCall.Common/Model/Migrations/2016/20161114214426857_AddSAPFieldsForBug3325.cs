using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161114214426857), Tags("Production")]
    public class AddSAPFieldsForBug3325 : Migration
    {
        public override void Up()
        {
            Alter.Table("TownSections").AddColumn("Zip").AsAnsiString(10).Nullable();

            Alter.Table("MainTypes").AddColumn("SAPCode").AsAnsiString(30).Nullable();
            Execute.Sql("Update MainTypes set SAPCode = 'OTH'  WHERE Description = '3M structural'");
            Execute.Sql("Update MainTypes set SAPCode = 'CI'   WHERE Description = 'CAST IRON'");
            Execute.Sql("Update MainTypes set SAPCode = 'CIL'  WHERE Description = 'Cast Iron Lined'");
            Execute.Sql("Update MainTypes set SAPCode = 'CIU'  WHERE Description = 'Cast Iron Unlined'");
            Execute.Sql("Update MainTypes set SAPCode = 'PCCP'  WHERE Description = 'Cement'");
            Execute.Sql("Update MainTypes set SAPCode = 'OTH'  WHERE Description = 'CIPP-epoxy sock'");
            Execute.Sql("Update MainTypes set SAPCode = 'CU' WHERE Description = 'Copper'");
            Execute.Sql("Update MainTypes set SAPCode = 'DI' WHERE Description = 'Ductile Iron'");
            Execute.Sql("Update MainTypes set SAPCode = 'GALV' WHERE Description = 'Galvanized'");
            Execute.Sql("Update MainTypes set SAPCode = 'HDPE' WHERE Description = 'HDPE'");
            Execute.Sql("Update MainTypes set SAPCode = 'CEM' WHERE Description = 'Lock Joint'");
            Execute.Sql("Update MainTypes set SAPCode = 'PVC' WHERE Description = 'Plastic'");
            Execute.Sql("Update MainTypes set SAPCode = 'PCCP' WHERE Description = 'Pre St Concrete'");
            Execute.Sql("Update MainTypes set SAPCode = 'ST' WHERE Description = 'Steel'");
            Execute.Sql("Update MainTypes set SAPCode = 'AC' WHERE Description = 'Transite'");
            Execute.Sql("Update MainTypes set SAPCode = 'VCP' WHERE Description = 'Vitrified Clay'");
            Alter.Column("SAPCode").OnTable("MainTypes").AsAnsiString(30).Nullable();

            Alter.Table("SewerManholeMaterials").AddColumn("SAPCode").AsAnsiString(30).Nullable();
            Execute.Sql("Update SewerManholeMaterials set SAPCode = 'OTHER' WHERE Description = 'Block'");
            Execute.Sql("Update SewerManholeMaterials set SAPCode = 'BRICK' WHERE Description = 'Brick'");
            Execute.Sql("Update SewerManholeMaterials set SAPCode = 'PRECASE CONCRETE' WHERE Description = 'Concrete'");
            Execute.Sql("Update SewerManholeMaterials set SAPCode = 'PVC' WHERE Description = 'PVC'");
            Alter.Column("SAPCode").OnTable("SewerManholeMaterials").AsAnsiString(30).NotNullable();
        }

        public override void Down()
        {
            Delete.Column("SAPCode").FromTable("SewerManholeMaterials");
            Delete.Column("SAPCode").FromTable("MainTypes");
            Delete.Column("Zip").FromTable("TownSections");
        }
    }
}
