using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140922163603936), Tags("Production")]
    public class AddThingsForBug2104 : Migration
    {
        public override void Up()
        {
            Alter.Table("EstimatingProjectsPermits").AddForeignKeyColumn("AssetTypeId", "AssetTypes", "AssetTypeId");
            Alter.Table("EstimatingProjectOtherCosts").AddForeignKeyColumn("AssetTypeId", "AssetTypes", "AssetTypeId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("EstimatingProjectsPermits", "AssetTypeId", "AssetTypes", "AssetTypeId");
            Delete.ForeignKeyColumn("EstimatingProjectOtherCosts", "AssetTypeId", "AssetTypes", "AssetTypeId");
        }
    }
}
