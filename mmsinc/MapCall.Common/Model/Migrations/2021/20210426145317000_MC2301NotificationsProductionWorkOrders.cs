using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20210426145317000), Tags("Production")]
    public class MC2301NotificationsProductionWorkOrders : Migration
    {
        public override void Up()
        {
            this.AddNotificationType("Production", "Production Work Management", "Supervisor Approval Required");
        }

        public override void Down()
        {
            this.RemoveNotificationType("Production", "Production Work Management", "Supervisor Approval Required");
        }
    }
}