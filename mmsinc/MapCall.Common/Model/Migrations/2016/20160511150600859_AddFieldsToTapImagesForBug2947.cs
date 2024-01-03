using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160511150600859), Tags("Production")]
    public class AddFieldsToTapImagesForBug2947 : Migration
    {
        public override void Up()
        {
            Alter.Table("TapImages")
                 .AddForeignKeyColumn("PreviousServiceMaterialId", "ServiceMaterials", "ServiceMaterialID");
            Alter.Table("TapImages").AddForeignKeyColumn("PreviousServiceSizeId", "ServiceSizes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("TapImages", "PreviousServiceSizeId", "ServiceSizes");
            Delete.ForeignKeyColumn("TapImages", "PreviousServiceMaterialId", "ServiceMaterials", "ServiceMaterialID");
        }
    }
}
