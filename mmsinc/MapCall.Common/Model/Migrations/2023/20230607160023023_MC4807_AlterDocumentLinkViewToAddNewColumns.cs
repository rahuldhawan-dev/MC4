using FluentMigrator;
using OldView = MapCall.Common.Model.Migrations._2023.MC5280_UpdateDocumentLinkViewSQLToIncludeCreatedAtProperty;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230607160023023), Tags("Production")]
    public class MC4807_AlterDocumentLinkViewToAddNewColumns : Migration
    {
        public const string VIEW_NAME = OldView.VIEW_NAME;
        public const string OLD_VIEW_SQL = OldView.NEW_VIEW_SQL;

        public const string NEW_VIEW_SQL = @"
SELECT
    link.DocumentLinkId as Id,
	doc.DocumentId,
	dat.DataTypeId,
	dot.DocumentTypeId,
	link.DataLinkId as LinkedId,
    dat.Table_Name as TableName,
    link.UpdatedAt,
    link.UpdatedById,
	doc.CreatedAt,
	link.DocumentStatusId,
	doc.[File_Name] as [FileName]
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
            Execute.Sql($"ALTER VIEW [{VIEW_NAME}] AS{NEW_VIEW_SQL}");
        }

        public override void Down()
        {
            Execute.Sql($"ALTER VIEW [{VIEW_NAME}] AS{OLD_VIEW_SQL}");
        }
    }
}

