using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140716150418467), Tags("Production")]
    public class AddIncidentReportedDateTimeToIncidentsForBug1989 : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents").AddColumn("IncidentReportedDate").AsDateTime().Nullable();
            Execute.Sql("UPDATE [Incidents] SET [IncidentReportedDate] = [CreatedOn]");
            Alter.Column("IncidentReportedDate").OnTable("Incidents").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("IncidentReportedDate").FromTable("Incidents");
        }
    }
}
