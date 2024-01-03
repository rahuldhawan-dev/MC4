using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200902095525814), Tags("Production")]
    public class MC1792AddSampleSitesNotificationsForServicesAndWorkorders : Migration
    {
        private const string APPLICATION = "Field Services",
                             APPLICATION2 = "Production",
                             MODULE = "Assets",
                             MODULE2 = "Work Management",
                             PURPOSE1 = "Service With Sample Site",
                             PURPOSE2 = "Work Order With Sample Site";

        public override void Up()
        {
            this.AddNotificationType(APPLICATION, MODULE, PURPOSE1);
            this.AddNotificationType(APPLICATION, MODULE2, PURPOSE2);
        }

        public override void Down()
        {
            this.RemoveNotificationType(APPLICATION, MODULE, PURPOSE1);
            this.RemoveNotificationType(APPLICATION, MODULE2, PURPOSE2);
        }
    }
}
