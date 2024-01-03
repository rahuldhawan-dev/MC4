using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160630151045186), Tags("Production")]
    public class AddPDESSystemsToThingsForBUg2997 : Migration
    {
        public override void Up()
        {
            Create.ManyToManyTable("WasteWaterSystems", "Towns", secondTableId: "TownId");

            Alter.Table("SewerOpenings").AddForeignKeyColumn("PDESSystemId", "WasteWaterSystems");

            Alter.Table("tblFacilities").AddForeignKeyColumn("PDESSystemId", "WasteWaterSystems");
        }

        public override void Down()
        {
            Delete.Table("WasteWaterSystems" + "Towns");

            Delete.ForeignKeyColumn("SewerOpenings", "PDESSystemId", "WasteWaterSystems");

            Delete.ForeignKeyColumn("tblFacilities", "PDESSystemId", "WasteWaterSystems");
        }
    }
}
