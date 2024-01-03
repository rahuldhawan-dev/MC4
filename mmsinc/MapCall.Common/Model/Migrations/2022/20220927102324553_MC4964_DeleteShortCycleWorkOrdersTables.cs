using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220927102324553), Tags("Production")]
    public class MC4964_DeleteShortCycleWorkOrdersTables : Migration
    {
        public override void Up()
        {
            Delete.Table("ShortCycleWorkOrderStatusUpdates");
            Delete.Table("ShortCycleWorkOrderSecurityThreats");
            Delete.Table("ShortCycleWorkOrdersEquipmentIds");
            Delete.Table("ShortCycleWorkOrderTimeConfirmations");
            Delete.Table("ShortCycleAssignments");
            Delete.Table("ShortCycleAssignmentNumbers");
            Delete.Table("ShortCycleWorkOrderCompletionRegisters");
            Delete.Table("ShortCycleWorkOrderCompletionsActivities");
            Delete.Table("ShortCycleWorkOrderCompletionActivityDescriptions");
            Delete.Table("ShortCycleWorkOrderCompletionsQualityIssues");
            Delete.Table("ShortCycleWorkOrderCompletionQualityIssueDescriptions");
            Delete.Table("ShortCycleWorkOrderCompletionTestResults");
            Delete.Table("ShortCycleWorkOrderCompletionTestResultInitialRepairs");
            Delete.Table("ShortCycleWorkOrderCompletionTestResultLowMedHighIndicators");
            Delete.Table("ShortCycleWorkOrderCompletionViolationCodes");
            Delete.Table("ShortCycleWorkOrderCompletions");
            Delete.Table("ShortCycleWorkOrderCompletionActivityReasons");
            Delete.Table("ShortCycleWorkOrderCompletionRegisterReasonCodes");
            Delete.Table("ShortCycleWorkOrderCompletionFSRInteractions");
            Delete.Table("ShortCycleWorkOrderCompletionHeatTypes");
            Delete.Table("ShortCycleWorkOrderCompletionPurposes");
            Delete.Table("ShortCycleWorkOrderCompletionAdditionalWorkNeeded");
            Delete.Table("ShortCycleSecureAccessTypes");
            Delete.Table("ShortCycleWorkOrderCompletionActionFlags");
            Delete.Table("ShortCycleWorkOrderEquipmentInstallationTypes");
            Delete.Table("ShortCycleWorkOrderPointsOfControl");
            Delete.Table("ShortCycleWorkOrderRequests");
            Delete.Table("ShortCycleWorkOrderRequestMaintenanceActivityTypes");
            Delete.Table("ShortCycleWorkOrderTimeConfirmationUnitsOfMeasure");
            Delete.Table("BusinessProcessExceptionManagementCases");
            Delete.Table("FSRNonAvailabilitiesEngineers");
            Delete.Table("FieldServiceRepresentativeNonAvailabilities");
            Delete.Table("ShortCycleWorkOrders");
            Delete.Table("ShortCycleWorkOrderPriorities");
            Delete.Table("ShortCycleWorkOrderServiceTypes");
            Delete.Table("ShortCycleWorkCenters");
            Delete.Table("ShortCycleWorkOrderDurationUnits");
            Delete.Table("ShortCycleReasonCodes");
            Delete.Table("ShortCycleWorkOrderLiabilityIndicators");
            Delete.Table("ShortCycleWorkOrderFixedChargeNoMeters");
            Delete.Table("ShortCycleWorkOrderDunningLockStatuses");
            Delete.Table("ShortCycleWorkOrderPremiseTypes");
            Delete.Table("ShortCycleWorkOrderLandlordAllocations");
            Delete.Table("ShortCycleDistricts");
        }

        public override void Down()
        {
            Create
               .Table("ShortCycleDistricts")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsString(40).NotNullable();

            Create
               .Table("ShortCycleWorkOrderLandlordAllocations")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(30).NotNullable();

            Create
               .Table("ShortCycleWorkOrderPremiseTypes")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(40).NotNullable();

            Create
               .Table("ShortCycleWorkOrderDunningLockStatuses")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(40).NotNullable();

            Create
               .Table("ShortCycleWorkOrderFixedChargeNoMeters")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(10).NotNullable();

            Create
               .Table("ShortCycleWorkOrderLiabilityIndicators")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(80).NotNullable();

            Create
               .Table("ShortCycleReasonCodes")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(35).Nullable()
               .WithColumn("SAPCode").AsFixedLengthAnsiString(4).NotNullable();

            Create
               .Table("ShortCycleWorkOrderDurationUnits")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(3).NotNullable();

            Create
               .Table("ShortCycleWorkCenters")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(8).NotNullable();

            Create
               .Table("ShortCycleWorkOrderServiceTypes")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(2).NotNullable();

            Create
               .Table("ShortCycleWorkOrderPriorities")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(20).NotNullable()
               .WithColumn("Level").AsInt32().NotNullable();

            Create
               .Table("ShortCycleWorkOrders")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
               .WithColumn("FunctionalLocation").AsString(30).Nullable()
               .WithColumn("SafetyConcern").AsCustom("text").Nullable()
               .WithColumn("NotificationNumber").AsString(12).Nullable()
               .WithColumn("BackReportingType").AsInt32().Nullable()
               .WithColumn("CompanyCode").AsInt32().Nullable()
               .WithColumn("PremiseNumber").AsString(10).Nullable()
               .WithColumn("WorkOrder").AsInt64().NotNullable()
               .WithColumn("Status").AsString(10).Nullable()
               .WithColumn("WBSElement").AsString(35).Nullable()
               .WithColumn("MATCode").AsString(3).Nullable()
               .WithColumn("MATCodeDescription").AsString(30).Nullable()
               .WithColumn("OperationText").AsString(40).Nullable()
               .WithColumn("FSRId").AsInt32().Nullable()
               .WithColumn("FSRName").AsString(81).Nullable()
               .WithColumn("CustomerNumber").AsString(10).Nullable()
               .WithColumn("NextReplacementYear").AsInt32().Nullable()
               .WithColumn("MeterSerialNumber").AsString(18).Nullable()
               .WithColumn("ManufacturerSerialNumber").AsString(30).Nullable()
               .WithColumn("IsCustomerEnrolledForEmail").AsBoolean().Nullable()
               .WithColumn("IsUpdate").AsBoolean().NotNullable()
               .WithColumn("OrderType").AsInt32().Nullable()
               .WithColumn("OperationId").AsInt32().Nullable()
               .WithColumn("CustomerAccount").AsString(12).Nullable()
               .WithColumn("NormalDuration ").AsInt32().Nullable()
               .WithColumn("PlannerGroup").AsInt32().Nullable()
               .WithColumn("ActiveMQStatus").AsString(-1).Nullable()
               .WithColumn("PhoneAhead").AsBoolean().NotNullable()
               .WithColumn("CustomerAtHome").AsBoolean().NotNullable()
               .WithColumn("OperationNumber").AsInt32().Nullable()
               .WithColumn("ModifiedBy").AsString(12).Nullable()
               .WithColumn("TimeModified").AsString(20).Nullable()
               .WithColumn("ReasonCodeComments").AsCustom("text").Nullable()
               .WithColumn("WorkOrderLongText").AsCustom("text").Nullable()
               .WithColumn("NotificationLongText").AsCustom("text").Nullable()
               .WithColumn("CrewSize").AsInt32().Nullable()
               .WithColumn("ReceivedAt").AsDateTime().Nullable()
               .WithColumn("Installation").AsString(10).Nullable()
               .WithColumn("PremiseAddress").AsString(255).Nullable()
               .WithColumn("BeforeAddress").AsString(255).Nullable()
               .WithColumn("AfterAddress").AsString(255).Nullable()
               .WithColumn("CoordinateId").AsInt32().Nullable()
               .WithColumn("MatCodeEscalator").AsString(30).Nullable()
               .WithColumn("AmountDue").AsDecimal(18, 2).Nullable()
               .WithColumn("CustomerName").AsString(40).Nullable()
               .WithColumn("ContactNumber").AsString(30).Nullable()
               .WithColumn("LandlordBusinessPartnerNumber").AsString(10).Nullable()
               .WithColumn("LandlordConnectionContractNumber").AsString(12).Nullable()
               .WithColumn("ConsecutiveIncompletesOnPremise").AsBoolean().Nullable()
               .WithColumn("ReplacementMeterFlag").AsBoolean().Nullable()
               .WithColumn("ReplacementMeterDescription").AsInt32().Nullable()
               .WithColumn("ServiceLineSize").AsString(50).Nullable()
               .WithColumn("MeterReadingUnit").AsString(8).Nullable()
               .WithColumn("LeakDetectedLastVisit").AsBoolean().Nullable()
               .WithColumn("DoNotUnschedule").AsBoolean().NotNullable()
               .WithColumn("LastDispatched").AsDateTime().Nullable()
               .WithColumn("AssignmentStart").AsDateTime().Nullable()
               .WithColumn("AssignmentEnd").AsDateTime().Nullable()
               .WithColumn("AppointmentStart").AsDateTime().Nullable()
               .WithColumn("AppointmentEnd").AsDateTime().Nullable()
               .WithColumn("DueDateTime").AsDateTime().Nullable()
               .WithColumn("SAPCommunicationStatusId").AsInt32().NotNullable()
               .WithColumn("HasSAPError").AsBoolean().NotNullable()
               .WithColumn("EarlyStartDate").AsDateTime().Nullable()
               .WithColumn("ReconnectionFee").AsDecimal(18, 2).Nullable()
               .WithColumn("ReconnectionFeeWaived").AsBoolean().Nullable()
               .WithColumn("InternalLeadPipingIndicator").AsBoolean().Nullable()
               .WithColumn("CustomerSideMaterialId").AsInt32().Nullable()
               .WithColumn("LocalityId").AsInt32().Nullable()
               .WithColumn("DistrictId").AsInt32().Nullable()
               .WithColumn("CriticalCareTypeId").AsInt32().Nullable()
               .WithColumn("SewerAuthorityCode1Id").AsInt32().Nullable()
               .WithColumn("SewerAuthorityCode2Id").AsInt32().Nullable()
               .WithColumn("SewerAuthorityCode3Id").AsInt32().Nullable()
               .WithColumn("WaterLineProtection").AsBoolean().Nullable()
               .WithColumn("SewerLineProtection").AsBoolean().Nullable()
               .WithColumn("InHomeProtection").AsBoolean().Nullable()
               .WithColumn("ServiceTypeId").AsInt32().Nullable()
               .WithColumn("PriorityId").AsInt32().Nullable()
               .WithColumn("WorkCenterId").AsInt32().Nullable()
               .WithColumn("NormalDurationUnitId").AsInt32().Nullable()
               .WithColumn("LiabilityIndicatorId").AsInt32().Nullable()
               .WithColumn("FixedChargeNoMeterId").AsInt32().Nullable()
               .WithColumn("DunningLockId").AsInt32().Nullable()
               .WithColumn("PremiseTypeId").AsInt32().Nullable()
               .WithColumn("LandlordAllocationId").AsInt32().Nullable()
               .WithColumn("ServiceFoundId").AsInt32().Nullable()
               .WithColumn("PlanningPlantId").AsInt32().Nullable()
               .WithColumn("ReasonCodeId").AsInt32().Nullable()
               .WithColumn("PremiseId").AsInt32().Nullable();

            Create
               .Table("FieldServiceRepresentativeNonAvailabilities")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("EngineerId").AsString(64).Nullable()
               .WithColumn("NonAvailabilityType").AsString(64).Nullable()
               .WithColumn("Comment").AsCustom("text").Nullable()
               .WithColumn("Start").AsDateTime().NotNullable()
               .WithColumn("Finish").AsDateTime().NotNullable()
               .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
               .WithColumn("ReceivedAt").AsDateTime().NotNullable()
               .WithForeignKeyColumn(
                    "SAPCommunicationStatusId",
                    "SAPCommunicationStatuses",
                    nullable: false)
               .WithColumn("HasSAPError").AsBoolean().NotNullable();

            Create
               .Table("FSRNonAvailabilitiesEngineers")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithForeignKeyColumn(
                    "FieldServiceRepresentativeNonAvailabilityId",
                    "FieldServiceRepresentativeNonAvailabilities",
                    nullable: false)
               .WithColumn("EngineerId").AsString(64).NotNullable();

            Create
               .Table("BusinessProcessExceptionManagementCases")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("CaseCategory").AsAnsiString(4).NotNullable()
               .WithColumn("CasePriority").AsAnsiString(1).Nullable()
               .WithColumn("AuthorizationGroup").AsAnsiString(4).Nullable()
               .WithColumn("ObjectType").AsAnsiString(10).NotNullable()
               .WithColumn("ObjectKey").AsAnsiString(70).NotNullable()
               .WithColumn("OriginalDateOfClarificationCase").AsAnsiString(10).Nullable()
               .WithColumn("CreationTimeOfClarificationCase").AsAnsiString(8).Nullable()
               .WithColumn("LogicalSystem").AsAnsiString(10).Nullable()
               .WithColumn("CompanyCode").AsAnsiString(4).Nullable()
               .WithColumn("BusinessPartnerNumber").AsAnsiString(10).Nullable()
               .WithColumn("ContractAccountNumber").AsAnsiString(12).Nullable()
               .WithColumn("Premise").AsAnsiString(10).Nullable()
               .WithColumn("ReceivedAt").AsDateTime().Nullable()
               .WithColumn("SAPErrorCode").AsCustom("ntext").Nullable()
               .WithColumn("CaseNumber").AsString(20).Nullable()
               .WithForeignKeyColumn(
                    "SAPCommunicationStatusId",
                    "SAPCommunicationStatuses",
                    nullable: false)
               .WithColumn("HasSAPError").AsBoolean().NotNullable();

            Create
               .Table("ShortCycleWorkOrderTimeConfirmationUnitsOfMeasure")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(3).NotNullable();

            Create
               .Table("ShortCycleWorkOrderRequestMaintenanceActivityTypes")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(3).NotNullable();

            Create
               .Table("ShortCycleWorkOrderRequests")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
               .WithColumn("Installation").AsString(10).NotNullable()
               .WithColumn("BusinessPartnerNumber").AsString(10).NotNullable()
               .WithColumn("ContractAccount").AsString(12).NotNullable()
               .WithColumn("DeviceLocation").AsString(30).NotNullable()
               .WithColumn("EquipmentNumber").AsString(18).NotNullable()
               .WithColumn("SerialNumber").AsString(30).NotNullable()
               .WithColumn("WorkDescription").AsCustom("text").Nullable()
               .WithColumn("WorkOrderNumber").AsString(40).Nullable()
               .WithColumn("Status").AsString(10).Nullable()
               .WithColumn("CreatedBy").AsString(12).Nullable()
               .WithColumn("ReceivedAt").AsDateTime().Nullable()
               .WithColumn("UniqueId").AsString(50).Nullable()
               .WithColumn("CompanyName").AsString(255).Nullable()
               .WithColumn("CompanyNumber").AsString(15).Nullable()
               .WithColumn("ContractorName").AsString(255).Nullable()
               .WithColumn("ContractorPhone").AsString(15).Nullable()
               .WithColumn("DayFrom").AsString(10).Nullable()
               .WithColumn("DayTo").AsString(10).Nullable()
               .WithColumn("HourAM").AsString(10).Nullable()
               .WithColumn("HourPM").AsString(10).Nullable()
               .WithColumn("LetterId").AsString(12).Nullable()
               .WithColumn("Telephone").AsString(20).Nullable()
               .WithColumn("FSRId").AsInt32().Nullable()
               .WithForeignKeyColumn(
                    "SAPCommunicationStatusId",
                    "SAPCommunicationStatuses",
                    nullable: false)
               .WithColumn("HasSAPError").AsBoolean().NotNullable()
               .WithForeignKeyColumn(
                    "MaintenanceActivityTypeId",
                    "ShortCycleWorkOrderRequestMaintenanceActivityTypes");

            Create
               .Table("ShortCycleWorkOrderPointsOfControl")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(4).NotNullable();

            Create
               .Table("ShortCycleWorkOrderEquipmentInstallationTypes")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(4).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionActionFlags")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(2).NotNullable();

            Create
               .Table("ShortCycleSecureAccessTypes")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(100).NotNullable()
               .WithColumn("SAPCode").AsString(2).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletions")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("ShortCycleWorkOrderId").AsInt32().Nullable()
               .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
               .WithColumn("ActiveMQStatus").AsString(-1).Nullable()
               .WithColumn("WorkOrderNumber").AsInt64().NotNullable()
               .WithColumn("MiscInvoice").AsString(5).Nullable()
               .WithColumn("BackOfficeReview").AsString(5).Nullable()
               .WithColumn("CompletionStatus").AsString(5).Nullable()
               .WithColumn("Notes").AsCustom("text").Nullable()
               .WithColumn("TechnicalInspectedBy").AsString(12).Nullable()
               .WithColumn("AdditionalInformation").AsCustom("text").Nullable()
               .WithColumn("CurbBoxMeasurementDescription").AsCustom("text").Nullable()
               .WithColumn("Safety").AsCustom("text").Nullable()
               .WithColumn("FSRId").AsInt32().Nullable()
               .WithColumn("SerialNumber").AsString(18).Nullable()
               .WithColumn("MeterSerialNumber").AsString(18).Nullable()
               .WithColumn("DeviceCategory").AsString(18).Nullable()
               .WithColumn("Installation").AsString(10).Nullable()
               .WithColumn("OldMeterSerialNumber").AsString(18).Nullable()
               .WithColumn("ReceivedAt").AsDateTime().NotNullable()
               .WithColumn("ManufacturerSerialNumber").AsString(255).Nullable()
               .WithColumn("InvestigationExpiryDate").AsDateTime().Nullable()
               .WithColumn("NotificationItemText").AsString(40).Nullable()
               .WithColumn("NeedTwoManCrew").AsBoolean().Nullable()
               .WithColumn("CoordinateId").AsInt32().Nullable()
               .WithColumn("LeakDetectedNonCompany").AsBoolean().Nullable()
               .WithColumn("LeakDetectedDate").AsDateTime().Nullable()
               .WithColumn("TechnicalInspectedOn").AsDateTime().Nullable()
               .WithColumn("SAPCommunicationStatusId").AsInt32().NotNullable()
               .WithColumn("HasSAPError").AsBoolean().NotNullable()
               .WithColumn("InspectionDate").AsDateTime().Nullable()
               .WithColumn("InspectionPassed").AsBoolean().Nullable()
               .WithColumn("InternalLeadPipingIndicator").AsBoolean().Nullable()
               .WithColumn("CustomerSideMaterialId").AsInt32().Nullable()
               .WithColumn("LeadInspectionDate").AsDateTime().Nullable()
               .WithColumn("LeadInspectedBy").AsString(9).Nullable()
               .WithColumn("AdditionalWorkNeededId").AsInt32().Nullable()
               .WithColumn("PurposeId").AsInt32().Nullable()
               .WithColumn("OperatedPointOfControlId").AsInt32().Nullable()
               .WithColumn("HeatTypeId").AsInt32().Nullable()
               .WithColumn("ActionFlagId").AsInt32().Nullable()
               .WithColumn("ActivityReasonId").AsInt32().Nullable()
               .WithColumn("Register1ReasonCodeId").AsInt32().Nullable()
               .WithColumn("FSRInteractionId").AsInt32().Nullable()
               .WithColumn("ServiceFoundId").AsInt32().Nullable()
               .WithColumn("ServiceLeftId").AsInt32().Nullable()
               .WithColumn("MeterPositionLocationId").AsInt32().Nullable()
               .WithColumn("MeterDirectionalLocationId").AsInt32().Nullable()
               .WithColumn("MeterSupplementalLocationId").AsInt32().Nullable()
               .WithColumn("ReadingDeviceDirectionalLocationId").AsInt32().Nullable()
               .WithColumn("ReadingDeviceSupplementalLocationId").AsInt32().Nullable()
               .WithColumn("ReadingDevicePositionalLocationId").AsInt32().Nullable()
               .WithColumn("Register2ReasonCodeId").AsInt32().Nullable()
               .WithColumn("SecureAccessTypeId").AsInt32().Nullable();

            Create
               .Table("ShortCycleWorkOrderCompletionViolationCodes")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("ShortCycleWorkOrderCompletionId").AsInt32().Nullable()
               .WithColumn("ViolationCode").AsInt32().NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionTestResults")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("ShortCycleWorkOrderCompletionId").AsInt32().Nullable()
               .WithColumn("RegisterId").AsInt32().Nullable()
               .WithColumn("Accuracy").AsString(50).Nullable()
               .WithColumn("CalculatedVolume").AsString(50).Nullable()
               .WithColumn("TestFlowRate").AsString(50).Nullable()
               .WithColumn("StartRead").AsString(50).Nullable()
               .WithColumn("EndRead").AsString(50).Nullable()
               .WithColumn("LowMedHighIndicatorId").AsInt32().Nullable()
               .WithColumn("InitialRepairId").AsInt32().Nullable()
               .WithColumn("Result").AsBoolean().Nullable();

            Create
               .Table("ShortCycleWorkOrderCompletionTestResultLowMedHighIndicators")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(6).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionTestResultInitialRepairs")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(2).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionsQualityIssues")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("ShortCycleWorkOrderCompletionId").AsInt32().NotNullable()
               .WithColumn("QualityIssueId").AsInt32().Nullable();

            Create
               .Table("ShortCycleWorkOrderCompletionsActivities")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("ShortCycleWorkOrderCompletionId").AsInt32().NotNullable()
               .WithColumn("DescriptionId").AsInt32().Nullable();

            Create
               .Table("ShortCycleWorkOrderCompletionRegisters")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("ShortCycleWorkOrderCompletionId").AsInt32().Nullable()
               .WithColumn("Size").AsString(50).Nullable()
               .WithColumn("MIUNumber").AsString(30).Nullable()
               .WithColumn("EncoderId").AsString(50).Nullable()
               .WithColumn("OldRead").AsString(32).Nullable()
               .WithColumn("ReadType").AsString(30).Nullable()
               .WithColumn("Dials").AsString(40).Nullable()
               .WithColumn("NewRead").AsString(32).Nullable();

            Create
               .Table("ShortCycleWorkOrderCompletionRegisterReasonCodes")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(30).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionQualityIssueDescriptions")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(3).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionPurposes")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(40).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionHeatTypes")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(30).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionFSRInteractions")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(30).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionAdditionalWorkNeeded")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(40).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionActivityReasons")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(4).NotNullable();

            Create
               .Table("ShortCycleWorkOrderCompletionActivityDescriptions")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(4).NotNullable();

            Create
               .Table("ShortCycleAssignmentNumbers")
               .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
               .WithColumn("Description").AsAnsiString(4).NotNullable();

            Create
               .Table("ShortCycleAssignments")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("CallId").AsInt64().NotNullable()
               .WithColumn("Status").AsString(64).Nullable()
               .WithColumn("Engineer").AsString(64).Nullable()
               .WithColumn("Start").AsDateTime().NotNullable()
               .WithColumn("Finish").AsDateTime().NotNullable()
               .WithColumn("SAPErrorCode").AsAnsiString(-1).Nullable()
               .WithColumn("ReceivedAt").AsDateTime().NotNullable()
               .WithColumn("SAPCommunicationStatusId").AsInt32().NotNullable()
               .WithColumn("HasSAPError").AsBoolean().NotNullable()
               .WithColumn("AssignmentNumberId").AsInt32().Nullable()
               .WithColumn("IsUpdate").AsBoolean().Nullable()
               .WithColumn("ShortCycleWorkOrderId").AsInt32().NotNullable();

            Create
               .Table("ShortCycleWorkOrderTimeConfirmations")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("ShortCycleWorkOrderId").AsInt32().Nullable()
               .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
               .WithColumn("WorkOrderNumber").AsInt64().NotNullable()
               .WithColumn("OperationId").AsInt32().Nullable()
               .WithColumn("PersonnelNumber").AsString(8).Nullable()
               .WithColumn("ActualWork").AsDecimal(5, 2).Nullable()
               .WithColumn("ActivityType").AsString(6).Nullable()
               .WithColumn("FinalConfirmation").AsString(1).Nullable()
               .WithColumn("NoRemainingWork").AsString(1).Nullable()
               .WithColumn("Reason").AsString(4).Nullable()
               .WithColumn("ConfirmationText").AsString(40).Nullable()
               .WithColumn("ReceivedAt").AsDateTime().NotNullable()
               .WithColumn("DateCompleted").AsDateTime().Nullable()
               .WithColumn("WorkStartDateTime").AsDateTime().Nullable()
               .WithColumn("WorkFinishDateTime").AsDateTime().Nullable()
               .WithColumn("SAPCommunicationStatusId").AsInt32().NotNullable()
               .WithColumn("HasSAPError").AsBoolean().NotNullable()
               .WithColumn("WorkCenterId").AsInt32().Nullable()
               .WithColumn("UnitOfMeasureId").AsInt32().Nullable();

            Create
               .Table("ShortCycleWorkOrdersEquipmentIds")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("ShortCycleWorkOrderId").AsInt32().Nullable()
               .WithColumn("EquipmentId").AsString(18).Nullable()
               .WithColumn("ProcessingIndicator").AsString(1).Nullable()
               .WithColumn("DeviceCategory").AsString(18).Nullable()
               .WithColumn("DeviceLocation").AsString(30).Nullable()
               .WithColumn("Installation").AsString(10).Nullable()
               .WithColumn("ServiceTypeId").AsInt32().Nullable()
               .WithColumn("InstallationTypeId").AsInt32().Nullable();

            Create
               .Table("ShortCycleWorkOrderSecurityThreats")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("ShortCycleWorkOrderId").AsInt32().NotNullable()
               .WithColumn("SecurityThreat").AsCustom("text").Nullable()
               .WithColumn("PoliceEscort").AsBoolean().Nullable()
               .WithColumn("ThreatStart").AsDateTime().Nullable()
               .WithColumn("ThreatEnd").AsDateTime().Nullable()
               .WithColumn("PoliceEscortActive").AsBoolean().Nullable()
               .WithColumn("Address").AsString(150).Nullable();

            Create
               .Table("ShortCycleWorkOrderStatusUpdates")
               .WithColumn("Id").AsInt32().Identity().NotNullable()
               .WithColumn("ShortCycleWorkOrderId").AsInt32().Nullable()
               .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
               .WithColumn("WorkOrderNumber").AsInt64().NotNullable()
               .WithColumn("OperationNumber").AsInt32().Nullable()
               .WithColumn("StatusNumber").AsString(10).Nullable()
               .WithColumn("AssignedEngineer").AsInt32().Nullable()
               .WithColumn("DispatcherId").AsString(12).Nullable()
               .WithColumn("EngineerId").AsString(12).Nullable()
               .WithColumn("ReceivedAt").AsDateTime().Nullable()
               .WithColumn("StatusNonNumber").AsString(5).Nullable()
               .WithColumn("StatusNotes").AsCustom("text").Nullable()
               .WithColumn("ItemTimeStamp").AsDateTime().Nullable()
               .WithColumn("AssignmentStart").AsDateTime().Nullable()
               .WithColumn("AssignmentFinish").AsDateTime().Nullable()
               .WithColumn("SAPCommunicationStatusId").AsInt32().NotNullable()
               .WithColumn("HasSAPError").AsBoolean().NotNullable();

            Create
               .ForeignKey("FK_ShortCycleWorkOrders_Coordinates_CoordinateId")
               .FromTable("ShortCycleWorkOrders")
               .ForeignColumn("CoordinateId")
               .ToTable("Coordinates")
               .PrimaryColumn("CoordinateId");
        }
    }
}
