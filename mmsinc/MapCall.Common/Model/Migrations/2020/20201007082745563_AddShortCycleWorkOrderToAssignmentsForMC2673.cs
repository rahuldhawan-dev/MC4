using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20201007082745563), Tags("Production")]
    public class AddShortCycleWorkOrderToAssignmentsForMC2673 : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleAssignments")
                 .AddColumn("IsUpdate").AsBoolean().Nullable()
                 .AddForeignKeyColumn("ShortCycleWorkOrderId", "ShortCycleWorkOrders");
            // UPDATE WORK ORDER
            Execute.Sql(" UPDATE" +
                        "   ShortCycleAssignments" +
                        " SET" +
                        "   ShortCycleWorkOrderId = scwo.Id" +
                        " FROM" +
                        "   ShortCycleAssignments A" +
                        " JOIN" +
                        "   ShortCycleWorkOrders scwo on scwo.WorkOrder = A.CallId");
            // UPDATE IS UPDATE BASED ON CURRENT LOGIC
            Execute.Sql("UPDATE" +
                        "   ShortCycleAssignments" +
                        " SET" +
                        "   IsUpdate = CASE WHEN(M.Id is null) then 1 else 0 end" +
                        " FROM" +
                        "   ShortCycleAssignments A " +
                        " LEFT JOIN " +
                        "   ShortCycleAssignmentsMinimumIdByCallId m on M.Id = A.Id");
            Execute.Sql(ChangeCallIdToLongForMC2428.DROP_VIEW);
            Alter.Table("ShortCycleAssignments")
                 .AlterColumn("ShortCycleWorkOrderId").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("IsUpdate").FromTable("ShortCycleAssignments");
            Delete.ForeignKeyColumn("ShortCycleAssignments", "ShortCycleWorkOrderId", "ShortCycleWorkOrders");
            Execute.Sql(ChangeCallIdToLongForMC2428.CREATE_VIEW);
        }
    }
}
