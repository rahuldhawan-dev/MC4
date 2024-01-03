using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210114095001383), Tags("Production")]
    public class MC2378AddSystemDeliveryEntryForeignKeyToSystemDeliveryReversals : Migration
    {
        public override void Up()
        {
            Alter.Table("SystemDeliveryEquipmentEntriesReversals").AddForeignKeyColumn("SystemDeliveryEntryId", "SystemDeliveryEntries");
            // Have to make these nullable here since we have no default values. Not null on the map
            Alter.Table("SystemDeliveryEquipmentEntriesReversals").AddColumn("OriginalEntryValue").AsDecimal(19, 3).Nullable();
            Alter.Table("SystemDeliveryEquipmentEntriesReversals").AddColumn("DateTimeEntered").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SystemDeliveryEquipmentEntriesReversals", "SystemDeliveryEntryId", "SystemDeliveryEntries");
            Delete.Column("OriginalEntryValue").FromTable("SystemDeliveryEquipmentEntriesReversals");
            Delete.Column("DateTimeEntered").FromTable("SystemDeliveryEquipmentEntriesReversals");
        }
    }
}