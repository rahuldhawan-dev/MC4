using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150713115153150), Tags("Production")]
    public class AddNotificationTypeForBactiSampleInputForBug2482 : Migration
    {
        public override void Up()
        {
            this.AddNotificationType("Water Quality", "General", "Bacti Input Trigger");
        }

        public override void Down()
        {
            this.RemoveNotificationType("Water Quality", "General", "Bacti Input Trigger");
        }
    }
}
