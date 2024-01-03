using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180731091452850), Tags("Production")]
    public class AddReceivedAtToShortCycle : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AddColumn("ReceivedAt").AsDateTime().Nullable();
            Alter.Table("ShortCycleWorkOrderRequests").AddColumn("ReceivedAt").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("ReceivedAt").FromTable("ShortCycleWorkOrders");
            Delete.Column("ReceivedAt").FromTable("ShortCycleWorkOrderRequests");
        }
    }
}
