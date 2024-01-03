using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220111120251040), Tags("Production")]
    public class MC3861UpdateColumnDatatypeInPublicWaterSupplyFirmCapacitiesTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"update PublicWaterSupplyFirmCapacities set CurrentSystemPeakDailyDemandYearMonth = null;");
            Alter.Column("CurrentSystemPeakDailyDemandYearMonth").OnTable("PublicWaterSupplyFirmCapacities").AsDateTime().Nullable();
        }
        public override void Down() { }
    }
}

