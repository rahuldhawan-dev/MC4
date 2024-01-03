using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220526084653725), Tags("Production")]
    public class MC4037AlterMaintenacePlansTableAddColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("MaintenancePlans").AddForeignKeyColumn("StateId", "States", "stateId");
            Alter.Table("MaintenancePlans").AddColumn("Name").AsAnsiString(50).NotNullable().WithDefaultValue("");
            Alter.Table("MaintenancePlans").AddForeignKeyColumn("FacilityId", "tblFacilities", "RecordId");
            Alter.Table("MaintenancePlans").AddColumn("HasACompletionRequirement").AsBoolean().NotNullable().WithDefaultValue(false);
            Alter.Table("MaintenancePlans").AddColumn("ForecastPeriodDaysMultiplier").AsInt32().NotNullable().WithDefaultValue(365);
            Alter.Table("MaintenancePlans").AddColumn("IsPlanPaused").AsBoolean().NotNullable().WithDefaultValue(false);
            Alter.Table("MaintenancePlans").AddColumn("PausedPlanNotes").AsAnsiString(250).Nullable();
            Alter.Table("MaintenancePlans").AddColumn("PausedPlanResumeDate").AsDate().Nullable();
            Alter.Table("MaintenancePlans").AddColumn("AdditionalTaskDetails").AsAnsiString(500).Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("MaintenancePlans", "StateId", "States", "stateId");
            Delete.Column("Name").FromTable("MaintenancePlans");
            Delete.ForeignKeyColumn("MaintenancePlans", "FacilityId", "tblFacilities", "RecordId");
            Delete.Column("HasACompletionRequirement").FromTable("MaintenancePlans");
            Delete.Column("ForecastPeriodDaysMultiplier").FromTable("MaintenancePlans");
            Delete.Column("IsPlanPaused").FromTable("MaintenancePlans");
            Delete.Column("PausedPlanNotes").FromTable("MaintenancePlans");
            Delete.Column("PausedPlanResumeDate").FromTable("MaintenancePlans");
            Delete.Column("AdditionalTaskDetails").FromTable("MaintenancePlans");
        }
    }
}

