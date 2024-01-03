using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20191107150945999), Tags("Production")]
    public class MC1719AddCommunicationStatusForShortCycleEntities : Migration
    {
        #region Constants

        public const string
            DROP_INDEXES = @"
                IF EXISTS (SELECT 1 FROM sysindexes where Name = 'ShortCycleWorkOrders_WorkOrder') DROP INDEX [ShortCycleWorkOrders_WorkOrder] ON [dbo].[ShortCycleWorkOrders];
            ",
            RESTORE_INDEXES = @"
CREATE NONCLUSTERED INDEX [ShortCycleWorkOrders_WorkOrder] ON [dbo].[ShortCycleWorkOrders]
        (

                [WorkOrder] ASC
        )
        INCLUDE([Id],
            [SAPCommunicationError],
            [FunctionalLocation],
            [NotificationNumber],
            [Priority],
            [BackReportingType],
            [CompanyCode],
            [Premise],
            [Status],
            [WBSElement],
            [PlanningPlant],
            [MATCode],
            [MATCodeDescription],
            [OperationText],
            [FSRId],
            [FSRName],
            [CustomerNumber],
            [NextReplacementYear],
            [ServiceType],
            [MeterSerialNumber],
            [ManufacturerSerialNumber],
            [IsCustomerEnrolledForEmail],
            [IsUpdate],
            [AssignmentStart],
            [AssignmentEnd],
            [OrderType],
            [OperationId],
            [WorkCenter],
            [CustomerAccount],
            [NormalDuration],
            [NormalDurationUnit],
            [PlannerGroup],
            [ActiveMQStatus],
            [PhoneAhead],
            [CustomerAtHome],
            [OperationNumber],
            [ModifiedBy],
            [TimeModified],
            [ReasonCode],
            [DueDateTime],
            [LiabilityIndicator],
            [AppointmentStart],
            [AppointmentEnd],
            [CrewSize],
            [ReceivedAt]) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON[PRIMARY]
        ";

        #endregion

        public override void Up()
        {
            Execute.Sql(DROP_INDEXES);
            this.CreateLookupTableWithValues("SAPCommunicationStatuses", "Pending", "Retry", "Success");

            // DEFAULT AS PENDING
            Alter.Table("BusinessProcessExceptionManagementCases")
                 .AddForeignKeyColumn("SAPCommunicationStatusId", "SAPCommunicationStatuses").NotNullable()
                 .WithDefaultValue(1);
            Alter.Table("FieldServiceRepresentativeNonAvailabilities")
                 .AddForeignKeyColumn("SAPCommunicationStatusId", "SAPCommunicationStatuses").NotNullable()
                 .WithDefaultValue(1);
            Alter.Table("ShortCycleWorkOrders")
                 .AddForeignKeyColumn("SAPCommunicationStatusId", "SAPCommunicationStatuses").NotNullable()
                 .WithDefaultValue(1);
            Alter.Table("ShortCycleWorkOrderRequests")
                 .AddForeignKeyColumn("SAPCommunicationStatusId", "SAPCommunicationStatuses").NotNullable()
                 .WithDefaultValue(1);
            Alter.Table("ShortCycleWorkOrderCompletions")
                 .AddForeignKeyColumn("SAPCommunicationStatusId", "SAPCommunicationStatuses").NotNullable()
                 .WithDefaultValue(1);
            Alter.Table("ShortCycleWorkOrderStatusUpdates")
                 .AddForeignKeyColumn("SAPCommunicationStatusId", "SAPCommunicationStatuses").NotNullable()
                 .WithDefaultValue(1);
            Alter.Table("ShortCycleWorkOrderTimeConfirmations")
                 .AddForeignKeyColumn("SAPCommunicationStatusId", "SAPCommunicationStatuses").NotNullable()
                 .WithDefaultValue(1);
            Alter.Table("ShortCycleAssignments")
                 .AddForeignKeyColumn("SAPCommunicationStatusId", "SAPCommunicationStatuses").NotNullable()
                 .WithDefaultValue(1);

            // UPDATE THE SUCCESSFUL ONES BASED ON COMMUNICATION ERROR
            Execute.Sql(
                "UPDATE BusinessProcessExceptionManagementCases SET SAPCommunicationStatusId = 3 WHERE SAPCommunicationError = 0");
            Execute.Sql(
                "UPDATE FieldServiceRepresentativeNonAvailabilities SET SAPCommunicationStatusId = 3 WHERE SAPCommunicationError = 0");
            Execute.Sql("UPDATE ShortCycleWorkOrders SET SAPCommunicationStatusId = 3 WHERE SAPCommunicationError = 0");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderRequests SET SAPCommunicationStatusId = 3 WHERE SAPCommunicationError = 0");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderCompletions SET SAPCommunicationStatusId = 3 WHERE SAPCommunicationError = 0");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderStatusUpdates SET SAPCommunicationStatusId = 3 WHERE SAPCommunicationError = 0");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderTimeConfirmations SET SAPCommunicationStatusId = 3 WHERE SAPCommunicationError = 0");
            Execute.Sql(
                "UPDATE ShortCycleAssignments SET SAPCommunicationStatusId = 3 WHERE SAPCommunicationError = 0");

            // UPDATE THE FAILED ONES BASED ON COMMUNICATION ERROR OR LOCKED IN SAPErrorCode
            Execute.Sql(
                "UPDATE BusinessProcessExceptionManagementCases SET SAPCommunicationStatusId = 2 WHERE SAPCommunicationError = 1 OR SAPErrorCode like '%locked%'");
            Execute.Sql(
                "UPDATE FieldServiceRepresentativeNonAvailabilities SET SAPCommunicationStatusId = 2 WHERE SAPCommunicationError = 1 OR SAPErrorCode like '%locked%'");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrders SET SAPCommunicationStatusId = 2 WHERE SAPCommunicationError = 1 OR SAPErrorCode like '%locked%'");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderRequests SET SAPCommunicationStatusId = 2 WHERE SAPCommunicationError = 1 OR SAPErrorCode like '%locked%'");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderCompletions SET SAPCommunicationStatusId = 2 WHERE SAPCommunicationError = 1 OR SAPErrorCode like '%locked%'");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderStatusUpdates SET SAPCommunicationStatusId = 2 WHERE SAPCommunicationError = 1 OR SAPErrorCode like '%locked%'");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderTimeConfirmations SET SAPCommunicationStatusId = 2 WHERE SAPCommunicationError = 1 OR SAPErrorCode like '%locked%'");
            Execute.Sql(
                "UPDATE ShortCycleAssignments SET SAPCommunicationStatusId = 2 WHERE SAPCommunicationError = 1 OR SAPErrorCode like '%locked%'");

            Delete.Column("SAPCommunicationError").FromTable("BusinessProcessExceptionManagementCases");
            Delete.Column("SAPCommunicationError").FromTable("FieldServiceRepresentativeNonAvailabilities");
            Delete.Column("SAPCommunicationError").FromTable("ShortCycleWorkOrders");
            Delete.Column("SAPCommunicationError").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("SAPCommunicationError").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("SAPCommunicationError").FromTable("ShortCycleWorkOrderStatusUpdates");
            Delete.Column("SAPCommunicationError").FromTable("ShortCycleWorkOrderTimeConfirmations");
            Delete.Column("SAPCommunicationError").FromTable("ShortCycleAssignments");

            // Add HasSAPError field
            Alter.Table("BusinessProcessExceptionManagementCases").AddColumn("HasSAPError").AsBoolean()
                 .WithDefaultValue(false).NotNullable();
            Alter.Table("FieldServiceRepresentativeNonAvailabilities").AddColumn("HasSAPError").AsBoolean()
                 .WithDefaultValue(false).NotNullable();
            Alter.Table("ShortCycleWorkOrders").AddColumn("HasSAPError").AsBoolean().WithDefaultValue(false)
                 .NotNullable();
            Alter.Table("ShortCycleWorkOrderRequests").AddColumn("HasSAPError").AsBoolean().WithDefaultValue(false)
                 .NotNullable();
            Alter.Table("ShortCycleWorkOrderCompletions").AddColumn("HasSAPError").AsBoolean().WithDefaultValue(false)
                 .NotNullable();
            Alter.Table("ShortCycleWorkOrderStatusUpdates").AddColumn("HasSAPError").AsBoolean().WithDefaultValue(false)
                 .NotNullable();
            Alter.Table("ShortCycleWorkOrderTimeConfirmations").AddColumn("HasSAPError").AsBoolean()
                 .WithDefaultValue(false).NotNullable();
            Alter.Table("ShortCycleAssignments").AddColumn("HasSAPError").AsBoolean().WithDefaultValue(false)
                 .NotNullable();

            // Set HasSAPError when HasSAPError
            Execute.Sql(
                "UPDATE BusinessProcessExceptionManagementCases SET HasSAPError = 1 WHERE CHARINDEX('SUCCESS', UPPER(cast(SAPErrorCode as varchar(8000)))) = 0 AND SAPErrorCode not like 'RETRY%'");
            Execute.Sql(
                "UPDATE FieldServiceRepresentativeNonAvailabilities SET HasSAPError = 1 WHERE CHARINDEX('SUCCESS', UPPER(cast(SAPErrorCode as varchar(8000)))) = 0 AND SAPErrorCode not like 'RETRY%'");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrders SET HasSAPError = 1 WHERE CHARINDEX('SUCCESS', UPPER(cast(SAPErrorCode as varchar(8000)))) = 0 AND SAPErrorCode not like 'RETRY%'");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderRequests SET HasSAPError = 1 WHERE CHARINDEX('SUCCESS', UPPER(cast(SAPErrorCode as varchar(8000)))) = 0 AND SAPErrorCode not like 'RETRY%'");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderCompletions SET HasSAPError = 1 WHERE CHARINDEX('SUCCESS', UPPER(cast(SAPErrorCode as varchar(8000)))) = 0 AND SAPErrorCode not like 'RETRY%'");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderStatusUpdates SET HasSAPError = 1 WHERE CHARINDEX('SUCCESS', UPPER(cast(SAPErrorCode as varchar(8000)))) = 0 AND SAPErrorCode not like 'RETRY%'");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderTimeConfirmations SET HasSAPError = 1 WHERE CHARINDEX('SUCCESS', UPPER(cast(SAPErrorCode as varchar(8000)))) = 0 AND SAPErrorCode not like 'RETRY%'");
            Execute.Sql(
                "UPDATE ShortCycleAssignments SET HasSAPError = 1 WHERE CHARINDEX('SUCCESS', UPPER(cast(SAPErrorCode as varchar(8000)))) = 0 AND SAPErrorCode not like 'RETRY%'");

            Execute.Sql(RESTORE_INDEXES.Replace("SAPCommunicationError", "SAPCommunicationStatusId"));
        }

        public override void Down()
        {
            Execute.Sql(DROP_INDEXES);

            Alter.Table("BusinessProcessExceptionManagementCases").AddColumn("SAPCommunicationError").AsBoolean()
                 .NotNullable().WithDefaultValue(false);
            Alter.Table("FieldServiceRepresentativeNonAvailabilities").AddColumn("SAPCommunicationError").AsBoolean()
                 .NotNullable().WithDefaultValue(false);
            Alter.Table("ShortCycleWorkOrders").AddColumn("SAPCommunicationError").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
            Alter.Table("ShortCycleWorkOrderRequests").AddColumn("SAPCommunicationError").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
            Alter.Table("ShortCycleWorkOrderCompletions").AddColumn("SAPCommunicationError").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
            Alter.Table("ShortCycleWorkOrderStatusUpdates").AddColumn("SAPCommunicationError").AsBoolean().NotNullable()
                 .WithDefaultValue(false);
            Alter.Table("ShortCycleWorkOrderTimeConfirmations").AddColumn("SAPCommunicationError").AsBoolean()
                 .NotNullable().WithDefaultValue(false);
            Alter.Table("ShortCycleAssignments").AddColumn("SAPCommunicationError").AsBoolean().NotNullable()
                 .WithDefaultValue(false);

            Execute.Sql(
                "UPDATE BusinessProcessExceptionManagementCases SET SAPCommunicationError = 1 WHERE SAPCommunicationStatusId <> 3");
            Execute.Sql(
                "UPDATE FieldServiceRepresentativeNonAvailabilities SET SAPCommunicationError = 1 WHERE SAPCommunicationStatusId <> 3");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrders SET SAPCommunicationError = 1 WHERE SAPCommunicationStatusId <> 3");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderRequests SET SAPCommunicationError = 1 WHERE SAPCommunicationStatusId <> 3");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderCompletions SET SAPCommunicationError = 1 WHERE SAPCommunicationStatusId <> 3");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderStatusUpdates SET SAPCommunicationError = 1 WHERE SAPCommunicationStatusId <> 3");
            Execute.Sql(
                "UPDATE ShortCycleWorkOrderTimeConfirmations SET SAPCommunicationError = 1 WHERE SAPCommunicationStatusId <> 3");
            Execute.Sql(
                "UPDATE ShortCycleAssignments SET SAPCommunicationError = 1 WHERE SAPCommunicationStatusId <> 3");

            Delete.ForeignKeyColumn("BusinessProcessExceptionManagementCases", "SAPCommunicationStatusId",
                "SAPCommunicationStatuses");
            Delete.ForeignKeyColumn("FieldServiceRepresentativeNonAvailabilities", "SAPCommunicationStatusId",
                "SAPCommunicationStatuses");
            Delete.ForeignKeyColumn("ShortCycleWorkOrders", "SAPCommunicationStatusId", "SAPCommunicationStatuses");
            Delete.ForeignKeyColumn("ShortCycleWorkOrderRequests", "SAPCommunicationStatusId",
                "SAPCommunicationStatuses");
            Delete.ForeignKeyColumn("ShortCycleWorkOrderCompletions", "SAPCommunicationStatusId",
                "SAPCommunicationStatuses");
            Delete.ForeignKeyColumn("ShortCycleWorkOrderStatusUpdates", "SAPCommunicationStatusId",
                "SAPCommunicationStatuses");
            Delete.ForeignKeyColumn("ShortCycleWorkOrderTimeConfirmations", "SAPCommunicationStatusId",
                "SAPCommunicationStatuses");
            Delete.ForeignKeyColumn("ShortCycleAssignments", "SAPCommunicationStatusId", "SAPCommunicationStatuses");

            Delete.Column("HasSAPError").FromTable("BusinessProcessExceptionManagementCases");
            Delete.Column("HasSAPError").FromTable("FieldServiceRepresentativeNonAvailabilities");
            Delete.Column("HasSAPError").FromTable("ShortCycleWorkOrders");
            Delete.Column("HasSAPError").FromTable("ShortCycleWorkOrderRequests");
            Delete.Column("HasSAPError").FromTable("ShortCycleWorkOrderCompletions");
            Delete.Column("HasSAPError").FromTable("ShortCycleWorkOrderStatusUpdates");
            Delete.Column("HasSAPError").FromTable("ShortCycleWorkOrderTimeConfirmations");
            Delete.Column("HasSAPError").FromTable("ShortCycleAssignments");

            Execute.Sql(RESTORE_INDEXES);
            Delete.Table("SAPCommunicationStatuses");
        }
    }
}
