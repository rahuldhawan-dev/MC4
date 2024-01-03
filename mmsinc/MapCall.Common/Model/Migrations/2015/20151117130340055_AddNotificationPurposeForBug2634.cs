using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151117130340055), Tags("Production")]
    public class AddNotificationPurposeForBug2634 : Migration
    {
        public override void Up()
        {
            this.AddNotificationType("Environmental", "General", "Environmental Permit Expiration");
        }

        public override void Down()
        {
            this.RemoveNotificationType("Environmental", "General", "Environmental Permit Expiration");
        }
    }
}
