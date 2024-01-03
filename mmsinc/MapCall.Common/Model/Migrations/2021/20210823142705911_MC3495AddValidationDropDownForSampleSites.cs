using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210823142705911)]
    [Tags("Production")]
    public class MC3495AddValidationDropDownForSampleSites : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SampleSiteValidationStatuses", "Validated", "Not Validated",
                "Needs Investigation", "Waiting on Information", "Retire");
            Alter.Table("SampleSites").AddForeignKeyColumn("SampleSiteValidationStatusId", "SampleSiteValidationStatuses");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SampleSites", "SampleSiteValidationStatusId", "SampleSiteValidationStatuses");
            Delete.Table("SampleSiteValidationStatuses");
        }
    }
}
