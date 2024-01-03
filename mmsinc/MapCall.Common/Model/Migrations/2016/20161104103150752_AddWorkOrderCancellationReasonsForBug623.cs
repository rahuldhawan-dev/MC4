using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20161104103150752), Tags("Production")]
    public class AddWorkOrderCancellationReasonsForBug623 : Migration
    {
        public override void Up()
        {
            Create.Table("WorkOrderCancellationReasons")
                  .WithIdentityColumn()
                  .WithColumn("Status").AsAnsiString(4).NotNullable()
                  .WithColumn("Description").AsAnsiString(50).NotNullable();
            Alter.Table("WorkOrders")
                 .AddForeignKeyColumn("WorkOrderCancellationReasonId", "WorkOrderCancellationReasons");

            Execute.Sql(
                "INSERT INTO WorkOrderCancellationReasons Values('CERR', 'Created In Error');" +
                "INSERT INTO WorkOrderCancellationReasons Values('CUST', 'Customer Request');" +
                "INSERT INTO WorkOrderCancellationReasons Values('ERRO', 'Company Error');" +
                "INSERT INTO WorkOrderCancellationReasons Values('EXPR', 'Order Past Expiration Date');" +
                "INSERT INTO WorkOrderCancellationReasons Values('NVAL', 'No Longer Valid');" +
                "INSERT INTO WorkOrderCancellationReasons Values('SUPI', 'Supervisor Instructed');" +
                "INSERT INTO WorkOrderCancellationReasons Values('WCOM', 'Work Already Completed');");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("WorkOrders", "WorkOrderCancellationReasonId", "WorkOrderCancellationReasons");
            Delete.Table("WorkOrderCancellationReasons");
        }
    }
}
