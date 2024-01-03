using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220926124017736), Tags("Production")]
    public class AddPWSIDAndWWSIDToSystemDeliveryEntries : Migration
    {
        public override void Up()
        {
            Create.Table("SystemDeliveryEntriesPublicWaterSupplies")
                  .WithForeignKeyColumn("PublicWaterSupplyId", "PublicWaterSupplies", nullable: false)
                  .WithForeignKeyColumn("SystemDeliveryEntryId", "SystemDeliveryEntries", nullable: false);

            Create.Table("SystemDeliveryEntriesWasteWaterSystems")
                  .WithForeignKeyColumn("WasteWaterSystemId", "WasteWaterSystems", nullable: false)
                  .WithForeignKeyColumn("SystemDeliveryEntryId", "SystemDeliveryEntries", nullable: false);
        }

        public override void Down()
        {
            Delete.Table("SystemDeliveryEntriesWasteWaterSystems");
            Delete.Table("SystemDeliveryEntriesPublicWaterSupplies");
        }
    }
}

