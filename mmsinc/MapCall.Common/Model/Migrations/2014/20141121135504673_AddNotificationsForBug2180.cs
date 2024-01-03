using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141121135504673), Tags("Production")]
    public class AddNotificationsForBug2180 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                SET IDENTITY_INSERT NotificationPurposes ON
                    INSERT INTO NotificationPurposes (NotificationPurposeID, ModuleID, Purpose) VALUES(29, 29, 'Pending Retirement')
                	INSERT INTO NotificationPurposes (NotificationPurposeID, ModuleID, Purpose) VALUES(30, 29, 'Retired')
                SET IDENTITY_INSERT NotificationPurposes OFF");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM NotificationPurposes WHERE Purpose = 'Pending Retirement';");
            Execute.Sql("DELETE FROM NotificationPurposes WHERE Purpose = 'Retired';");
        }
    }
}
