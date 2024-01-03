using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180507090151640), Tags("Production")]
    public class AddStatusNoteFieldToStatusUpdatesForWO230579 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderStatusUpdates")
                 .AddColumn("StatusNotes").AsCustom("text").Nullable();
        }

        public override void Down()
        {
            Delete.Column("StatusNotes").FromTable("ShortCycleWorkOrderStatusUpdates");
        }
    }
}
