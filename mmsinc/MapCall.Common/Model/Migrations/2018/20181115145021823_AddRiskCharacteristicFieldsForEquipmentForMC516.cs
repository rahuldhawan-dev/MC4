using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181115145021823), Tags("Production")]
    public class AddRiskCharacteristicFieldsForEquipmentForMC516 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("EquipmentConditions", "Poor", "Average", "Good");
            this.CreateLookupTableWithValues("EquipmentPerformanceRatings", "Poor", "Average", "Good");

            this.CreateLookupTableWithValues("EquipmentStaticDynamicTypes", "Static", "Dynamic");

            this.CreateLookupTableWithValues("EquipmentConsequencesOfFailureRatings", "Low", "Medium", "High");
            this.CreateLookupTableWithValues("EquipmentLikelyhoodOfFailureRatings", "Low", "Medium", "High");
            this.CreateLookupTableWithValues("EquipmentReliabilityRatings", "Low", "Medium", "High");
            this.CreateLookupTableWithValues("EquipmentFailureRiskRatings", "Low", "Medium", "High");

            this.CreateLookupTableWithValues("StrategyTiers", "Tier 1", "Tier 2", "Tier 3");
            // 8 new facility fields
            // 8 new equipment fields
            Alter.Table("Equipment").AddForeignKeyColumn("ConditionId", "EquipmentConditions");
            Alter.Table("Equipment").AddForeignKeyColumn("PerformanceRatingId", "EquipmentPerformanceRatings");
            Alter.Table("Equipment").AddForeignKeyColumn("StaticDynamicTypeId", "EquipmentStaticDynamicTypes");
            Alter.Table("Equipment")
                 .AddForeignKeyColumn("ConsequenceOfFailureId", "EquipmentConsequencesOfFailureRatings");
            Alter.Table("Equipment")
                 .AddForeignKeyColumn("LikelyhoodOfFailureId", "EquipmentLikelyhoodOfFailureRatings");
            Alter.Table("Equipment").AddForeignKeyColumn("ReliabilityId", "EquipmentReliabilityRatings");
            Alter.Table("Equipment").AddColumn("LocalizedRiskOfFailure").AsInt32().Nullable();
            Alter.Table("Equipment").AddForeignKeyColumn("RiskOfFailureId", "EquipmentFailureRiskRatings");
        }

        public override void Down()
        {
            Delete.Column("LocalizedRiskOfFailure").FromTable("Equipment");
            Delete.ForeignKeyColumn("Equipment", "RiskOfFailureId", "EquipmentFailureRiskRatings");
            Delete.ForeignKeyColumn("Equipment", "ReliabilityId", "EquipmentReliabilityRatings");
            Delete.ForeignKeyColumn("Equipment", "LikelyhoodOfFailureId", "EquipmentLikelyhoodOfFailureRatings");
            Delete.ForeignKeyColumn("Equipment", "ConsequenceOfFailureId", "EquipmentConsequencesOfFailureRatings");
            Delete.ForeignKeyColumn("Equipment", "StaticDynamicTypeId", "EquipmentStaticDynamicTypes");
            Delete.ForeignKeyColumn("Equipment", "PerformanceRatingId", "EquipmentPerformanceRatings");
            Delete.ForeignKeyColumn("Equipment", "ConditionId", "EquipmentConditions");

            Delete.Table("StrategyTiers");
            Delete.Table("EquipmentFailureRiskRatings");
            Delete.Table("EquipmentReliabilityRatings");
            Delete.Table("EquipmentLikelyhoodOfFailureRatings");
            Delete.Table("EquipmentConsequencesOfFailureRatings");
            Delete.Table("EquipmentStaticDynamicTypes");
            Delete.Table("EquipmentPerformanceRatings");
            Delete.Table("EquipmentConditions");
        }
    }
}
