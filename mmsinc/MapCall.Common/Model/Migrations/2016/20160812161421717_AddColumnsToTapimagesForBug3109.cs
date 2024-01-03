using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160812161421717), Tags("Production")]
    public class AddColumnsToTapimagesForBug3109 : Migration
    {
        public override void Up()
        {
            Alter.Table("TapImages")
                 .AddForeignKeyColumn("CustomerSideMaterialId", "ServiceMaterials", "ServiceMaterialID")
                 .AddForeignKeyColumn("CustomerSideSizeId", "ServiceSizes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("TapImages", "CustomerSideMaterialId", "ServiceMaterials",
                "ServiceMaterialID");
            Delete.ForeignKeyColumn("TapImages", "CustomerSideSizeId", "ServiceSizes");
        }
    }
}
