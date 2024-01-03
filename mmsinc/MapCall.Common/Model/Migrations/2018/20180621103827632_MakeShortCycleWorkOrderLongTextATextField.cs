using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180621103827632), Tags("Production")]
    public class MakeShortCycleWorkOrderLongTextATextField : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AlterColumn("WorkOrderLongText").AsCustom("text").Nullable();
        }

        public override void Down()
        {
            // Can't down, would lose data
        }
    }
}
