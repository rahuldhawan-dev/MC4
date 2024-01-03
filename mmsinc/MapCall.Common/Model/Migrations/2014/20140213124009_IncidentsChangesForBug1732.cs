using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140213124009), Tags("Production")]
    public class IncidentsChangesForBug1732 : Migration
    {
        public struct StringLengths
        {
            public const int DECISIONS_DESCRIPTION = 50,
                             RESULT_DESCRIPTION = 50;
        }

        private void CreateLookup(string tableName, params string[] descriptions)
        {
            Create.Table(tableName)
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsString(50).NotNullable().Unique();

            foreach (var d in descriptions)
            {
                Insert.IntoTable(tableName).Row(new {Description = d});
            }
        }

        public override void Up()
        {
            CreateLookup("IncidentDrugAndAlcoholTestingDecisions",
                "TEST - Reasonable Suspicion", // 1
                "TEST - Post Incident-Injury", // 2
                "TEST - Post Incident-Vehicle Accident", // 3
                "TEST - Post Incident-Environmental Release", // 4
                "TEST - Post Incident-Other", // 5
                "NO TEST - Back Injury/Strain", // 6
                "NO TEST - Slip Fall", // 7
                "NO TEST - Car Accident", // 8
                "NO TEST - Other"); // 9

            Alter.Table("Incidents")
                 .AddColumn("IncidentDrugAndAlcoholTestingDecisionId")
                 .AsInt32()
                 .NotNullable()
                 .WithDefaultValue(9)
                 .ForeignKey(
                      "FK_Incidents_IncidentDrugAndAlcoholTestingDecisions_IncidentDrugAndAlcoholTestingDecisionId",
                      "IncidentDrugAndAlcoholTestingDecisions", "Id");

            Update.Table("Incidents")
                  .Set(new {IncidentDrugAndAlcoholTestingDecisionId = 2})
                  .Where(new {IsDrugAndAlcoholTestingRequired = true});

            CreateLookup("IncidentDrugAndAlcoholTestingResults",
                "Not Required", // 1
                "Test Negative", // 2
                "Test Positive"); // 3

            Alter.Table("Incidents")
                 .AddColumn("IncidentDrugAndAlcoholTestingResultId")
                 .AsInt32()
                 .Nullable()
                 .ForeignKey("FK_Incidents_IncidentDrugAndAlcoholTestingResults_IncidentDrugAndAlcoholTestingResultId",
                      "IncidentDrugAndAlcoholTestingResults", "Id");

            Execute.Sql(
                "UPDATE [Incidents] SET [IncidentDrugAndAlcoholTestingResultId] = 1 WHERE [ICRCompletionDate] IS NOT NULL");

            Delete.Column("IsDrugAndAlcoholTestingRequired").FromTable("Incidents");
            Rename.Column("DrugAndAlcoholTestingReason")
                  .OnTable("Incidents")
                  .To("DrugAndAlcoholTestingNotes");

            Alter.Table("Incidents")
                 .AddColumn("PoliceReportFiled")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("PoliceReportFiled").FromTable("Incidents");

            Rename.Column("DrugAndAlcoholTestingNotes")
                  .OnTable("Incidents")
                  .To("DrugAndAlcoholTestingReason");

            Alter.Table("Incidents")
                 .AddColumn("IsDrugAndAlcoholTestingRequired")
                 .AsBoolean()
                 .NotNullable()
                 .WithDefaultValue(false);

            Update.Table("Incidents")
                  .Set(new {IsDrugAndAlcoholTestingRequired = true})
                  .Where(new {IncidentDrugAndAlcoholTestingDecisionId = 2});

            Delete.ForeignKey("FK_Incidents_IncidentDrugAndAlcoholTestingResults_IncidentDrugAndAlcoholTestingResultId")
                  .OnTable("Incidents");
            Delete.Column("IncidentDrugAndAlcoholTestingResultId")
                  .FromTable("Incidents");

            Delete.ForeignKey(
                       "FK_Incidents_IncidentDrugAndAlcoholTestingDecisions_IncidentDrugAndAlcoholTestingDecisionId")
                  .OnTable("Incidents");
            Delete.Column("IncidentDrugAndAlcoholTestingDecisionId")
                  .FromTable("Incidents");

            Delete.Table("IncidentDrugAndAlcoholTestingResults");
            Delete.Table("IncidentDrugAndAlcoholTestingDecisions");
        }
    }
}
