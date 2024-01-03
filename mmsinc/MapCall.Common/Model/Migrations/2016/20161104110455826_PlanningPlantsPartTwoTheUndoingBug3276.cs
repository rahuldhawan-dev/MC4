using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161104110455826), Tags("Production")]
    public class PlanningPlantsPartTwoTheUndoingBug3276 : Migration
    {
        public override void Up()
        {
            Alter.Table("PlanningPlants")
                 .AddColumn("OperatingCenterId").AsInt32().Nullable()
                 .ForeignKey("FK_PlanningPlants_OperatingCenters_OperatingCenterId", "OperatingCenters",
                      "OperatingCenterId");

            Execute.Sql(@"
update PlanningPlants set OperatingCenterId = (select OperatingCenterId from OperatingCenters opc where opc.ProductionPlanningPlantId = Id) where OperatingCenterId is null
update PlanningPlants set OperatingCenterId = (select OperatingCenterId from OperatingCenters opc where opc.DistributionPlanningPlantId = Id) where OperatingCenterId is null
update PlanningPlants set OperatingCenterId = (select OperatingCenterId from OperatingCenters opc where opc.SewerPlanningPlantId = Id) where OperatingCenterId is null
");

            Delete.ForeignKey("FK_OperatingCenters_PlanningPlants_ProductionPlanningPlantId")
                  .OnTable("OperatingCenters");
            Delete.ForeignKey("FK_OperatingCenters_PlanningPlants_DistributionPlanningPlantId")
                  .OnTable("OperatingCenters");
            Delete.ForeignKey("FK_OperatingCenters_PlanningPlants_SewerPlanningPlantId").OnTable("OperatingCenters");
            Delete.Column("ProductionPlanningPlantId").FromTable("OperatingCenters");
            Delete.Column("DistributionPlanningPlantId").FromTable("OperatingCenters");
            Delete.Column("SewerPlanningPlantId").FromTable("OperatingCenters");
        }

        public override void Down()
        {
            Alter.Table("OperatingCenters")
                 .AddColumn("ProductionPlanningPlantId").AsInt32().Nullable()
                 .ForeignKey("FK_OperatingCenters_PlanningPlants_ProductionPlanningPlantId", "PlanningPlants", "Id")
                 .AddColumn("DistributionPlanningPlantId").AsInt32().Nullable()
                 .ForeignKey("FK_OperatingCenters_PlanningPlants_DistributionPlanningPlantId", "PlanningPlants", "Id")
                 .AddColumn("SewerPlanningPlantId").AsInt32().Nullable()
                 .ForeignKey("FK_OperatingCenters_PlanningPlants_SewerPlanningPlantId", "PlanningPlants", "Id");

            Delete.ForeignKey("FK_PlanningPlants_OperatingCenters_OperatingCenterId").OnTable("PlanningPlants");
            Delete.Column("OperatingCenterId").FromTable("PlanningPlants");
        }
    }
}
