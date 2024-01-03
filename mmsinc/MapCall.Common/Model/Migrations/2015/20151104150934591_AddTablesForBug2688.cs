using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151104150934591), Tags("Production")]
    public class AddTablesForBug2688 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("FacilityProcessStepAlarms");
            this.CreateLookupTableWithValues("FacilityProcessStepTriggerLevels", "Low", "Low-Low", "High", "High-High");
            this.CreateLookupTableWithValues("FacilityProcessStepTriggerTypes", "Operator", "SCADA");

            Create.Table("FacilityProcessStepTriggers")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("Description").AsString(255).NotNullable()
                  .WithColumn("FacilityProcessStepId").AsInt32().NotNullable()
                  .ForeignKey("FK_FacilityProcessStepTriggers_FacilityProcessSteps_FacilityProcessStepId",
                       "FacilityProcessSteps", "Id")
                  .WithColumn("Sequence").AsInt32().NotNullable()
                  .WithColumn("FacilityProcessStepTriggerTypeId").AsInt32().NotNullable()
                  .ForeignKey(
                       "FK_FacilityProcessStepTriggers_FacilityProcessStepTriggerTypes_FacilityProcessStepTriggerTypeId",
                       "FacilityProcessStepTriggerTypes", "Id")
                  .WithColumn("FacilityProcessStepTriggerLevelId").AsInt32().NotNullable()
                  .ForeignKey(
                       "FK_FacilityProcessStepTriggers_FacilityProcessStepTriggerLevels_FacilityProcessStepTriggerLevelId",
                       "FacilityProcessStepTriggerLevels", "Id")
                  .WithColumn("FacilityProcessStepAlarmId").AsInt32().NotNullable()
                  .ForeignKey("FK_FacilityProcessStepTriggers_FacilityProcessStepAlarms_FacilityProcessStepAlarmId",
                       "FacilityProcessStepAlarms", "Id");

            Create.Table("FacilityProcessStepTriggerActions")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("FacilityProcessStepTriggerId").AsInt32().NotNullable()
                  .ForeignKey(
                       "FK_FacilityProcessStepTriggerActions_FacilityProcessStepTriggers_FacilityProcessStepTriggerId",
                       "FacilityProcessStepTriggers", "Id")
                  .WithColumn("Sequence").AsInt32().NotNullable()
                  .WithColumn("Action").AsString(255).NotNullable()
                  .WithColumn("ActionResponse").AsString(255).NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("FK_FacilityProcessStepTriggers_FacilityProcessSteps_FacilityProcessStepId")
                  .OnTable("FacilityProcessStepTriggers");
            Delete
               .ForeignKey(
                    "FK_FacilityProcessStepTriggers_FacilityProcessStepTriggerTypes_FacilityProcessStepTriggerTypeId")
               .OnTable("FacilityProcessStepTriggers");
            Delete
               .ForeignKey(
                    "FK_FacilityProcessStepTriggers_FacilityProcessStepTriggerLevels_FacilityProcessStepTriggerLevelId")
               .OnTable("FacilityProcessStepTriggers");
            Delete.ForeignKey("FK_FacilityProcessStepTriggers_FacilityProcessStepAlarms_FacilityProcessStepAlarmId")
                  .OnTable("FacilityProcessStepTriggers");
            Delete
               .ForeignKey(
                    "FK_FacilityProcessStepTriggerActions_FacilityProcessStepTriggers_FacilityProcessStepTriggerId")
               .OnTable("FacilityProcessStepTriggerActions");

            Delete.Table("FacilityProcessStepTriggerActions");
            Delete.Table("FacilityProcessStepTriggers");
            Delete.Table("FacilityProcessStepTriggerTypes");
            Delete.Table("FacilityProcessStepTriggerLevels");
            Delete.Table("FacilityProcessStepAlarms");
        }
    }
}
