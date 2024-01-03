using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210617111414548), Tags("Production")]
    public class MC3115AddNewRoleForFacilityAreaManagement : Migration
    {
        public override void Up()
        {
            this.CreateModule("Facility Area Management", "Production", 91);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Production", "Facility Area Management");
        }
    }
}

