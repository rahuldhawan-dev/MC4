using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211216141911086), Tags("Production")]
    public class MC3679AddingNewRoleForJ100AssessmentData : Migration
    {
        public override void Up()
        {
            this.CreateModule("J100 Assessment Data", "Engineering", 95);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Engineering", "J100 Assessment Data");
        }
    }
}

