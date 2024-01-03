using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20231112215031558), Tags("Production")]
    public class MC5518_AddWorkOrderSpecialInstructionsColumn : Migration
    {
        public override void Up()
        {
            Execute.Sql("ALTER TABLE WorkOrders ADD SpecialInstructions TEXT");
        }

        public override void Down()
        {
            Execute.Sql("ALTER TABLE WorkOrders DROP Column SpecialInstructions");
        }
    }
}

