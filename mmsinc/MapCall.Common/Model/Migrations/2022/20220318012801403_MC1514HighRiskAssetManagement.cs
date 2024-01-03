using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220318012801403), Tags("Production")]
    public class MC1514HighRiskAssetManagement : Migration
    {
        public struct TableNames
        {
            public const string HIGH_RISK_ASSETS = "HighRiskAssets",
                                GROUPS = "HighRiskAssetGroups",
                                IMPACTS = "HighRiskAssetImpacts",
                                PROBABILITIES = "HighRiskAssetProbabilities",
                                CATEGORIES = "HighRiskAssetCategories";
        }

        public override void Up()
        {
            this.CreateLookupTableWithValues(TableNames.GROUPS, "System", "Facility", "Equipment", "Transmission & Distribution", "Metering", "Customer");
            this.CreateLookupTableWithValues(TableNames.PROBABILITIES, "Low < 25%", "Possible 50%", "Probable 75%", "Very Probable 90%", "Imminent 99%");
            this.CreateLookupTableWithValues(TableNames.IMPACTS, "Slight", "Moderate", "Serious", "Severe");
            this.CreateLookupTableWithValues(TableNames.CATEGORIES, "Asset Condition", "Financial", "Resilience", "Regulatory", "Reputational", "Service Level");

            Create.Table(TableNames.HIGH_RISK_ASSETS)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("HighRiskAssetProbabilityId", "HighRiskAssetProbabilities", nullable: false)
                  .WithForeignKeyColumn("HighRiskAssetGroupId", "HighRiskAssetGroups", nullable: false)
                  .WithForeignKeyColumn("HighRiskAssetImpactId", "HighRiskAssetImpacts", nullable: false)
                  .WithForeignKeyColumn("HighRiskAssetCategoryId", "HighRiskAssetCategories")
                  .WithForeignKeyColumn("StateId", "States", "stateID", false)
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID", false)
                  .WithForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies")
                  .WithForeignKeyColumn("WasteWaterSystemId", "WasteWaterSystems")
                  .WithForeignKeyColumn("FacilityId", "tblFacilities", "RecordId")
                  .WithForeignKeyColumn("EquipmentId", "Equipment", "EquipmentID")
                  .WithForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID")
                  .WithForeignKeyColumn("EmployeeId", "tblEmployee", "tblEmployeeID", false)
                  .WithColumn("ImpactDescription").AsAnsiString(255).NotNullable()
                  .WithColumn("RiskDescription").AsAnsiString(255).NotNullable()
                  .WithColumn("RiskQuadrant").AsInt32().Nullable()
                  .WithColumn("IdentifiedAt").AsDateTime().NotNullable()
                  .WithColumn("InterimMitigationMeasuresTaken").AsAnsiString(150).Nullable()
                  .WithColumn("InterimMitigationMeasuresTakenAt").AsDateTime().Nullable()
                  .WithColumn("InterimMitigationMeasuresTakenEstimatedCosts").AsCurrency().Nullable()
                  .WithColumn("FinalMitigationMeasuresTaken").AsAnsiString(255).Nullable()
                  .WithColumn("FinalMitigationMeasuresTakenAt").AsDateTime().Nullable()
                  .WithColumn("FinalMitigationMeasuresTakenEstimatedCosts").AsCurrency().Nullable()
                  .WithColumn("CompletionTargetDate").AsDateTime().Nullable()
                  .WithColumn("CompletionActualDate").AsDateTime().Nullable()
                  .WithColumn("IsProjectInComprehensivePlanningStudy").AsBoolean().NotNullable()
                  .WithColumn("IsProjectInCapitalPlan").AsBoolean().NotNullable()
                  .WithColumn("RelatedWorkBreakdownStructure").AsAnsiString(25).Nullable();

            this.AddDataType(TableNames.HIGH_RISK_ASSETS);
            this.AddDocumentType("Photo", TableNames.HIGH_RISK_ASSETS);

            this.CreateModule("High Risk Asset Management", "Field Services", 88);
        }

        public override void Down()
        {
            this.DeleteModuleAndAssociatedRoles("Field Services", "High Risk Asset Management");

            this.RemoveDocumentTypeAndAllRelatedDocuments("Photo", TableNames.HIGH_RISK_ASSETS);
            this.RemoveDataType(TableNames.HIGH_RISK_ASSETS);

            Delete.Table(TableNames.HIGH_RISK_ASSETS);
            Delete.Table(TableNames.CATEGORIES);
            Delete.Table(TableNames.IMPACTS);
            Delete.Table(TableNames.PROBABILITIES);
            Delete.Table(TableNames.GROUPS);
        }
    }
}

