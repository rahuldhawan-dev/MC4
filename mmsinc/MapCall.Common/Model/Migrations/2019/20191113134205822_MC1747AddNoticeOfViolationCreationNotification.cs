using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191113134205822), Tags("Production")]
    public class MC1747AddNoticeOfViolationCreationNotification : Migration
    {
        public override void Up()
        {
            Execute.Sql("INSERT INTO NotificationPurposes Values(58, 'Environmental NonCompliance Event Created')");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM NotificationConfigurations WHERE NotificationPurposeID = (SELECT NotificationPurposeID from NotificationPurposes WHERE ModuleID = 58 and Purpose = 'Environmental NonCompliance Event Created')");
            Execute.Sql(
                "DELETE FROM NotificationPurposes WHERE ModuleID = 58 and Purpose = 'Environmental NonCompliance Event Created'");
        }
    }
}
