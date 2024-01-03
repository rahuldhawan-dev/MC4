using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170329115223506), Tags("Production")]
    public class CreateReasonsForInvalidationForBug3701 : Migration
    {
        public const string TABLE_NAME = "ReasonsForSampleInvalidation";

        public override void Up()
        {
            Create.LookupTable(TABLE_NAME, 35);

            Insert.IntoTable(TABLE_NAME)
                  .Rows(
                       new {Description = "Sample Collected Improperly"},
                       new {Description = "Sample Damaged in Transit"},
                       new {Description = "Improper Analysis or Calibration"},
                       new {Description = "Other"});

            Alter.Table("WaterSamples").AddForeignKeyColumn("ReasonForInvalidationId", TABLE_NAME);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WaterSamples", "ReasonForInvalidationId", TABLE_NAME);
            Delete.Table(TABLE_NAME);
        }
    }
}
