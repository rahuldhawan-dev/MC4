using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210107101525236), Tags("Production")]
    public class MC2816AddSupplierFacilityToFacilitySystemDeliveryEntryType : Migration
    {
        public override void Up()
        {
            Alter.Table("FacilitiesSystemDeliveryEntryTypes")
                 .AddForeignKeyColumn("SupplierFacilityId", "tblFacilities", "RecordId");
        }

        public override void Down()
        { 
            Delete.ForeignKeyColumn("FacilitiesSystemDeliveryEntryTypes", "SupplierFacilityId", "tblFacilities", "RecordId");
        }
    }
}

