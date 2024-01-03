using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160630105025677), Tags("Production")]
    public class AddDocumentsAndNotesToSamplePlansForBug3019 : Migration
    {
        public override void Up()
        {
            this.CreateDocumentType("SamplePlans", "SamplePlans");
        }

        public override void Down()
        {
            this.DeleteDataType("SamplePlans");
        }
    }
}
