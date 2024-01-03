using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190819145536916), Tags("Production")]
    public class MC1583IncreaseReasonCodeLength : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AlterColumn("ReasonCode").AsAnsiString(35).Nullable();
        }

        public override void Down()
        {
            // noop
        }
    }
}
