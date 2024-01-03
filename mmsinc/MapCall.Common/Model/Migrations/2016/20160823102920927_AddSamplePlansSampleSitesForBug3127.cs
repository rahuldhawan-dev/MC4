using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160823102920927), Tags("Production")]
    public class AddSamplePlansSampleSitesForBug3127 : Migration
    {
        public override void Up()
        {
            Create.Table("SamplePlansSampleSites")
                  .WithForeignKeyColumn("SamplePlanId", "SamplePlans").NotNullable()
                  .WithForeignKeyColumn("SampleSiteId", "tblWQSample_Sites", "SampleSiteID").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("SamplePlansSampleSites");
        }
    }
}
