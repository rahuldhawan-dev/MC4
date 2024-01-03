using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20180109153531820), Tags("Production")]
    public class WO0000000208749 : Migration
    {
        public override void Up()
        {
            Create.Column("TurnedOffAfterHours").OnTable("MeterChangeOuts").AsBoolean().Nullable();
            Execute.Sql("Update MeterChangeOuts Set TurnedOffAfterHours = 0");
        }

        public override void Down()
        {
            Delete.Column("TurnedOffAfterHours").FromTable("MeterChangeOuts");
        }
    }
}
