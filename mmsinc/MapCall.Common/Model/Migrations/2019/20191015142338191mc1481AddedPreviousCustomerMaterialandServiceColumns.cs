using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191015142338191), Tags("Production")]
    public class mc1481AddedPreviousCustomerMaterialandServiceColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("Services")
                 .AddForeignKeyColumn("PreviousServiceCustomerMaterialId", "ServiceMaterials", "ServiceMaterialID",
                      true)
                 .AddForeignKeyColumn("PreviousServiceCustomerSizeId", "ServiceSizes", "Id", true);
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("Services", "PreviousServiceCustomerMaterialId",
                "ServicesMaterials", "ServiceMaterialID");
            Delete.ForeignKeyColumn("Services", "PreviousServiceCustomerSizeId",
                "ServiceSizes", "Id");
        }
    }
}
