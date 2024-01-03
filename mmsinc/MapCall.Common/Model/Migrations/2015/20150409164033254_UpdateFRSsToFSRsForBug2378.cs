using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150409164033254), Tags("Production")]
    public class UpdateFRSsToFSRsForBug2378 : Migration
    {
        public override void Up()
        {
            Execute.Sql("update VehicleIcons set Description = 'FSRs' where Description = 'FRSs'");
        }

        public override void Down()
        {
            Execute.Sql("update VehicleIcons set Description = 'FRSs' where Description = 'FSRs'");
        }
    }
}
