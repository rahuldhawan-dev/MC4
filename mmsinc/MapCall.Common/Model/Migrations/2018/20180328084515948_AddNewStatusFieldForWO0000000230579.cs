using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180328084515948), Tags("Production")]
    public class AddNewStatusFieldForWO0000000230579 : Migration
    {
        public override void Up()
        {
            Rename.Column("Status").OnTable("ShortCycleWorkOrderStatusUpdates").To("StatusNumber");
            Alter.Table("ShortCycleWorkOrderStatusUpdates").AddColumn("StatusNonNumber").AsAnsiString(5)
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("StatusNonNumber").FromTable("ShortCycleWorkOrderStatusUpdates");
            Rename.Column("StatusNumber").OnTable("ShortCycleWorkOrderStatusUpdates").To("Status");
        }
    }
}
