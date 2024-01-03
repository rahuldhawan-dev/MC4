using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131009102951), Tags("Production")]
    public class AddFacilityNameToInterconnections : Migration
    {
        public const string TABLE_NAME = CreateTablesForBug1481.TableNames.INTERCONNECTIONS;
        public const string COLUMN_NAME = "FacilityName";
        public const int COLUMN_LENGTH = 75;

        public override void Up()
        {
            Alter.Table(TABLE_NAME).AddColumn(COLUMN_NAME).AsAnsiString(COLUMN_LENGTH).Nullable();
        }

        public override void Down()
        {
            Delete.Column(COLUMN_NAME).FromTable(TABLE_NAME);
        }
    }
}
