using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230907154301980), Tags("Production")]
    public class MC6172_AddPlanningPlantPublicWaterSuppliesReferenceTable : Migration
    {
        public override void Up()
        {
            Create.Table("PlanningPlantsPublicWaterSupplies")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("PlanningPlantId", "PlanningPlants")
                  .WithForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies");
        }

        public override void Down()
        {
            Delete.Table("PlanningPlantsPublicWaterSupplies");
        }
    }
}

