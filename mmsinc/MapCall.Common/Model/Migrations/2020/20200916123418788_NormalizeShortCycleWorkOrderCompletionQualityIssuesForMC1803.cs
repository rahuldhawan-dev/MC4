using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916123418788), Tags("Production")]
    public class NormalizeShortCycleWorkOrderCompletionQualityIssuesForMC1803 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "DELETE FROM ShortCycleWorkOrderCompletionsQualityIssues WHERE LTRIM(RTRIM(QualityIssue)) = '';");

            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletionsQualityIssues", "QualityIssue",
                "ShortCycleWorkOrderCompletionQualityIssueDescriptions", 3, newColumnName: "QualityIssueId");

            Alter.Table("ShortCycleWorkOrderCompletionsQualityIssues").AlterColumn("ShortCycleWorkOrderCompletionId")
                 .AsInt32().NotNullable();
        }

        public override void Down()
        {
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletionsQualityIssues", "QualityIssue",
                "ShortCycleWorkOrderCompletionQualityIssueDescriptions", 3, newColumnName: "QualityIssueId");
        }
    }
}
