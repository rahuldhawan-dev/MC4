using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160816100741276), Tags("Production")]
    public class AddLookupValueForBug3111 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM WorkOrderRequesters WHERE description = 'FRCC') INSERT INTO WorkOrderRequesters Values('FRCC');");
            Execute.Sql(
                "INSERT INTO NotificationPurposes SELECT (select ModuleID from Modules where name = 'Work Management'), 'FRCC Emergency Completed'");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM NotificationPurposes Where ModuleID = (select ModuleID from Modules where name = 'Work Management') and Purpose = 'FRCC Emergency Completed'");
        }
    }
}
