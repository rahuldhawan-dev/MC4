using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180320133706143), Tags("Production")]
    public class AlterShortCycleWorkOrdersForWO0000000230579 : Migration
    {
        public override void Up()
        {
            Alter.Column("FSRName").OnTable("ShortCycleWorkOrders")
                 .AsAnsiString().Nullable();
            Alter.Column("WorkCenter").OnTable("ShortCycleWorkOrders")
                 .AsAnsiString(8).Nullable();
            Alter.Column("SafetyConcern").OnTable("ShortCycleWorkOrders")
                 .AsCustom("text").Nullable();
        }

        public override void Down()
        {
            //Noop
        }
    }
}
