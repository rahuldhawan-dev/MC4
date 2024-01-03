using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200218075957707), Tags("Production")]
    public class MC1560AddMaintenancePlans : Migration
    {
        public override void Up()
        {
            Create.Table("MaintenancePlans")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID").NotNullable()
                  .WithForeignKeyColumn("PlanningPlantId", "PlanningPlants").NotNullable()
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId").NotNullable()
                  .WithForeignKeyColumn("EquipmentClassId", "SAPEquipmentTypes").NotNullable()
                  .WithForeignKeyColumn("TaskGroupId", "TaskGroups").NotNullable()
                  .WithForeignKeyColumn("WorkDescriptionId", "ProductionWorkDescriptions").NotNullable()
                  .WithColumn("Frequency").AsInt32().NotNullable()
                  .WithForeignKeyColumn("RecurringFrequencyUnitId", "RecurringFrequencyUnits").NotNullable()
                  .WithColumn("Start").AsDateTime().NotNullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable().WithDefaultValue(false);
            this.CreateLookupTableWithValues("ComplianceRequirements", "Company", "OSHA", "PSM", "Regulatory", "TCPA");
            Create.Table("ComplianceRequirementsMaintenancePlans")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("MaintenancePlanId", "MaintenancePlans").NotNullable()
                  .WithForeignKeyColumn("ComplianceRequirementId", "ComplianceRequirements").NotNullable();
            Create.Table("EquipmentMaintenancePlans")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentID").NotNullable()
                  .WithForeignKeyColumn("MaintenancePlanId", "MaintenancePlans").NotNullable();
            Alter.Table("ProductionWorkOrders").AddForeignKeyColumn("MaintenancePlanId", "MaintenancePlans").Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ProductionWorkOrders", "MaintenancePlanId", "MaintenancePlans");
            Delete.Table("EquipmentMaintenancePlans");
            Delete.Table("ComplianceRequirementsMaintenancePlans");
            Delete.Table("ComplianceRequirements");
            Delete.Table("MaintenancePlans");
        }
    }
}
