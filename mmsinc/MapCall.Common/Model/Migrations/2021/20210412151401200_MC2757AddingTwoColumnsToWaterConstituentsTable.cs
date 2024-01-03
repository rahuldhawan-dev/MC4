using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210412151401200)]
    [Tags("Production")]
    public class MC2757AddingTwoColumnsToWaterConstituentsTable : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("DrinkingWaterContaminantCategories", "Inorganic Chemicals",
                "Organic Chemicals", "Disinfectants & Disinfection By Product", "Microbiological Contaminants",
                "Secondary Standards", "Emerging Contaminant");
            this.CreateLookupTableWithValues("WasteWaterContaminantCategories", "Nutrients", "BOD", "Petroleum",
                "Emerging Contaminant");

            Alter.Table("WaterConstituents")
                 .AddForeignKeyColumn("DrinkingWaterContaminantCategoryId", "DrinkingWaterContaminantCategories")
                 .AddForeignKeyColumn("WasteWaterContaminantCategoryId", "WasteWaterContaminantCategories");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WaterConstituents", "DrinkingWaterContaminantCategoryId", "DrinkingWaterContaminantCategories");
            Delete.ForeignKeyColumn("WaterConstituents", "WasteWaterContaminantCategoryId", "WasteWaterContaminantCategories");

            Delete.Table("WasteWaterContaminantCategories");
            Delete.Table("DrinkingWaterContaminantCategories");
        }
    }
}