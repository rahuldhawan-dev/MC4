using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170216101319323), Tags("Production")]
    public class Bug2969MainCrossings : Migration
    {
        public override void Up()
        {
            Create.Column("AssetCategoryId").OnTable("MainCrossings")
                  .AsInt32().Nullable()
                  .ForeignKey("FK_MainCrossings_AssetCategories_AssetCategoryId", "AssetCategories", "Id");

            // Set all the existing rows to Water
            Execute.Sql(@"
                declare @waterId int;
                set @waterId = (select top 1 Id from AssetCategories where Description = 'Water')
                update MainCrossings set AssetCategoryId = @waterId
                ");

            Alter.Column("AssetCategoryId").OnTable("MainCrossings").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_MainCrossings_AssetCategories_AssetCategoryId").OnTable("MainCrossings");
            Delete.Column("AssetCategoryId").FromTable("MainCrossings");
        }
    }
}
