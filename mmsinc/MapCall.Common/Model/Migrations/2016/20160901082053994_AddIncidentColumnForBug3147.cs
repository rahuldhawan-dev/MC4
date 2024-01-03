using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160901082053994), Tags("Production")]
    public class AddIncidentColumnForBug3147 : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents").AddColumn("NumberOfHoursOvertimeInPastWeek").AsDecimal(4, 2).Nullable();
        }

        public override void Down()
        {
            Delete.Column("NumberOfHoursOvertimeInPastWeek").FromTable("Incidents");
        }
    }
}
