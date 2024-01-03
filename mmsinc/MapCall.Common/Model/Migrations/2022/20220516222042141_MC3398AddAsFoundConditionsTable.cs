using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220516222042141), Tags("Production")]
    public class MC3398AddAsFoundConditionsTable : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("AsFoundConditions", "Unable to Inspect", "Serious Deterioration",
                "Some Deterioration", "Questionable", "Acceptable / Good");
        }
        public override void Down()
        {
            Delete.Table("AsFoundConditions");
        }
    }
}