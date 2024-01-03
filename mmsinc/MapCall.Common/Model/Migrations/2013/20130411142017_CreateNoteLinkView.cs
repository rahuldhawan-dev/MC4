using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130411142017), Tags("Production")]
    public class CreateNoteLinkView : Migration
    {
        public const string VIEW_NAME = "NoteLinkView";
        public const string DROP_SQL = "DROP VIEW [" + VIEW_NAME + "];";

        public const string CREATE_SQL = "CREATE VIEW [" + VIEW_NAME + @"] AS
SELECT
  Note.NoteId as Id,
  Note.DataLinkId as LinkedId,
  Note.DataTypeId,
  dt.Table_Name as TableName
FROM
  Note
INNER JOIN
  DataType dt
ON
  dt.DataTypeId = Note.DataTypeId;";

        public override void Up()
        {
            Execute.Sql(CREATE_SQL);
        }

        public override void Down()
        {
            Execute.Sql(DROP_SQL);
        }
    }
}
