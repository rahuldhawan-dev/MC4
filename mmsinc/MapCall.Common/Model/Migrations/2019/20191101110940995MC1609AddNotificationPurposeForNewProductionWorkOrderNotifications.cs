using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191101110940995), Tags("Production")]
    public class AddNotificationPurposeForNewProductionWorkOrderNotifications : Migration
    {
        public override void Up()
        {
            Execute.Sql("INSERT INTO NotificationPurposes VALUES(78, 'Production Work Order Created')");
            Execute.Sql("INSERT INTO NotificationPurposes VALUES(78, 'Production Work Order Assigned')");
            Execute.Sql("INSERT INTO NotificationPurposes VALUES(78, 'Production Work Order Completed')");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM NotificationConfigurations WHERE NotificationPurposeID = (SELECT NotificationPurposeID from NotificationPurposes WHERE ModuleID = 78 and Purpose = 'Production Work Order Created')");
            Execute.Sql(
                "DELETE FROM NotificationConfigurations WHERE NotificationPurposeID = (SELECT NotificationPurposeID from NotificationPurposes WHERE ModuleID = 78 and Purpose = 'Production Work Order Assigned')");
            Execute.Sql(
                "DELETE FROM NotificationConfigurations WHERE NotificationPurposeID = (SELECT NotificationPurposeID from NotificationPurposes WHERE ModuleID = 78 and Purpose = 'Production Work Order Completed')");

            Execute.Sql(
                "DELETE FROM NotificationPurposes WHERE ModuleID = 78 and Purpose = 'Production Work Order Created'");
            Execute.Sql(
                "DELETE FROM NotificationPurposes WHERE ModuleID = 78 and Purpose = 'Production Work Order Assigned'");
            Execute.Sql(
                "DELETE FROM NotificationPurposes WHERE ModuleID = 78 and Purpose = 'Production Work Order Completed'");
        }
    }
}
