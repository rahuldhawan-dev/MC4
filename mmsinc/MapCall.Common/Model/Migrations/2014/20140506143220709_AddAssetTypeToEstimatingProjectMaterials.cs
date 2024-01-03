using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140506143220709), Tags("Production")]
    public class AddAssetTypeToEstimatingProjectMaterials : Migration
    {
        public override void Up()
        {
            Alter.Table("EstimatingProjectsMaterials")
                 .AddColumn("AssetTypeId")
                 .AsInt32()
                 .ForeignKey("FK_EstimatingProjectsMaterials_AssetTypes_AssetTypeId", "AssetTypes", "AssetTypeId")
                 .WithDefaultValue("1");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_EstimatingProjectsMaterials_AssetTypes_AssetTypeId")
                  .OnTable("EstimatingProjectsMaterials");
            Delete.Column("AssetTypeId").FromTable("EstimatingProjectsMaterials");
        }
    }
}
