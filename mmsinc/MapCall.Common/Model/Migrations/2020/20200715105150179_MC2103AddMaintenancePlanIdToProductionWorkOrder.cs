using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200715105150179), Tags("Production")]
    public class MC2103AddSAPMaintenancePlanIdToProductionWorkOrder : Migration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrders")
                 .AddColumn("SAPMaintenancePlanId").AsInt64().Nullable();
        }

        public override void Down()
        {
            Delete.Column("SAPMaintenancePlanId").FromTable("ProductionWorkOrders");
        }
    }
}
