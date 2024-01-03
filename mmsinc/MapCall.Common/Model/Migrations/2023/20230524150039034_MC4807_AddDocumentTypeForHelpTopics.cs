using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230524150039034), Tags("Production")]
    public class MC4807_AddDocumentTypeForHelpTopics : Migration
    {
        private const string HELP_TOPICS_TABLE_NAME = "HelpTopics";

        public override void Up()
        {
            this.AddDocumentType("Document", HELP_TOPICS_TABLE_NAME);
            this.AddDocumentType("Video", HELP_TOPICS_TABLE_NAME);
            this.AddDocumentType("URL", HELP_TOPICS_TABLE_NAME);
            this.AddDocumentType("Photo", HELP_TOPICS_TABLE_NAME);
            this.AddDocumentType("Template", HELP_TOPICS_TABLE_NAME);
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Template", HELP_TOPICS_TABLE_NAME);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Photo", HELP_TOPICS_TABLE_NAME);
            this.RemoveDocumentTypeAndAllRelatedDocuments("URL", HELP_TOPICS_TABLE_NAME);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Video", HELP_TOPICS_TABLE_NAME);
            this.RemoveDocumentTypeAndAllRelatedDocuments("Document", HELP_TOPICS_TABLE_NAME);
        }
    }
}

