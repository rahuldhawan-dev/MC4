using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210420091801702), Tags("Production")]
    public class MC2487AddDocsNotes : Migration
    {
        public override void Up()
        {
            this.AddDataType("TankInspections");
            this.AddDocumentType("Tank Inspection", "TankInspections");
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("Tank Inspection", "TankInspections");
            this.RemoveDataType("TankInspections");
        }
    }
}