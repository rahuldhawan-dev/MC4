using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140910141958266), Tags("Production")]
    public class SplitOutLookupTablesForJobObservationsBug2092 : Migration
    {
        public struct TableNames
        {
            public const string JOB_OBSERVATIONS = "tblJobObservations",
                                JOB_CATEGORIES = "JobCategories",
                                OVERALL_QUALITY_RATINGS = "OverallQualityRatings",
                                OVERALL_SAFETY_RATINGS = "OverallSafetyRatings";
        }

        public struct ColumnNames
        {
            public const string JOB_CATEGORY = "JobCategory",
                                OVERALL_QUALITY_RATING = "OverallQualityRating",
                                OVERALL_SAFETY_RATING = "OverallSafetyRating";
        }

        public override void Up()
        {
            // Job Category
            this.ExtractLookupTableLookup(
                TableNames.JOB_OBSERVATIONS,
                ColumnNames.JOB_CATEGORY,
                TableNames.JOB_CATEGORIES,
                50,
                ColumnNames.JOB_CATEGORY);

            // Overall Quality Rating
            this.ExtractLookupTableLookup(
                TableNames.JOB_OBSERVATIONS,
                ColumnNames.OVERALL_QUALITY_RATING,
                TableNames.OVERALL_QUALITY_RATINGS,
                50,
                ColumnNames.OVERALL_QUALITY_RATING);
            // Overall Safety Rating
            this.ExtractLookupTableLookup(
                TableNames.JOB_OBSERVATIONS,
                ColumnNames.OVERALL_SAFETY_RATING,
                TableNames.OVERALL_SAFETY_RATINGS,
                50,
                ColumnNames.OVERALL_SAFETY_RATING);

            Execute.Sql(@"
                ALTER TABLE [dbo].[tblJobObservations] DROP CONSTRAINT [FK_tblJobObservations_Lookup_JobCategory]
                ALTER TABLE [dbo].[tblJobObservations] DROP CONSTRAINT [FK_tblJobObservations_Lookup_OverallQualityRating]
                ALTER TABLE [dbo].[tblJobObservations] DROP CONSTRAINT [FK_tblJobObservations_Lookup_OverallSafetyRating]");
        }

        public override void Down()
        {
            this.ReplaceLookupTableLookup(
                TableNames.JOB_OBSERVATIONS,
                ColumnNames.JOB_CATEGORY,
                TableNames.JOB_CATEGORIES,
                50,
                ColumnNames.JOB_CATEGORY);
            this.ReplaceLookupTableLookup(
                TableNames.JOB_OBSERVATIONS,
                ColumnNames.OVERALL_QUALITY_RATING,
                TableNames.OVERALL_QUALITY_RATINGS,
                50,
                ColumnNames.OVERALL_QUALITY_RATING);
            this.ReplaceLookupTableLookup(
                TableNames.JOB_OBSERVATIONS,
                ColumnNames.OVERALL_SAFETY_RATING,
                TableNames.OVERALL_SAFETY_RATINGS,
                50,
                ColumnNames.OVERALL_SAFETY_RATING);

            Execute.Sql(@"
                ALTER TABLE [dbo].[tblJobObservations]  WITH NOCHECK ADD CONSTRAINT [FK_tblJobObservations_Lookup_JobCategory] FOREIGN KEY([JobCategory])
                REFERENCES [dbo].[Lookup] ([LookupID])
                ALTER TABLE [dbo].[tblJobObservations] CHECK CONSTRAINT [FK_tblJobObservations_Lookup_JobCategory]

                ALTER TABLE [dbo].[tblJobObservations]  WITH NOCHECK ADD CONSTRAINT [FK_tblJobObservations_Lookup_OverallQualityRating] FOREIGN KEY([OverallQualityRating])
                REFERENCES [dbo].[Lookup] ([LookupID])
                ALTER TABLE [dbo].[tblJobObservations] CHECK CONSTRAINT [FK_tblJobObservations_Lookup_OverallQualityRating]

                ALTER TABLE [dbo].[tblJobObservations]  WITH NOCHECK ADD CONSTRAINT [FK_tblJobObservations_Lookup_OverallSafetyRating] FOREIGN KEY([OverallSafetyRating])
                REFERENCES [dbo].[Lookup] ([LookupID])
                ALTER TABLE [dbo].[tblJobObservations] CHECK CONSTRAINT [FK_tblJobObservations_Lookup_OverallSafetyRating]                ");
        }
    }
}
