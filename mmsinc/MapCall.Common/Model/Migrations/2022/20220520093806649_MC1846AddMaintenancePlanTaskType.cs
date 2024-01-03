using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220520093806649), Tags("Production")]
    public class MC1846AddMaintenancePlanTaskType : Migration
    {
        private const string TABLE_NAME = "MaintenancePlanTaskTypes";
        
        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithIdentityColumn()
                  .WithColumn("Description").AsFixedLengthAnsiString(50).NotNullable()
                  .WithColumn("IsActive").AsBoolean().NotNullable();
            
            Insert.IntoTable(TABLE_NAME)
                  .Row(new { Description = "Calibration", IsActive = true })
                  .Row(new { Description = "Inspection", IsActive = true })
                  .Row(new { Description = "Predictive Maintenance", IsActive = true })
                  .Row(new { Description = "Preventive Maintenance", IsActive = true })
                  .Row(new { Description = "Replacement", IsActive = true })
                  .Row(new { Description = "Operational", IsActive = true });
        }

        public override void Down()
        {
            Delete.Table("MaintenancePlanTaskTypes");
        }
    }
}

