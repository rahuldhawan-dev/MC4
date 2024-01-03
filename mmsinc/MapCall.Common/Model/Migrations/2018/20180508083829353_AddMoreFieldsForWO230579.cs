using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180508083829353), Tags("Production")]
    public class AddMoreFieldsForWO230579 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderCompletions").AddColumn("OldMeterSerialNumber")
                 .AsAnsiString().Nullable();
            Alter.Table("ShortCycleWorkOrderCompletionRegisters").AddColumn("Dials")
                 .AsAnsiString().Nullable();
        }

        public override void Down()
        {
            Delete.Column("OldMeterSerialNumber").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("Dials").FromTable("ShortCycleWorkOrderCompletionRegisters");
        }
    }
}
