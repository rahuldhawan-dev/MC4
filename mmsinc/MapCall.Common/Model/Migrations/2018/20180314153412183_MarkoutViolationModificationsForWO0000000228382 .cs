using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180314153412183), Tags("Production")]
    public class MarkoutViolationModificationsForWO0000000228382 : Migration
    {
        public override void Up()
        {
            Alter.Table("MarkoutViolations")
                 .AddForeignKeyColumn("WorkOrderID", "WorkOrders", "WorkOrderID");
            Rename.Column("MarkoutViolationID").OnTable("MarkoutViolations").To("Id");
        }

        public override void Down()
        {
            Rename.Column("Id").OnTable("MarkoutViolations").To("MarkoutViolationID");
            Delete.ForeignKeyColumn("MarkoutViolations", "WorkOrderID", "WorkOrders", "WorkOrderID");
        }
    }
}
