using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221005110933732), Tags("Production")]
    public class AddingInterconnectionsRole : Migration
    {
        public override void Up()
        {
            this.CreateModule("Interconnections", "Production", 102);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Production", "Interconnections");
        }
    }
}

