using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220527164352451), Tags("Production")]
    public class MC1607AttachDocumentsToAMaintenancePlan : Migration
    {
        public override void Up()
        {
            this.CreateDocumentType("MaintenancePlans", "MaintenancePlans");
        }

        public override void Down() { }
    }
}
