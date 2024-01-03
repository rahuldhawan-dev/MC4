using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20171212122655942), Tags("Production")]
    public class AlterAddColumnsForWO209108 : Migration
    {
        /*
            The following fields from Town need to be moved to the OperatingCenterTown object instead:
                         MainSAPEquipment
                         MainSAPFunctionalLocation
                         SewerMainSAPEquipment
                         SewerMainSAPFunctionLocation

            These two fields need to be Added to OperatingCenterTown, but are not required
                         DistributionPlanningPlant
                         SewerPlanningPlant

            When sending the values to SAP the SAPWorkOrder class needs to use the OperatingCenterTown object now for the values, unless it does not have a value in the case of Planning Plant and it would fall through and use the OperatingCenter.PlanningPlant instead:
                         Valve/Hydrant - PlanningPlant - OperatingCenterTown.DistributionPlanningPlant ?? OperatingCenter.DistributionPlanningPlant
                         Sewer Manhole - PlanningPlant - OperatingCenterTown.SewerPlanningPlant ?? OperatingCenter.SewerPlanningPlant
                         Main - FunctionalLocation = OperatingCenterTown.MainSAPFunctionalLocation
                         Main - Equipment No = OperatingCenterTown.MainSAPEquipment
                         Main - Planning Plant - OperatingCenterTown.DistributionPlanningPlant ?? OperatingCenter.DistributionPlanningPlant
                         SewerMain - FunctionalLocation = OperatingCenterTown.MainSAPFunctionalLocation
                         SewerMain - Equipment No = OperatingCenterTown.MainSAPEquipment
                         SewerMain - Planning Plant - OperatingCenterTown.SewerPlanningPlant ?? OperatingCenter.SewerPlanningPlant
                         Service - Planning Plant - OperatingCenterTown.DistributionPlanningPlant ?? OperatingCenter.DistributionPlanningPlant
                         SewerLateral - Planning Plant - OperatingCenterTown.SewerPlanningPlant ?? OperatingCenter.SewerPlanningPlant
                         MainCrossing - FunctionalLocation = OperatingCenterTown.MainSAPFunctionalLocation
                         MainCrossing - Equipment No = OperatingCenterTown.MainSAPEquipment
         */

        public override void Up()
        {
            // add the new columns
            Alter.Table("OperatingCentersTowns")
                 .AddColumn("MainSAPEquipmentId").AsInt32().Nullable()
                 .AddColumn("SewerMainSAPEquipmentId").AsInt32().Nullable()
                 .AddForeignKeyColumn("MainSAPFunctionalLocationId", "FunctionalLocations",
                      "FunctionalLocationID")
                 .AddForeignKeyColumn("SewerMainSAPFunctionalLocationId", "FunctionalLocations",
                      "FunctionalLocationID")
                 .AddForeignKeyColumn("DistributionPlanningPlantId", "PlanningPlants")
                 .AddForeignKeyColumn("SewerPlanningPlantId", "PlanningPlants");

            // Move the values
            Execute.Sql(@"
                UPDATE
	                OperatingCentersTowns
                SET
	                MainSAPEquipmentId = Towns.MainSAPEquipmentId, 
	                MainSAPFunctionalLocationId = Towns.MainSAPFunctionalLocationId,
	                SewerMainSAPEquipmentId = Towns.SewerMainSAPEquipmentId,
	                SewerMainSAPFunctionalLocationId = Towns.SewerMainSAPFunctionalLocationId
                FROM
	                Towns 
                WHERE
	                Towns.TownID = OperatingCentersTowns.TownID");

            // Remove the old columns
            Delete.Column("MainSAPEquipmentId").FromTable("Towns");
            Delete.Column("SewerMainSAPEquipmentId").FromTable("Towns");
            Delete.ForeignKeyColumn("Towns", "MainSAPFunctionalLocationId", "FunctionalLocations",
                "FunctionalLocationID");
            Delete.ForeignKeyColumn("Towns", "SewerMainSAPFunctionalLocationId",
                "FunctionalLocations", "FunctionalLocationID");
        }

        public override void Down()
        {
            // add the old columns back
            Alter.Table("Towns")
                 .AddColumn("MainSAPEquipmentId").AsInt32().Nullable()
                 .AddColumn("SewerMainSAPEquipmentId").AsInt32().Nullable()
                 .AddForeignKeyColumn("MainSAPFunctionalLocationId", "FunctionalLocations",
                      "FunctionalLocationID")
                 .AddForeignKeyColumn("SewerMainSAPFunctionalLocationId", "FunctionalLocations",
                      "FunctionalLocationID");

            // populate the old colums
            Execute.Sql(
                "UPDATE Towns SET MainSAPEquipmentId = (SELECT Top 1 oct.MainSAPEquipmentId FROM OperatingCentersTowns oct where oct.TownID = Towns.TownId and isNull(MainSAPEquipmentId, 0) <> '0') ");
            Execute.Sql(
                "UPDATE Towns SET SewerMainSAPEquipmentId = (SELECT Top 1 oct.SewerMainSAPEquipmentId FROM OperatingCentersTowns oct where oct.TownID = Towns.TownId and isNull(SewerMainSAPEquipmentId, 0) <> '0') ");

            Execute.Sql(
                "UPDATE Towns SET MainSAPFunctionalLocationId = (SELECT Top 1 oct.MainSAPFunctionalLocationId FROM OperatingCentersTowns oct where oct.TownID = Towns.TownId and isNull(MainSAPFunctionalLocationId, 0) <> 0) ");
            Execute.Sql(
                "UPDATE Towns SET SewerMainSAPFunctionalLocationId = (SELECT Top 1 oct.SewerMainSAPFunctionalLocationId FROM OperatingCentersTowns oct where oct.TownID = Towns.TownId and isNull(SewerMainSAPFunctionalLocationId, 0) <> 0) ");

            // remove the new columns
            Delete.ForeignKeyColumn("OperatingCentersTowns", "SewerPlanningPlantId",
                "PlanningPlants");
            Delete.ForeignKeyColumn("OperatingCentersTowns", "DistributionPlanningPlantId",
                "PlanningPlants");
            Delete.ForeignKeyColumn("OperatingCentersTowns", "MainSAPFunctionalLocationId",
                "FunctionalLocations", "FunctionalLocationID");
            Delete.ForeignKeyColumn("OperatingCentersTowns", "SewerMainSAPFunctionalLocationId",
                "FunctionalLocations", "FunctionalLocationID");
            Delete.Column("MainSAPEquipmentId").FromTable("OperatingCentersTowns");
            Delete.Column("SewerMainSAPEquipmentId").FromTable("OperatingCentersTowns");
        }
    }
}
