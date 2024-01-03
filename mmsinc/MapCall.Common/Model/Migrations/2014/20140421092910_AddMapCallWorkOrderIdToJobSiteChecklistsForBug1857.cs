using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140421092910), Tags("Production")]
    public class AddMapCallWorkOrderIdToJobSiteChecklistsForBug1857 : Migration
    {
        public const string TABLE_NAME = "JobSiteChecklists",
                            COLUMN_NAME = "MapCallWorkOrderId";

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
