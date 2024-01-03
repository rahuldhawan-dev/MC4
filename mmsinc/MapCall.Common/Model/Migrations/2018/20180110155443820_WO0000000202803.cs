using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20180110155443820), Tags("Production")]
    public class WO0000000202803 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM AssetInvestmentCategories WHERE Description = 'Crossing Risk Reduction ') INSERT INTO AssetInvestmentCategories(Description, CreatedBy) VALUES ('Crossing Risk Reduction', 'import')");
        }

        public override void Down()
        {
            // AssetInvestmentCategories.Description is set to not null
            //
            //Execute.Sql("DELETE FROM AssetInvestmentCategories WHERE Description = 'Crossing Risk Reduction'");
        }
    }
}
