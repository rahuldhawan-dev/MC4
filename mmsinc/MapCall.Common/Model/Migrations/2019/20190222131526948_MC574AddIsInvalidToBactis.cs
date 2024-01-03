using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190222131526948), Tags("Production")]
    public class MC574AddIsInvalidToBactis : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("BacterialWaterSampleReasonsForInvalidation",
                "No COC",
                "Incorrect COC",
                "Out of Temp",
                "Broken/Incorrect Bottle");

            // Victoria said it's okay for all existing samples to be marked as false for IsInvalid.
            Create.Column("IsInvalid").OnTable("BacterialWaterSamples").AsBoolean().NotNullable()
                  .WithDefaultValue(false);
            Create.Column("ReasonForInvalidationId").OnTable("BacterialWaterSamples").AsInt32().Nullable()
                  .ForeignKey(
                       "FK_BacterialWaterSamples_BacterialWaterSampleReasonsForInvalidation_ReasonForInvalidationId",
                       "BacterialWaterSampleReasonsForInvalidation", "Id");
        }

        public override void Down()
        {
            Delete.ForeignKey(
                       "FK_BacterialWaterSamples_BacterialWaterSampleReasonsForInvalidation_ReasonForInvalidationId")
                  .OnTable("BacterialWaterSamples");
            Delete.Column("ReasonForInvalidationId").FromTable("BacterialWaterSamples");
            Delete.Column("IsInvalid").FromTable("BacterialWaterSamples");

            Delete.Table("BacterialWaterSampleReasonsForInvalidation");
        }
    }
}
