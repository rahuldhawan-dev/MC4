using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210310090255531), Tags("Production")]
    public class MC2576AddingNewRoleModuleToModulesTable : Migration
    {
        public override void Up()
        {
            this.CreateModule("IncidentsDrugTesting", "Operations", 89);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Operations", "IncidentsDrugTesting");
        }
    }
}

