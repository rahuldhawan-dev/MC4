using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20221201142305041), Tags("Production")]
    public class MC4560AddingBrassToServiceMaterials : Migration
    {
        public override void Up()
        {
            Execute.Sql("Insert into ServiceMaterials values ('Brass', 'BP')");
        }

        public override void Down()
        {
            Execute.Sql("Delete from ServiceMaterials where Description = 'Brass'");
        }
    }
}

