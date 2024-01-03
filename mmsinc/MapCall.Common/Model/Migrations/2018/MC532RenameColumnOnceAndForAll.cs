using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181211155721642), Tags("Production")]
    public class MC532RenameColumnOnceAndForAll : Migration
    {
        public override void Up()
        {
            Rename.Column("TurnedOffAfterHours").OnTable("MeterChangeOuts").To("MeterTurnedOnAfterHours");
        }

        public override void Down()
        {
            Rename.Column("MeterTurnedOnAfterHours").OnTable("MeterChangeOuts").To("TurnedOffAfterHours");
        }
    }
}
