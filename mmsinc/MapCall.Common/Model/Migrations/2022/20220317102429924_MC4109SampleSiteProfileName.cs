using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220317102429924), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4109SampleSiteProfileName : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Column("Name").OnTable("SampleSiteProfiles").AsAnsiString(255).Nullable();
        }
    }
}

