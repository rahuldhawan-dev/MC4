using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220518070023787), Tags("Production")]
    public class MC4506HramChanges : Migration
    {
        public override void Up()
        {
            Execute.Sql("DELETE FROM RiskRegisterAssetCategories;" +
                        "DBCC CHECKIDENT('RiskRegisterAssetCategories', RESEED, 0); ");
            Insert.IntoTable("RiskRegisterAssetCategories")
                  .Rows(new { Description = "Capacity" }, new { Description = "Compliance" },
                       new { Description = "Contamination" }, new { Description = "Dependency Threats" },
                       new { Description = "Malevolent Threats" }, new { Description = "Not Assigned" },
                       new { Description = "Other Regulatory" }, new { Description = "Physical Deterioration" },
                       new { Description = "Proximity Threats" }, new { Description = "Safety" },
                       new { Description = "Service Quality" });
            Execute.Sql("DELETE FROM RiskRegisterAssetGroups;" +
                        "DBCC CHECKIDENT('RiskRegisterAssetGroups', RESEED, 0); ");
            Insert.IntoTable("RiskRegisterAssetGroups")
                  .Rows(new { Description = "Equipment" }, new { Description = "Facility" },
                       new { Description = "Other" }, new { Description = "Pipeline" },
                       new { Description = "System" });
            this.CreateLookupTableWithValues("RiskRegisterAssetZones", "A1", "A2", "B1", "B2", "C", "Not Required");
            Alter.Table("RiskRegisterAssets")
                 .AddForeignKeyColumn("ZoneId", "RiskRegisterAssetZones");
        }

        public override void Down()
        {
            this.DeleteForeignKeyColumn("RiskRegisterAssets", "ZoneId", "RiskRegisterAssetZones");
            Delete.Table("RiskRegisterAssetZones");
        }
    }
}

