using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160223101125845), Tags("Production")]
    public class AddRoadwayImprovementNotificationBug2801 : Migration
    {
        public override void Up()
        {
            // Add day notification purpose yo
            Insert.IntoTable("NotificationPurposes").Row(new {
                // FieldServicesProjects
                ModuleID = 60,
                Purpose = "Roadway Improvement Notification Created"
            });
        }

        public override void Down()
        {
            Delete.FromTable("NotificationPurposes").Row(new {Purpose = "Roadway Improvement Notification Created"});
        }
    }
}
