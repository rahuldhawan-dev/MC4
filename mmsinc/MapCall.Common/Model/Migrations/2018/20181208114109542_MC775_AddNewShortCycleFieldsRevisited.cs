using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181208114109542), Tags("Production")]
    public class Mc775AddNewShortCycleFieldsRevisited : Migration
    {
        public override void Up()
        {
            Delete.Column("DeviceCategory").FromTable("ShortCycleWorkOrders");
            Alter.Table("ShortCycleWorkOrdersEquipmentIds")
                 .AddColumn("DeviceCategory").AsAnsiString()
                 .Nullable();
        }

        public override void Down()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("DeviceCategory").AsAnsiString()
                 .Nullable();
            Delete.Column("DeviceCategory").FromTable("ShortCycleWorkOrdersEquipmentIds");
        }
    }
}
