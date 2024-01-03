using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220803122634109), Tags("Production")]
    public class MC4729_UnlinkNearMissesFromShortCycleWorkOrders : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey(
                       Utilities.CreateForeignKeyName(
                           "NearMisses",
                           "ShortCycleWorkOrders",
                           "ShortCycleWorkOrderId"))
                  .OnTable("NearMisses");
            
            Execute.Sql(
                "UPDATE NearMisses " +
                "SET ShortCycleWorkOrderId = scwo.WorkOrder " +
                "FROM ShortCycleWorkOrders scwo " +
                "WHERE scwo.Id = ShortCycleWorkOrderId;");
            
            Rename.Column("ShortCycleWorkOrderId")
                  .OnTable("NearMisses")
                  .To("ShortCycleWorkOrderNumber");
        }

        public override void Down()
        {
            Rename.Column("ShortCycleWorkOrderNumber")
                  .OnTable("NearMisses")
                  .To("ShortCycleWorkOrderId");
            
            Execute.Sql(
                "UPDATE NearMisses " +
                "SET ShortCycleWorkOrderId = scwo.Id " +
                "FROM ShortCycleWorkOrders scwo " +
                "WHERE scwo.WorkOrder = ShortCycleWorkOrderId;");

            Create.ForeignKey(
                       Utilities.CreateForeignKeyName(
                           "NearMisses",
                           "ShortCycleWorkOrders",
                           "ShortCycleWorkOrderId"))
                  .FromTable("NearMisses")
                  .ForeignColumn("ShortCycleWorkOrderId")
                  .ToTable("ShortCycleWorkOrders")
                  .PrimaryColumn("Id");
        }
    }
}

