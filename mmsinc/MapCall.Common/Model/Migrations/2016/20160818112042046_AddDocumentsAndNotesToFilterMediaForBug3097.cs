using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160818112042046), Tags("Production")]
    public class AddDocumentsAndNotesToFilterMediaForBug3097 : Migration
    {
        public override void Up()
        {
            this.CreateDocumentType("FilterMedia", "FilterMedia", "Filter Media Document");
        }

        public override void Down()
        {
            this.DeleteDataType("FilterMedia");
        }
    }
}
