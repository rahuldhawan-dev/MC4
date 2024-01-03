using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190910123813836), Tags("Production")]
    public class AddPrivateValveBillingForMC409 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
set IDENTITY_INSERT ValveBillings ON;
INSERT INTO ValveBillings (Id, Description) VALUES (5, 'PRIVATE');
set IDENTITY_INSERT ValveBillings OFF;");
        }

        public override void Down()
        {
            Delete.FromTable("ValveBillings").Row(new {Description = "PRIVATE"});
        }
    }
}
