using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210817085835073), Tags("Production")]
    public class MC3562MakeDateFieldsNullableOnSysDelEquipmentEntry : Migration
    {
        public override void Up()
        {
            Alter.Column("MondayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().Nullable();
            Alter.Column("TuesdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().Nullable();
            Alter.Column("WednesdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().Nullable();
            Alter.Column("ThursdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().Nullable();
            Alter.Column("FridayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().Nullable();
            Alter.Column("SaturdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().Nullable();
            Alter.Column("SundayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().Nullable();
        }

        public override void Down()
        {
            Alter.Column("MondayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("TuesdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("WednesdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("ThursdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("FridayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("SaturdayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
            Alter.Column("SundayEntryDate").OnTable("SystemDeliveryEquipmentEntries").AsDate().NotNullable();
        }
    }
}

