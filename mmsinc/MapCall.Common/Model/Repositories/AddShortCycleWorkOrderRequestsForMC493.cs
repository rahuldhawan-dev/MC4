using FluentMigrator;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Repositories
{
    [Migration(20180625134258475), Tags("Production")]
    public class AddShortCycleWorkOrderRequestsForMC493 : Migration
    {
        public override void Up()
        {
            Create.Table("ShortCycleWorkOrderRequests")
                  .WithIdentityColumn()
                  .WithColumn("SAPCommunicationError").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
                  .WithColumn("MaintenanceActivityType").AsAnsiString(3).NotNullable()
                  .WithColumn("Installation").AsAnsiString()
                  .NotNullable()
                  .WithColumn("BusinessPartnerNumber")
                  .AsAnsiString().NotNullable()
                  .WithColumn("ContractAccount").AsAnsiString()
                  .NotNullable()
                  .WithColumn("FunctionalLocation")
                  .AsAnsiString().NotNullable()
                  .WithColumn("Equipment").AsAnsiString()
                  .NotNullable()
                  .WithColumn("ManufacturerSerialNumber")
                  .AsAnsiString().NotNullable()
                  .WithColumn("WorkOrderLongText").AsCustom("text").Nullable()
                  .WithColumn("WorkOrderNumber")
                  .AsAnsiString().Nullable()
                  .WithColumn("Status").AsAnsiString().Nullable();
        }

        public override void Down()
        {
            Delete.Table("ShortCycleWorkOrderRequests");
        }
    }
}
