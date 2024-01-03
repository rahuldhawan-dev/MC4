using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221013100536767), Tags("Production")]
    public class MC4609CreatingNotificationPurposes : Migration
    {
        public override void Up()
        {
            Insert
               .IntoTable("NotificationPurposes")
                // FieldServicesWorkManagement
               .Rows(new { ModuleId = 34, Purpose = "Service Line Renewal Lead Entered" },
                    new { ModuleId = 34, Purpose = "Service Line Renewal Lead Completed" });
        }

        public override void Down()
        {
            Delete
               .FromTable("NotificationPurposes")
               .Rows(new { ModuleId = 34, Purpose = "Service Line Renewal Lead Entered" },
                    new { ModuleId = 34, Purpose = "Service Line Renewal Lead Completed" });
        }
    }
}

