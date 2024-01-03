using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180614091820593), Tags("Production")]
    public class AddMoreShortCycleWorkOrderColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AddColumn("LongText").AsAnsiString(40).Nullable();
        }

        public override void Down()
        {
            Delete.Column("LongText").FromTable("ShortCycleWorkOrders");
        }
    }
}
