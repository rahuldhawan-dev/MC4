using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210126144204536), Tags("Production")]
    public class MC2816AddSupplierFacilityToSystemDeliveryEquipmentEntries : Migration
    {
        public override void Up()
        {
            Alter.Table("SystemDeliveryEquipmentEntries").AddForeignKeyColumn("SupplierFacilityId", "tblFacilities", "RecordId").Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SystemDeliveryEquipmentEntries", "SupplierFacilityId", "tblFacilities", "RecordId");
        }
    }
}

