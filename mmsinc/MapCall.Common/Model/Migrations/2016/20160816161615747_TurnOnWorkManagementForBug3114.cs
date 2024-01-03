using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160816161615747), Tags("Production")]
    public class TurnOnWorkManagementForBug3114 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "UPDATE OperatingCenters SET WorkOrdersEnabled = 1 where OperatingCenterCode in ('NJ10', 'NJ11', 'NJ12', 'NJ13');");
        }

        public override void Down()
        {
            Execute.Sql(
                "UPDATE OperatingCenters SET WorkOrdersEnabled = 0 where OperatingCenterCode in ('NJ10', 'NJ11', 'NJ12', 'NJ13');");
        }
    }
}
