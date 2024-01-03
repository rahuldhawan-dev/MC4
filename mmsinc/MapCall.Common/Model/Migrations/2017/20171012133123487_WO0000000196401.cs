using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20171012133123487), Tags("Production")]
    public class WO0000000196401 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SampleSiteBracketSiteLocationTypes", "Upstream", "Downstream",
                "First Service Connection", "Other");
            Create.Table("SampleSitesBracketSites")
                  .WithIdentityColumn()
                  .WithColumn("SampleSiteId").AsInt32().NotNullable()
                  .ForeignKey("FK_SampleSitesBracketSites_tblWQSample_Sites_SampleSiteId", "tblWQSample_Sites",
                       "SampleSiteId")
                  .WithColumn("BracketSiteSampleSiteId").AsInt32().NotNullable()
                  .ForeignKey("FK_SampleSitesBracketSites_tblWQSample_Sites_BracketSiteSampleSiteId",
                       "tblWQSample_Sites", "SampleSiteId")
                  .WithColumn("SampleSiteBracketSiteLocationTypeId").AsInt32().NotNullable()
                  .ForeignKey(
                       "FK_SampleSitesBracketSites_SampleSiteBracketSiteLocationTypes_SampleSiteBracketSiteLocationTypeId",
                       "SampleSiteBracketSiteLocationTypes", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_SampleSitesBracketSites_tblWQSample_Sites_SampleSiteId")
                  .OnTable("SampleSitesBracketSites");
            Delete.ForeignKey("FK_SampleSitesBracketSites_tblWQSample_Sites_BracketSiteSampleSiteId")
                  .OnTable("SampleSitesBracketSites");
            Delete.ForeignKey(
                       "FK_SampleSitesBracketSites_SampleSiteBracketSiteLocationTypes_SampleSiteBracketSiteLocationTypeId")
                  .OnTable("SampleSitesBracketSites");
            Delete.Table("SampleSitesBracketSites");
            Delete.Table("SampleSiteBracketSiteLocationTypes");
        }
    }
}
