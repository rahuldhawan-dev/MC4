using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180313083646796), Tags("Production")]
    public class RenameCreateDateTimeOnShortCycleWorkOrderStatusUpdatesToReceivedAt : Migration
    {
        public override void Up()
        {
            Rename.Column("CreateDateTime").OnTable("ShortCycleWorkOrderStatusUpdates").To("ReceivedAt");
        }

        public override void Down()
        {
            Rename.Column("ReceivedAt").OnTable("ShortCycleWorkOrderStatusUpdates").To("CreateDateTime");
        }
    }
}
