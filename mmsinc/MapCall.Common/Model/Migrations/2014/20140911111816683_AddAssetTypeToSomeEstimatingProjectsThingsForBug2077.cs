using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140911111816683), Tags("Production")]
    public class AddAssetTypeToSomeEstimatingProjectsThingsForBug2077 : Migration
    {
        public override void Up()
        {
            Alter.Table("EstimatingProjectContractorLaborCosts")
                 .AddColumn("AssetTypeId").AsInt32()
                 .ForeignKey("FK_EstimatingProjectContractorLaborCosts_AssetTypes_AssetTypeId", "AssetTypes",
                      "AssetTypeId").Nullable();
            Alter.Table("EstimatingProjectsCompanyLaborCosts")
                 .AddColumn("AssetTypeId").AsInt32()
                 .ForeignKey("FK_EstimatingProjectsCompanyLaborCosts_AssetTypes_AssetTypeId", "AssetTypes",
                      "AssetTypeId").Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_EstimatingProjectContractorLaborCosts_AssetTypes_AssetTypeId")
                  .OnTable("EstimatingProjectContractorLaborCosts");
            Delete.ForeignKey("FK_EstimatingProjectsCompanyLaborCosts_AssetTypes_AssetTypeId")
                  .OnTable("EstimatingProjectsCompanyLaborCosts");

            Delete.Column("AssetTypeId").FromTable("EstimatingProjectContractorLaborCosts");
            Delete.Column("AssetTypeId").FromTable("EstimatingProjectsCompanyLaborCosts");
        }
    }
}
