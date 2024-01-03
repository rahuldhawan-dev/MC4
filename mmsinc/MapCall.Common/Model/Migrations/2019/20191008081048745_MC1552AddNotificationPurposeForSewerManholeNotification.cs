using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191008081048745), Tags("Production")]
    public class MC1552AddNotificationPurposeForSewerManholeNotification : Migration
    {
        public const string APPLICATION = "Field Services",
                            MODULE = "Assets",
                            PURPOSE = "SewerManhole";

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
