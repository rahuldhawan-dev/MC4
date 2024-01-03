using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230725112947239), Tags("Production")]
    public class MC5519_AddNsiPendingAssetStatus : Migration
    {
        public const int NSI_PENDING = 15;

        public override void Up()
        {
            Execute.Sql("SET IDENTITY_INSERT [dbo].[AssetStatuses] ON");
            Insert.IntoTable("AssetStatuses").Row(new { AssetStatusID = NSI_PENDING, Description = "NSI PENDING", IsUserAdminOnly = 1 });
            Execute.Sql("SET IDENTITY_INSERT [dbo].[AssetStatuses] OFF");
        }

        public override void Down()
        {
            Delete.FromTable("AssetStatuses").Row(new { Description = "NSI PENDING" });
        }
    }
}

