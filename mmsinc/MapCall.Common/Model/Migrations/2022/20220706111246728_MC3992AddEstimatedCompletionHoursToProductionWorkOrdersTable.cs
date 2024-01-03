using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220706111246728), Tags("Production")]
    public class MC3992AddEstimatedCompletionHoursToProductionWorkOrdersTable : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("ProductionWorkOrders")
                 .AddColumn("EstimatedCompletionHours")
                 .AsDecimal(6, 2)
                 .NotNullable()
                 .WithDefaultValue(0);
        }
    }
}
