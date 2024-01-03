using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20161114210621543), Tags("Production")]
    public class AddMarkupBoolToWorkOrderInvoiceScheduleOfValueForBug3344 : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrderInvoicesScheduleOfValues")
                 .AddColumn("IncludeMarkup")
                 .AsBoolean()
                 .WithDefaultValue(true);
        }

        public override void Down()
        {
            Delete.Column("IncludeMarkup").FromTable("WorkOrderInvoicesScheduleOfValues");
        }
    }
}
