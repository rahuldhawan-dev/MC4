using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190502101520627), Tags("Production")]
    public class MC1249AddMoreColumnsToShortCycleWorkOrder : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("AmountDue").AsDecimal(18, 2).Nullable()
                 .AddColumn("CustomerName").AsAnsiString().Nullable()
                 .AddColumn("ContactNumber").AsAnsiString().Nullable()
                 .AddColumn("DunningLock").AsAnsiString(40).Nullable()
                 .AddColumn("PremiseType").AsAnsiString(40).Nullable()
                 .AddColumn("LandlordAllocation").AsAnsiString(30).Nullable()
                 .AddColumn("LandlordBusinessPartnerNumber")
                 .AsAnsiString().Nullable()
                 .AddColumn("LandlordConnectionContractNumber")
                 .AsAnsiString().Nullable()
                 .AddColumn("ConsecutiveIncompletesOnPremise").AsBoolean().Nullable()
                 .AddColumn("ReplacementMeterFlag").AsBoolean().Nullable()
                 .AddColumn("ReplacementMeterDescription").AsInt32().Nullable()
                 .AddColumn("ServiceLineSize").AsAnsiString()
                 .Nullable()
                 .AddColumn("MeterReadingUnit").AsAnsiString()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("AmountDue").FromTable("ShortCycleWorkOrders");
            Delete.Column("CustomerName").FromTable("ShortCycleWorkOrders");
            Delete.Column("ContactNumber").FromTable("ShortCycleWorkOrders");
            Delete.Column("DunningLock").FromTable("ShortCycleWorkOrders");
            Delete.Column("PremiseType").FromTable("ShortCycleWorkOrders");
            Delete.Column("LandlordAllocation").FromTable("ShortCycleWorkOrders");
            Delete.Column("LandlordBusinessPartnerNumber").FromTable("ShortCycleWorkOrders");
            Delete.Column("LandlordConnectionContractNumber").FromTable("ShortCycleWorkOrders");
            Delete.Column("ConsecutiveIncompletesOnPremise").FromTable("ShortCycleWorkOrders");
            Delete.Column("ReplacementMeterFlag").FromTable("ShortCycleWorkOrders");
            Delete.Column("ReplacementMeterDescription").FromTable("ShortCycleWorkOrders");
            Delete.Column("ServiceLineSize").FromTable("ShortCycleWorkOrders");
            Delete.Column("MeterReadingUnit").FromTable("ShortCycleWorkOrders");
        }
    }
}
