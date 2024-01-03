using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221212075343517), Tags("Production")]
    public class MC5199CreateViewForCount : Migration
    {
        public const string VIEW_NAME = "CountEquipmentMaintenancePlansByMaintenancePlanView",
                            CREATE_VIEW = "CREATE VIEW " + VIEW_NAME + " AS " +
                                          "SELECT MaintenancePlanId, count(MaintenancePlanId) as AssetCount " +
                                          "FROM EquipmentMaintenancePlans " +
                                          "GROUP BY MaintenancePlanId",
                            DROP_VIEW = "DROP VIEW " + VIEW_NAME;

        public override void Up()
        {
            Execute.Sql(CREATE_VIEW);
        }

        public override void Down()
        {
            Execute.Sql(DROP_VIEW);
        }
    }
}

