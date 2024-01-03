using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220413154508526), Tags("Production")]
    public class MC2444AddingCompanyServiceLineMaterialAndSizeFieldsToWorkOrders : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("CompanyServiceLineMaterialId", "ServiceMaterials", "ServiceMaterialID");
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("CompanyServiceLineSizeId", "ServiceSizes");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WorkOrders", "CompanyServiceLineMaterialId", "ServiceMaterials",
                "ServiceMaterialID");
            Delete.ForeignKeyColumn("WorkOrders", "CompanyServiceLineSizeId", "ServiceSizes");
        }
    }
}

