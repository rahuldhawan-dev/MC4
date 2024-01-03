using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141103132710849), Tags("Production")]
    public class OperatingCenterNeedsStateAndStateRegionSoThisMigrationAddsThoseThusSatisfyingBug2187 : Migration
    {
        public override void Up()
        {
            Create.Table("StateRegions")
                  .WithIdentityColumn()
                  .WithColumn("Region").AsString(50).NotNullable()
                  .WithForeignKeyColumn("StateId", "States", "StateId", false);

            Alter.Table("OperatingCenters").AddForeignKeyColumn("StateRegionId", "StateRegions");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("OperatingCenters", "StateRegionId", "StateRegions");

            Delete.Table("StateRegions");
        }
    }
}
