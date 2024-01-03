using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230120073600001), Tags("Production")]
    public class MC4609CreatingNotificMC3637AddNonComplianceActionItemsNotificationPurposes : Migration
    {
        public override void Up()
        {
            Insert
               .IntoTable("NotificationPurposes")
                // EnvironmentalGeneral
               .Rows(new { ModuleId = 58, Purpose = "Environmental NonCompliance Action Item Assigned" });
        }

        public override void Down()
        {
            Delete
               .FromTable("NotificationPurposes")
               .Rows(new { ModuleId = 58, Purpose = "Environmental NonCompliance Action Item Assigned" });
        }
    }
}