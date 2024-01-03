using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211110160622680), Tags("Production")]
    public class MC3341AddNotificationPurposeForBelowGroundHazardCreated : Migration
    {
        private const string APPLICATION = "Field Services", MODULE = "Assets", PURPOSE = "Below Ground Hazard Created";
        public override void Up()
        {
            this.AddNotificationType(APPLICATION, MODULE, PURPOSE);
        }

        public override void Down()
        {
            this.RemoveNotificationPurpose(APPLICATION, MODULE, PURPOSE);
        }
    }
}

