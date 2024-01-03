using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210216145655419), Tags("Production")]
    public class MC2973AddingIncidentSummaryToIncidents : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents").AddColumn("IncidentSummary").AsString(130).Nullable();
        }

        public override void Down()
        {
            Delete.Column("IncidentSummary").FromTable("Incidents");
        }
    }
}