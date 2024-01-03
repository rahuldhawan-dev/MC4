using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160620104705367), Tags("Production")]
    public class CleanUpAndNormalizeSampleResultsComplaintsForBug2941 : Migration
    {
        #region Constants

        public const string TABLE_NAME = "WaterQualityComplaintSampleResults";

        #endregion

        #region Exposed Methods

        public override void Up()
        {
            var oldName = "tblWQSample_Results_Complaints";

            Rename.Column("SampleId").OnTable(oldName).To("Id");
            Rename.Column("WQ_Complaint_Number").OnTable(oldName).To("ComplaintId");
            Rename.Column("WaterConstituent_Id").OnTable(oldName).To("WaterConstituentId");
            Rename.Column("Sample_Date").OnTable(oldName).To("SampleDate");
            Rename.Column("Sample_Value").OnTable(oldName).To("SampleValue");
            Rename.Column("Analysis_Performed_By").OnTable(oldName).To("AnalysisPerformedBy");

            Rename.Table(oldName).To(TABLE_NAME);

            Execute.Sql($"DELETE FROM {TABLE_NAME}");

            Alter.Column("ComplaintId")
                 .OnTable(TABLE_NAME)
                 .AsInt32()
                 .NotNullable()
                 .ForeignKey(
                      $"FK_{TABLE_NAME}_{CleanUpAndNormalizeWQComplaintsAndSuchForBug2941.TABLE_NAME}_ComplaintId",
                      CleanUpAndNormalizeWQComplaintsAndSuchForBug2941.TABLE_NAME, "Id");
        }

        public override void Down()
        {
            var oldName = "tblWQSample_Results_Complaints";

            Delete.ForeignKey(
                       $"FK_{TABLE_NAME}_{CleanUpAndNormalizeWQComplaintsAndSuchForBug2941.TABLE_NAME}_ComplaintId")
                  .OnTable(TABLE_NAME);

            Rename.Table(TABLE_NAME).To(oldName);

            Rename.Column("Id").OnTable(oldName).To("SampleId");
            Rename.Column("ComplaintId").OnTable(oldName).To("WQ_Complaint_Number");
            Rename.Column("WaterConstituentId").OnTable(oldName).To("WaterConstituent_Id");
            Rename.Column("SampleDate").OnTable(oldName).To("Sample_Date");
            Rename.Column("SampleValue").OnTable(oldName).To("Sample_Value");
            Rename.Column("AnalysisPerformedBy").OnTable(oldName).To("Analysis_Performed_By");
        }

        #endregion
    }
}
