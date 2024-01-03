using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180504111840128), Tags("Production")]
    public class AddEquipmentNodeAndOtherFieldsForPredispatch : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("DueDate").AsAnsiString().Nullable()
                 .AddColumn("DueTime").AsAnsiString().Nullable();
            Alter.Table("ShortCycleWorkOrdersEquipmentIds")
                 .AddColumn("ProcessingIndicator")
                 .AsAnsiString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("DueTime").FromTable("ShortCycleWorkOrders");
            Delete.Column("DueDate").FromTable("ShortCycleWorkOrders");
            Delete.Column("ProcessingIndicator").FromTable("ShortCycleWorkOrdersEquipmentIds");
        }
    }
}
