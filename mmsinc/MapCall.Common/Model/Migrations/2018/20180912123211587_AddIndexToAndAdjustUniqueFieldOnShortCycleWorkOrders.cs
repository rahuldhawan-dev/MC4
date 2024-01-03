using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180912123211587), Tags("Production")]
    public class AddIndexToAndAdjustUniqueFieldOnShortCycleWorkOrders : Migration
    {
        public override void Up()
        {
            Execute.Sql(
                @"CREATE NONCLUSTERED INDEX IX_ShortCycleWorkOrders_Id_WorkOrder ON ShortCycleWorkOrders (Id, WorkOrder)");
            Alter.Column("WorkOrder").OnTable("ShortCycleWorkOrders").AsAnsiString(40)
                 .Unique("UIX_ShortCycleWorkOrders_WorkOrder").Nullable();
        }

        public override void Down()
        {
            Delete.Index("IX_ShortCycleWorkOrders_Id_WorkOrder").OnTable("ShortCycleWorkOrders");
            Delete.Index("UIX_ShortCycleWorkOrders_WorkOrder").OnTable("ShortCycleWorkOrders");
        }
    }
}
