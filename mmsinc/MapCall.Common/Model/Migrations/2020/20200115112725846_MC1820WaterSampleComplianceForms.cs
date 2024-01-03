using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200115112725846), Tags("Production")]
    public class MC1820WaterSampleComplianceForms : Migration
    {
        #region Consts

        private const string WATER_SAMPLE_COMPLIANCE_FORMS = "WaterSampleComplianceForms",
                             ANSWER_TYPES_TABLE = "WaterSampleComplianceFormAnswerTypes";

        #endregion

        public override void Up()
        {
            // Create lookup table
            this.CreateLookupTableWithValues(ANSWER_TYPES_TABLE, "N/A", "Yes", "No");

            // Create new columns
            // Import existing data
            // Delete old columns
            void CreateColumnAndImportDataAndDeleteOldColumn(string oldColumnName)
            {
                // NOTE: This should be AnswerTypeId instead, but it results in one of the columns
                // having a foreign key name longer than 128 characters and that throws an error.
                var newColumnName = $"{oldColumnName}AnswerId";

                Alter.Table(WATER_SAMPLE_COMPLIANCE_FORMS).AddColumn(newColumnName).AsInt32().Nullable()
                     .ForeignKey($"FK_{WATER_SAMPLE_COMPLIANCE_FORMS}_{ANSWER_TYPES_TABLE}_{newColumnName}",
                          ANSWER_TYPES_TABLE, "Id");

                Execute.Sql($@"
    declare @answerNA int; set @answerNA = (select Id from {ANSWER_TYPES_TABLE} where Description = 'N/A')
    declare @answerNo int; set @answerNo = (select Id from {ANSWER_TYPES_TABLE} where Description = 'No')
    declare @answerYes int; set @answerYes = (select Id from {ANSWER_TYPES_TABLE} where Description = 'Yes')

update {WATER_SAMPLE_COMPLIANCE_FORMS} set {newColumnName} = @answerNA where {oldColumnName} is null
update {WATER_SAMPLE_COMPLIANCE_FORMS} set {newColumnName} = @answerNo where {oldColumnName} = 0
update {WATER_SAMPLE_COMPLIANCE_FORMS} set {newColumnName} = @answerYes where {oldColumnName} = 1
");

                Delete.Column(oldColumnName).FromTable(WATER_SAMPLE_COMPLIANCE_FORMS);
            }

            CreateColumnAndImportDataAndDeleteOldColumn("CentralLabSamplesHaveBeenCollectedAndReported");
            CreateColumnAndImportDataAndDeleteOldColumn("ContractedLabsSamplesHaveBeenCollectedAndReported");
            CreateColumnAndImportDataAndDeleteOldColumn("InternalLabsSamplesHaveBeenCollectedAndReported");
            CreateColumnAndImportDataAndDeleteOldColumn("BactiSamplesHaveBeenCollectedAndReported");
            CreateColumnAndImportDataAndDeleteOldColumn("LeadAndCopperSamplesHaveBeenCollectedAndReported");
            CreateColumnAndImportDataAndDeleteOldColumn("WQPSamplesHaveBeenCollectedAndReported");
            CreateColumnAndImportDataAndDeleteOldColumn("SurfaceWaterPlantSamplesHaveBeenCollectedAndReported");
            CreateColumnAndImportDataAndDeleteOldColumn("ChlorineResidualsHaveBeenCollectedAndReported");
        }

        public override void Down()
        {
            // Recreate old columns
            // Import existing data back to old columns
            // Delete new columns + foreign keys
            void RecreateOldColumnAndDataAndDeleteNewColumn(string oldColumnName)
            {
                var newColumnName = $"{oldColumnName}AnswerId";
                Alter.Table(WATER_SAMPLE_COMPLIANCE_FORMS).AddColumn(oldColumnName).AsBoolean().Nullable();

                Execute.Sql($@"
    declare @answerNA int; set @answerNA = (select Id from {ANSWER_TYPES_TABLE} where Description = 'N/A')
    declare @answerNo int; set @answerNo = (select Id from {ANSWER_TYPES_TABLE} where Description = 'No')
    declare @answerYes int; set @answerYes = (select Id from {ANSWER_TYPES_TABLE} where Description = 'Yes')

update {WATER_SAMPLE_COMPLIANCE_FORMS} set {oldColumnName} = null where {newColumnName} = @answerNA
update {WATER_SAMPLE_COMPLIANCE_FORMS} set {oldColumnName} = 0 where {newColumnName} = @answerNo
update {WATER_SAMPLE_COMPLIANCE_FORMS} set {oldColumnName} = 1 where {newColumnName} = @answerYes

");
                Delete.ForeignKey($"FK_{WATER_SAMPLE_COMPLIANCE_FORMS}_{ANSWER_TYPES_TABLE}_{newColumnName}")
                      .OnTable(WATER_SAMPLE_COMPLIANCE_FORMS);
                Delete.Column(newColumnName).FromTable(WATER_SAMPLE_COMPLIANCE_FORMS);
            }

            RecreateOldColumnAndDataAndDeleteNewColumn("CentralLabSamplesHaveBeenCollectedAndReported");
            RecreateOldColumnAndDataAndDeleteNewColumn("ContractedLabsSamplesHaveBeenCollectedAndReported");
            RecreateOldColumnAndDataAndDeleteNewColumn("InternalLabsSamplesHaveBeenCollectedAndReported");
            RecreateOldColumnAndDataAndDeleteNewColumn("BactiSamplesHaveBeenCollectedAndReported");
            RecreateOldColumnAndDataAndDeleteNewColumn("LeadAndCopperSamplesHaveBeenCollectedAndReported");
            RecreateOldColumnAndDataAndDeleteNewColumn("WQPSamplesHaveBeenCollectedAndReported");
            RecreateOldColumnAndDataAndDeleteNewColumn("SurfaceWaterPlantSamplesHaveBeenCollectedAndReported");
            RecreateOldColumnAndDataAndDeleteNewColumn("ChlorineResidualsHaveBeenCollectedAndReported");

            Delete.Table(ANSWER_TYPES_TABLE);
        }
    }
}
