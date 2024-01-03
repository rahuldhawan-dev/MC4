using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220616113214576), Tags("Production")]
    public class MC4506AdditionalHramChanges : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("RiskRegisterAssetCategories").Row(new { Description = "Natural Threats" });
        }

        public override void Down()
        {
            Execute.Sql("delete from RiskRegisterAssetCategories where Description='Natural Threats';");
        }
    }
}

