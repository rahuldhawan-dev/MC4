using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151014101201015), Tags("Production")]
    public class RoadwayImprovementUpdatesBug2633 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("RoadwayImprovementNotificationPreconActions");
            Alter.Table("RoadwayImprovementNotifications")
                 .AddColumn("PreconMeetingDate").AsDateTime().Nullable()
                 .AddColumn("RoadwayImprovementNotificationPreconActionId").AsInt32().Nullable()
                 .ForeignKey(
                      "FK_RoadwayImprovementNotifications_RoadwayImprovementNotificationPreconActions_RoadwayImprovementNotificationPreconActionId",
                      "RoadwayImprovementNotificationPreconActions", "Id");
        }

        public override void Down()
        {
            Delete
               .ForeignKey(
                    "FK_RoadwayImprovementNotifications_RoadwayImprovementNotificationPreconActions_RoadwayImprovementNotificationPreconActionId")
               .OnTable("RoadwayImprovementNotifications");
            Delete.Column("RoadwayImprovementNotificationPreconActionId").FromTable("RoadwayImprovementNotifications");
            Delete.Column("PreconMeetingDate").FromTable("RoadwayImprovementNotifications");
            Delete.Table("RoadwayImprovementNotificationPreconActions");
        }
    }
}
