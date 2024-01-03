using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160613102026207), Tags("Production")]
    public class UpdateNotificationsForBug2995 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "update NotificationPurposes set purpose = 'Service Renewal Record Closed' where Purpose = 'Service Renewal Created';" +
                "INSERT INTO NotificationPurposes VALUES(34,'Service Line Renewal Completed')");
        }

        public override void Down()
        {
            Execute.Sql(
                "update NotificationPurposes set purpose = 'Service Renewal Created' where Purpose = 'Service Renewal Record Closed'");
        }
    }
}
