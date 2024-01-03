using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160912012257853), Tags("Production")]
    public class UpdateSampleSiteAlternateStatusForBug3165 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "update tblWQSample_Sites set IsAlternateSite = 1, SiteStatusID = (select id from SampleSiteStatuses where description = 'Active') where SiteStatusId = (select id from SampleSiteStatuses where description = 'Alternate');" +
                "Delete from SampleSiteStatuses where description = 'Alternate';");
        }

        public override void Down()
        {
            Execute.Sql("SET IDENTITY_INSERT SampleSiteStatuses ON " +
                        "INSERT INTO SampleSiteStatuses(Id, Description) Values(3, 'Alternate') " +
                        "SET IDENTITY_INSERT SampleSiteStatuses OFF");
        }
    }
}
