using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150901161827687), Tags("Production")]
    public class AddTablesForRoadwayImprovementNotificationsForBug2549 : Migration
    {
        public struct TableNames
        {
            public const string ROADWAY_IMPROVEMENT_NOTIFICATIONS = "RoadwayImprovementNotifications",
                                ROADWAY_IMPROVEMENT_NOTIFICATION_STATUSES = "RoadwayImprovementNotificationStatuses",
                                ROADWAY_IMPROVEMENT_NOTIFICATION_ENTITIES = "RoadwayImprovementNotificationEntities";
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.ROADWAY_IMPROVEMENT_NOTIFICATION_ENTITIES, "State", "County",
                "Municipal");
            this.CreateLookupTableWithValues(TableNames.ROADWAY_IMPROVEMENT_NOTIFICATION_STATUSES, "Pending",
                "Complete");

            Create.Table(TableNames.ROADWAY_IMPROVEMENT_NOTIFICATIONS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId").NotNullable()
                  .WithForeignKeyColumn("TownId", "Towns", "TownId").NotNullable()
                  .WithForeignKeyColumn("RoadwayImprovementNotificationEntityId",
                       TableNames.ROADWAY_IMPROVEMENT_NOTIFICATION_STATUSES).NotNullable()
                  .WithColumn("Description").AsCustom("text").NotNullable()
                  .WithColumn("ExpectedProjectStartDate").AsDateTime().NotNullable()
                  .WithColumn("DateReceived").AsDateTime().Nullable().NotNullable()
                  .WithForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateId").NotNullable()
                  .WithForeignKeyColumn("RoadwayImprovementNotificationStatus",
                       TableNames.ROADWAY_IMPROVEMENT_NOTIFICATION_STATUSES).NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableNames.ROADWAY_IMPROVEMENT_NOTIFICATIONS);
            Delete.Table(TableNames.ROADWAY_IMPROVEMENT_NOTIFICATION_ENTITIES);
            Delete.Table(TableNames.ROADWAY_IMPROVEMENT_NOTIFICATION_STATUSES);
        }
    }
}
