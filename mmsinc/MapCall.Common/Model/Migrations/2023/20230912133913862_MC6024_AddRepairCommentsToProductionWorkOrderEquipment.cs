using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230912133913862), Tags("Production")]
    public class MC6024_AddRepairCommentsToProductionWorkOrderEquipment : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrdersEquipment").AddColumn("RepairComment").AsAnsiString(100).Nullable();
        }

        public override void Down()
        {
            Delete.Column("RepairComment").FromTable("ProductionWorkOrdersEquipment");
        }
    }
}

