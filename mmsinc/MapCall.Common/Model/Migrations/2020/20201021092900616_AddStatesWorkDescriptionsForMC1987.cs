using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201021092900616), Tags("Production")]
    public class AddStatesWorkDescriptionsForMC1987 : Migration
    {
        public override void Up()
        {
            Create.Table("StatesWorkDescriptions")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("StateId", "States", "StateID")
                  .WithForeignKeyColumn("WorkDescriptionId", "WorkDescriptions", "WorkDescriptionID")
                  .WithForeignKeyColumn("PlantMaintenanceActivityTypeId", "PlantMaintenanceActivityTypes");

            Insert.IntoTable("StatesWorkDescriptions")
                  .Rows(new {StateId = 5, WorkDescriptionId = 81, PlantMaintenanceActivityTypeId = 11},
                       new {StateId = 5, WorkDescriptionId = 121, PlantMaintenanceActivityTypeId = 11},
                       new {StateId = 15, WorkDescriptionId = 126, PlantMaintenanceActivityTypeId = 6},
                       new {StateId = 14, WorkDescriptionId = 81, PlantMaintenanceActivityTypeId = 11},
                       new {StateId = 14, WorkDescriptionId = 121, PlantMaintenanceActivityTypeId = 11},
                       new {StateId = 14, WorkDescriptionId = 132, PlantMaintenanceActivityTypeId = 11},
                       new {StateId = 14, WorkDescriptionId = 133, PlantMaintenanceActivityTypeId = 11},
                       new {StateId = 13, WorkDescriptionId = 126, PlantMaintenanceActivityTypeId = 6},
                       new {StateId = 5, WorkDescriptionId = 47, PlantMaintenanceActivityTypeId = 11},
                       new {StateId = 5, WorkDescriptionId = 132, PlantMaintenanceActivityTypeId = 11},
                       new {StateId = 5, WorkDescriptionId = 260, PlantMaintenanceActivityTypeId = 11},
                       new {StateId = 13, WorkDescriptionId = 121, PlantMaintenanceActivityTypeId = 11});
        }

        public override void Down()
        {
            Delete.Table("StatesWorkDescriptions");
        }
    }
}
