using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150923094239817), Tags("Production")]
    public class AddOpenWorkOrdersToRoadwayImprovementNotificationStreetsForBug2628 : Migration
    {
        public const string TABLE_NAME =
                                AddRoadwayNotificationStreetsForBug2596.TableNames.ROADWAY_NOTIFICATION_STREETS,
                            COLUMN_NAME = "OpenWorkOrders";

        public override void Up()
        {
            Alter.Table(TABLE_NAME).AddColumn(COLUMN_NAME).AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column(COLUMN_NAME).FromTable(TABLE_NAME);
        }
    }
}
