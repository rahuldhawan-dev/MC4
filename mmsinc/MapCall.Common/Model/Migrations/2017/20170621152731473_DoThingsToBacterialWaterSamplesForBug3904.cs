using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170621152731473), Tags("Production")]
    public class DoThingsToBacterialWaterSamplesForBug3904 : Migration
    {
        public override void Up()
        {
            Alter.Table("BacterialWaterSamples").AddColumn("SampleId").AsString(25).Nullable();
            Alter.Table("BacterialWaterSamples").AddColumn("ComplianceSample").AsBoolean().WithDefaultValue(false);

            Execute.Sql(
                "UPDATE BacterialWaterSamples SET BacterialSampleTypeId = NULL WHERE BacterialSampleTypeID NOT IN (SELECT Id from BacterialSampleTypes WHERE Description IN ('Compliance', 'Recheck'))");
            Execute.Sql("DELETE FROM BacterialSampleTypes WHERE Description NOT IN ('Compliance', 'Recheck')");
            Execute.Sql("UPDATE BacterialSampleTypes SET Description = 'Routine' WHERE Description = 'Compliance'");
            Execute.Sql("UPDATE BacterialSampleTypes SET Description = 'Repeat' WHERE Description = 'Recheck'");

            Insert.IntoTable("BacterialSampleTypes").Rows(
                new {Description = "Confirmation"},
                new {Description = "Special"},
                new {Description = "Duplicate"},
                new {Description = "Split"},
                new {Description = "Shipping Blank"},
                new {Description = "Field Blank"},
                new {Description = "Batch Blank"},
                new {Description = "Split Blank"},
                new {Description = "Performance Evaluation"},
                new {Description = "Max Residence Time"});

            Create.LookupTable("BacterialWaterSampleRepeatLocationTypes", 30);

            Insert.IntoTable("BacterialWaterSampleRepeatLocationTypes").Rows(
                new {Description = "Downstream"},
                new {Description = "Near First Service Connection"},
                new {Description = "Original Site"},
                new {Description = "Other"},
                new {Description = "Upstream"});

            Alter.Table("BacterialWaterSamples")
                 .AddForeignKeyColumn("RepeatLocationTypeId", "BacterialWaterSampleRepeatLocationTypes");

            Alter.Table("BacterialWaterSamples").AddColumn("SetupAnalyst").AsString(50).Nullable();
            Alter.Table("BacterialWaterSamples").AddColumn("ReadAnalyst").AsString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("SetupAnalyst").FromTable("BacterialWaterSamples");
            Delete.Column("ReadAnalyst").FromTable("BacterialWaterSamples");
            Delete.Column("SampleId").FromTable("BacterialWaterSamples");
            Delete.Column("ComplianceSample").FromTable("BacterialWaterSamples");

            Execute.Sql(
                "UPDATE BacterialWaterSamples SET BacterialSampleTypeId = NULL WHERE BacterialSampleTypeID NOT IN (SELECT Id from BacterialSampleTypes WHERE Description IN ('Routine', 'Repeat'))");
            Execute.Sql("DELETE FROM BacterialSampleTypes WHERE Description NOT IN ('Routine', 'Repeat')");
            Execute.Sql("UPDATE BacterialSampleTypes SET Description = 'Compliance' WHERE Description = 'Routine'");
            Execute.Sql("UPDATE BacterialSampleTypes SET Description = 'Recheck' WHERE Description = 'Repeat'");

            Insert.IntoTable("BacterialSampleTypes").Rows(
                new {Description = "New Main"},
                new {Description = "Process Control"},
                new {Description = "System Repair"});

            Delete.ForeignKeyColumn("BacterialWaterSamples", "RepeatLocationTypeId",
                "BacterialWaterSampleRepeatLocationTypes");

            Delete.Table("BacterialWaterSampleRepeatLocationTypes");
        }
    }
}
