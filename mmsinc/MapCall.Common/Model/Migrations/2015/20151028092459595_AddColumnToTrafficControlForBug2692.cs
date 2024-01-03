using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151028092459595), Tags("Production")]
    public class AddColumnToTrafficControlForBug2692 : Migration
    {
        public override void Up()
        {
            Alter.Table("TrafficControlTickets").AddColumn("PaidByNJAW").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
        }

        public override void Down()
        {
            Delete.Column("PaidByNJAW").FromTable("TrafficControlTickets");
        }
    }
}
