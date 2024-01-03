using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220621102924053), Tags("Production")]
    public class MC3099_AddNewFieldsToMainCrossings : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("TypicalOperatingPressureRanges", 100, 
                "Less than 40 PSI", 
                "40-80 PSI", 
                "80-120 PSI", 
                "Greater than 120 PSI",
                "Needs Review");

            this.CreateLookupTableWithValues("PressureSurgePotentialTypes", 100,
                "Yes - within 500 ft of suction or discharge of pump station",
                "Yes - closed gradient with no floating storage",
                "No, needs review");

            Alter.Table("MainCrossings").AddForeignKeyColumn("TypicalOperatingPressureRangeId", "TypicalOperatingPressureRanges");
            Alter.Table("MainCrossings").AddForeignKeyColumn("PressureSurgePotentialTypeId", "PressureSurgePotentialTypes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MainCrossings", "PressureSurgePotentialTypeId", "PressureSurgePotentialTypes");
            Delete.ForeignKeyColumn("MainCrossings", "TypicalOperatingPressureRangeId", "TypicalOperatingPressureRanges");

            Delete.Table("PressureSurgePotentialTypes");
            Delete.Table("TypicalOperatingPressureRanges");
        }
    }
}

