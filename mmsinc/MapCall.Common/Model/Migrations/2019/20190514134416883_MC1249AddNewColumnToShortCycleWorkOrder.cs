using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190514134416883), Tags("Production")]
    public class MC1249AddNewColumnToShortCycleWorkOrder : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AddColumn("LeakDetectedLastVisit").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("LeakDetectedLastVisit").FromTable("ShortCycleWorkOrders");
        }
    }
}
