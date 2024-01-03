using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200326075957490), Tags("Production")]
    public class MC1955SeparateOutSampleCollectedAndSampleReportedColumnsOnWaterSampleComplianceForm : Migration
    {
        public override void Up()
        {
            // Rename Sample collected and reported to rename to sample collected and sample reported
            Rename.Column("CentralLabSamplesHaveBeenCollectedAndReportedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("CentralLabSamplesHaveBeenCollectedAnswerId");
            Rename.Column("ContractedLabsSamplesHaveBeenCollectedAndReportedAnswerId")
                  .OnTable("WaterSampleComplianceForms").To("ContractedLabsSamplesHaveBeenCollectedAnswerId");
            Rename.Column("InternalLabsSamplesHaveBeenCollectedAndReportedAnswerId")
                  .OnTable("WaterSampleComplianceForms").To("InternalLabsSamplesHaveBeenCollectedAnswerId");
            Rename.Column("BactiSamplesHaveBeenCollectedAndReportedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("BactiSamplesHaveBeenCollectedAnswerId");
            Rename.Column("LeadAndCopperSamplesHaveBeenCollectedAndReportedAnswerId")
                  .OnTable("WaterSampleComplianceForms").To("LeadAndCopperSamplesHaveBeenCollectedAnswerId");
            Rename.Column("WQPSamplesHaveBeenCollectedAndReportedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("WQPSamplesHaveBeenCollectedAnswerId");
            Rename.Column("SurfaceWaterPlantSamplesHaveBeenCollectedAndReportedAnswerId")
                  .OnTable("WaterSampleComplianceForms").To("SurfaceWaterPlantSamplesHaveBeenCollectedAnswerId");
            Rename.Column("ChlorineResidualsHaveBeenCollectedAndReportedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("ChlorineResidualsHaveBeenCollectedAnswerId");

            // Add new columns 
            Alter.Table("WaterSampleComplianceForms").AddForeignKeyColumn("CentralLabSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Alter.Table("WaterSampleComplianceForms")
                 .AddForeignKeyColumn("ContractedLabsSamplesHaveBeenReportedAnswerId",
                      "WaterSampleComplianceFormAnswerTypes");
            Alter.Table("WaterSampleComplianceForms").AddForeignKeyColumn("InternalLabsSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Alter.Table("WaterSampleComplianceForms").AddForeignKeyColumn("BactiSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Alter.Table("WaterSampleComplianceForms")
                 .AddForeignKeyColumn("LeadAndCopperSamplesHaveBeenReportedAnswerId",
                      "WaterSampleComplianceFormAnswerTypes");
            Alter.Table("WaterSampleComplianceForms").AddForeignKeyColumn("WQPSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Alter.Table("WaterSampleComplianceForms").AddForeignKeyColumn(
                "SurfaceWaterPlantSamplesHaveBeenReportedAnswerId", "WaterSampleComplianceFormAnswerTypes");
            Alter.Table("WaterSampleComplianceForms").AddForeignKeyColumn("ChlorineResidualsHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WaterSampleComplianceForms", "CentralLabSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Delete.ForeignKeyColumn("WaterSampleComplianceForms", "ContractedLabsSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Delete.ForeignKeyColumn("WaterSampleComplianceForms", "InternalLabsSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Delete.ForeignKeyColumn("WaterSampleComplianceForms", "BactiSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Delete.ForeignKeyColumn("WaterSampleComplianceForms", "LeadAndCopperSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Delete.ForeignKeyColumn("WaterSampleComplianceForms", "WQPSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Delete.ForeignKeyColumn("WaterSampleComplianceForms", "SurfaceWaterPlantSamplesHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");
            Delete.ForeignKeyColumn("WaterSampleComplianceForms", "ChlorineResidualsHaveBeenReportedAnswerId",
                "WaterSampleComplianceFormAnswerTypes");

            // rename columns back
            Rename.Column("CentralLabSamplesHaveBeenCollectedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("CentralLabSamplesHaveBeenCollectedAndReportedAnswerId");
            Rename.Column("ContractedLabsSamplesHaveBeenCollectedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("ContractedLabsSamplesHaveBeenCollectedAndReportedAnswerId");
            Rename.Column("InternalLabsSamplesHaveBeenCollectedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("InternalLabsSamplesHaveBeenCollectedAndReportedAnswerId");
            Rename.Column("BactiSamplesHaveBeenCollectedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("BactiSamplesHaveBeenCollectedAndReportedAnswerId");
            Rename.Column("LeadAndCopperSamplesHaveBeenCollectedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("LeadAndCopperSamplesHaveBeenCollectedAndReportedAnswerId");
            Rename.Column("WQPSamplesHaveBeenCollectedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("WQPSamplesHaveBeenCollectedAndReportedAnswerId");
            Rename.Column("SurfaceWaterPlantSamplesHaveBeenCollectedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("SurfaceWaterPlantSamplesHaveBeenCollectedAndReportedAnswerId");
            Rename.Column("ChlorineResidualsHaveBeenCollectedAnswerId").OnTable("WaterSampleComplianceForms")
                  .To("ChlorineResidualsHaveBeenCollectedAndReportedAnswerId");
        }
    }
}
