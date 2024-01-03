using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210423105954695), Tags("Production")]
    public class MC2476UpdateSystemDeliveryEquipmentEntriesToFiveDecimalPlaces : Migration
    {
        public override void Up()
        {
            Alter.Column("MondayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 5).Nullable();
            Alter.Column("TuesdayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 5).Nullable();
            Alter.Column("WednesdayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 5).Nullable();
            Alter.Column("ThursdayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 5).Nullable();
            Alter.Column("FridayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 5).Nullable();
            Alter.Column("SaturdayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 5).Nullable();
            Alter.Column("SundayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 5).Nullable();
            Alter.Column("WeeklyTotal").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 5).Nullable();
            Alter.Column("ReversalEntryValue").OnTable("SystemDeliveryEquipmentEntriesReversals").AsDecimal(19, 5).NotNullable();
            Alter.Column("OriginalEntryValue").OnTable("SystemDeliveryEquipmentEntriesReversals").AsDecimal(19, 5).Nullable();
        }

        public override void Down()
        {
            Alter.Column("MondayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 3).Nullable();
            Alter.Column("TuesdayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 3).Nullable();
            Alter.Column("WednesdayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 3).Nullable();
            Alter.Column("ThursdayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 3).Nullable();
            Alter.Column("FridayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 3).Nullable();
            Alter.Column("SaturdayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 3).Nullable();
            Alter.Column("SundayEntry").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 3).Nullable();
            Alter.Column("WeeklyTotal").OnTable("SystemDeliveryEquipmentEntries").AsDecimal(19, 3).Nullable();
            Alter.Column("ReversalEntryValue").OnTable("SystemDeliveryEquipmentEntriesReversals").AsDecimal(19, 3).NotNullable();
            Alter.Column("OriginalEntryValue").OnTable("SystemDeliveryEquipmentEntriesReversals").AsDecimal(19, 3).Nullable();
        }
    }
}

