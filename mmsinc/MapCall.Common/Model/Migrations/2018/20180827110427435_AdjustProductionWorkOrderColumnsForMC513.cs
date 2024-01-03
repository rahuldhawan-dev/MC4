using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180827110427435), Tags("Production")]
    public class AdjustProductionWorkOrderColumnsForMC513 : Migration
    {
        public override void Up()
        {
            Execute.Sql("ALTER TABLE ProductionWorkOrders ALTER COLUMN FacilityId int NULL");
            Execute.Sql("ALTER TABLE ProductionWorkOrdersEquipment ALTER COLUMN EquipmentId int NULL");
            Alter.Table("ProductionWorkOrdersEquipment").AddColumn("SAPEquipmentId").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("SAPEquipmentId").FromTable("ProductionWorkOrdersEquipment");
        }
    }
}
