using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220808122511264), Tags("Production")]
    public class AddMapCallWorkOrderToIncident : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents").AddForeignKeyColumn("MapCallWorkOrderId", "WorkOrders", "WorkOrderID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Incidents", "MapCallWorkOrderId", "WorkOrders", "WorkOrderID");
        }
    }
}
