using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150918133658808), Tags("Production")]
    public class AddNotesFieldToRoadwayImprovementNotificationStreetBug2618 : Migration
    {
        public override void Up()
        {
            Alter.Table("RoadwayNotificationStreets")
                 .AddColumn("Notes")
                 .AsCustom("ntext")
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("Notes").FromTable("RoadwayNotificationStreets");
        }
    }
}
