using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210730135710935), Tags("Production")]
    public class MC3023AddNotificationPurposeForNearMiss : Migration
    {
        public override void Up()
        {
            this.AddNotificationType("Operations", "Health And Safety", "Near Miss Notification");
        }

        public override void Down()
        {
            this.RemoveNotificationType("Operations", "Health And Safety", "Near Miss Notification");
        }
    }
}

