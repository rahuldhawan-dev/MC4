using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190613083338416), Tags("Production")]
    public class MC1222AddProductionWorkOrderPriorities : Migration
    {
        public override void Up()
        {
            // Emergency 2, High 3, Routine 4, Medium 6, Low 7 -- what sap needs to know them as
            this.CreateLookupTableWithValues("ProductionWorkOrderPriorities", "Emergency", "High", "Routine", "Medium",
                "Low");
            Alter.Table("ProductionWorkOrders").AddForeignKeyColumn("NewPriorityId", "ProductionWorkOrderPriorities");
            Execute.Sql("UPDATE ProductionWorkOrders SET NewPriorityId = 1 WHERE PriorityId = 1");
            Execute.Sql("UPDATE ProductionWorkOrders SET NewPriorityId = 2 WHERE PriorityId = 2");
            Execute.Sql("UPDATE ProductionWorkOrders SET NewPriorityId = 3 WHERE PriorityId = 4");
            Delete.ForeignKeyColumn("ProductionWorkOrders", "PriorityId", "WorkOrderPriorities", "WorkOrderPriorityID");
            Execute.Sql(
                "exec sp_rename 'FK_ProductionWorkOrders_ProductionWorkOrderPriorities_NewPriorityId', 'FK_ProductionWorkOrders_ProductionWorkOrderPriorities_PriorityId'");
            Rename.Column("NewPriorityId").OnTable("ProductionWorkOrders").To("PriorityId");
        }

        public override void Down()
        {
            Alter.Table("ProductionWorkOrders")
                 .AddForeignKeyColumn("OldPriorityId", "WorkOrderPriorities", "WorkOrderPriorityID");
            Execute.Sql("UPDATE ProductionWorkOrders SET OldPriorityId = 1 WHERE PriorityId = 1");
            Execute.Sql("UPDATE ProductionWorkOrders SET OldPriorityId = 2 WHERE PriorityId = 2");
            Execute.Sql("UPDATE ProductionWorkOrders SET OldPriorityId = 4 WHERE PriorityId = 3");

            Delete.ForeignKeyColumn("ProductionWorkOrders", "PriorityId", "ProductionWorkOrderPriorities");
            Rename.Column("OldPriorityId").OnTable("ProductionWorkOrders").To("PriorityId");
            Delete.Table("ProductionWorkOrderPriorities");
            Execute.Sql(
                "exec sp_rename 'FK_ProductionWorkOrders_WorkOrderPriorities_OldPriorityId', 'FK_ProductionWorkOrders_WorkOrderPriorities_PriorityId'");
        }
    }
}
