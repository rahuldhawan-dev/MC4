using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151005084223295), Tags("Production")]
    public class AddDataTypeForMarkoutTickets : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "INSERT INTO DataType(Data_Type, Table_Name) Values('OneCallMarkoutTicket', 'OneCallMarkoutTickets')");
        }

        public override void Down()
        {
            Execute.Sql(
                "DELETE FROM DataType WHERE Data_Type = 'OneCallMarkoutTicket' AND Table_Name = 'OneCallMarkoutTicketss'");
        }
    }
}
