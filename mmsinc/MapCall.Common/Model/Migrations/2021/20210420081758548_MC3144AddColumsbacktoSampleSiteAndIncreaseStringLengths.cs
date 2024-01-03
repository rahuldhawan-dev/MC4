using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210420081758548), Tags("Production")]
    public class MC3144AddColumsbacktoSampleSiteAndIncreaseStringLengths : Migration
    {
        public override void Up()
        {
            Alter.Column("LocationNameDescription").OnTable("SampleSites").AsAnsiString(100).Nullable();
            Alter.Column("CommonSiteName").OnTable("SampleSites").AsAnsiString(100).Nullable();
            Alter.Table("SampleSites").AddColumn("Route").AsAnsiString(50).Nullable();
            Alter.Table("SampleSites").AddColumn("Route_Sequence").AsAnsiString(50).Nullable();
        }

        public override void Down()
        {
            Alter.Column("LocationNameDescription").OnTable("SampleSites").AsAnsiString(40).Nullable();
            Alter.Column("CommonSiteName").OnTable("SampleSites").AsAnsiString(20).Nullable();
            Delete.Column("Route").FromTable("SampleSites");
            Delete.Column("Route_Sequence").FromTable("SampleSites");
        }
    }
}

