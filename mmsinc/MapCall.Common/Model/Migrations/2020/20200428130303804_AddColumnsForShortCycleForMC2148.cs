using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200428130303804), Tags("Production")]
    public class AddColumnsForShortCycleForMC2148 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("ReconnectionFee").AsDecimal(18, 2).Nullable()
                 .AddColumn("ReconnectionFeeWaived").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("ReconnectionFeeWaived").FromTable("ShortCycleWorkOrders");
            Delete.Column("ReconnectionFee").FromTable("ShortCycleWorkOrders");
        }
    }
}
