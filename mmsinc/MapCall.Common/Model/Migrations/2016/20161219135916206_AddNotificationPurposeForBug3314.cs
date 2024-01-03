using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161219135916206), Tags("Production")]
    public class AddNotificationPurposeForBug3314 : Migration
    {
        public override void Up()
        {
            Execute.Sql("insert into NotificationPurposes Values(73, 'SAPErrorCode Occurred');" +
                        "insert into NotificationConfigurations SELECT 466, 14, NotificationPurposeId from notificationPurposes where ModuleId = 73 AND Purpose = 'SAPErrorCode Occurred'" +
                        "insert into NotificationConfigurations SELECT 217, 14, NotificationPurposeId from notificationPurposes where ModuleId = 73 AND Purpose = 'SAPErrorCode Occurred' " +
                        "IF NOT EXISTS (SELECT 1 FROM HydrantStatuses WHERE Description = 'INACTIVE') INSERT INTO HydrantStatuses VALUES('INACTIVE', 1)" +
                        "IF NOT EXISTS(SELECT 1 FROM HydrantStatuses WHERE Description = 'REMOVED') INSERT INTO HydrantStatuses VALUES('REMOVED', 1)" +
                        "IF NOT EXISTS(SELECT 1 FROM AssetStatuses WHERE Description = 'INACTIVE')  INSERT INTO AssetStatuses VALUES('INACTIVE')" +
                        "IF NOT EXISTS(SELECT 1 FROM AssetStatuses WHERE Description = 'INSTALLED') INSERT INTO AssetStatuses VALUES('INSTALLED')" +
                        "IF NOT EXISTS(SELECT 1 FROM AssetStatuses WHERE Description = 'REQUEST CANCELLATION') INSERT INTO AssetStatuses VALUES('REQUEST CANCELLATION')" +
                        "IF NOT EXISTS(SELECT 1 FROM AssetStatuses WHERE Description = 'REQUEST RETIREMENT') INSERT INTO AssetStatuses VALUES('REQUEST RETIREMENT')");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM NotificationConfigurations WHERE NotificationPurposeID = (Select NotificationPurposeId from NotificationPurposes WHERE ModuleId = 73 AND Purpose = 'SAPErrorCode Occurred')" +
                "DELETE FROM NotificationPurposes WHERE ModuleID = 73 AND Purpose = 'SAPErrorCode Occurred'");
        }
    }
}
