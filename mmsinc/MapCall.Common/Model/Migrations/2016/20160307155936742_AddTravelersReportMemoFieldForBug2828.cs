using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160307155936742), Tags("Production")]
    public class AddTravelersReportMemoFieldForBug2828 : Migration
    {
        public override void Up()
        {
            Alter.Table("Incidents").AddColumn("TravelersReport").AsCustom("text").Nullable();
        }

        public override void Down()
        {
            Delete.Column("TravelersReport").FromTable("Incidents");
        }
    }
}
