using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160111140614573), Tags("Production")]
    public class FixedDecimalPlacesInTrafficControlTicketsForBug2748 : Migration
    {
        public override void Up()
        {
            Alter.Column("MTOTFee").OnTable("TrafficControlTickets").AsDecimal(7, 2).Nullable();
        }

        public override void Down()
        {
            Alter.Column("MTOTFee").OnTable("TrafficControlTickets").AsDecimal(19, 5).Nullable();
        }
    }
}
