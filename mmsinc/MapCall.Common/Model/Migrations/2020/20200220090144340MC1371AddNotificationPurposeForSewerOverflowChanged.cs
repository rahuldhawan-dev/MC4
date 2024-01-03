using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200220090144340), Tags("Production")]
    public class MC1371AddNotificationPurposeForSewerOverflowChanged : Migration
    {
        private const string APPLICATION = "Field Services",
                             MODULE = "Work Management",
                             PURPOSE = "Sewer Overflow Changed";

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
