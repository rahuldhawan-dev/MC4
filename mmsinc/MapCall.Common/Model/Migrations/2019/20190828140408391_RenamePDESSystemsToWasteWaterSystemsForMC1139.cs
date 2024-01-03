using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190828140408391), Tags("Production")]
    public class RenamePDESSystemsToWasteWaterSystemsForMC1139 : Migration
    {
        public override void Up()
        {
            Rename.Table("PDESSystems").To("WasteWaterSystems");

            Rename.Column("PDESID").OnTable("EnvironmentalPermits").To("WasteWaterSystemId");

            Rename.Column("PDESPermitNumber").OnTable("WasteWaterSystems").To("PermitNumber");

            Rename.Column("PDESSystemId").OnTable("SewerManholes").To("WasteWaterSystemId");

            Rename.Column("PDESSystemId").OnTable("tblFacilities").To("WasteWaterSystemId");

            Rename.Table("PDESSystemsTowns").To("WasteWaterSystemsTowns");

            Rename.Column("PDESSystemId").OnTable("WasteWaterSystemsTowns").To("WasteWaterSystemId");
        }

        public override void Down()
        {
            Rename.Column("WasteWaterSystemId").OnTable("WasteWaterSystemsTowns").To("PDESSystemId");

            Rename.Table("WasteWaterSystemsTowns").To("PDESSystemsTowns");

            Rename.Column("WasteWaterSystemId").OnTable("tblFacilities").To("PDESSystemId");

            Rename.Column("WasteWaterSystemId").OnTable("SewerManholes").To("PDESSystemId");

            Rename.Column("PermitNumber").OnTable("WasteWaterSystems").To("PDESPermitNumber");

            Rename.Column("WasteWaterSystemId").OnTable("EnvironmentalPermits").To("PDESID");

            Rename.Table("WasteWaterSystems").To("PDESSystems");
        }
    }
}
