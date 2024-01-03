using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220225101307459), Tags("Production")]
    public class CreateV2Role : Migration
    {
        public override void Up()
        {
            this.CreateModule("V2 Access", "Field Services", 94);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Field Services", "V2 Access");
        }
    }
}

