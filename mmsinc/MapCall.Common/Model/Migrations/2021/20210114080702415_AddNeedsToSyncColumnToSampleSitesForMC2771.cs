using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20210114080702415), Tags("Production")]
    public class AddNeedsToSyncColumnToSampleSitesForMC2771 : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Column("NeedsToSync").OnTable("SampleSites").AsBoolean().NotNullable().WithDefaultValue(true);
            Create.Column("LastSyncedAt").OnTable("SampleSites").AsDateTime().Nullable();
        }
    }
}
