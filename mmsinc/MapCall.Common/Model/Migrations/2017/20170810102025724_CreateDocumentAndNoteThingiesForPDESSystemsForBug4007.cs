using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170810102025724), Tags("Production")]
    public class CreateDocumentAndNoteThingiesForPDESSystemsForBug4007 : Migration
    {
        #region Exposed Methods

        public override void Up()
        {
            this.AddDataType("WasteWaterSystems");
            this.AddDocumentType("PDES System Document", "WasteWaterSystems");
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("PDES System Document", "WasteWaterSystems");
            this.RemoveDataType("WasteWaterSystems");
        }

        #endregion
    }
}
