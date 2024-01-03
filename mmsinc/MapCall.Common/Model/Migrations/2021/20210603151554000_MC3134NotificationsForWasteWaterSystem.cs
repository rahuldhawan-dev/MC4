using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210603151554000), Tags("Production")]
    public class MC3134NotificationsForWasteWaterSystem : Migration
    {
        public override void Up()
        {
            this.AddNotificationType("Environmental", "General", "Waste Water System Created");
            this.AddNotificationType("Environmental", "General", "Waste Water System Updated");
        }

        public override void Down()
        {
            this.RemoveNotificationType("Environmental", "General", "Waste Water System Created");
            this.RemoveNotificationType("Environmental", "General", "Waste Water System Updated");
        }
    }
} 