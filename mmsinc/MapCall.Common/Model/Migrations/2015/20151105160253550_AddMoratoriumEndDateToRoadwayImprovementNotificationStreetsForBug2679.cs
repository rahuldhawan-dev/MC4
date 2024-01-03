using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151105160253550), Tags("Production")]
    public class AddMoratoriumEndDateToRoadwayImprovementNotificationStreetsForBug2679 : Migration
    {
        public override void Up()
        {
            Alter.Table("RoadwayNotificationStreets").AddColumn("MoratoriumEndDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("MoratoriumEndDate").FromTable("RoadwayNotificationStreets");
        }
    }
}
