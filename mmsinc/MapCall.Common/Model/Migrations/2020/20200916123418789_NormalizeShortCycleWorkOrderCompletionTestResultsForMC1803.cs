using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916123418789), Tags("Production")]
    public class NormalizeShortCycleWorkOrderCompletionTestResultsForMC1803 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderCompletionTestResults SET LowMedHighIndicator = NULL WHERE LTRIM(RTRIM(LowMedHighIndicator)) = '';");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderCompletionTestResults SET InitialRepair = NULL WHERE LTRIM(RTRIM(InitialRepair)) = '';");

            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletionTestResults", "LowMedHighIndicator",
                "ShortCycleWorkOrderCompletionTestResultLowMedHighIndicators", 6,
                newColumnName: "LowMedHighIndicatorId");
            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletionTestResults", "InitialRepair",
                "ShortCycleWorkOrderCompletionTestResultInitialRepairs", 2, newColumnName: "InitialRepairId");
        }

        public override void Down()
        {
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletionTestResults", "LowMedHighIndicator",
                "ShortCycleWorkOrderCompletionTestResultLowMedHighIndicators", 6,
                newColumnName: "LowMedHighIndicatorId");
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletionTestResults", "InitialRepair",
                "ShortCycleWorkOrderCompletionTestResultInitialRepairs", 2, newColumnName: "InitialRepairId");
        }
    }
}
