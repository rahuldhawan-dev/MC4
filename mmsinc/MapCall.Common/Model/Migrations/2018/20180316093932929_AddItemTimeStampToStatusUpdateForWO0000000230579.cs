using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180316093932929), Tags("Production")]
    public class AddItemTimeStampToStatusUpdateForWO0000000230579 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderStatusUpdates")
                 .AddColumn("ItemTimeStamp").AsAnsiString()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("ItemTimeStamp").FromTable("ShortCycleWorkOrderStatusUpdates");
        }
    }
}
