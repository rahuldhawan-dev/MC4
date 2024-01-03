using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190311085838211), Tags("Production")]
    public class MC779FacilityRiskCharacteristics : Migration
    {
        public override void Up()
        {
            Create.LookupTable("FacilityConditions");
            Create.LookupTable("FacilityPerformances");
            Create.LookupTable("FacilityLikelihoodsOfFailure");
            Create.LookupTable("FacilityConsequencesOfFailure");
            Create.LookupTable("FacilityAssetManagementMaintenanceStrategyTiers");

            Insert.IntoTable("FacilityConditions").Rows(
                new {Description = "Good"},
                new {Description = "Average"},
                new {Description = "Poor"});
            Insert.IntoTable("FacilityPerformances").Rows(
                new {Description = "Good"},
                new {Description = "Average"},
                new {Description = "Poor"});
            Insert.IntoTable("FacilityLikelihoodsOfFailure").Rows(
                new {Description = "Low"},
                new {Description = "Medium"},
                new {Description = "High"});
            Insert.IntoTable("FacilityConsequencesOfFailure").Rows(
                new {Description = "Low"},
                new {Description = "Medium"},
                new {Description = "High"});
            Insert.IntoTable("FacilityAssetManagementMaintenanceStrategyTiers").Rows(
                new {Description = "Tier 1"},
                new {Description = "Tier 2"},
                new {Description = "Tier 3"});

            Alter.Table("tblFacilities").AddForeignKeyColumn("ConditionId", "FacilityConditions");
            Alter.Table("tblFacilities").AddForeignKeyColumn("PerformanceId", "FacilityPerformances");
            Alter.Table("tblFacilities")
                 .AddForeignKeyColumn("LikelihoodOfFailureId", "FacilityLikelihoodsOfFailure");
            Alter.Table("tblFacilities")
                 .AddForeignKeyColumn("ConsequenceOfFailureId", "FacilityConsequencesOfFailure");
            Alter.Table("tblFacilities")
                 .AddForeignKeyColumn("StrategyTierId", "FacilityAssetManagementMaintenanceStrategyTiers");
            Alter.Table("tblFacilities").AddColumn("RiskOfFailureScore").AsInt16().Nullable();
            Alter.Table("tblFacilities").AddColumn("ConsequenceOfFailureFactor").AsFloat().Nullable();
            Alter.Table("tblFacilities").AddColumn("WeightedRiskOfFailureScore").AsDouble().Nullable();

            Alter.Table("PublicWaterSupplies").AddColumn("UsageLastYear").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblFacilities", "ConditionId", "FacilityConditions");
            Delete.ForeignKeyColumn("tblFacilities", "PerformanceId", "FacilityPerformances");
            Delete.ForeignKeyColumn("tblFacilities", "LikelihoodOfFailureId", "FacilityLikelihoodsOfFailure");
            Delete.ForeignKeyColumn("tblFacilities", "ConsequenceOfFailureId", "FacilityConsequencesOfFailure");
            Delete.ForeignKeyColumn("tblFacilities", "StrategyTierId",
                "FacilityAssetManagementMaintenanceStrategyTiers");
            Delete.Column("RiskOfFailureScore").FromTable("tblFacilities");
            Delete.Column("ConsequenceOfFailureFactor").FromTable("tblFacilities");
            Delete.Column("WeightedRiskOfFailureScore").FromTable("tblFacilities");

            Delete.Column("TotalCustomersServed").FromTable("PublicWaterSupplies");

            Delete.Table("FacilityConditions");
            Delete.Table("FacilityPerformances");
            Delete.Table("FacilityLikelihoodsOfFailure");
            Delete.Table("FacilityConsequencesOfFailure");
            Delete.Table("FacilityAssetManagementMaintenanceStrategyTiers");
        }
    }
}
