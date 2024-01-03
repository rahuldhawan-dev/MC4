using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190807133214962), Tags("Production")]
    public class MC1288AddNotificationPurpose : Migration
    {
        public const string APPLICATION = "Production",
                            MODULE = "Production Work Management",
                            PURPOSE = "Permit Related Equipment Work Order Created";

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
