using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230127054234820), Tags("Production")]
    public class MC2358AddingDocumentTypeForSewerOpeningInspection : Migration
    {
        private const string SewerOpeningInspectionTableName = "SewerOpeningInspections";

        public override void Up()
        {
            this.AddDataType(SewerOpeningInspectionTableName);
            this.AddDocumentType("Document", SewerOpeningInspectionTableName);
            this.AddDocumentType("Photo", SewerOpeningInspectionTableName);
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Photo", SewerOpeningInspectionTableName);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Document", SewerOpeningInspectionTableName);
            this.RemoveDataType(SewerOpeningInspectionTableName);
        }
    }
}

