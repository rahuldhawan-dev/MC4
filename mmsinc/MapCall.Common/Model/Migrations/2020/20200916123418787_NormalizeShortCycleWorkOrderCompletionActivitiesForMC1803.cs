using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200916123418787), Tags("Production")]
    public class NormalizeShortCycleWorkOrderCompletionActivitiesForMC1803 : Migration
    {
        public override void Up()
        {
            Execute.Sql("DELETE FROM ShortCycleWorkOrderCompletionsActivities WHERE LTRIM(RTRIM(Description)) = '';");

            this.ExtractNonLookupTableLookup("ShortCycleWorkOrderCompletionsActivities", "Description",
                "ShortCycleWorkOrderCompletionActivityDescriptions", 4, newColumnName: "DescriptionId");

            Alter.Table("ShortCycleWorkOrderCompletionsActivities").AlterColumn("ShortCycleWorkOrderCompletionId")
                 .AsInt32().NotNullable();
        }

        public override void Down()
        {
            this.ReplaceNonLookupTableLookup("ShortCycleWorkOrderCompletionsActivities", "Description",
                "ShortCycleWorkOrderCompletionActivityDescriptions", 4, newColumnName: "DescriptionId");
        }
    }
}
