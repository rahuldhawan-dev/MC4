using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181207114109542), Tags("Production")]
    public class MC775_AddNewShortCycleFields : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("Installation").AsAnsiString().Nullable()
                 .AddColumn("DeviceCategory").AsAnsiString()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("DeviceCategory").FromTable("ShortCycle");
            Delete.Column("Installation").FromTable("ShortCycle");
        }
    }
}
