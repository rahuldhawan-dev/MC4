using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190108092937579), Tags("Production")]
    public class AddNotificationPurposeForMC630 : Migration
    {
        public override void Up()
        {
            Execute.Sql("INSERT INTO NotificationPurposes Values(34, 'FRCC Emergency Created')");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM NotificationConfigurations WHERE NotificationPurposeID = (SELECT NotificationPurposeID from NotificationPurposes WHERE ModuleID = 34 and Purpose = 'FRCC Emergency Created')");
            Execute.Sql("DELETE FROM NotificationPurposes WHERE ModuleID = 34 and Purpose = 'FRCC Emergency Created')");
        }
    }
}
