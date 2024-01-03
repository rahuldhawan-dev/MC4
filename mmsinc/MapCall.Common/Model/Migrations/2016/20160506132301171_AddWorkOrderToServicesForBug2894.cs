using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160506132301171), Tags("Production")]
    public class AddWorkOrderToServicesForBug2894 : Migration
    {
        public override void Up()
        {
            Alter.Table("Services").AddForeignKeyColumn("WorkOrderId", "WorkOrders", "WorkOrderID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Services", "WorkOrderId", "WorkOrders", "WorkOrderID");
        }
    }
}
