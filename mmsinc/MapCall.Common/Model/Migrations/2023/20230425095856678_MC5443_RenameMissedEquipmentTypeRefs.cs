using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230425095856678), Tags("Production")]
    public class MC5443_RenameMissedEquipmentTypeRefs : Migration
    {
        public override void Up()
        {
            Execute.Sql("EXEC sp_rename 'FK_EquipmentTypesMaintenancePlan_MaintenancePlans_MaintenancePlanId', 'FK_EquipmentPurposesMaintenancePlan_MaintenancePlans_MaintenancePlanId'");

            Rename.Table("TaskGroupsEquipmentTypes").To("TaskGroupsEquipmentPurposes");
            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentTypes_TaskGroups_TaskGroupId', 'FK_TaskGroupsEquipmentPurposes_TaskGroups_TaskGroupId'");
        }
        
        public override void Down()
        {
            Execute.Sql("EXEC sp_rename 'FK_TaskGroupsEquipmentPurposes_TaskGroups_TaskGroupId', 'FK_TaskGroupsEquipmentTypes_TaskGroups_TaskGroupId'");
            Rename.Table("TaskGroupsEquipmentPurposes").To("TaskGroupsEquipmentTypes");

            Execute.Sql("EXEC sp_rename 'FK_EquipmentPurposesMaintenancePlan_MaintenancePlans_MaintenancePlanId', 'FK_EquipmentTypesMaintenancePlan_MaintenancePlans_MaintenancePlanId'");
        }
    }
}

