using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180213135754940), Tags("Production")]
    public class WO0000000193808 : Migration
    {
        public override void Up()
        {
            // add the new columns
            Alter.Table("TownSections")
                 .AddColumn("MainSAPEquipmentId").AsInt32().Nullable()
                 .AddColumn("SewerMainSAPEquipmentId").AsInt32().Nullable()
                 .AddForeignKeyColumn("MainSAPFunctionalLocationId", "FunctionalLocations",
                      "FunctionalLocationID")
                 .AddForeignKeyColumn("SewerMainSAPFunctionalLocationId", "FunctionalLocations",
                      "FunctionalLocationID")
                 .AddForeignKeyColumn("DistributionPlanningPlantId", "PlanningPlants")
                 .AddForeignKeyColumn("SewerPlanningPlantId", "PlanningPlants");
        }

        public override void Down()
        {
            // remove the new columns
            Delete.ForeignKeyColumn("TownSections", "SewerPlanningPlantId",
                "PlanningPlants");
            Delete.ForeignKeyColumn("TownSections", "DistributionPlanningPlantId",
                "PlanningPlants");
            Delete.ForeignKeyColumn("TownSections", "MainSAPFunctionalLocationId",
                "FunctionalLocations", "FunctionalLocationID");
            Delete.ForeignKeyColumn("TownSections", "SewerMainSAPFunctionalLocationId",
                "FunctionalLocations", "FunctionalLocationID");
            Delete.Column("MainSAPEquipmentId").FromTable("TownSections");
            Delete.Column("SewerMainSAPEquipmentId").FromTable("TownSections");
        }
    }
}
