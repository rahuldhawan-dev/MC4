using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230616081100001), Tags("Production")]
    public class MC5736_AddNonRevenueWaterModule : Migration
    {
        public const int NON_REVENUE_WATER = 103,
                         PRODUCTION_APPLICATION = 2;

        public override void Up()
        {
            Execute.Sql("SET IDENTITY_INSERT [dbo].[Modules] ON");
            Insert.IntoTable("Modules").Row(new { ModuleId = NON_REVENUE_WATER, ApplicationId = PRODUCTION_APPLICATION, Name = "NonRevenue Water Unbilled Usage" });
            Execute.Sql("SET IDENTITY_INSERT [dbo].[Modules] OFF");
        }

        public override void Down()
        {
            Delete.FromTable("Modules").Row(new { ModuleID = NON_REVENUE_WATER });
        }
    }
}