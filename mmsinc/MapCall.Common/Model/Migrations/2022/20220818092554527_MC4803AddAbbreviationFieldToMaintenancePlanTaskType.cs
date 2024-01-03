using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220818092554527), Tags("Production")]
    public class MC4803AddAbbreviationFieldToMaintenancePlanTaskType : Migration
    {
        public override void Up()
        {
            // The reason this is made nullable: There is a UI where users can create MaintenancePlanTaskTypes, and so there is no guarantee that there won't be
            // records outside the initial "guaranteed" ones created by the MC1846 migration.
            Alter.Table("MaintenancePlanTaskTypes")
                 .AddColumn("Abbreviation").AsAnsiString(4)
                 .Nullable(); 

            Update.Table("MaintenancePlanTaskTypes").Set(new { Abbreviation = "CALB" }).Where(new { Description = "Calibration" });
            Update.Table("MaintenancePlanTaskTypes").Set(new { Abbreviation = "INSP" }).Where(new { Description = "Inspection" });
            Update.Table("MaintenancePlanTaskTypes").Set(new { Abbreviation = "PDM" }).Where(new { Description = "Predictive Maintenance" });
            Update.Table("MaintenancePlanTaskTypes").Set(new { Abbreviation = "PM" }).Where(new { Description = "Preventive Maintenance" });
            Update.Table("MaintenancePlanTaskTypes").Set(new { Abbreviation = "REPL" }).Where(new { Description = "Replacement" });
            Update.Table("MaintenancePlanTaskTypes").Set(new { Abbreviation = "OPS" }).Where(new { Description = "Operational" });
        }

        public override void Down()
        {
            Delete.Column("Abbreviation").FromTable("MaintenancePlanTaskTypes");
        }
    }
}

