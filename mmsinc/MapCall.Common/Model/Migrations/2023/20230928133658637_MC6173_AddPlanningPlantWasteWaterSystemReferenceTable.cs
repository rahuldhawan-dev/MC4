using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230928133658637), Tags("Production")]
    public class MC6173_AddPlanningPlantWasteWaterSystemReferenceTable : Migration
    {
        public override void Up()
        {
            Create.Table("PlanningPlantsWasteWaterSystems")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("PlanningPlantId", "PlanningPlants")
                  .WithForeignKeyColumn("WasteWaterSystemId", "WasteWaterSystems");
        }

        public override void Down()
        {
            Delete.Table("PlanningPlantsWasteWaterSystems");
        }
    }
}

