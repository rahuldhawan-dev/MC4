using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161011085514403), Tags("Production")]
    public class AddCancelFieldsToWorkOrdersForBug623 : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders")
                 .AddColumn("CancelledAt")
                 .AsDateTime()
                 .Nullable();

            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("CancelledById", "tblPermissions",
                      "RecId");
        }

        public override void Down()
        {
            Delete.Column("CancelledAt").FromTable("WorkOrders");

            Delete.ForeignKeyColumn("WorkOrders", "CancelledById",
                "tblPermissions", "RecId");
        }
    }
}
