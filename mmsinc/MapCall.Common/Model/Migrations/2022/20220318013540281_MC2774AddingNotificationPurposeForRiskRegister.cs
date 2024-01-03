using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220318013540281), Tags("Production")]
    public class MC2774AddingNotificationPurposeForRiskRegister : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("RiskRegisterAssetGroups")
                  .Rows(new { Description = "Pipeline" }, new { Description = "Other" });
            Insert.IntoTable("NotificationPurposes").Rows(new { ModuleId = 88, Purpose = "Risk Register Created" },
                new { ModuleId = 88, Purpose = "Risk Register Completed" });
            Insert.IntoTable("RiskRegisterAssetCategories")
                  .Rows(new { Description = "Safety" }, new { Description = "Not Assigned" },
                       new { Description = "Compliance" }, new { Description = "Capacity" },
                       new { Description = "Aging Infrastructure" });
            Delete.ForeignKeyColumn("RiskRegisterAssets", "RiskRegisterAssetProbabilityId", "RiskRegisterAssetProbabilities");
            Delete.ForeignKeyColumn("RiskRegisterAssets", "RiskRegisterAssetImpactId", "RiskRegisterAssetImpacts");
            Alter.Table("RiskRegisterAssets")
                 .AddColumn("COFMax").AsInt32().NotNullable().WithDefaultValue(0)
                 .AddColumn("LOFMax").AsInt32().NotNullable().WithDefaultValue(0)
                 .AddColumn("TotalRiskWeighted").AsInt32().NotNullable().WithDefaultValue(0)
                 .AddColumn("RiskRegisterId").AsString(25).Nullable();
            Delete.Table("RiskRegisterAssetProbabilities");
            Delete.Table("RiskRegisterAssetImpacts");
        }

        public override void Down()
        {
            Create.LookupTable("RiskRegisterAssetImpacts", 50);
            Create.LookupTable("RiskRegisterAssetProbabilities", 50);
            Delete.Column("RiskRegisterId").FromTable("RiskRegisterAssets");
            Delete.Column("TotalRiskWeighted").FromTable("RiskRegisterAssets");
            Delete.Column("LOFMax").FromTable("RiskRegisterAssets");
            Delete.Column("COFMax").FromTable("RiskRegisterAssets");
            Alter.Table("RiskRegisterAssets")
                 .AddForeignKeyColumn("RiskRegisterAssetProbabilityId", "RiskRegisterAssetProbabilities")
                 .AddForeignKeyColumn("RiskRegisterAssetImpactId", "RiskRegisterAssetImpacts");
            Delete.FromTable("RiskRegisterAssetCategories")
                  .Rows(new { Description = "Safety" }, new { Description = "Not Assigned" },
                       new { Description = "Compliance" }, new { Description = "Capacity" },
                       new { Description = "Aging Infrastructure" });
            Delete.FromTable("NotificationPurposes").Rows(new { ModuleId = 88, Purpose = "Risk Register Created" },
                new { ModuleId = 88, Purpose = "Risk Register Completed" });
            Delete.FromTable("RiskRegisterAssetGroups")
                  .Rows(new { Description = "Pipeline" }, new { Description = "Other" });
        }
    }
}

