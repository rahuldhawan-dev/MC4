using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180503150712195), Tags("Production")]
    public class SewerOverflowChangesForMC76 : Migration
    {
        public override void Up()
        {
            Alter.Table("SewerOverflows")
                 .AddForeignKeyColumn("WorkOrderId", "WorkOrders",
                      "WorkOrderId");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SewerOverflows", "WorkOrderId",
                "WorkOrders", "WorkOrderId");
        }
    }
}
