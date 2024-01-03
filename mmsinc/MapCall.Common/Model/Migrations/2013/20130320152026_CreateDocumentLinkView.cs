using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130320152026), Tags("Production")]
    public class CreateDocumentLinkView : Migration
    {
        public const string VIEW_NAME = "DocumentLinkView";
        public const string DROP_SQL = "DROP VIEW [" + VIEW_NAME + "];";

        public const string CREATE_SQL = "CREATE VIEW [" + VIEW_NAME + @"] AS
SELECT
    link.DocumentLinkId as Id,
	doc.DocumentId,
	dat.DataTypeId,
	dot.DocumentTypeId,
	link.DataLinkId as LinkedId,
    dat.Table_Name as TableName
FROM
	Document doc
INNER JOIN
	DocumentLink link
ON
	link.DocumentID = doc.DocumentID
INNER JOIN
	DocumentType dot
ON
	dot.DocumentTypeId = link.DocumentTypeId
INNER JOIN
	DataType dat
ON
	dat.DataTypeID = dot.DataTypeID;";

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
