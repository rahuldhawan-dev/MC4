using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230422004829416), Tags("Production")]
    public class MC5536_ChangingWorkOrderColumnToAForeignKey : Migration
    {
        public override void Up()
        {
            Rename.Column("WorkOrder").OnTable("tblJobObservations").To("WorkOrderId");
            Execute.Sql(@"
                ALTER TABLE [dbo].[tblJobObservations]  WITH NOCHECK ADD CONSTRAINT [FK_tblJobObservations_WorkOrders_WorkOrderId] FOREIGN KEY([WorkOrderId])
                REFERENCES [dbo].[WorkOrders] ([WorkOrderID])
                ALTER TABLE [dbo].[tblJobObservations] CHECK CONSTRAINT [FK_tblJobObservations_WorkOrders_WorkOrderId]");
        }

        public override void Down()
        {
            Execute.Sql("ALTER TABLE [dbo].[tblJobObservations] DROP CONSTRAINT [FK_tblJobObservations_WorkOrders_WorkOrderId]");
            Rename.Column("WorkOrderId").OnTable("tblJobObservations").To("WorkOrder");
        }
    }
}

