using FluentMigrator;
using OldView = MapCall.Common.Model.Migrations.CreateDocumentLinkView;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20230314092846125), Tags("Production")]
    public class MC5116_FixDocumentLinkViewForNewChangeTrackingColumns : Migration
    {
        public const string VIEW_NAME = OldView.VIEW_NAME;

        public const string NEW_VIEW_SQL = @"
SELECT
    link.DocumentLinkId as Id,
	doc.DocumentId,
	dat.DataTypeId,
	dot.DocumentTypeId,
	link.DataLinkId as LinkedId,
    dat.Table_Name as TableName,
    link.UpdatedAt,
    link.UpdatedById
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
            // NOOP, this is reverted by `MC5116_RevertDocumentLinkViewColumnNameChanges`, see that class
            // for more information.
        }
    }
}

