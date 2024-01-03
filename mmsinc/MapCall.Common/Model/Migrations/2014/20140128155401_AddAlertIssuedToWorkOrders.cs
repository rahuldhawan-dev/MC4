using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140128155401), Tags("Production")]
    public class AddAlertIssuedToWorkOrders : Migration
    {
        public const string COLUMN_NAME = "AlertIssued";

        public override void Up()
        {
            Alter.Table("WorkOrders")
                 .AddColumn(COLUMN_NAME)
                 .AsBoolean()
                 .Nullable();

            Execute.Sql("UPDATE WorkOrders SET AlertIssued = 1 WHERE AlertID IS NOT NULL AND AlertID <> '';");
        }

        public override void Down()
        {
            Delete.Column(COLUMN_NAME).FromTable("WorkOrders");
        }
    }
}
