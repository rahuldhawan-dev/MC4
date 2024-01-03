using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200819164121681), Tags("Production")]
    public class MC2511ConfinedSpaceFormsDocsAndNotesAdded : Migration
    {
        private const string CONFINED_SPACE = "ConfinedSpaceForms";

        public override void Up()
        {
            this.AddDataType(CONFINED_SPACE);
            this.AddDocumentType("Confined Space Form", CONFINED_SPACE);
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Confined Space Form", CONFINED_SPACE);
            this.RemoveDataType(CONFINED_SPACE);
        }
    }
}
