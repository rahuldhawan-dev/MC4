using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180403121331083), Tags("Production")]
    public class AddShortCycleWorkOrderCompletionOtherForWO230579 : Migration
    {
        public override void Up()
        {
            //throw new Exception("This migration is not complete.");
            Create.Table("ShortCycleWorkOrderCompletionOthers")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderId", "ShortCycleWorkOrders")
                  .WithColumn("SAPCommunicationError").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
                  .WithColumn("ActiveMQStatus").AsCustom("varchar(max)").Nullable()
                  .WithColumn("WorkOrderNumber")
                  .AsAnsiString().Nullable()
                  .WithColumn("MiscInvoice").AsAnsiString()
                  .Nullable()
                  .WithColumn("BackOfficeReview")
                  .AsAnsiString().Nullable()
                  .WithColumn("CompletionStatus")
                  .AsAnsiString().Nullable()
                  .WithColumn("Notes").AsCustom("text").Nullable()
                  .WithColumn("Activity1").AsAnsiString().Nullable()
                  .WithColumn("Activity2").AsAnsiString().Nullable()
                  .WithColumn("Activity3").AsAnsiString().Nullable()
                  .WithColumn("AdditionalWorkNeeded").AsAnsiString(40).Nullable()
                  .WithColumn("Purpose").AsAnsiString().Nullable()
                  .WithColumn("TechnicalInspectedOn")
                  .AsAnsiString().Nullable()
                  .WithColumn("TechnicalInspectedBy")
                  .AsAnsiString().Nullable()
                  .WithColumn("ServiceFound").AsAnsiString(4).Nullable()
                  .WithColumn("ServiceLeft").AsAnsiString()
                  .Nullable()
                  .WithColumn("OperatedPointOfControl").AsAnsiString(4).Nullable()
                  .WithColumn("AdditionalInformation").AsCustom("text").Nullable()
                  .WithColumn("CurbBoxMeasurementDescription").AsCustom("text").Nullable()
                  .WithColumn("Safety").AsCustom("text").Nullable()
                  .WithColumn("HeatType").AsAnsiString(30).Nullable()
                  .WithColumn("MeterPositionLocation").AsAnsiString(2).Nullable()
                  .WithColumn("MeterDirectionalLocation").AsAnsiString(2).Nullable()
                  .WithColumn("MeterSupplementalLocation").AsAnsiString(30).Nullable()
                  .WithColumn("ReadingDevicePositionalLocation").AsAnsiString(30).Nullable()
                  .WithColumn("ReadingDeviceSupplementalLocation").AsAnsiString(30).Nullable()
                  .WithColumn("ReadingDeviceDirectionalLocation").AsAnsiString(30).Nullable()
                  .WithColumn("FSRId").AsInt32().Nullable()
                  .WithColumn("SerialNumber").AsAnsiString()
                  .Nullable()
                  .WithColumn("MeterSerialNumber")
                  .AsAnsiString().Nullable()
                  .WithColumn("DeviceCategory")
                  .AsAnsiString().Nullable()
                  .WithColumn("ActionFlag").AsAnsiString(2).Nullable();
            Create.Table("ShortCycleWorkOrderCompletionOtherTestResults")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderCompletionOtherId", "ShortCycleWorkOrderCompletionOthers")
                  .WithColumn("RegisterId").AsInt32().Nullable()
                  .WithColumn("LowMedHighIndicator")
                  .AsAnsiString().Nullable()
                  .WithColumn("InitialRepair").AsInt32().Nullable()
                  .WithColumn("Accuracy").AsAnsiString()
                  .Nullable()
                  .WithColumn("CalculatedVolume")
                  .AsAnsiString().Nullable()
                  .WithColumn("TestFlowRate")
                  .AsAnsiString().Nullable()
                  .WithColumn("StartRead")
                  .AsAnsiString().Nullable()
                  .WithColumn("EndRead").AsAnsiString()
                  .Nullable();

            Create.Table("ShortCycleWorkOrderCompletionOtherRegisters")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderCompletionOtherId", "ShortCycleWorkOrderCompletionOthers")
                  .WithColumn("Size").AsAnsiString().Nullable()
                  .WithColumn("MIUNumber").AsAnsiString()
                  .Nullable()
                  .WithColumn("EncoderId").AsAnsiString()
                  .Nullable()
                  .WithColumn("Read").AsAnsiString().Nullable()
                  .WithColumn("ReadType").AsAnsiString()
                  .Nullable();
        }

        public override void Down()
        {
            Delete.Table("ShortCycleWorkOrderCompletionOtherRegisters");
            Delete.Table("ShortCycleWorkOrderCompletionOtherTestResults");
            Delete.Table("ShortCycleWorkOrderCompletionOthers");
        }
    }
}
