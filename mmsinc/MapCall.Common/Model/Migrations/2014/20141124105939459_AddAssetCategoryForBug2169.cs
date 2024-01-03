using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141124105939459), Tags("Production")]
    public class AddAssetCategoryForBug2169 : Migration
    {
        public const string TABLE_NAME = "AssetCategories";

        public override void Up()
        {
            this.CreateLookupTableWithValues(TABLE_NAME, "Water", "Wastewater");
            Alter.Table("RPProjects").AddForeignKeyColumn("AssetCategoryId", TABLE_NAME);
            Execute.Sql("UPDATE RPProjects set AssetCategoryId = 1");
            Alter.Table("RPProjects").AlterColumn("AssetCategoryId").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("RPProjects", "AssetCategoryId", TABLE_NAME);
            Delete.Table(TABLE_NAME);
        }
    }
}
