using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170621145701494), Tags("Production")]
    public class MessWithReasonsForSampleSiteInvalidationForBug3903 : Migration
    {
        public override void Up()
        {
            Alter.Column("Description").OnTable("ReasonsForSampleSiteInvalidation").AsString(50);
            Execute.Sql(
                "update ReasonsForSampleSiteInvalidation set Description = 'Customer opted out of current Sampling Period' where Description = 'Customer opted out of Program'");
        }

        public override void Down()
        {
            Execute.Sql(
                "update ReasonsForSampleSiteInvalidation set Description = 'Customer opted out of Program' where Description = 'Customer opted out of current Sampling Period'");
        }
    }
}
