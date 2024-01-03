using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201109101609957), Tags("Production")]
    public class MC2717CreateSafetyBriefFormForW1V : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("ShortCycleWorkOrderSafetyBriefLocationTypes", "Meter Reading",
                "FSR Work (Residence)",
                "FSR Work (Businesses)", "Vault(s)", "Booster Station(s)");
            this.CreateLookupTableWithValues("ShortCycleWorkOrderSafetyBriefHazardTypes", "Slips, Trips, Falls",
                "Cuts/Abrasions", "Ergonomic/Lifting", "Traffic", "Tools",
                "Electrical Grounding", "Limited Workspace", "Weather/Lighting", "Heat/Cold Stress", "Poisonous Plants",
                "Animals/Insects", "Confined Space", "Ladder Safety", "Pandemic Precaution", "Other");
            this.CreateLookupTableWithValues("ShortCycleWorkOrderSafetyBriefPPETypes", "Hardhat", "Hearing Protection",
                "Class III Apparel", "Harness", "Gloves", "Safety-Toe Shoes", "00 Electrical Gloves", "Other");
            this.CreateLookupTableWithValues("ShortCycleWorkOrderSafetyBriefToolTypes", "Hand Tools", "Pump/Vaccum",
                "Multimeter", "Air Monitor", "Tripod", "Meter Reading Equipment", "Other");

            Create.Table("ShortCycleWorkOrderSafetyBriefs")
                  .WithIdentityColumn()
                  .WithColumn("DateCompleted").AsDateTime().NotNullable()
                  .WithForeignKeyColumn("FSRId", "tblEmployee", "tblEmployeeId", false)
                  .WithColumn("IsPPEInGoodCondition").AsBoolean().NotNullable()
                  .WithColumn("HasCompletedDailyStretchingRoutine").AsBoolean().NotNullable()
                  .WithColumn("HasPerformedInspectionOnVehicle").AsBoolean().NotNullable();

            Create.Table("ShortCycleWorkOrderSafetyBriefLocations")
                  .WithForeignKeyColumn("ShortCycleWorkOrderSafetyBriefId", "ShortCycleWorkOrderSafetyBriefs").Indexed()
                  .WithForeignKeyColumn("SafetyBriefLocationTypeId", "ShortCycleWorkOrderSafetyBriefLocationTypes");

            Create.Table("ShortCycleWorkOrderSafetyBriefHazards")
                  .WithForeignKeyColumn("ShortCycleWorkOrderSafetyBriefId", "ShortCycleWorkOrderSafetyBriefs").Indexed()
                  .WithForeignKeyColumn("SafetyBriefHazardTypeId", "ShortCycleWorkOrderSafetyBriefHazardTypes");

            Create.Table("ShortCycleWorkOrderSafetyBriefPPE")
                  .WithForeignKeyColumn("ShortCycleWorkOrderSafetyBriefId", "ShortCycleWorkOrderSafetyBriefs").Indexed()
                  .WithForeignKeyColumn("SafetyBriefPPETypeId", "ShortCycleWorkOrderSafetyBriefPPETypes");

            Create.Table("ShortCycleWorkOrderSafetyBriefTools")
                  .WithForeignKeyColumn("ShortCycleWorkOrderSafetyBriefId", "ShortCycleWorkOrderSafetyBriefs").Indexed()
                  .WithForeignKeyColumn("SafetyBriefToolTypeId", "ShortCycleWorkOrderSafetyBriefToolTypes");

            this.AddNotificationType("Field Services", "Short Cycle", "Short Cycle Safety Brief");
        }

        public override void Down()
        {
            Delete.Table("ShortCycleWorkOrderSafetyBriefLocations");
            Delete.Table("ShortCycleWorkOrderSafetyBriefHazards");
            Delete.Table("ShortCycleWorkOrderSafetyBriefPPE");
            Delete.Table("ShortCycleWorkOrderSafetyBriefTools");
            Delete.Table("ShortCycleWorkOrderSafetyBriefs");
            Delete.Table("ShortCycleWorkOrderSafetyBriefLocationTypes");
            Delete.Table("ShortCycleWorkOrderSafetyBriefHazardTypes");
            Delete.Table("ShortCycleWorkOrderSafetyBriefPPETypes");
            Delete.Table("ShortCycleWorkOrderSafetyBriefToolTypes");

            this.RemoveNotificationType("Field Services", "Short Cycle", "Short Cycle Safety Brief");
        }
    }
}
