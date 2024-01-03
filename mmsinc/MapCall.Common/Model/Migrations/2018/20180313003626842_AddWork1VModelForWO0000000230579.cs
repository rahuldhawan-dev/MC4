using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180313003626842), Tags("Production")]
    public class AddWork1VModelForWO0000000230579 : Migration
    {
        private struct StringLengths
        {
            public const int
                AFTER_ADDRESS = 255,
                ASSIGNMENT_END = 20,
                ASSIGNMENT_START = 20,
                APPOINTNMENT_END = 20,
                APPOINTMENT_START = 20,
                BACK_REPORTING_TYPE = 2,
                BASIC_START_DATE = 10,
                BASIC_START_TIME = 10,
                BEFORE_ADDRESS = 255,
                CREATE_DATE = 10,
                CREATE_TIME = 10,
                COMPANY_CODE = 4,
                CONTACT_NUMBER = 30,
                CUSTOMER_ACCOUNT = 12,
                CUSTOMER_NAME = 40,
                CUSTOMER_NUMBER = 10,
                DISPATCHER_ID = 12,
                DUE_DATE = 8,
                DUE_TIME = 6,
                DUNNING_LOCK = 40,
                ENGINEER_ID = 12,
                EQUIPMENT_ID = 18,
                FIXED_CHARGE_NO_METER = 5,
                FUNCTIONAL_LOCATION = 30,
                FIELD_SERVICE_REP_NAME = 81,
                INSTALLATION = 10,
                INSTALLATION_DATE = 8,
                LANDLORD_ALLOCATION = 30,
                LANDLORD_BUSINESS_PARTNER_NUMBER = 10,
                LANDLORD_CONNECTION_CONTRACT_NUMBER = 12,
                LIABILITY_INDICATOR = 80,
                LONG_TEXT = 40, // no longer applicable, here for old migration
                MANUFACTURER = 30,
                MANUFACTURER_SERIAL_NUMBER = 30,
                MAT_CODE = 3,
                MAT_CODE_DESCRIPTION = 30,
                MAT_CODE_ESCALATOR = 30,
                METER_READING_UNIT = 8,
                METER_SERIAL_NUMBER = 18,
                MODIFIED_BY = 12,
                NORMAL_DURATION_UNIT = 3,
                NOTIFICATION_NUMBER = 12,
                OPERATION_NUMBER = 4,
                OPERATION_TEXT = 40,
                OPERATION_ID = 4,
                ORDER_TYPE = 4,
                PLANNER_GROUP = 3,
                PLANNING_PLANT = 4,
                POLICE_ESCORT = 3,
                PREMISE = 10,
                PREMISE_ADDRESS = 255,
                PREMISE_TYPE = 40,
                PRIORITY = 20,
                REASON_CODE = 35,
                SAFETY_CONCERN = 30,
                SERVICE_FOUND = 40,
                SERVICE_LINE_SIZE = 50,
                TIME_MODIFIED = 20,
                WORK_CENTER = 8,
                WORK_ORDER_NUMBER = 12,
                SERVICE_TYPE = 2,
                STATUS = 10,
                WBS_NUMBER = 35;
        }

        public override void Up()
        {
            Create.Table("ShortCycleWorkOrders")
                  .WithIdentityColumn()
                  .WithColumn("SAPCommunicationError").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
                   //FOR CREATE PRE-DISPATCH
                  .WithColumn("EquipmentId").AsAnsiString(StringLengths.EQUIPMENT_ID).Nullable()
                  .WithColumn("FunctionalLocation").AsAnsiString(StringLengths.FUNCTIONAL_LOCATION).Nullable()
                  .WithColumn("SafetyConcern").AsAnsiString(StringLengths.SAFETY_CONCERN).Nullable()
                  .WithColumn("NotificationNumber").AsAnsiString(StringLengths.NOTIFICATION_NUMBER).Nullable()
                  .WithColumn("Priority").AsAnsiString(StringLengths.PRIORITY).Nullable()
                  .WithColumn("BackReportingType").AsAnsiString(StringLengths.BACK_REPORTING_TYPE).Nullable()
                  .WithColumn("CompanyCode").AsAnsiString(StringLengths.COMPANY_CODE).Nullable()
                  .WithColumn("Premise").AsAnsiString(StringLengths.PREMISE).Nullable()
                  .WithColumn("WorkOrder").AsAnsiString(40).Nullable()
                  .WithColumn("Status").AsAnsiString(StringLengths.STATUS).Nullable()
                  .WithColumn("WBSElement").AsAnsiString(StringLengths.WBS_NUMBER).Nullable()
                  .WithColumn("PlanningPlant").AsAnsiString(StringLengths.PLANNING_PLANT).Nullable()
                  .WithColumn("MATCode").AsAnsiString(StringLengths.MAT_CODE).Nullable()
                  .WithColumn("MATCodeDescription").AsAnsiString(StringLengths.MAT_CODE_DESCRIPTION).Nullable()
                  .WithColumn("OperationText").AsAnsiString(StringLengths.OPERATION_TEXT).Nullable()
                  .WithColumn("FSRId").AsInt32().Nullable()
                  .WithColumn("FSRName").AsAnsiString(StringLengths.FIELD_SERVICE_REP_NAME).Nullable()
                  .WithColumn("CustomerNumber").AsAnsiString(StringLengths.CUSTOMER_NUMBER).Nullable()
                  .WithColumn("NextReplacementYear").AsInt32().Nullable()
                  .WithColumn("ServiceType").AsAnsiString(StringLengths.SERVICE_TYPE).Nullable()
                  .WithColumn("MeterSerialNumber").AsAnsiString(StringLengths.METER_SERIAL_NUMBER).Nullable()
                  .WithColumn("ManufacturerSerialNumber").AsAnsiString(StringLengths.MANUFACTURER_SERIAL_NUMBER)
                  .Nullable()
                  .WithColumn("IsCustomerEnrolledForEmail").AsBoolean().Nullable().WithDefaultValue(false)
                  .WithColumn("SecurityThreat").AsCustom("text").Nullable()
                  .WithColumn("PoliceEscort").AsAnsiString(StringLengths.POLICE_ESCORT).Nullable()
                  .WithColumn("IsUpdate").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("AssignmentStart").AsAnsiString(StringLengths.ASSIGNMENT_START).Nullable()
                  .WithColumn("AssignmentEnd").AsAnsiString(StringLengths.ASSIGNMENT_END).Nullable()
                  .WithColumn("OrderType").AsAnsiString(StringLengths.ORDER_TYPE).Nullable()
                  .WithColumn("OperationId").AsAnsiString(StringLengths.OPERATION_ID).Nullable()
                  .WithColumn("WorkCenter").AsInt32().Nullable()
                  .WithColumn("CustomerAccount").AsAnsiString(StringLengths.CUSTOMER_ACCOUNT).Nullable()
                  .WithColumn("NormalDuration ").AsInt32().Nullable()
                  .WithColumn("NormalDurationUnit ").AsAnsiString(StringLengths.NORMAL_DURATION_UNIT).Nullable()
                  .WithColumn("PlannerGroup").AsAnsiString(StringLengths.PLANNER_GROUP).Nullable();
            Create.Table("ShortCycleWorkOrderStatusUpdates")
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ShortCycleWorkOrderId", "ShortCycleWorkOrders")
                  .WithColumn("SAPCommunicationError").AsBoolean().NotNullable().WithDefaultValue(false)
                  .WithColumn("SAPErrorCode").AsCustom("text").Nullable()
                  .WithColumn("WorkOrderNumber").AsAnsiString(StringLengths.WORK_ORDER_NUMBER).Nullable()
                  .WithColumn("OperationNumber").AsAnsiString(StringLengths.OPERATION_NUMBER).Nullable()
                  .WithColumn("AssignmentStart").AsAnsiString(StringLengths.ASSIGNMENT_START).Nullable()
                  .WithColumn("AssignmentFinish").AsAnsiString(StringLengths.ASSIGNMENT_END).Nullable()
                  .WithColumn("Status").AsAnsiString(StringLengths.STATUS).Nullable()
                  .WithColumn("AssignedEngineer").AsInt32().Nullable()
                  .WithColumn("DispatcherId").AsAnsiString(StringLengths.ENGINEER_ID).Nullable()
                  .WithColumn("EngineerId").AsAnsiString(StringLengths.ENGINEER_ID).Nullable()
                  .WithColumn("CreateDateTime").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("ShortCycleWorkOrderStatusUpdates");
            Delete.Table("ShortCycleWorkOrders");
        }
    }
}
