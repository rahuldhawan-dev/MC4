using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180620142906346), Tags("Production")]
    public class UpdateFieldNameShortCycleWorkOrder : Migration
    {
        public override void Up()
        {
            Rename.Column("LongText").OnTable("ShortCycleWorkOrders").To("WorkOrderLongText");
        }

        public override void Down()
        {
            Rename.Column("WorkOrderLongText").OnTable("ShortCycleWorkOrders").To("LongText");
        }
    }
}
