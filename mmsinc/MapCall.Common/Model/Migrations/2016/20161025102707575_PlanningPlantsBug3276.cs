using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161025102707575), Tags("Production")]
    public class PlanningPlantsBug3276 : Migration
    {
        public override void Up()
        {
            Create.Table("PlanningPlants")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("Code").AsString(4).NotNullable()
                  .WithColumn("Description").AsString(50).NotNullable();

            Alter.Table("OperatingCenters")
                 .AddColumn("ProductionPlanningPlantId").AsInt32().Nullable()
                 .ForeignKey("FK_OperatingCenters_PlanningPlants_ProductionPlanningPlantId", "PlanningPlants", "Id")
                 .AddColumn("DistributionPlanningPlantId").AsInt32().Nullable()
                 .ForeignKey("FK_OperatingCenters_PlanningPlants_DistributionPlanningPlantId", "PlanningPlants", "Id")
                 .AddColumn("SewerPlanningPlantId").AsInt32().Nullable()
                 .ForeignKey("FK_OperatingCenters_PlanningPlants_SewerPlanningPlantId", "PlanningPlants", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_OperatingCenters_PlanningPlants_ProductionPlanningPlantId")
                  .OnTable("OperatingCenters");
            Delete.ForeignKey("FK_OperatingCenters_PlanningPlants_DistributionPlanningPlantId")
                  .OnTable("OperatingCenters");
            Delete.ForeignKey("FK_OperatingCenters_PlanningPlants_SewerPlanningPlantId").OnTable("OperatingCenters");
            Delete.Column("ProductionPlanningPlantId").FromTable("OperatingCenters");
            Delete.Column("DistributionPlanningPlantId").FromTable("OperatingCenters");
            Delete.Column("SewerPlanningPlantId").FromTable("OperatingCenters");
            Delete.Table("PlanningPlants");
        }
    }
}
