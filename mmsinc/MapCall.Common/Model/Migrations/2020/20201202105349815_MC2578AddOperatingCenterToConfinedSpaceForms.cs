using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201202105349815), Tags("Production")]
    public class MC2578AddOperatingCenterToConfinedSpaceForms : Migration
    {
        public override void Up()
        {
            Alter.Table("ConfinedSpaceForms")
                 .AddForeignKeyColumn("OperatingCenterId", "OperatingCenters", "OperatingCenterId");

            Execute.Sql(
                @"update ConfinedSpaceForms set OperatingCenterId = (select OperatingCenterId from ProductionWorkOrders o where o.Id = ProductionWorkOrderId) where ProductionWorkOrderId is not null
update ConfinedSpaceForms set OperatingCenterId = (select pp.OperatingCenterId from ShortCycleWorkOrders o 
inner join PlanningPlants pp on pp.Id = o.PlanningPlantId where o.Id = ShortCycleWorkOrderId) where ShortCycleWorkOrderId is not null
update ConfinedSpaceForms set OperatingCenterId = (select OperatingCenterId from WorkOrders o where o.WorkOrderId = WorkOrderId) where WorkOrderId is not null");

            Alter.Column("OperatingCenterId").OnTable("ConfinedSpaceForms").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ConfinedSpaceForms", "OperatingCenterId", "OperatingCenters", "OperatingCenterId");
        }
    }
}
