using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201217083009935), Tags("Production")]
    public class MC2378AddTableForReversals : Migration
    {
        public override void Up()
        {
            Create.Table("SystemDeliveryEquipmentEntriesReversals")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("SystemDeliveryEquipmentEntryId", "SystemDeliveryEquipmentEntries")
                  .WithForeignKeyColumn("EnteredById", "tblEmployee", "tblEmployeeId")
                  .WithColumn("DateForReversal").AsDate()
                  .WithColumn("ReversalEntryValue").AsDecimal(19, 3);
        }

        public override void Down()
        {
            Delete.Table("SystemDeliveryEquipmentEntriesReversals");
        }
    }
}