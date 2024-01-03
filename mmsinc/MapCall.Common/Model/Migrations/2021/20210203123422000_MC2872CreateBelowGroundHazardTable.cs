using FluentMigrator;
using FluentMigrator.Expressions;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210203123422000), Tags("Production")]
    public class MC2872CreateBelowGroundHazardTable : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("HazardTypes", "Electrical", "Gas Main", "Ground Water", "Sewer Main", "Telephone to Start", "Water Main");
            this.CreateLookupTableWithValues("HazardApproachRecommended", "Use Vactor", "Hand Dig to Start");

            Create.Table("BelowGroundHazards")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("WorkOrderId", "WorkOrders", "WorkOrderId").Nullable()
                  .WithForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterID").NotNullable()
                  .WithForeignKeyColumn("TownId", "Towns", "TownId").NotNullable()
                  .WithForeignKeyColumn("TownSectionId", "TownSections", "TownSectionId").Nullable()
                  .WithColumn("StreetNumber").AsInt64().Nullable()
                  .WithForeignKeyColumn("StreetId", "Streets", "StreetId").NotNullable()
                  .WithForeignKeyColumn("CrossStreetId", "Streets", "StreetId").Nullable()
                  .WithForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateId").NotNullable()
                  .WithColumn("HazardAreaFt").AsInt64().NotNullable()
                  .WithForeignKeyColumn("HazardTypeId", "HazardTypes").NotNullable()
                  .WithColumn("DepthOfHazardInch").AsInt64().Nullable()
                  .WithForeignKeyColumn("AssetStatusId", "AssetStatuses", "AssetStatusId").NotNullable()
                  .WithColumn("HazardDescription").AsAnsiString(255).NotNullable()
                  .WithColumn("ProximityFromAmWaterAssetFt").AsInt64().Nullable()
                  .WithForeignKeyColumn("ApproachRecommendedId", "HazardApproachRecommended").Nullable();

            this.AddDataType("BelowGroundHazards");
            this.AddDocumentType("Below Ground Hazard", "BelowGroundHazards");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("BelowGroundHazards", "HazardTypeId", "HazardTypes");
            Delete.ForeignKeyColumn("BelowGroundHazards", "ApproachRecommendedId", "HazardApproachRecommended");
            Delete.ForeignKeyColumn("BelowGroundHazards", "AssetStatusId", "AssetStatuses");
            Delete.ForeignKeyColumn("BelowGroundHazards", "CoordinateId", "Coordinates");
            Delete.ForeignKeyColumn("BelowGroundHazards", "WorkOrderId", "WorkOrders");

            Delete.Table("BelowGroundHazards");
            Delete.Table("HazardTypes");
            Delete.Table("HazardApproachRecommended");

            this.RemoveDocumentTypeAndAllRelatedDocuments("Below Ground Hazard", "BelowGroundHazards");
            this.RemoveDataType("BelowGroundHazards");
        }
    }
}