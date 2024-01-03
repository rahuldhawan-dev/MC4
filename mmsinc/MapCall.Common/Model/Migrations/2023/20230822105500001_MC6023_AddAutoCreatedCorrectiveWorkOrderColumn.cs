using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230822105500001), Tags("Production")]
    public class MC6023_AddAutoCreatedCorrectiveWorkOrderColumn : Migration
    {
        private const int NEEDS_EMERGENCY_REPAIR = 2,
                          NEEDS_REPAIR = 3;

        public override void Up()
        {
            Alter.Table("ProductionWorkOrders").AddColumn("AutoCreatedCorrectiveWorkOrder").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
            Execute.Sql(@"UPDATE ProductionWorkOrders SET AutoCreatedCorrectiveWorkOrder = 1 FROM ProductionWorkOrders pwo
                            JOIN ProductionWorkOrdersEquipment e
                                ON pwo.Id = e.ProductionWorkOrderId
                            WHERE e.AsLeftConditionId IN (" + NEEDS_EMERGENCY_REPAIR + ", " + NEEDS_REPAIR + ")");
        }

        public override void Down()
        {
            Delete.Column("AutoCreatedCorrectiveWorkOrder").FromTable("ProductionWorkOrders");
        }
    }
}
