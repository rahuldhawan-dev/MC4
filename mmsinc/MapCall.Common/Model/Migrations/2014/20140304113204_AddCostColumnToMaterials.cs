using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140304113204), Tags("Production")]
    public class AddCostColumnToMaterials : Migration
    {
        public override void Up()
        {
            Create.Column("Cost").OnTable("Materials").AsCurrency().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Cost").FromTable("Materials");
        }
    }
}
