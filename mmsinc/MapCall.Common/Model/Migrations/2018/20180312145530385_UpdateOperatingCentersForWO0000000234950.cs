using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180312145530385), Tags("Production")]
    public class UpdateOperatingCentersForWO0000000234950 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                update OperatingCenters set OperatingCenterCode = 'MO2' where OperatingCenterId = 65;
                update OperatingCenters set OperatingCenterCode = 'MO9' where OperatingCenterId = 63;
                update OperatingCenters set OperatingCenterCode = 'MO3' where OperatingCenterId = 64;
                update OperatingCenters set OperatingCenterCode = 'MO4' where OperatingCenterId = 62;
                update OperatingCenters set OperatingCenterCode = 'MO6' where OperatingCenterId = 66;");
        }

        public override void Down()
        {
            Execute.Sql(@"
                update OperatingCenters set OperatingCenterCode = 'MO1' where OperatingCenterId = 65;
                update OperatingCenters set OperatingCenterCode = 'MO2' where OperatingCenterId = 63;
                update OperatingCenters set OperatingCenterCode = 'MO3' where OperatingCenterId = 64;
                update OperatingCenters set OperatingCenterCode = 'MO6' where OperatingCenterId = 62;
                update OperatingCenters set OperatingCenterCode = 'MO7' where OperatingCenterId = 66;");
        }
    }
}
