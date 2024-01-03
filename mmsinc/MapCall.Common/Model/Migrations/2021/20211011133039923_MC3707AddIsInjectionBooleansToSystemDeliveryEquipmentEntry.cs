using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211011133039923), Tags("Production")]
    public class MC3707AddIsInjectionBooleansToSystemDeliveryEquipmentEntry : Migration
    {
        public override void Up()
        {
            Alter.Table("SystemDeliveryEquipmentEntries")
                 .AddColumn("IsMondayEntryAnInjection").AsBoolean().Nullable()
                 .AddColumn("IsTuesdayEntryAnInjection").AsBoolean().Nullable()
                 .AddColumn("IsWednesdayEntryAnInjection").AsBoolean().Nullable()
                 .AddColumn("IsThursdayEntryAnInjection").AsBoolean().Nullable()
                 .AddColumn("IsFridayEntryAnInjection").AsBoolean().Nullable()
                 .AddColumn("IsSaturdayEntryAnInjection").AsBoolean().Nullable()
                 .AddColumn("IsSundayEntryAnInjection").AsBoolean().Nullable();
        }

        public override void Down()
        {
            Delete.Column("IsMondayEntryAnInjection").FromTable("SystemDeliveryEquipmentEntries");
            Delete.Column("IsTuesdayEntryAnInjection").FromTable("SystemDeliveryEquipmentEntries");
            Delete.Column("IsWednesdayEntryAnInjection").FromTable("SystemDeliveryEquipmentEntries");
            Delete.Column("IsThursdayEntryAnInjection").FromTable("SystemDeliveryEquipmentEntries");
            Delete.Column("IsFridayEntryAnInjection").FromTable("SystemDeliveryEquipmentEntries");
            Delete.Column("IsSaturdayEntryAnInjection").FromTable("SystemDeliveryEquipmentEntries");
            Delete.Column("IsSundayEntryAnInjection").FromTable("SystemDeliveryEquipmentEntries");
        }
    }
}
