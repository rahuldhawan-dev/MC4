using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170126023101768), Tags("Production")]
    public class AddSAPWorkOrdersEnabledForBug3387 : Migration
    {
        public override void Up()
        {
            Alter.Table("OperatingCenters").AddColumn("SAPWorkOrdersEnabled").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
            Execute.Sql("insert into NotificationPurposes Values(34, 'SAPErrorCode Occurred');" +
                        "insert into NotificationConfigurations SELECT 466, 14, NotificationPurposeId from notificationPurposes where ModuleId = 34 AND Purpose = 'SAPErrorCode Occurred'" +
                        "insert into NotificationConfigurations SELECT 217, 14, NotificationPurposeId from notificationPurposes where ModuleId = 34 AND Purpose = 'SAPErrorCode Occurred' ");
        }

        public override void Down()
        {
            Delete.Column("SAPWorkOrdersEnabled").FromTable("OperatingCenters");
            Execute.Sql(
                "DELETE FROM NotificationConfigurations WHERE NotificationPurposeID = (Select NotificationPurposeId from NotificationPurposes WHERE ModuleId = 34 AND Purpose = 'SAPErrorCode Occurred')" +
                "DELETE FROM NotificationPurposes WHERE ModuleID = 34 AND Purpose = 'SAPErrorCode Occurred'");
        }
    }
}
