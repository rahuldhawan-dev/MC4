using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210316101653314), Tags("Production")]
    public class MC3133AddPurchaseSupplierToFacilitySystemDeliveryEntryTypes : Migration
    {
        public override void Up()
        {
            Alter.Table("FacilitiesSystemDeliveryEntryTypes").AddColumn("PurchaseSupplier").AsAnsiString(100).Nullable();
        }

        public override void Down()
        {
            Delete.Column("PurchaseSupplier").FromTable("FacilitiesSystemDeliveryEntryTypes");
        }
    }
}

