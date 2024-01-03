using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20141210113830682), Tags("Production")]
    public class AddIndexForOneCallTicketsTableBug2249 : Migration
    {
        private const string INDEX_NAME = "IDX_RequestNum_TransmitDate_State_Town_Street_CompleteDateStr";

        // NOTE: THIS MIGRATION IS SLOW, ESPECIALLY ON SQL2000BACKUP
        public override void Up()
        {
            Create.Index(INDEX_NAME).OnTable("OneCallTickets")
                  .OnColumn("RequestNum").Ascending()
                  .OnColumn("TransmitDate").Ascending()
                  .OnColumn("CompleteDateStr").Ascending()
                  .OnColumn("State").Ascending()
                  .OnColumn("Town").Ascending()
                  .OnColumn("Street").Ascending();
        }

        public override void Down()
        {
            Delete.Index(INDEX_NAME).OnTable("OneCallTickets");
        }
    }
}
