using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231027115414463), Tags("Production")]
    public class DeleteV2AccessRole : Migration
    {
        public override void Up()
        {
            this.DeleteModuleAndAssociatedRoles("Field Services", "V2 Access");
        }

        public override void Down()
        {
            this.CreateModule("V2 Access", "Field Services", 94);
        }
    }
}

