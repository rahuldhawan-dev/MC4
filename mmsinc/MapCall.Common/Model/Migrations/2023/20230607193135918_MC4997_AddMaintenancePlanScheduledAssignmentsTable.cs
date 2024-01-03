using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230607193135918), Tags("Production")]
    public class MC4997_AddMaintenancePlanScheduledAssignmentsTable : Migration
    {
        public override void Up()
        {
            Create.Table("MaintenancePlanScheduledAssignments")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("MaintenancePlanId", "MaintenancePlans", nullable: false)
                  .WithForeignKeyColumn("AssignedToId", "tblEmployee", foreignId: "tblEmployeeID", nullable: false)
                  .WithForeignKeyColumn("CreatedById", "tblEmployee", foreignId: "tblEmployeeID", nullable: true)
                  .WithColumn("AssignedFor").AsDateTime().NotNullable()
                  .WithColumn("ScheduledDate").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("MaintenancePlanScheduledAssignments");
        }
    }
}