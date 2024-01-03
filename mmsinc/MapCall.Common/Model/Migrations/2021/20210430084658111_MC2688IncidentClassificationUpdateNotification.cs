using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210430084658111), Tags("Production")]
    public class MC2688IncidentClassificationUpdateNotification : Migration
    {
        public override void Up()
        {
            this.AddNotificationType("Operations", "Incidents", "HS Incident Classification Updated");
        }

        public override void Down()
        {
            this.RemoveNotificationType("Operations", "Incidents", "HS Incident Classification Updated");
        }
    }
}