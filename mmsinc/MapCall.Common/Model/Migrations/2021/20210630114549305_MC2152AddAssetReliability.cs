using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210630114549305), Tags("Production")]
    public class MC2152AddAssetReliability : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("AssetReliabilityTechnologyUsedTypes", "IR", "Vibration", "MWA", "Visual", "Ultrasound", "Laser Alignment", "Earth Ground Testing", "Overload Testing", "W2W", "O Scope",
                "Motion Amplification", "Relay Testing", "Other");

            Create.Table("AssetReliabilities")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ProductionWorkOrderId", "ProductionWorkOrders")
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentId")
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeId")
                  .WithForeignKeyColumn("AssetReliabilityTechnologyUsedTypeId", "AssetReliabilityTechnologyUsedTypes")
                  .WithColumn("DateTimeEntered").AsDateTime().NotNullable()
                  .WithColumn("RepairCostNotAllowedToFail").AsInt32().NotNullable()
                  .WithColumn("RepairCostAllowedToFail").AsInt32().NotNullable()
                  .WithColumn("CostAvoidance").AsInt32().NotNullable()
                  .WithColumn("TechnologyUsedNote").AsAnsiString(255).Nullable()
                  .WithColumn("CostAvoidanceNote").AsAnsiString(255).Nullable();

            this.CreateModule("AssetReliability", "Production", 92);
        }

        public override void Down()
        {
            Delete.Table("AssetReliabilities");
            Delete.Table("AssetReliabilityTechnologyUsedTypes");
            this.DeleteModuleAndAssociatedRoles("Production", "AssetReliability");
        }
    }
}

