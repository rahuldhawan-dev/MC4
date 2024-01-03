using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211215115243806), Tags("Production")]
    public class MC4067SampleSitesLimsSequenceNumber : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Column("LimsSequenceNumber").OnTable("SampleSites").AsInt32().Nullable();
        }
    }
}

