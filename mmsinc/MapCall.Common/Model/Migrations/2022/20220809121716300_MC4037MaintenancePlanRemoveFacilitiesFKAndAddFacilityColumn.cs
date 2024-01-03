using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220809121716300), Tags("Production")]
    public class MC4037_MaintenancePlanRemoveFacilitiesFKAndAddFacilityColumn : Migration
    {
        public override void Up()
        {
            Delete.Table("FacilitiesMaintenancePlan");
            Alter.Table("MaintenancePlans")
                 .AddForeignKeyColumn("FacilityId", "tblFacilities", "RecordId", nullable: false);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MaintenancePlans", "FacilityId", "tblFacilities", "RecordId");
            Create.Table("FacilitiesMaintenancePlan")
                  .WithForeignKeyColumn("MaintenancePlanId", "MaintenancePlans", nullable: false)
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId", nullable: false);
        }
    }
}

