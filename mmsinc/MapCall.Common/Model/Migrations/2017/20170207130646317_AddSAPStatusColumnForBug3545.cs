using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20170207130646317), Tags("Production")]
    public class AddSAPStatusColumnForBug3545 : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("SAPWorkOrderSteps", "CREATE", "UPDATE", "COMPLETE", "APPROVE GOODS");
            Alter.Table("WorkOrders").AddForeignKeyColumn("SAPWorkOrderStepId", "SAPWorkOrderSteps");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WorkOrders", "SAPWorkOrderStepId", "SAPWorkOrderSteps");
            Delete.Table("SAPWorkOrderSteps");
        }
    }
}
