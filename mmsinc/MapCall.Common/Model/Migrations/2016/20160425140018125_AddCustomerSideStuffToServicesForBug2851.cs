using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160425140018125), Tags("Production")]
    public class AddCustomerSideStuffToServicesForBug2851 : Migration
    {
        public override void Up()
        {
            Alter.Table("Services")
                 .AddForeignKeyColumn("CustomerSideMaterialId", "ServiceMaterials", "ServiceMaterialID")
                 .AddForeignKeyColumn("CustomerSideSizeId", "ServiceSizes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Services", "CustomerSideMaterialId", "ServiceMaterials",
                "ServiceMaterialID");
            Delete.ForeignKeyColumn("Services", "CustomerSideSizeId", "ServiceSizes");
        }
    }
}
