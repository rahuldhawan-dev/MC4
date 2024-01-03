using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141028101850174), Tags("Production")]
    public class AddTotalChlorineToHydrantInspectionsForBug2175 : Migration
    {
        public const string TABLE_NAME = "tblNJAWHydInspData";
        public const string COLUMN_NAME = "TotalChlorine";

        public override void Up()
        {
            Alter.Table(TABLE_NAME).AddColumn(COLUMN_NAME).AsDecimal(5, 4).Nullable();
        }

        public override void Down()
        {
            Delete.Column(COLUMN_NAME).FromTable(TABLE_NAME);
        }
    }
}
