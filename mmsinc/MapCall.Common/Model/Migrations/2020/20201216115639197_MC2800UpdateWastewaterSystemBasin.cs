using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201216115639197), Tags("Production")]
    public class MC2800UpdateWastewaterSystemBasin : Migration
    {
        private const string WASTE_WATER_SYSTEM_BASINS = "WasteWaterSystemBasins";

        public override void Up()
        {
            Alter.Column("FirmCapacityDateUpdated").OnTable(WASTE_WATER_SYSTEM_BASINS).AsDate().Nullable();
        }

        public override void Down()
        {
            //changing the column back to a non-nullable would cause errors 
        }
    }
}
