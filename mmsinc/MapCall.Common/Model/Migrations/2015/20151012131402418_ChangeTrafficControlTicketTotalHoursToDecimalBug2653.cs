using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151012131402418), Tags("Production")]
    public class ChangeTrafficControlTicketTotalHoursToDecimalBug2653 : Migration
    {
        public override void Up()
        {
            Alter.Column("TotalHours").OnTable("TrafficControlTickets").AsDecimal(5, 2).NotNullable();
        }

        public override void Down()
        {
            Alter.Column("TotalHours").OnTable("TrafficControlTickets").AsInt32().NotNullable();
        }
    }
}
