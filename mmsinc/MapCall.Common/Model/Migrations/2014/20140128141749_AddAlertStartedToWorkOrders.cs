using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140128141749), Tags("Production")]
    public class AddAlertStartedToWorkOrders : Migration
    {
        public const string COLUMN_NAME = "AlertStarted";

        public override void Up()
        {
            Alter.Table("WorkOrders")
                 .AddColumn(COLUMN_NAME)
                 .AsDateTime()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column(COLUMN_NAME).FromTable("WorkOrders");
        }
    }
}
