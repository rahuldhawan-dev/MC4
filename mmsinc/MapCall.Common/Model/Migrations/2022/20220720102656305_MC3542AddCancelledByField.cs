using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220720102656305), Tags("Production")]
    public class AddCancelledByField : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrders").AddForeignKeyColumn("CancelledById", "tblPermissions", "RecId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ProductionWorkOrders", "CancelledById", "tblPermissions", "RecId");
        }
    }
}