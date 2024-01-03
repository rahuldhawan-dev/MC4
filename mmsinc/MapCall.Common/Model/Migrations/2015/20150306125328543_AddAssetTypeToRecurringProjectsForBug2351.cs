using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150306125328543), Tags("Production")]
    public class AddAssetTypeToRecurringProjectsForBug2351 : Migration
    {
        public struct TableNames
        {
            public const string RP_PROJECTS = "RPProjects",
                                ASSET_TYPES = "AssetTypes";
        }

        public const string COLUMN_NAME = "AssetTypeID";

        public override void Up()
        {
            Alter.Table(TableNames.RP_PROJECTS).AddForeignKeyColumn(COLUMN_NAME, TableNames.ASSET_TYPES, COLUMN_NAME);
            Execute.Sql("UPDATE " + TableNames.RP_PROJECTS + " SET " + COLUMN_NAME + " = 3 where AssetCategoryID = 1");
            Execute.Sql("UPDATE " + TableNames.RP_PROJECTS + " SET " + COLUMN_NAME + " = 7 where AssetCategoryID = 2");
            Alter.Column(COLUMN_NAME).OnTable(TableNames.RP_PROJECTS).AsInt32().NotNullable();
            //Alter.Table(TableNames.RP_PROJECTS).AddForeignKeyColumn(COLUMN_NAME, TableNames.ASSET_TYPES, COLUMN_NAME, nullable: false);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn(TableNames.RP_PROJECTS, COLUMN_NAME, TableNames.ASSET_TYPES);
        }
    }
}
