using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231027094120211), Tags("Production")]
    public class MC6314_DeleteRecordsFromConsolidatedCustomerSideMaterialsTable : Migration
    {
        public override void Up()
        {
            Execute.Sql("Delete From ConsolidatedCustomerSideMaterials Where CustomerSideEPACodeId Is Null or CustomerSideExternalEPACodeId Is Null");
        }

        public override void Down() { }
    }
}

