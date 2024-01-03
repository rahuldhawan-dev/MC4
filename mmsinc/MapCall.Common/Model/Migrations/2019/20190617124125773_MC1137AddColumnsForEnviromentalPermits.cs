using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190617124125773), Tags("Production")]
    public class MC1137AddColumnsForEnviromentalPermits : Migration
    {
        public override void Up()
        {
            Alter.Table("EnvironmentalPermits").AddForeignKeyColumn("StateId", "States", "StateId");
            Alter.Table("EnvironmentalPermits").AddForeignKeyColumn("FacilityTypeId", "WaterTypes");
            Execute.Sql(
                "UPDATE EnvironmentalPermits SET StateId = (SELECT pws.StateID FROM PublicWaterSupplies pws WHERE pws.Id = PublicWaterSupplyId)");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("EnvironmentalPermits", "StateId", "States", "StateID");
            Delete.ForeignKeyColumn("EnvironmentalPermits", "FacilityTypeId", "WaterTypes");
        }
    }
}
