using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230213140602668), Tags("Production")]
    public class MC4852_AddAsBuiltCompletedToWorkOrders : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Column("DigitalAsBuiltCompleted").OnTable("WorkOrders").AsBoolean().Nullable();
        }
    }
}

