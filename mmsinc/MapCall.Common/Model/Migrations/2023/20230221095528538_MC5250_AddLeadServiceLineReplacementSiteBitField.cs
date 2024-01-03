using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230221095528538), Tags("Production")]
    public class MC5250AddLeadServiceLineReplacementSiteBitField : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Column("LeadServiceLineReplacementSite").OnTable("SampleSites").AsBoolean().Nullable();
        }
    }
}
