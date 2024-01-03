using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220627135226421), Tags("Production")]
    public class MC4690ConfinedSpaceFormUpdateShortCycleIdToNumber : Migration
    {
        public override void Up()
        {
            Alter.Table("ConfinedSpaceForms")
                 .AddColumn("ShortCycleWorkOrderNumber").AsInt64().Nullable();
            Execute.Sql(@"update ConfinedSpaceForms
                        SET ConfinedSpaceForms.ShortCycleWorkOrderNumber= ShortCycleWorkOrders.WorkOrder
                        FROM ConfinedSpaceForms
                        INNER JOIN ShortCycleWorkOrders
                        ON ShortCycleWorkOrders.Id = ConfinedSpaceForms.ShortCycleWorkOrderId");
            Delete.ForeignKeyColumn("ConfinedSpaceForms", "ShortCycleWorkOrderId", "ShortCycleWorkOrders");
        }

        public override void Down()
        {
            Alter.Table("ConfinedSpaceForms")
                 .AddForeignKeyColumn("ShortCycleWorkOrderId", "ShortCycleWorkOrders").Nullable();
            Execute.Sql(@"update ConfinedSpaceForms
                            set ConfinedSpaceForms.ShortCycleWorkOrderId = ShortCycleWorkOrders.Id
                            From ConfinedSpaceForms
                            INNER JOIN ShortCycleWorkOrders
                            ON ShortCycleWorkOrders.WorkOrder  = ConfinedSpaceForms.ShortCycleWorkOrderNumber");
            Delete.Column("ShortCycleWorkOrderNumber").FromTable("ConfinedSpaceForms");
        }
    }
}

