using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231103141054371), Tags("Production")]
    public class MC6549_AddChemicalDataModule : Migration
    {
        public override void Up()
        {
            this.CreateModule("Chemical Data", "Environmental", 104);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Environmental", "Chemical Data");
        }
    }
}

