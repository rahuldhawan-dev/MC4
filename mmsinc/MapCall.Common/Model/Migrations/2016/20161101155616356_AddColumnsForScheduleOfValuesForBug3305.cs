using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161101155616356), Tags("Production")]
    public class AddColumnsForScheduleOfValuesForBug3305 : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrderInvoices")
                 .AddColumn("InvoiceNotes")
                 .AsAnsiString()
                 .Nullable();
            Alter.Table("WorkOrderInvoicesScheduleOfValues")
                 .AddColumn("IncludeWithInvoice")
                 .AsBoolean()
                 .WithDefaultValue(true);
            Execute.Sql("update ScheduleOfValues set UnitOfMeasureId = 12 where Id = 100");
        }

        public override void Down()
        {
            Delete.Column("InvoiceNotes").FromTable("WorkOrderInvoices");
            Delete.Column("IncludeWithInvoice").FromTable("WorkOrderInvoicesScheduleOfValues");
            Execute.Sql("update ScheduleOfValues set UnitOfMeasureId = 15 where Id = 100");
        }
    }
}
