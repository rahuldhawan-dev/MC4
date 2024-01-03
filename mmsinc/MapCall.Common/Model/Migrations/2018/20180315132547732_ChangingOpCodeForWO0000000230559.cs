using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180315132547732), Tags("Production")]
    public class ChangingOpCodeForWO0000000230559 : Migration
    {
        public override void Up()
        {
            Execute.Sql("UPDATE OperatingCenters SET OperatingCenterCode = 'VA3' WHERE OperatingCenterId = 126");
        }

        public override void Down()
        {
            Execute.Sql("UPDATE OperatingCenters SET OperatingCenterCode = 'PA79' WHERE OperatingCenterId = 126");
        }
    }
}
