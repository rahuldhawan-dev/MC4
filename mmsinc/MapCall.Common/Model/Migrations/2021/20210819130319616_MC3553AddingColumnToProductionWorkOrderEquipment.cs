using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210819130319616), Tags("Production")]
    public class AddingColumnToProductionWorkOrderEquipment : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrdersEquipment").AddColumn("IsParent").AsBoolean().Nullable();
            Execute.Sql(@"INSERT INTO ProductionWorkOrdersEquipment (ProductionWorkOrderId, EquipmentId, IsParent) SELECT Id,EquipmentId, 1 FROM ProductionWorkOrders WHERE EquipmentId is not null;");
            Delete.ForeignKeyColumn("ProductionWorkOrders", "EquipmentId", "Equipment");
        }

        public override void Down()
        {
            Alter.Table("ProductionWorkOrders").AddForeignKeyColumn("EquipmentId", "Equipment", "EquipmentID", nullable: true);
            Execute.Sql(@"UPDATE ProductionWorkOrders 
                          SET ProductionWorkOrders.EquipmentId = ProductionWorkOrdersEquipment.EquipmentId 
                          FROM ProductionWorkOrders  INNER JOIN  ProductionWorkOrdersEquipment 
                          ON ProductionWorkOrders.Id = ProductionWorkOrdersEquipment.ProductionWorkOrderId 
                          WHERE ProductionWorkOrdersEquipment.IsParent = 1");
            Delete.Column("IsParent").FromTable("ProductionWorkOrdersEquipment");
        }
    }
}

