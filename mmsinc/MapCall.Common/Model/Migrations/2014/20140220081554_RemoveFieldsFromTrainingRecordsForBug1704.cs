using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140220081554), Tags("Production")]
    public class RemoveFieldsFromTrainingRecordsForBug1704 : Migration
    {
        public const string TABLE_NAME = "tblTrainingRecords";

        public struct ColumnNames
        {
            public const string TITLE = "Title", PRESENTED_BY = "PresentedBy";
        }

        public override void Up()
        {
            Delete.Column(ColumnNames.TITLE).FromTable(TABLE_NAME);
            Delete.Column(ColumnNames.PRESENTED_BY).FromTable(TABLE_NAME);
        }

        public override void Down()
        {
            Create.Column(ColumnNames.TITLE).OnTable(TABLE_NAME).AsAnsiString(50).Nullable();
            Create.Column(ColumnNames.PRESENTED_BY).OnTable(TABLE_NAME).AsAnsiString(255).Nullable();
        }
    }
}
