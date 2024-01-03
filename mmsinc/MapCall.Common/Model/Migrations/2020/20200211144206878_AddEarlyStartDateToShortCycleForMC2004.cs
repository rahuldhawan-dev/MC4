using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200211144206878), Tags("Production")]
    public class AddEarlyStartDateToShortCycleForMC2004 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders").AddColumn("EarlyStartDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("EarlyStartDate").FromTable("ShortCycleWorkOrders");
        }
    }
}
