using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221027154320094), Tags("Production")]
    public class MC4848_UpdateEmailNotificationsForInterconnections : Migration
    {
        public override void Up()
        {
            Update.Table("NotificationPurposes").Set(new { ModuleId = 102 }).Where(new { Purpose = "Interconnection Contract Ends In 30 Days" });
        }

        public override void Down()
        {
            Update.Table("NotificationPurposes").Set(new { ModuleId = 29 }).Where(new { Purpose = "Interconnection Contract Ends In 30 Days" });
        }
    }
}

