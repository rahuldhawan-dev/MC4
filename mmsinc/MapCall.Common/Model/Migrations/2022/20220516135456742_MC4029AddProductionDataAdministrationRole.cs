using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220516135456742), Tags("Production")]
    public class MC4029AddProductionDataAdministrationRole : Migration
    {
        public override void Up()
        {
            this.CreateModule("Data Administration", "Production", 100);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Production", "Data Administration");
        }
    }
}

