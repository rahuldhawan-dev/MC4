using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160414094002119), Tags("Production")]
    public class AddNotificationPurposeForBug2876 : Migration
    {
        public const string PURPOSE = "Asset Order Completed";

        public override void Up()
        {
            Execute.Sql($"INSERT INTO NotificationPurposes (ModuleId, Purpose) VALUES (34, '{PURPOSE}')");
        }

        public override void Down()
        {
            Execute.Sql($"DELETE FROM NotificationPurposes WHERE Purpose = '{PURPOSE}'");
        }
    }
}
