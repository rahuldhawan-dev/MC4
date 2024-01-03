using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180919094609586), Tags("Production")]
    public class CreateNotificationPurposeForMC634 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "insert into NotificationPurposes (ModuleId, Purpose) SELECT m.ModuleId, 'Short Cycle Work Order SAP Error' FROM Modules m INNER JOIN Applications a ON m.ApplicationID = a.ApplicationID WHERE a.Name = 'Field Services' AND m.Name = 'Work Management'");
        }

        public override void Down()
        {
            Execute.Sql(
                "delete from NotificationPurposes WHERE Purpose = 'Short Cycle Work Order SAP Error' AND ModuleId = (SELECT TOP 1 m.ModuleId FROM Modules m INNER JOIN Applications a ON m.ApplicationID = a.ApplicationID WHERE a.Name = 'Field Services' AND m.Name = 'Work Management')");
        }
    }
}
