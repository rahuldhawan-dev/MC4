using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150916105109996), Tags("Production")]
    public class AddMainBreakNotificationToContactTypesForBug2235 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"SET IDENTITY_INSERT ContactTypes ON
                    INSERT INTO ContactTypes(ContactTypeID, Name) Values(8, 'Main Break Notification')
                    SET IDENTITY_INSERT ContactTypes OFF
                    ");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM ContactLink WHERE ContactTypeID = 8; DELETE FROM ContactTypes WHERE ContactTypeID = 8;");
        }
    }
}
