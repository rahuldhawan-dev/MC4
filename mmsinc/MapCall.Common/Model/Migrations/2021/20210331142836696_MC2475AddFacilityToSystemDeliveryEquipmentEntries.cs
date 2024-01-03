using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210331142836696), Tags("Production")]
    public class MC2475AddFacilityToSystemDeliveryEquipmentEntries : Migration
    {
        public override void Up()
        {
            Alter.Table("SystemDeliveryEquipmentEntries").AddForeignKeyColumn("FacilityId", "tblFacilities", "RecordId");

            //Below probably won't do anything in prod since were not live, added to fix any QA data lingering
            Execute.Sql($@"UPDATE SystemDeliveryEquipmentEntries 
            SET FacilityId = e.FacilityID
            FROM SystemDeliveryEquipmentEntries sde
            INNER JOIN Equipment e
            ON sde.EquipmentId = e.EquipmentID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SystemDeliveryEquipmentEntries", "FacilityId", "tblFacilities", "RecordId");
        }
    }
}

