using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220914101613240), Tags("Production")]
    public class CreateEngineeringArcFlashRole : Migration
    {
        public override void Up()
        {
            this.CreateModule("Arc Flash", "Engineering", 101);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Engineering", "Arc Flash");
        }
    }
}

