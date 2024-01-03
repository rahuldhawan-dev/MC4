using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161122103408257), Tags("Production")]
    public class SyncAssetStatusesForBug3322 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "IF ((SELECT 1 FROM HydrantStatuses WHERE Description = 'INACTIVE') = 0) INSERT INTO HydrantStatuses VALUES('INACTIVE', 1);" +
                "IF ((SELECT 1 FROM HydrantStatuses WHERE Description = 'REMOVED') = 0) INSERT INTO HydrantStatuses VALUES('REMOVED', 1);" +
                "IF ((SELECT 1 FROM AssetStatuses WHERE Description = 'INACTIVE') = 0) INSERT INTO AssetStatuses VALUES('INACTIVE');" +
                "IF ((SELECT 1 FROM AssetStatuses WHERE Description = 'INSTALLED') = 0) INSERT INTO AssetStatuses VALUES('INSTALLED');" +
                "IF ((SELECT 1 FROM AssetStatuses WHERE Description = 'REQUEST CANCELLATION') = 0) INSERT INTO AssetStatuses VALUES('REQUEST CANCELLATION');" +
                "IF ((SELECT 1 FROM AssetStatuses WHERE Description = 'REQUEST RETIREMENT') = 0) INSERT INTO AssetStatuses VALUES('REQUEST RETIREMENT');");
        }

        public override void Down() { }
    }
}
