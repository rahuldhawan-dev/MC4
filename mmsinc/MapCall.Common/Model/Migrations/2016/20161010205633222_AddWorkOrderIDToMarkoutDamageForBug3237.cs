using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161010205633222), Tags("Production")]
    public class AddWorkOrderIDToMarkoutDamageForBug3237 : Migration
    {
        public override void Up()
        {
            Alter.Table("MarkoutDamages").AddForeignKeyColumn("WorkOrderId", "WorkOrders", "WorkOrderID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MarkoutDamages", "WorkOrderId", "WorkOrders", "WorkOrderID");
        }
    }
}
