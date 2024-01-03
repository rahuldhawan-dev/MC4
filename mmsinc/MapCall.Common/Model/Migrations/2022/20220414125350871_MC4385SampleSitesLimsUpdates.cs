using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220414125350871), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4385SampleSitesLimsUpdates : AutoReversingMigration
    {
        public override void Up()
        {
            Rename.Column("IsSentToLims").OnTable("SampleSites").To("IsLimsLocation");
        }
    }
}

