using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170413104246687), Tags("Production")]
    public class CreateReasonsForSampleSiteInvalidationTableForBug3316 : Migration
    {
        public const string TABLE_NAME = "ReasonsForSampleSiteInvalidation";

        public override void Up()
        {
            Create.LookupTable(TABLE_NAME, 30);

            Insert.IntoTable(TABLE_NAME).Rows(
                new {Description = "Customer Declined Program"},
                new {Description = "Customer Opted out of Program"},
                new {Description = "Customer Service Line Replaced"},
                new {Description = "Company Service Line Replaced"},
                new {Description = "Internal Plumbing Replaced"},
                new {Description = "Building Demolished"});

            Alter.Table("tblWQSample_Sites").AddForeignKeyColumn("ReasonForInvalidationId", TABLE_NAME);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("tblWQSample_Sites", "ReasonForInvalidationId", TABLE_NAME);

            Delete.Table(TABLE_NAME);
        }
    }
}