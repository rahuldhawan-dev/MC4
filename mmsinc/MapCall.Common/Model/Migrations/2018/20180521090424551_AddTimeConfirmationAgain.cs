using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180521090424551), Tags("Production")]
    public class AddTimeConfirmationAgain : Migration
    {
        public override void Up()
        {
            Create.Table("ShortCycleWorkOrderTimeConfirmations")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderId", "ShortCycleWorkOrders")
                  .WithColumn("SAPCommunicationError").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
                  .WithColumn("WorkOrderNumber")
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
                  .WithColumn("ReceivedAt").AsDateTime().NotNullable();
            Alter.Table("ShortCycleWorkOrderCompletions").AddColumn("ReceivedAt").AsDateTime().Nullable();
            Execute.Sql("UPDATE ShortCycleWorkOrderCompletions SET ReceivedAt = GetDate();");
            Alter.Column("ReceivedAt").OnTable("ShortCycleWorkOrderCompletions").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("ReceivedAt").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Table("ShortCycleWorkOrderTimeConfirmations");
        }
    }
}
