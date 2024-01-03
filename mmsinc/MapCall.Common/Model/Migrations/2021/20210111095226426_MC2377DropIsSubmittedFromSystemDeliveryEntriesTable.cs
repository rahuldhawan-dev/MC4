using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210111095226426), Tags("Production")]
    public class MC2377DropIsSubmittedFromSystemDeliveryEntriesTable : Migration
    {
        public override void Up()
        {
            Delete.Column("IsSubmitted").FromTable("SystemDeliveryEntries");
            Alter.Column("MondayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("TuesdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("WednesdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("ThursdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("FridayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("SaturdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("SundayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
        }

        public override void Down()
        {
            Alter.Table("SystemDeliveryEntries").AddColumn("IsSubmitted").AsBoolean();
        }
    }
}