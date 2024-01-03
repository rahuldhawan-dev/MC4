using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200116114942560), Tags("Production")]
    public class MC1795AddNotificationPurposeForArcFlashStudyExpirationEmail : Migration
    {
        public const string APPLICATION = "Human Resources",
                            MODULE = "Facilities",
                            PURPOSE = "Arc Flash Study Expires In 1 Year";

        public override void Up()
        {
            this.AddNotificationType(APPLICATION, MODULE, PURPOSE);
        }

        public override void Down()
        {
            this.RemoveNotificationType(APPLICATION, MODULE, PURPOSE);
        }
    }
}
