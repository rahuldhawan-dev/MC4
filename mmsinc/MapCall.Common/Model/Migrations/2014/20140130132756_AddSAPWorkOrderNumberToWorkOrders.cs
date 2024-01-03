using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140130132756), Tags("Production")]
    public class AddSAPWorkOrderNumberToWorkOrders : Migration
    {
        public const string TABLE = "WorkOrders";
        public const string COLUMN = "SAPWorkOrderNumber";

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
