using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231103102449230), Tags("Production")]
    public class MC6549_CreateChemicalStorageLocationTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("ChemicalStorageLocations")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("StateId", "States", "stateID", nullable: false)
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID", nullable: false)
                  .WithForeignKeyColumn("PlanningPlantId", "PlanningPlants", nullable: true)
                  .WithForeignKeyColumn("ChemicalWarehouseNumberId", "ChemicalWarehouseNumbers", nullable: true)
                  .WithColumn("StorageLocationNumber").AsString(10).NotNullable()
                  .WithColumn("StorageLocationDescription").AsString(25).NotNullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(true)
                  .WithColumn("CreatedAt").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("CreatedById", "tblPermissions", "RecId", nullable: false)
                  .WithColumn("UpdatedAt").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("UpdatedById", "tblPermissions", "RecId", nullable: false);
        }
    }
}

