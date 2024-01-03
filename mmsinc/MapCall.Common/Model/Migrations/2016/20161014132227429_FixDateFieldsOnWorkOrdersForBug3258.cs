using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161014132227429), Tags("Production")]
    public class FixDateReceivedOnWorkOrdersForBug3258 : Migration
    {
        public override void Up()
        {
            Execute.Sql("UPDATE WorkOrders SET DateReceived = DATEADD(dd, DATEDIFF(dd, 0, DateReceived), 0)");
        }

        public override void Down() { }
    }
}
