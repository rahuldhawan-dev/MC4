using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150521150212224), Tags("Production")]
    public class AddSewerAssetTypesForNJ5ForBug2414 : Migration
    {
        public override void Up()
        {
            Execute.Sql("INSERT INTO OperatingCenterAssetTypes VALUES(13, 5)");
            Execute.Sql("INSERT INTO OperatingCenterAssetTypes VALUES(13, 6)");
            Execute.Sql("INSERT INTO OperatingCenterAssetTypes VALUES(13, 7)");
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM OperatingCenterAssetTypes WHERE OperatingCenterID = 13 AND AssetTypeID = 5");
            Execute.Sql("DELETE FROM OperatingCenterAssetTypes WHERE OperatingCenterID = 13 AND AssetTypeID = 6");
            Execute.Sql("DELETE FROM OperatingCenterAssetTypes WHERE OperatingCenterID = 13 AND AssetTypeID = 7");
        }
    }
}
