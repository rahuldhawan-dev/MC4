using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180427100048242), Tags("Production")]
    public class PredispatchEquipmentIdsForWO230579 : Migration
    {
        public override void Up()
        {
            Create.Table("ShortCycleWorkOrdersEquipmentIds")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderId", "ShortCycleWorkOrders")
                  .WithColumn("EquipmentId").AsAnsiString().Nullable();
            Execute.Sql(
                "INSERT INTO [ShortCycleWorkOrdersEquipmentIds] SELECT Id, EquipmentId FROM [ShortCycleWorkOrders]");
            Delete.Column("EquipmentId").FromTable("ShortCycleWorkOrders");
        }

        public override void Down()
        {
            Alter.Table("ShortCycleWorkOrders").AddColumn("EquipmentId")
                 .AsAnsiString().Nullable();
            Execute.Sql(
                "Update [ShortCycleWorkOrders] set EquipmentId = (SELECT TOP 1 EquipmentId FROM ShortCycleWorkOrdersEquipmentIds where ShortCycleWorkOrderId = ShortCycleWorkOrders.Id)");
            Delete.Table("ShortCycleWorkOrdersEquipmentIds");
        }
    }
}
