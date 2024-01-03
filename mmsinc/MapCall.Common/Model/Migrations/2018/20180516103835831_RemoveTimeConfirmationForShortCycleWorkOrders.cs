using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180516103835831), Tags("Production")]
    public class RemoveTimeConfirmationForShortCycleWorkOrders : Migration
    {
        public override void Up()
        {
            // They decided they didn't want to do this today
            Delete.Table("ShortCycleWorkOrderTimeConfirmations");
        }

        public override void Down()
        {
            Create.Table("ShortCycleWorkOrderTimeConfirmations")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderId", "ShortCycleWorkOrders")
                  .WithColumn("SAPCommunicationError").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
                  .WithColumn("WorkOrder")
                  .AsAnsiString().Nullable()
                  .WithColumn("OperationId").AsAnsiString(4).Nullable()
                  .WithColumn("DateCompleted").AsAnsiString(8).Nullable()
                  .WithColumn("WorkCenter").AsAnsiString(8).Nullable()
                  .WithColumn("PersonnelNumber")
                  .AsAnsiString().Nullable()
                  .WithColumn("ActualWork").AsDecimal(5, 2).Nullable()
                  .WithColumn("UnitOfMeasure").AsAnsiString(3).Nullable()
                  .WithColumn("ActivityType")
                  .AsAnsiString().Nullable()
                  .WithColumn("FinalConfirmation")
                  .AsAnsiString().Nullable()
                  .WithColumn("NoRemainingWork")
                  .AsAnsiString().Nullable()
                  .WithColumn("WorkStartDate").AsAnsiString(8).Nullable()
                  .WithColumn("WorkStartTime").AsAnsiString(8).Nullable()
                  .WithColumn("WorkFinishDate").AsAnsiString(8).Nullable()
                  .WithColumn("WorkFinishTime").AsAnsiString(8).Nullable()
                  .WithColumn("Reason").AsAnsiString()
                  .Nullable()
                  .WithColumn("ConfirmationText")
                  .AsAnsiString().Nullable()
                  .WithColumn("ReceivdAt").AsDateTime().NotNullable();
        }
    }
}
