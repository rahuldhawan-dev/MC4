using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221107120406701), Tags("Production")]
    public class MC5081_PlannedWorkModulePlanEquipmentTabAddMissingField : Migration
    {
        public override void Up()
        {
            Create.Table("EquipmentTypesMaintenancePlan")
                  .WithForeignKeyColumn("MaintenancePlanId", "MaintenancePlans").NotNullable()
                  .WithForeignKeyColumn("EquipmentTypeId", "EquipmentTypes", "EquipmentTypeID").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("EquipmentTypesMaintenancePlan");
        }
    }
}