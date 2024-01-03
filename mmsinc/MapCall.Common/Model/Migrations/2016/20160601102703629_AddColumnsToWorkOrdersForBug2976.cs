using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160601102703629), Tags("Production")]
    public class AddColumnsToWorkOrdersForBug2976 : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("PreviousServiceLineMaterialID", "ServiceMaterials", "ServiceMaterialID");
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("PreviousServiceLineSizeID", "ServiceSizes");
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("CustomerServiceLineMaterialID", "ServiceMaterials", "ServiceMaterialID");
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("CustomerServiceLineSizeID", "ServiceSizes");
            Alter.Table("WorkOrders")
                 .AddColumn("DoorNoticeLeftDate")
                 .AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WorkOrders", "PreviousServiceLineMaterialID", "ServiceMaterials",
                "ServiceMaterialID");
            Delete.ForeignKeyColumn("WorkOrders", "PreviousServiceLineSizeID", "ServiceSizes");
            Delete.ForeignKeyColumn("WorkOrders", "CustomerServiceLineMaterialID", "ServiceMaterials",
                "ServiceMaterialID");
            Delete.ForeignKeyColumn("WorkOrders", "CustomerServiceLineSizeID", "ServiceSizes");
            Delete.Column("DoorNoticeLeftDate").FromTable("WorkOrders");
        }
    }
}
