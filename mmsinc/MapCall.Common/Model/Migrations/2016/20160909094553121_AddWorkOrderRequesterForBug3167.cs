using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160909094553121), Tags("Production")]
    public class AddWorkOrderRequesterForBug3167 : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                "IF NOT EXISTS (SELECT 1 FROM WorkOrderRequesters WHERE description = 'Echologics') INSERT INTO WorkOrderRequesters Values('Echologics');");
        }

        public override void Down() { }
    }
}
