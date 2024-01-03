using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220318013255791), Tags("Production")]
    public class MC1514HighRiskAssetRenamedToRiskRegisterAsset : Migration
    {
        public struct OldTableNames
        {
            public const string ASSETS = "HighRiskAssets",
                                GROUPS = "HighRiskAssetGroups",
                                IMPACTS = "HighRiskAssetImpacts",
                                PROBABILITIES = "HighRiskAssetProbabilities",
                                CATEGORIES = "HighRiskAssetCategories";
        }

        public struct NewTableNames
        {
            public const string ASSETS = "RiskRegisterAssets",
                                GROUPS = "RiskRegisterAssetGroups",
                                IMPACTS = "RiskRegisterAssetImpacts",
                                PROBABILITIES = "RiskRegisterAssetProbabilities",
                                CATEGORIES = "RiskRegisterAssetCategories";
        }

        public override void Up()
        {
            this.DeleteForeignKeyColumn(OldTableNames.ASSETS, "HighRiskAssetGroupId", OldTableNames.GROUPS);
            this.DeleteForeignKeyColumn(OldTableNames.ASSETS, "HighRiskAssetImpactId", OldTableNames.IMPACTS);
            this.DeleteForeignKeyColumn(OldTableNames.ASSETS, "HighRiskAssetProbabilityId", OldTableNames.PROBABILITIES);
            this.DeleteForeignKeyColumn(OldTableNames.ASSETS, "HighRiskAssetCategoryId", OldTableNames.CATEGORIES);

            Rename.Table(OldTableNames.ASSETS).To(NewTableNames.ASSETS);
            Rename.Table(OldTableNames.GROUPS).To(NewTableNames.GROUPS);
            Rename.Table(OldTableNames.IMPACTS).To(NewTableNames.IMPACTS);
            Rename.Table(OldTableNames.PROBABILITIES).To(NewTableNames.PROBABILITIES);
            Rename.Table(OldTableNames.CATEGORIES).To(NewTableNames.CATEGORIES);

            Alter.Table(NewTableNames.ASSETS)
                 .AddForeignKeyColumn("RiskRegisterAssetProbabilityId", "RiskRegisterAssetProbabilities", nullable: false)
                 .AddForeignKeyColumn("RiskRegisterAssetGroupId", "RiskRegisterAssetGroups", nullable: false)
                 .AddForeignKeyColumn("RiskRegisterAssetImpactId", "RiskRegisterAssetImpacts", nullable: false)
                 .AddForeignKeyColumn("RiskRegisterAssetCategoryId", "RiskRegisterAssetCategories");

            this.RenameDataType(OldTableNames.ASSETS, NewTableNames.ASSETS);
            this.RenameModule("High Risk Asset Management", "Risk Register");
            this.ReassignModule("Risk Register", "Engineering");
        }

        public override void Down()
        {
            this.DeleteForeignKeyColumn(NewTableNames.ASSETS, "RiskRegisterAssetGroupId", NewTableNames.GROUPS);
            this.DeleteForeignKeyColumn(NewTableNames.ASSETS, "RiskRegisterAssetImpactId", NewTableNames.IMPACTS);
            this.DeleteForeignKeyColumn(NewTableNames.ASSETS, "RiskRegisterAssetProbabilityId", NewTableNames.PROBABILITIES);
            this.DeleteForeignKeyColumn(NewTableNames.ASSETS, "RiskRegisterAssetCategoryId", NewTableNames.CATEGORIES);

            Rename.Table(NewTableNames.ASSETS).To(OldTableNames.ASSETS);
            Rename.Table(NewTableNames.GROUPS).To(OldTableNames.GROUPS);
            Rename.Table(NewTableNames.IMPACTS).To(OldTableNames.IMPACTS);
            Rename.Table(NewTableNames.PROBABILITIES).To(OldTableNames.PROBABILITIES);
            Rename.Table(NewTableNames.CATEGORIES).To(OldTableNames.CATEGORIES);

            // This looks dangerous, but it isn't. This migration exists because there was potential the previous 
            // migrations in this story ran in lower environments. 
            Delete.FromTable(OldTableNames.ASSETS).AllRows();

            Alter.Table(OldTableNames.ASSETS)
                 .AddForeignKeyColumn("HighRiskAssetProbabilityId", "HighRiskAssetProbabilities", nullable: false)
                 .AddForeignKeyColumn("HighRiskAssetGroupId", "HighRiskAssetGroups", nullable: false)
                 .AddForeignKeyColumn("HighRiskAssetImpactId", "HighRiskAssetImpacts", nullable: false)
                 .AddForeignKeyColumn("HighRiskAssetCategoryId", "HighRiskAssetCategories");

            this.RenameDataType(NewTableNames.ASSETS, OldTableNames.ASSETS);
            this.RenameModule("Risk Register", "High Risk Asset Management");
            this.ReassignModule("High Risk Asset Management", "Field Services");
            this.DeleteApplication("Engineering");
        }
    }
}

