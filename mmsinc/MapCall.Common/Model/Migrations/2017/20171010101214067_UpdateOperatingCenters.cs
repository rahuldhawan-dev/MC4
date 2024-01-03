using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20171010101214067), Tags("Production")]
    public class UpdateOperatingCenters : Migration
    {
        public override void Up()
        {
            // WO0000000195014
            // WO0000000195871

            Execute.Sql(@"
update OperatingCenters set OperatingCenterCode = 'KY2' where OperatingCenterCode = 'KY1'
update OperatingCenters set OperatingCenterCode = 'IL65' where OperatingCenterCode = 'IL11'
update OperatingCenters set OperatingCenterCode = 'IL77' where OperatingCenterCode = 'IL9'
update OperatingCenters set OperatingCenterCode = 'IL34' where OperatingCenterCode = 'IL12'
update OperatingCenters set OperatingCenterCode = 'IL86' where OperatingCenterCode = 'IL13'
update OperatingCenters set OperatingCenterCode = 'IL90' where OperatingCenterCode = 'IL4'
update OperatingCenters set OperatingCenterCode = 'IL35' where OperatingCenterCode = 'IL8'
");
        }

        public override void Down()
        {
            Execute.Sql(@"
update OperatingCenters set OperatingCenterCode = 'KY1' where OperatingCenterCode = 'KY2'
update OperatingCenters set OperatingCenterCode = 'IL11' where OperatingCenterCode = 'IL65'
update OperatingCenters set OperatingCenterCode = 'IL9' where OperatingCenterCode = 'IL77'
update OperatingCenters set OperatingCenterCode = 'IL12' where OperatingCenterCode = 'IL34'
update OperatingCenters set OperatingCenterCode = 'IL13' where OperatingCenterCode = 'IL86'
update OperatingCenters set OperatingCenterCode = 'IL4' where OperatingCenterCode = 'IL90'
update OperatingCenters set OperatingCenterCode = 'IL8' where OperatingCenterCode = 'IL35'
");
        }
    }
}
