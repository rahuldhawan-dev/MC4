using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131125095002), Tags("Production")]
    public class AddDocumentDataRefToDocumentsTable : Migration
    {
        private const string DOCUMENT_DATA_FOREIGNKEY = "FK_Document_DocumentData_DocumentDataId";

        public override void Up()
        {
            Alter.Table("Document")
                 .AddColumn("DocumentDataId")
                 .AsInt32()
                 .ForeignKey(DOCUMENT_DATA_FOREIGNKEY, "DocumentData", "Id")
                 .Nullable(); // This will need to be made NotNullable at a later migration.
        }

        public override void Down()
        {
            Delete.ForeignKey(DOCUMENT_DATA_FOREIGNKEY).OnTable("Document");
            Delete.Column("DocumentDataId").FromTable("Document");
        }
    }
}
