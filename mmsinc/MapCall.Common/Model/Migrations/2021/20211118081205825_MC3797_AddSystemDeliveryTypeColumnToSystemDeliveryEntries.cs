using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20211118081205825), Tags("Production")]
    public class MC3797_AddSystemDeliveryTypeColumnToSystemDeliveryEntries : Migration
    {
        public override void Up()
        {
            // SystemDeliveryTypeId can only be 1-Water or 2-Wastewater, so setting all to 1 for now.
            // Then update the records where SystemDeliveryTypeId should be a 2
            Alter.Table("SystemDeliveryEntries").AddForeignKeyColumn("SystemDeliveryTypeId", "SystemDeliveryTypes")
                 .NotNullable().SetExistingRowsTo(1);

            Execute.Sql(@"UPDATE SystemDeliveryEntries 
                        SET SystemDeliveryTypeId = sdee.SystemDeliveryTypeId 
                        FROM SystemDeliveryEntries sde 
                        INNER JOIN SystemDeliveryEquipmentEntries sdee ON sde.Id = sdee.SystemDeliveryEntryId
                        WHERE sdee.SystemDeliveryTypeId != 1");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SystemDeliveryEntries", "SystemDeliveryTypeId", "SystemDeliveryTypes");
        }
    }
}

