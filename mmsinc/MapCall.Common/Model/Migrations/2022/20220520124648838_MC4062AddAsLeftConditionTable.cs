using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220520124648838), Tags("Production")]
    public class MC4062AddAsLeftConditionTable : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("AsLeftConditions", "Unable to Inspect", "Needs Emergency Repair",
                "Needs Repair", "Needs Re-Inspection", "Needs Re-Inspection Sooner than Normal", "Acceptable / Good");
        }
        public override void Down()
        {
            Delete.Table("AsLeftConditions");
        }
    }
}