using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130911140256), Tags("Production")]
    public class AddSAPNotificationNumberToWorkOrders : Migration
    {
        public const string TABLE = "WorkOrders";
        public const string COLUMN = "SAPNotificationNumber";

        public override void Up()
        {
            Alter.Table(TABLE).AddColumn(COLUMN).AsInt64().Nullable();
        }

        public override void Down()
        {
            Delete.Column(COLUMN).FromTable(TABLE);
        }
    }
}
