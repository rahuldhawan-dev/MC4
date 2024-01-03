using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20181004121144033), Tags("Production")]
    public class RenameFieldsForMC602Work1V : Migration
    {
        public override void Up()
        {
            Rename.Column("FunctionalLocation").OnTable("ShortCycleWorkOrderRequests").To("DeviceLocation");
            Rename.Column("Equipment").OnTable("ShortCycleWorkOrderRequests").To("EquipmentNumber");
            Rename.Column("ManufacturerSerialNumber").OnTable("ShortCycleWorkOrderRequests").To("SerialNumber");
            Rename.Column("WorkOrderLongText").OnTable("ShortCycleWorkOrderRequests").To("WorkDescription");
        }

        public override void Down()
        {
            Rename.Column("DeviceLocation").OnTable("ShortCycleWorkOrderRequests").To("FunctionalLocation");
            Rename.Column("EquipmentNumber").OnTable("ShortCycleWorkOrderRequests").To("Equipment");
            Rename.Column("SerialNumber").OnTable("ShortCycleWorkOrderRequests").To("ManufacturerSerialNumber");
            Rename.Column("WorkDescription").OnTable("ShortCycleWorkOrderRequests").To("WorkOrderLongText");
        }
    }
}
