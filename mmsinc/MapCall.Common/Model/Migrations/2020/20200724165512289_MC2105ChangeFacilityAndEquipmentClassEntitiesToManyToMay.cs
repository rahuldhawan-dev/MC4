using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200724165512289), Tags("Production")]
    public class MC2105ChangeFacilityAndEquipmentClassEntitiesToManyToMay : Migration
    {
        public override void Up()
        {
            Create.Table("FacilitiesMaintenancePlan")
                  .WithForeignKeyColumn("MaintenancePlanId", "MaintenancePlans").NotNullable()
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId").NotNullable();

            Create.Table("SAPEquipmentTypesMaintenancePlan")
                  .WithForeignKeyColumn("MaintenancePlanId", "MaintenancePlans").NotNullable()
                  .WithForeignKeyColumn("SAPEquipmentTypeId", "SAPEquipmentTypes").NotNullable();

            Delete.ForeignKeyColumn("MaintenancePlans", "FacilityId", "tblFacilities");
            Delete.ForeignKeyColumn("MaintenancePlans", "EquipmentClassId", "SAPEquipmentTypes");
        }

        public override void Down()
        {
            Alter.Table("MaintenancePlans")
                 .AddForeignKeyColumn("FacilityId", "tblFacilities", "RecordId")
                 .NotNullable()
                 .AddForeignKeyColumn("EquipmentClassId", "SAPEquipmentTypes")
                 .NotNullable();

            Delete.Table("FacilitiesMaintenancePlan");
            Delete.Table("SAPEquipmentTypesMaintenancePlan");
        }
    }
}
