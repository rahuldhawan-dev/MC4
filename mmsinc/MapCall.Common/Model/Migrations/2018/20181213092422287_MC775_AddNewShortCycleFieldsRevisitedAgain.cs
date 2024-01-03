using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181213092422287), Tags("Production")]
    public class MC775_AddNewShortCycleFieldsRevisitedAgain : Migration
    {
        public override void Up()
        {
            //Equipment.DeviceLocation Char 30
            //Equipment.Installation Char 10
            //Equipment.ServiceType Char 2
            //Equipment.InstallationType Char 4
            //Order.FixedChargeNoMeter Char 5
            Alter.Table("ShortCycleWorkOrdersEquipmentIds")
                 .AddColumn("DeviceLocation").AsAnsiString()
                 .Nullable()
                 .AddColumn("Installation").AsAnsiString()
                 .Nullable()
                 .AddColumn("ServiceType").AsAnsiString(2).Nullable()
                 .AddColumn("InstallationType").AsAnsiString(4).Nullable();
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("FixedChargeNoMeter").AsAnsiString(5).Nullable();
        }

        public override void Down()
        {
            Delete.Column("FixedChargeNoMeter").FromTable("ShortCycleWorkOrders");
            Delete.Column("InstallationType").FromTable("ShortCycleWorkOrdersEquipmentIds");
            Delete.Column("ServiceType").FromTable("ShortCycleWorkOrdersEquipmentIds");
            Delete.Column("Installation").FromTable("ShortCycleWorkOrdersEquipmentIds");
            Delete.Column("DeviceLocation").FromTable("ShortCycleWorkOrdersEquipmentIds");
        }
    }
}
