using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161005122819493), Tags("Production")]
    public class SwitchRelationshipBetweenServicesAndWorkOrdersForBug2927 : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders").AddForeignKeyColumn("ServiceId", "Services");

            Execute.Sql(
                "UPDATE WorkOrders SET ServiceId = s.Id FROM Services s WHERE s.WorkOrderId = WorkOrders.WorkOrderId");

            Delete.ForeignKeyColumn("Services", "WorkOrderId", "WorkOrders", "WorkOrderId");
        }

        public override void Down()
        {
            Alter.Table("Services")
                 .AddForeignKeyColumn("WorkOrderId", "WorkOrders", "WorkOrderId");

            Execute.Sql(
                "UPDATE Services SET WorkOrderId = wo.WorkOrderId FROM WorkOrders wo WHERE wo.ServiceId = Services.Id");

            Delete.ForeignKeyColumn("WorkOrders", "ServiceId", "Services");
        }
    }
}
