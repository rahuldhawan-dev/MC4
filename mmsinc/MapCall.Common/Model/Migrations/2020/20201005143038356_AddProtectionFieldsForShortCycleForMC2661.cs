using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201005143038356), Tags("Production")]
    public class AddProtectionFieldsForShortCycleForMC2661 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("WaterLineProtection").AsBoolean().Nullable()
                 .AddColumn("SewerLineProtection").AsBoolean().Nullable()
                 .AddColumn("InHomeProtection").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("WaterLineProtection").FromTable("ShortCycleWorkOrders");
            Delete.Column("SewerLineProtection").FromTable("ShortCycleWorkOrders");
            Delete.Column("InHomeProtection").FromTable("ShortCycleWorkOrders");
        }
    }
}
