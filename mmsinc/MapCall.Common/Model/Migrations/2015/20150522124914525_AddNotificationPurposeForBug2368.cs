using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150522124914525), Tags("Production")]
    public class AddNotificationPurposeForBug2368 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "INSERT INTO NotificationPurposes SELECT (select ModuleID from Modules where name = 'Services'), 'Renewal Installation'");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM NotificationPurposes Where ModuleID = (select ModuleID from Modules where name = 'Services') and Purpose = 'Renewal Installation'");
        }
    }
}
