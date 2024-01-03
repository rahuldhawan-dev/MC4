using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180604155219534), Tags("Production")]
    public class AddMainBreakMaterialForMC306 : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("MainBreakMaterials")
                  .Rows(new {Description = "Steel"});
        }

        public override void Down()
        {
            //noop
        }
    }
}
