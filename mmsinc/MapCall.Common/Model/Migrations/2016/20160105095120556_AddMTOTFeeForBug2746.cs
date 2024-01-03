using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160105095120556), Tags("Production")]
    public class AddMTOTFeeForBug2746 : Migration
    {
        public override void Up()
        {
            Alter.Table("TrafficControlTickets").AddColumn("MTOTFee").AsDecimal().Nullable();
        }

        public override void Down()
        {
            Delete.Column("MTOTFee").FromTable("TrafficControlTickets");
        }
    }
}
