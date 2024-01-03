using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220531091136555), Tags("Production")]
    public class MC4037AddFacilityAreaMaintenancePlansTable : Migration
    {
        public override void Up()
        {
            Create.Table("MaintenancePlansFacilityFacilityAreas")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("MaintenancePlanId", "MaintenancePlans", nullable: false)
                  .WithForeignKeyColumn("FacilitiesFacilityAreaId", "FacilitiesFacilityAreas", nullable: false);
        }

        public override void Down()
        {
            Delete.Table("MaintenancePlansFacilityFacilityAreas");
        }
    }
}

