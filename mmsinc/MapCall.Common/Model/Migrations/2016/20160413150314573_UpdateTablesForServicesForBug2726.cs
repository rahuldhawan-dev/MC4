using System;
using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Data;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20160413150314573), Tags("Production")]
    public class UpdateTablesForServicesForBug2726 : Migration
    {
        #region Constants

        public struct TableNamesOld
        {
            public const string
                INSTALLATION_MATERIALS = "tblNJAWInstMatl",
                INSTALLATION_PURPOSES = "tblNJAWPurpInst",
                OPERATING_CENTERS_SERVICE_CATEGORIES = "tblNJAWCategoryService",
                OPERATING_CENTERS_SERVICE_MATERIALS = "tblNJAWServMatl",
                RESTORATIONS = "tblNJAWRestore",
                SERVICE_PRIORITY = "tblNJAWPriority",
                SERVICE_RESTORATION_CONTRACTORS = "tblNJAWContractor",
                SERVICE_RESTORATION_METHODS = "tblNJAWTypeRestore",
                SERVICE_SIZES = "tblNJAWSizeServ",
                SERVICES = "tblNJAWService";
        }

        public struct TableNames
        {
            public const string
                INSTALLATION_MATERIALS = "ServiceInstallationMaterials",
                INSTALLATION_PURPOSES = "ServiceInstallationPurposes",
                OPERATING_CENTERS = "OperatingCenters",
                OPERATING_CENTERS_SERVICE_CATEGORIES = "OperatingCentersServiceCategories",
                OPERATING_CENTERS_SERVICE_MATERIALS = "OperatingCentersServiceMaterials",
                RESTORATION_METHODS = "RestorationMethods",
                RESTORATION_TYPES = "RestorationTypes",
                RESTORATIONS = "ServiceRestorations",
                SERVICE_PRIORITY = "ServicePriorities",
                SERVICE_CATEGORIES = "ServiceCategories",
                SERVICE_TYPES = "ServiceTypes",
                SERVICE_RESTORATION_CONTRACTORS = "ServiceRestorationContractors",
                SERVICE_RESTORATION_METHODS = "ServiceRestorationMethods",
                SERVICE_SIZES = "ServiceSizes",
                SERVICES = "Services",
                SMART_GROWTH_METHODS = "SmartGrowthMethods",
                STATES = "States",
                STREET_MATERIAL = "StreetMaterials",
                STREETS = "Streets";
        }

        public struct ColumnNamesOld
        {
            public const string
                P_ID = "RecID",
                P_SERVICE_CATEGORY = "CatOfServ",
                P_PART_SIZE = "ptSize",
                P_PART_QUANTITY = "ptQty",
                P_OPERATING_CENTER = "opCntr",
                P_SIZE_OF_SERVICE = "SizeOfService",
                P_ORDER = "RecOrd",
                R_ID = "RecID",
                R_APPROVED_BY = "AppBy",
                R_APPROVED_ON = "AppDate",
                R_CANCEL = "Cancel",
                R_ESTIMATED_RESTORATION_AMOUNT = "EstRestAmnt",
                R_FINAL_COMPLETION_BY = "FinalCompBy",
                R_FINAL_COST = "FinalCost",
                R_FINAL_DATE = "FinalDate",
                R_FINAL_INVOICE_NUMBER = "FinalInvNum",
                R_FINAL_RESTORATION_METHOD = "FinalMethod",
                R_FINAL_RESTORATION_AMOUNT = "FinalRestAmnt",
                R_FINAL_TRAFFIC = "FinalTraffic",
                R_INITIATED_BY = "InitiatedBy",
                R_NOTES = "Notes",
                R_PARTIAL_COMPLETION_BY = "PartCompBy",
                R_PARTIAL_RESTORATION_DATE = "PartDate",
                R_PARTIAL_INVOICE_NUMBER = "PartInvNum",
                R_PARTIAL_METHOD = "PartMethod",
                R_PARTIAL_RESTORATION_AMOUNT = "PartRestAmnt",
                R_PARTIAL_RESTORATION_COST = "PartCost",
                R_PARTIAL_TRAFFIC_CONTROL_HOURS = "PartTraffic",
                R_REJECTED_BY = "RejBy",
                R_REJECTED_ON = "RejDate",
                R_SERVICE_ID = "ServID",
                R_RESTORATION_TYPE = "TypeRestore",
                R_PURCHASE_ORDER_NUMBER = "PurchaseOrderNumber",
                R_ESTIMATED_VALUE = "EstimatedValue",
                SS_RECID = "RecID",
                SS_HYDRANT = "Hyd",
                SS_LAT = "Lat",
                SS_ORDER = "RecOrd",
                SS_SERVICE = "Serv",
                SS_SIZE_SERVICE = "Valve",
                SS_SIZE_SERVICE_DESCRIPTION = "SizeServ",
                OCSM_ID = "RecID",
                OCSM_SERVICE_MATERIAL = "ServMatl",
                OCSM_OPERATING_CENTER = "OpCntr",
                OCSM_NEW_SERVICE_RECORD = "NSR";

            public const string ID = "RecID",
                                AMOUNT_RECEIVED = "AmntRcvd",
                                APPLICATION_RECEIVED = "ApplRcvd",
                                APPLICATION_APPROVED = "ApplApvd",
                                APPLICATION_SENT = "ApplSent",
                                APARTMENT_NUMBER = "Apt",
                                AGREEMENT = "Agreement",
                                BUREAU_OF_SAFE_DRINKING_WATER_PERMIT_REQUIRED = "BSDWPermit",
                                CATEGORY_OF_SERVICE = "CatOfService",
                                CLEANED_COORDINATES = "CleanedCoordinates",
                                COUNT_RECORD = "CntRec",
                                CROSS_STREET = "CrossStreet",
                                DATE_ADDED = "DateAdded",
                                DEPTH_MAIN = "DepthMain",
                                DEVELOPER_SERVICES_DRIVEN = "DevServD",
                                ISACTIVE = "InActSrv",
                                INITIATOR = "Initiator",
                                INSPECTION_DATE = "InspDate",
                                INSPECTION_STATUS = "InspSignoffReady",
                                INSTALLATION_COST = "InstCost",
                                INSTALLATION_INVOICE_NUMBER = "InstInv",
                                INSTALLATION_INVOICE_DATE = "InstInvDate",
                                LATITUDE = "Lat",
                                LINK = "Link",
                                LONGITUDE = "Lon",
                                LENGTH_OF_SERVICE = "LengthService",
                                MAILING_PHONE_NUMBER = "MailPhoneNum",
                                MAILING_STREET_NAME = "MailStName",
                                MAILING_STREET_NUMBER = "MailStNum",
                                METER_SETTING_REQUIREMENT = "MeterSetReq",
                                OPERATING_CENTER = "OpCntr",
                                ORD_CREATION_DATE = "OrdCreationDate",
                                CREATED_ON = "CreatedOn",
                                ORIGINAL_INSTALLATION_DATE = "OrigInstDate",
                                PARENT_TASK_NUMBER = "ParentTaskNum",
                                PAYMENT_REFERENCE_NUMBER = "PayRefNum",
                                PERMIT_EXPIRATION_DATE = "PermitExpDate",
                                PERMIT_NUMBER = "PermitNum",
                                PERMIT_RECEIVED_DATE = "PermitRcvdDate",
                                PERMIT_TYPE = "PermitType",
                                PHONE_NUMBER = "PhoneNum",
                                PREMISE_NUMBER = "PremNum",
                                PREVIOUS_SERVICE_MATERIAL = "PrevServiceMatl",
                                PREVIOUS_SERVICE_SIZE = "PrevServiceSize",
                                PRIORITY = "Priority",
                                PURPOSE_OF_INSTALLATION = "PurpInstal",
                                REC_ID_PAV = "RecIdPav",
                                RETIRED_DATE = "RetireDate",
                                RETIRED_ACCOUNT_NUMBER = "RetireAcct",
                                ROAD_OPENING_FEE = "RoadOpenFee",
                                SERVICE_INSTALLATION_FEE = "ServInstFee",
                                SERVICE_MATERIAL = "ServMatl",
                                SERVICE_NUMBER = "ServNum",
                                SMART_GROWTH = "SmartGrowth",
                                SMART_GROWTH_METHOD_USED = "SGMethodUsed",
                                SMART_GROWTH_COST = "SGCost",
                                SIZE_OF_MAIN = "SizeOfMain",
                                SIZE_OF_SERVICE = "SizeOfService",
                                SIZE_OF_TAP = "SizeOfTap",
                                STATE = "State",
                                STREET_MATERIAL = "StreetMatl",
                                STREET_NAME = "StName",
                                STREET_NUMBER = "StNum",
                                TAP_ORDER_NOTES = "TapOrdNote",
                                TASK_NUMBER_1 = "TaskNum1",
                                TASK_NUMBER_2 = "TaskNum2",
                                TOWN = "Town",
                                TOWN_SECTION = "TwnSection",
                                MAIN_TYPE = "TypeMain",
                                QUESTIONAIRE_SENT_DATE = "QuesSentDate",
                                QUESTIONAIRE_RECEIVED_DATE = "QuesRecvDate",
                                FLOWBACK_DEVICE = "FlowbackDevice",
                                SERVICE_TYPE = "ServType";
        }

        public struct ColumnNames
        {
            public const string
                P_ID = "Id",
                P_SERVICE_CATEGORY = "ServiceCategoryId",
                P_PART_SIZE = "PartSize",
                P_PART_QUANTITY = "PartQuantity",
                P_OPERATING_CENTER = "OperatingCenterId",
                P_SIZE_OF_SERVICE = "ServiceSizeId",
                P_ORDER = "SortOrder",
                R_ID = "Id",
                R_APPROVED_BY = "ApprovedById",
                R_APPROVED_ON = "ApprovedOn",
                R_CANCEL = "Cancel",
                R_ESTIMATED_RESTORATION_AMOUNT = "EstimatedRestorationAmount",
                R_FINAL_COMPLETION_BY = "FinalRestorationCompletionBy",
                R_FINAL_COST = "FinalRestorationCost",
                R_FINAL_DATE = "FinalRestorationDate",
                R_FINAL_INVOICE_NUMBER = "FinalRestorationInvoiceNumber",
                R_FINAL_RESTORATION_METHOD = "FinalRestorationMethodId",
                R_FINAL_RESTORATION_AMOUNT = "FinalRestorationAmount",
                R_FINAL_TRAFFIC = "FinalRestorationTrafficControlHours",
                R_INITIATED_BY = "InitiatedById",
                R_NOTES = "Notes",
                R_PARTIAL_COMPLETION_BY = "PartialRestorationCompletionBy",
                R_PARTIAL_RESTORATION_DATE = "PartialRestorationDate",
                R_PARTIAL_INVOICE_NUMBER = "PartialRestorationInvoiceNumber",
                R_PARTIAL_METHOD = "PartialRestorationMethodId",
                R_PARTIAL_RESTORATION_AMOUNT = "PartialRestorationAmount",
                R_PARTIAL_RESTORATION_COST = "PartialRestorationCost",
                R_PARTIAL_TRAFFIC_CONTROL_HOURS = "PartialRestorationTrafficControlHours",
                R_REJECTED_BY = "RejectedById",
                R_REJECTED_ON = "RejectedOn",
                R_SERVICE_ID = "ServiceID",
                R_RESTORATION_TYPE = "RestorationTypeId",
                R_PURCHASE_ORDER_NUMBER = "PurchaseOrderNumber",
                R_ESTIMATED_VALUE = "EstimatedValue",
                SS_RECID = "Id",
                SS_HYDRANT = "Hydrant",
                SS_LAT = "Lateral",
                SS_ORDER = "SortOrder",
                SS_MAIN = "Main",
                SS_METER = "Meter",
                SS_SERVICE = "Service",
                SS_SIZE_SERVICE = "Size",
                SS_SIZE_SERVICE_DESCRIPTION = "ServiceSizeDescription",
                OCSM_ID = "Id",
                OCSM_SERVICE_MATERIAL = "ServiceMaterialId",
                OCSM_OPERATING_CENTER = "OperatingCenterId",
                OCSM_NEW_SERVICE_RECORD = "NewServiceRecord";

            public const string ID = "Id",
                                AGREEMENT = "Agreement",
                                AMOUNT_RECEIVED = "AmountReceived",
                                APPLICATION_RECEIVED = "ApplicationReceivedOn",
                                APPLICATION_APPROVED = "ApplicationApprovedOn",
                                APPLICATION_SENT = "ApplicationSentOn",
                                APARTMENT_NUMBER = "ApartmentNumber",
                                BUREAU_OF_SAFE_DRINKING_WATER_PERMIT_REQUIRED =
                                    "BureauOfSafeDrinkingWaterPermitRequired",
                                CATEGORY_OF_SERVICE = "ServiceCategoryId",
                                CLEANED_COORDINATES = "CleanedCoordinates",
                                CREATED_ON = "CreatedOn",
                                CONTACT_DATE = "ContactDate",
                                CROSS_STREET = "CrossStreetId",
                                DATE_CLOSED = "DateClosed",
                                DATE_INSTALLED = "DateInstalled",
                                DATE_ISSUED_TO_FIELD = "DateIssuedToField",
                                DEPTH_MAIN_FEET = "DepthMainFeet",
                                DEPTH_MAIN_INCHES = "DepthMainInches",
                                DEVELOPER_SERVICES_DRIVEN = "DeveloperServicesDriven",
                                ISACTIVE = "IsActive",
                                INITIATOR = "InitiatorId",
                                INSPECTION_DATE = "InspectionDate",
                                INSPECTION_STATUS = "ServiceStatusId",
                                INSTALLATION_COST = "InstallationCost",
                                INSTALLATION_INVOICE_NUMBER = "InstallationInvoiceNumber",
                                INSTALLATION_INVOICE_DATE = "InstallationInvoiceDate",
                                MAILING_PHONE_NUMBER = "MailPhoneNumber",
                                LAST_UPDATED = "LastUpdated",
                                LENGTH_OF_SERVICE = "LengthOfService",
                                MAILING_STREET_NAME = "MailStreetName",
                                MAILING_STREET_NUMBER = "MailStreetNumber",
                                METER_SETTING_REQUIREMENT = "MeterSettingRequirement",
                                OPERATING_CENTER = "OperatingCenterId",
                                ORIGINAL_INSTALLATION_DATE = "OriginalInstallationDate",
                                PARENT_TASK_NUMBER = "ParentTaskNumber",
                                PAYMENT_REFERENCE_NUMBER = "PaymentReferenceNumber",
                                PERMIT_EXPIRATION_DATE = "PermitExpirationDate",
                                PERMIT_NUMBER = "PermitNumber",
                                PERMIT_RECEIVED_DATE = "PermitReceivedDate",
                                PERMIT_SENT_DATE = "PermitSentDate",
                                PERMIT_TYPE = "PermitTypeId",
                                PHONE_NUMBER = "PhoneNumber",
                                PREMISE_NUMBER = "PremiseNumber",
                                PREVIOUS_SERVICE_MATERIAL = "PreviousServiceMaterialId",
                                PREVIOUS_SERVICE_SIZE = "PreviousServiceSizeId",
                                PRIORITY = "ServicePriorityId",
                                PURPOSE_OF_INSTALLATION = "PurposeOfInstallationId",
                                // TODO: RetiredMeterSet
                                // TODO: RetireAcct
                                RETIRED_ACCOUNT_NUMBER = "RetiredAccountNumber",
                                RETIRED_DATE = "RetiredDate",
                                ROAD_OPENING_FEE = "RoadOpeningFee",
                                SERVICE_INSTALLATION_FEE = "ServiceInstallationFee",
                                SERVICE_MATERIAL = "ServiceMaterialId",
                                SERVICE_NUMBER = "ServiceNumber",
                                SMART_GROWTH = "SmartGrowth",
                                SMART_GROWTH_METHOD_USED = "SmartGrowthMethodId",
                                SMART_GROWTH_COST = "SmartGrowthCost",
                                SIZE_OF_MAIN = "MainSizeId",
                                SIZE_OF_SERVICE = "ServiceSizeId",
                                SIZE_OF_TAP = "MeterSettingSizeId",
                                STATE = "StateId",
                                STREET_MATERIAL = "StreetMaterialId",
                                STREET_NAME = "StreetId",
                                STREET_NUMBER = "StreetNumber",
                                TAP_ORDER_NOTES = "TapOrderNotes",
                                TASK_NUMBER_1 = "TaskNumber1",
                                TASK_NUMBER_2 = "TaskNumber2",
                                TOWN = "TownId",
                                TOWN_SECTION = "TownSectionId",
                                MAIN_TYPE = "MainTypeId",
                                QUESTIONAIRE_SENT_DATE = "QuestionaireSentDate",
                                QUESTIONAIRE_RECEIVED_DATE = "QuestionaireReceivedDate",
                                FLOWBACK_DEVICE = "BackflowDeviceId",
                                SERVICE_TYPE = "ServiceType";
        }

        public struct StringLengths
        {
            public const int APARTMENT_NUMBER = 15,
                             BLOCK = 9,
                             DEVELOPMENT = 30,
                             DEPTH_MAIN = 10,
                             FAX = 12,
                             INSTALLATION_INVOICE_NUMBER = 20,
                             INITIATOR = 25,
                             LOT = 9,
                             MAILING_PHONE_NUMBER = 12,
                             MAILING_STREET_NAME = 30,
                             MAILING_STREET_NUMBER = 10,
                             MAIL_TOWN = 30,
                             MAIL_STATE = 2,
                             MAIL_ZIP = 10,
                             NAME = 40,
                             PARENT_TASK_NUMBER = 10,
                             PAYMENT_REFERENCE_NUMBER = 15,
                             PERMIT_NUMBER = 20,
                             PHONE_NUMBER = 12,
                             PREMISE_NUMBER = 10,
                             RETIRE_ACCOUNT = 10,
                             RETIRE_METER_SETTING = 10,
                             SERVICE_NUMBER = 10,
                             SMART_GROWTH_METHOD_USED = 15,
                             STATE = 2,
                             STREET_MATERIAL = 15,
                             STREET_NUMBER = 10,
                             TASK_NUMBER_1 = 18,
                             TASK_NUMBER_2 = 10,
                             WORK_ISSUED_TO = 30,
                             ZIP = 10,
                             BUSINESS_PARTNER = 10,
                             PURCHASE_ORDER_NUMBER = 20,
                             EMAIL = 50,
                             LAT = 15,
                             LON = 15,
                             CROSS_STREET = 30,
                             IS_ACTIVE = 2;
        }

        public struct Sql
        {
            public const string
                UPDATE_RESTORATIONS =
                    "UPDATE [tblNJAWRestore] SET PartMethod = NULL where PartMethod = '';" +
                    "UPDATE [tblNJAWRestore] SET PartMethod = (Select RestorationMethodId from RestorationMethods where Description = '2\" Top Overlay') where PartMethod in ('2 Inch Top', '2\" Top Overlay');" +
                    "UPDATE [tblNJAWRestore] SET PartMethod = (Select RestorationMethodId from RestorationMethods where Description = 'Dirt') where PartMethod = 'Dirt';" +
                    "UPDATE [tblNJAWRestore] SET PartMethod = (Select RestorationMethodId from RestorationMethods where Description = 'Cold Patch') where PartMethod = 'Cold Patch';" +
                    "UPDATE [tblNJAWRestore] SET FinalMethod = NULL where FinalMethod = '';" +
                    "UPDATE [tblNJAWRestore] SET FinalMethod = (Select RestorationMethodId from RestorationMethods where Description = 'Concrete') where FinalMethod in ('Concrete', '4\" Concrete');" +
                    "UPDATE [tblNJAWRestore] SET FinalMethod = (Select RestorationMethodId from RestorationMethods where Description = 'Top Soil and Seed') where FinalMethod in  ('Top Soil / Seed','Top Soil and Seed');" +
                    "UPDATE [tblNJAWRestore] SET FinalMethod = (Select RestorationMethodId from RestorationMethods where Description = 'Sod') where FinalMethod = 'Sod';" +
                    "UPDATE [tblNJAWRestore] SET FinalMethod = (Select RestorationMethodId from RestorationMethods where Description = '6\" Stab. Base and 2\" FABC') where FinalMethod in ('Asphalt','6\" Stab. Base and 2\" FABC');" +
                    "UPDATE [tblNJAWRestore] SET FinalMethod = (Select RestorationMethodId from RestorationMethods where Description = 'Infra Red Restoration') where FinalMethod in ('Infra Red Restoration','Infra Red')" +
                    "UPDATE [tblNJAWRestore] SET TypeRestore = NULL WHERE TypeRestore = '';" +
                    "UPDATE [tblNJAWRestore] SET FinalCompBy = (SELECT src.Id FROM ServiceRestorationContractors src JOIN Services S ON S.Id = ServID WHERE src.Contractor = FinalCompBy and src.OperatingCenterID = s.OperatingCenterId);" +
                    "UPDATE [tblNJAWRestore] SET PartCompBy = (SELECT src.Id FROM ServiceRestorationContractors src JOIN Services S on S.Id = ServID WHERE src.Contractor = PartCompBy and src.OperatingCenterID = s.OperatingCenterId);" +
                    "IF (NOT EXISTS (SELECT 1 From RestorationTypes WHERE Description = 'ASPHALT - ALLEY')) INSERT INTO RestorationTypes Values('ASPHALT - ALLEY');" +
                    "UPDATE [tblNJAWRestore] SET TypeRestore = (Select RestorationTypeId from RestorationTypes where Description = TypeRestore);" +
                    "UPDATE tblNJAWRestore set ServID = null where isNull(ServID, '') = '' OR ServID = '0';" +
                    "ALTER TABLE [dbo].[tblNJAWRestore] DROP CONSTRAINT [DF_tblNJAWRestore_Cancel];" +
                    "IF EXISTS(SELECT 1 FROM sysobjects where name = 'DF_tblNJAWContractor_Pav') ALTER TABLE [dbo].[ServiceRestorationContractors] DROP CONSTRAINT [DF_tblNJAWContractor_Pav];" +
                    "IF EXISTS(SELECT 1 FROM sysobjects where name = 'DF_tblNJAWContractor_Serv')ALTER TABLE [dbo].[ServiceRestorationContractors] DROP CONSTRAINT [DF_tblNJAWContractor_Serv];" +
                    "UPDATE ServiceRestorationContractors SET Serv = '1' where IsNull(Serv, '') = 'ON';" +
                    "UPDATE ServiceRestorationContractors SET Serv = '0' where IsNull(Serv, '') <> '1';" +
                    "UPDATE ServiceRestorationContractors SET Pav = '1' where IsNull(Pav, '') = 'ON';" +
                    "UPDATE ServiceRestorationContractors SET Pav = '0' where IsNull(Pav, '') <> '1';",
                RESTORE_RESTORATIONS =
                    "UPDATE [ServiceRestorations] SET PartMethod = (select Description from RestorationMethods where RestorationMethodID = PartMethod);" +
                    "UPDATE [ServiceRestorations] SET FinalMethod = (select Description from RestorationMethods where RestorationMethodID = FinalMethod);",
                UPDATE_FOREIGN_KEY_NAMES =
                    "exec sp_rename 'FK_tblNJAWService_BackflowDevices_FlowbackDevice', 'FK_Services_BackflowDevices_BackflowDeviceId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_PermitTypes_PermitType', 'FK_Services_PermitTypes_PermitTypeId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_PremiseTypes_PremiseTypeID', 'FK_Services_PremiseTypes_PremiseTypeId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_ServiceCategories_CatofService', 'FK_Services_ServiceCategories_ServiceCategoryId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_ServiceMaterials_PrevServiceMatl', 'FK_Services_ServiceMaterials_PreviousServiceMaterialId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_ServiceMaterials_ServMatl', 'FK_Services_ServiceMaterials_ServiceMaterialId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_ServiceStatuses_InspSignoffReady', 'FK_Services_ServiceStatuses_ServiceStatusId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_tblNJAWPriority_Priority', 'FK_Services_ServicePriorities_ServicePriorityId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_tblNJAWPurpInst_PurpInstal', 'FK_Services_ServiceInstallationPurposes_ServiceInstallationPurposeId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_tblNJAWSizeServ_PrevServiceSize', 'FK_Services_ServiceSizes_PreviousServiceSizeId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_tblNJAWSizeServ_SizeofTap', 'FK_Services_ServiceSizes_MeterSettingSizeId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_tblNJAWSizeServ_SizeOfMain', 'FK_Services_ServiceSizes_MainSizeId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_tblNJAWSizeServ_SizeofService', 'FK_Services_ServiceSizes_ServiceSizeId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_Towns_Town', 'FK_Services_Towns_TownId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_TownSections_TwnSection', 'FK_Services_TownSections_TownSectionId', 'OBJECT';" +
                    "exec sp_rename 'FK_tblNJAWService_OperatingCenters_OpCntr', 'FK_Services_OperatingCenters_OperatingCenterId', 'OBJECT';",
                ROLLBACK_FOREIGN_KEY_NAMES =
                    "exec sp_rename 'FK_Services_BackflowDevices_BackflowDeviceId','FK_tblNJAWService_BackflowDevices_FlowbackDevice', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_PermitTypes_PermitTypeId','FK_tblNJAWService_PermitTypes_PermitType', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_PremiseTypes_PremiseTypeId','FK_tblNJAWService_PremiseTypes_PremiseTypeID', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_ServiceCategories_ServiceCategoryId','FK_tblNJAWService_ServiceCategories_CatofService', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_ServiceMaterials_PreviousServiceMaterialId','FK_tblNJAWService_ServiceMaterials_PrevServiceMatl', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_ServiceMaterials_ServiceMaterialId','FK_tblNJAWService_ServiceMaterials_ServMatl', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_ServiceStatuses_ServiceStatusId','FK_tblNJAWService_ServiceStatuses_InspSignoffReady', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_ServicePriorities_ServicePriorityId','FK_tblNJAWService_tblNJAWPriority_Priority', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_ServiceInstallationPurposes_ServiceInstallationPurposeId','FK_tblNJAWService_tblNJAWPurpInst_PurpInstal', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_ServiceSizes_PreviousServiceSizeId','FK_tblNJAWService_tblNJAWSizeServ_PrevServiceSize', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_ServiceSizes_MeterSettingSizeId','FK_tblNJAWService_tblNJAWSizeServ_SizeofTap', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_ServiceSizes_MainSizeId','FK_tblNJAWService_tblNJAWSizeServ_SizeOfMain', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_ServiceSizes_ServiceSizeId','FK_tblNJAWService_tblNJAWSizeServ_SizeofService', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_Towns_TownId','FK_tblNJAWService_Towns_Town', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_TownSections_TownSectionId','FK_tblNJAWService_TownSections_TwnSection', 'OBJECT';" +
                    "exec sp_rename 'FK_Services_OperatingCenters_OperatingCenterId', 'FK_tblNJAWService_OperatingCenters_OpCntr', 'OBJECT';",
                SQL_DROP_STATISTICS =
                    "DROP STATISTICS [dbo].[tblNJAWService].[_dta_stat_1933965966_10];" +
                    "IF EXISTS (select  1 from Sysindexes where name = '_dta_index_tblNJAWRestore_15_1453248232__K24_K25_K5_K7_K14_K16_K12_K21') DROP INDEX [_dta_index_tblNJAWRestore_15_1453248232__K24_K25_K5_K7_K14_K16_K12_K21] ON [dbo].[tblNJAWRestore];" +
                    "DROP STATISTICS [dbo].[tblNJAWRestore].[_dta_stat_1453248232_25_24_5_7_14_16_12_21];" +
                    "ALTER TABLE [dbo].[tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_InActSrv];" +
                    "ALTER TABLE [dbo].[tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_LengthService];" +
                    "ALTER TABLE [dbo].[tblNJAWService] DROP CONSTRAINT [DF_tblNJAWService_OrigInstDate];" +
                    "IF EXISTS (select  1 from Sysindexes where name = '_dta_index_tblNJAWService_19_1933965966__K88_K56_K68_K69_K78_K40_K9_K73_7_14_31_38_51_64_74_76_77_79') DROP INDEX [_dta_index_tblNJAWService_19_1933965966__K88_K56_K68_K69_K78_K40_K9_K73_7_14_31_38_51_64_74_76_77_79] ON [dbo].[tblNJAWService];" +
                    "IF EXISTS (select  1 from Sysindexes where name = '_dta_index_tblNJAWService_15_1933965966__K40_K64') DROP INDEX [_dta_index_tblNJAWService_15_1933965966__K40_K64] ON [dbo].[tblNJAWService];" +
                    "DROP STATISTICS [dbo].[tblNJAWService].[_dta_stat_1933965966_56_64]",
                SQL_RESTORE_STATISTICS =
                    "CREATE STATISTICS [_dta_stat_1933965966_10] ON [dbo].[tblNJAWService]([CntRec], [RecID]);" +
                    "ALTER TABLE [dbo].[tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_InActSrv]  DEFAULT ('') FOR [InActSrv];" +
                    "ALTER TABLE [dbo].[tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_LengthService]  DEFAULT ('') FOR [LengthService];" +
                    "ALTER TABLE [dbo].[tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_OrigInstDate]  DEFAULT (1 / 1 / 1900) FOR [OrigInstDate];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_19_1933965966__K88_K56_K68_K69_K78_K40_K9_K73_7_14_31_38_51_64_74_76_77_79] ON [dbo].[tblNJAWService] ([ImageActionID] ASC, [RecID] ASC, [SizeofMain] ASC, [SizeofService] ASC, [Town] ASC, [OpCntr] ASC, [CatofService] ASC, [StName] ASC) INCLUDE ([Block], [DateInstalled], [Lot], [Name], [PremNum], [ServNum], [StNum], [TaskNum1], [TaskNum2], [TwnSection]) ON [PRIMARY];" +
                    "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWService_15_1933965966__K40_K64] ON [dbo].[tblNJAWService] ([OpCntr] ASC, [ServNum] ASC)  ON [PRIMARY];" +
                    "CREATE STATISTICS [_dta_stat_1933965966_56_64] ON [dbo].[tblNJAWService]([RecID], [ServNum]);",
                UPDATE_COORDINATES = @"SET NOCOUNT ON 
                        DECLARE @latitude float
                        DECLARE @longitude float
                        DECLARE @id int
                        DECLARE @coordinateID int

                        DECLARE	tableCursor 
                        CURSOR FOR 
	                        SELECT Id, Lat, Lon FROM [Services] WHERE Lat is not null and Lon is not null 

                        OPEN tableCursor 
	                        FETCH NEXT FROM tableCursor INTO @id, @latitude, @longitude; 
	                        WHILE @@FETCH_STATUS = 0 
	                        BEGIN 
		                        Insert Into Coordinates(latitude, longitude) values(@latitude, @longitude)
		                        update [Services] set coordinateID = @@Identity where Id = @id
		                        FETCH NEXT FROM tableCursor INTO @id, @latitude, @longitude; 
	                        END
                        CLOSE tableCursor; 
                        DEALLOCATE tableCursor;",
                ROLLBACK_SERVICE_COORDINATES =
                    "Update [Services] Set Lat = (SELECT Latitude from Coordinates where Coordinates.CoordinateID = [Services].CoordinateID);" +
                    "Update [Services] Set Lon = (SELECT Longitude from Coordinates where Coordinates.CoordinateID = [Services].CoordinateID);",
                UPDATES =
                    "UPDATE tblNJAWService SET CrossStreet = 'MASSACHUSETTS AVE' where CrossStreet = 'MASSACHUETTS AVE' And Town = 195;" +
                    "UPDATE tblNJAWService SET CrossStreet = 'WYNATT ST' where CrossStreet = 'WYNAT ST' AND Town = 195;" +
                    "UPDATE tblNJAWService SET CrossStreet = 'CRANBERRY DR' where CrossStreet = 'CRANBURY DR' AND Town = 238;" +
                    "UPDATE tblNJAWService SET CrossStreet = 'HOLLY BROOK RD'  where CrossStreet = 'HOLLY BROOK RD' and Town = 77;" +
                    "UPDATE tblNJAWService SET CleanedCoordinates = 0 where CleanedCoordinates is null;" +
                    "UPDATE tblNJAWService SET CreatedOn = OrdCreationDate WHERE IsNUll(CreatedOn, '01/01/1900') = '01/01/1900';" +
                    "UPDATE tblNJAWService SET CreatedOn = DateAdded WHERE IsNUll(CreatedOn, '01/01/1900') = '01/01/1900';",
                UPDATE_DEPTH_MAIN =
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 5 WHERE DepthMain = '.3.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '.30'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 8 WHERE DepthMain = '.34'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 0, DepthMainInches = 8 WHERE DepthMain = '.8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 0, DepthMainInches = 6 WHERE DepthMain = '0'' 6\"in'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '04'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 0 WHERE DepthMain = '05'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 0 WHERE DepthMain = '1'			;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 2 WHERE DepthMain = '1'' 2\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 5 WHERE DepthMain = '1''5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 6 WHERE DepthMain = '1''6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 8 WHERE DepthMain = '1''8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 0 WHERE DepthMain = '1.0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 10 WHERE DepthMain = '1.10'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 10 WHERE DepthMain = '1.10 FT'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 3 WHERE DepthMain = '1.3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 4 WHERE DepthMain = '1.4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 5 WHERE DepthMain = '1.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 6 WHERE DepthMain = '1.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 8 WHERE DepthMain = '1.8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 1, DepthMainInches = 9 WHERE DepthMain = '1.9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 10, DepthMainInches = 0 WHERE DepthMain = '10'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 10, DepthMainInches = 0 WHERE DepthMain = '10'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 10, DepthMainInches = 0 WHERE DepthMain = '10.'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 10, DepthMainInches = 0 WHERE DepthMain = '10.0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 10, DepthMainInches = 6 WHERE DepthMain = '10.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 10, DepthMainInches = 5 WHERE DepthMain = '10.5'''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 10, DepthMainInches = 6 WHERE DepthMain = '10.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 10, DepthMainInches = 8 WHERE DepthMain = '10.8'''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 11, DepthMainInches = 0 WHERE DepthMain = '11'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 11, DepthMainInches = 0 WHERE DepthMain = '11'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 11, DepthMainInches = 6 WHERE DepthMain = '11.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 11, DepthMainInches = 7 WHERE DepthMain = '11.7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 11, DepthMainInches = 9 WHERE DepthMain = '11.9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 12, DepthMainInches = 0 WHERE DepthMain = '12'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 12, DepthMainInches = 0 WHERE DepthMain = '12'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 12, DepthMainInches = 6 WHERE DepthMain = '12'' 6'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 12, DepthMainInches = 3 WHERE DepthMain = '12''3\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 12, DepthMainInches = 6 WHERE DepthMain = '12''6'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 12, DepthMainInches = 6 WHERE DepthMain = '12.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 12, DepthMainInches = 9 WHERE DepthMain = '12.9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 0, DepthMainInches = 0 WHERE DepthMain = '124'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 13, DepthMainInches = 0 WHERE DepthMain = '13'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 13, DepthMainInches = 0 WHERE DepthMain = '13'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 13, DepthMainInches = 6 WHERE DepthMain = '13.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 13, DepthMainInches = 7 WHERE DepthMain = '13.7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 0, DepthMainInches = 0 WHERE DepthMain = '135'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 14, DepthMainInches = 0 WHERE DepthMain = '14'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 14, DepthMainInches = 0 WHERE DepthMain = '14'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 14, DepthMainInches = 3 WHERE DepthMain = '14.3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 14, DepthMainInches = 4 WHERE DepthMain = '14.4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 14, DepthMainInches = 5 WHERE DepthMain = '14.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 14, DepthMainInches = 6 WHERE DepthMain = '14.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 15, DepthMainInches = 0 WHERE DepthMain = '15'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 15, DepthMainInches = 0 WHERE DepthMain = '15'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 15, DepthMainInches = 0 WHERE DepthMain = '15''0\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 15, DepthMainInches = 5 WHERE DepthMain = '15.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 15, DepthMainInches = 6 WHERE DepthMain = '15.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 16, DepthMainInches = 0 WHERE DepthMain = '16'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 16, DepthMainInches = 2 WHERE DepthMain = '16.2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 16, DepthMainInches = 4 WHERE DepthMain = '16.4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 17, DepthMainInches = 0 WHERE DepthMain = '17'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 17, DepthMainInches = 0 WHERE DepthMain = '17'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 17, DepthMainInches = 3 WHERE DepthMain = '17.3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 17, DepthMainInches = 6 WHERE DepthMain = '17.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 18, DepthMainInches = 0 WHERE DepthMain = '18'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 18, DepthMainInches = 0 WHERE DepthMain = '18\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 18, DepthMainInches = 3 WHERE DepthMain = '18.3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 19, DepthMainInches = 0 WHERE DepthMain = '19'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 19, DepthMainInches = 8 WHERE DepthMain = '19''8\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 0 WHERE DepthMain = '2'			;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 0 WHERE DepthMain = '2 ft'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 10 WHERE DepthMain = '2 FT 10 IN';" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 11 WHERE DepthMain = '2 FT 11'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 11 WHERE DepthMain = '2 FT 11 IN';" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 6 WHERE DepthMain = '2 FT 6 IN'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 8 WHERE DepthMain = '2 FT 8 IN'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 0 WHERE DepthMain = '2'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 6 WHERE DepthMain = '2'' 6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 0 WHERE DepthMain = '2''-0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 4 WHERE DepthMain = '2''-4\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 6 WHERE DepthMain = '2''-6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 6 WHERE DepthMain = '2''-6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 1 WHERE DepthMain = '2''1'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 10 WHERE DepthMain = '2''10'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 10 WHERE DepthMain = '2''10\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 11 WHERE DepthMain = '2''11\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 2 WHERE DepthMain = '2''2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 2 WHERE DepthMain = '2''2\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 3 WHERE DepthMain = '2''3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 3 WHERE DepthMain = '2''3\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 4 WHERE DepthMain = '2''4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 4 WHERE DepthMain = '2''4\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 4 WHERE DepthMain = '2''4'''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 5 WHERE DepthMain = '2''5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 5 WHERE DepthMain = '2''5\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 6 WHERE DepthMain = '2''6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 6 WHERE DepthMain = '2''6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 7 WHERE DepthMain = '2''7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 7 WHERE DepthMain = '2''7\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 8 WHERE DepthMain = '2''8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 8 WHERE DepthMain = '2''8\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 9 WHERE DepthMain = '2''9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 9 WHERE DepthMain = '2''9\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 9 WHERE DepthMain = '2''9'''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 0 WHERE DepthMain = '2.0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 1 WHERE DepthMain = '2.1'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 10 WHERE DepthMain = '2.10'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 10 WHERE DepthMain = '2.10 FT'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 11 WHERE DepthMain = '2.11'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 2 WHERE DepthMain = '2.2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 3 WHERE DepthMain = '2.3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 3 WHERE DepthMain = '2.36'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 4 WHERE DepthMain = '2.4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 5 WHERE DepthMain = '2.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 6 WHERE DepthMain = '2.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 7 WHERE DepthMain = '2.7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 7 WHERE DepthMain = '2.75'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 7 WHERE DepthMain = '2.75'''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 8 WHERE DepthMain = '2.8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 8 WHERE DepthMain = '2.8 FT'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 9 WHERE DepthMain = '2.9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 20, DepthMainInches = 0 WHERE DepthMain = '20'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 20, DepthMainInches = 6 WHERE DepthMain = '20.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 21, DepthMainInches = 0 WHERE DepthMain = '21'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 21, DepthMainInches = 6 WHERE DepthMain = '21.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 22, DepthMainInches = 0 WHERE DepthMain = '22'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 23, DepthMainInches = 0 WHERE DepthMain = '23'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 23, DepthMainInches = 6 WHERE DepthMain = '23.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 24, DepthMainInches = 0 WHERE DepthMain = '24'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 24, DepthMainInches = 6 WHERE DepthMain = '24.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 25, DepthMainInches = 0 WHERE DepthMain = '25'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 25, DepthMainInches = 10 WHERE DepthMain = '25''10\"';" +
                    "UPDATE [Services] SET DepthMainFeet = 26, DepthMainInches = 0 WHERE DepthMain = '26'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 27, DepthMainInches = 0 WHERE DepthMain = '27'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 28, DepthMainInches = 0 WHERE DepthMain = '28'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 29, DepthMainInches = 0 WHERE DepthMain = '29'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 2, DepthMainInches = 0 WHERE DepthMain = '2ft'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '3'			;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '3 FT'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 2 WHERE DepthMain = '3 FT 2 IN'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 3 WHERE DepthMain = '3 FT 3 IN'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 4 WHERE DepthMain = '3 FT 4 IN'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3 FT 6 IN'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 1 WHERE DepthMain = '3\"1\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '3\"7\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '3'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '3'' \"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 10 WHERE DepthMain = '3'' 10'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 10 WHERE DepthMain = '3'' 10 in';" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 10 WHERE DepthMain = '3'' 10\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 2 WHERE DepthMain = '3'' 2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 3 WHERE DepthMain = '3'' 3\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 4 WHERE DepthMain = '3'' 4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3'' 6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3'' 6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '3'' 7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '3'' 7\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 8 WHERE DepthMain = '3'' 8\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '3''-0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '3''-0\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 10 WHERE DepthMain = '3''-10\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 5 WHERE DepthMain = '3''-5\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3''-6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3''-6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '3''0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 1 WHERE DepthMain = '3''1'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 1 WHERE DepthMain = '3''1\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 10 WHERE DepthMain = '3''10'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 10 WHERE DepthMain = '3''10\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 10 WHERE DepthMain = '3''10\"\"';" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 11 WHERE DepthMain = '3''11'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 11 WHERE DepthMain = '3''11\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 2 WHERE DepthMain = '3''2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 2 WHERE DepthMain = '3''2\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 3 WHERE DepthMain = '3''3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 3 WHERE DepthMain = '3''3\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 3 WHERE DepthMain = '3''3'''''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 4 WHERE DepthMain = '3''4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 4 WHERE DepthMain = '3''4\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 5 WHERE DepthMain = '3''5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 5 WHERE DepthMain = '3''5\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3''6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3''6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3''6\"\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3''6'''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3''6'''''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '3''7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '3''7\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 8 WHERE DepthMain = '3''8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 8 WHERE DepthMain = '3''8\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 8 WHERE DepthMain = '3''8'''''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 9 WHERE DepthMain = '3''9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 9 WHERE DepthMain = '3''9\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3-6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '3-7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '3.'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 5 WHERE DepthMain = '3..5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '3.0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 1 WHERE DepthMain = '3.1'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 10 WHERE DepthMain = '3.10'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 11 WHERE DepthMain = '3.11'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 1 WHERE DepthMain = '3.16'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 2 WHERE DepthMain = '3.2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 2 WHERE DepthMain = '3.2 FT'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 2 WHERE DepthMain = '3.25'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 3 WHERE DepthMain = '3.3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 3 WHERE DepthMain = '3.36'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 4 WHERE DepthMain = '3.4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 4 WHERE DepthMain = '3.4 FT'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 4 WHERE DepthMain = '3.4'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 4 WHERE DepthMain = '3.42'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 5 WHERE DepthMain = '3.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 5 WHERE DepthMain = '3.5'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 5 WHERE DepthMain = '3.58'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3.6 FT'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3.6\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3.6'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3.6.FT'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '3.7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '3.75'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '3.75'''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 8 WHERE DepthMain = '3.8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 8 WHERE DepthMain = '3.8 FT'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 9 WHERE DepthMain = '3.9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '30'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 1 WHERE DepthMain = '31'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 3 WHERE DepthMain = '33'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 4 WHERE DepthMain = '34'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 5 WHERE DepthMain = '35\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '36'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '36\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 8 WHERE DepthMain = '38'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 9 WHERE DepthMain = '39'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '3ft 6in'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4'			;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 6 WHERE DepthMain = '4 1/2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 6 WHERE DepthMain = '4 1/2 FT'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4 ft'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 4 WHERE DepthMain = '4 FT 4 IN'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 6 WHERE DepthMain = '4 FT 6 IN'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 9 WHERE DepthMain = '4\"9\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 10 WHERE DepthMain = '4'' 10'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '4'' 2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '4'' 2\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '4'' 3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '4'' 3\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 5 WHERE DepthMain = '4'' 5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 6 WHERE DepthMain = '4'' 6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 8 WHERE DepthMain = '4'' 8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4''-0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4''-0\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '4''-2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '4''-2\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 4 WHERE DepthMain = '4''-4\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 5 WHERE DepthMain = '4''-5\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 6 WHERE DepthMain = '4''-6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 6 WHERE DepthMain = '4''-6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 7 WHERE DepthMain = '4''-7\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4''0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4''0\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 1 WHERE DepthMain = '4''1'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 1 WHERE DepthMain = '4''1\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 10 WHERE DepthMain = '4''10'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 10 WHERE DepthMain = '4''10\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 11 WHERE DepthMain = '4''11'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 11 WHERE DepthMain = '4''11\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '4''2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '4''2\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '4''2:\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '4''3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '4''3  \"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '4''3\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '4''3'''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '4''3'''''	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 4 WHERE DepthMain = '4''4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 4 WHERE DepthMain = '4''4\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 5 WHERE DepthMain = '4''5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 5 WHERE DepthMain = '4''5\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 6 WHERE DepthMain = '4''6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 6 WHERE DepthMain = '4''6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 7 WHERE DepthMain = '4''7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 7 WHERE DepthMain = '4''7\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 8 WHERE DepthMain = '4''8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 8 WHERE DepthMain = '4''8\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 9 WHERE DepthMain = '4''9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 9 WHERE DepthMain = '4''9\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4.'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 4 WHERE DepthMain = '4..4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4.0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 8 WHERE DepthMain = '4.08'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 1 WHERE DepthMain = '4.1'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 10 WHERE DepthMain = '4.10'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 11 WHERE DepthMain = '4.11'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '4.2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '4.25'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '4.3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '4.33'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 4 WHERE DepthMain = '4.4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 5 WHERE DepthMain = '4.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 5 WHERE DepthMain = '4.5 ft'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 5 WHERE DepthMain = '4.5'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 5 WHERE DepthMain = '4.55'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 5 WHERE DepthMain = '4.59/22/10';" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 6 WHERE DepthMain = '4.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 7 WHERE DepthMain = '4.7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 7 WHERE DepthMain = '4.75'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 8 WHERE DepthMain = '4.8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 9 WHERE DepthMain = '4.9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '40'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 0 WHERE DepthMain = '40\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 1 WHERE DepthMain = '41'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 1 WHERE DepthMain = '41\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '42'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '42 inches'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '42\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '43'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '44'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '44\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 6 WHERE DepthMain = '44'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '45'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '45\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '46\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '47'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '47\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '48'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 7 WHERE DepthMain = '48\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 3, DepthMainInches = 8 WHERE DepthMain = '49'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4ft'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4ft 0in'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 0 WHERE DepthMain = '4q'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 0 WHERE DepthMain = '5'			;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 4 WHERE DepthMain = '5 ''4\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 0 WHERE DepthMain = '5 FT'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 0 WHERE DepthMain = '5'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 2 WHERE DepthMain = '5'' 2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 2 WHERE DepthMain = '5'' 2\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 6 WHERE DepthMain = '5'' 6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 6 WHERE DepthMain = '5'' 6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 6 WHERE DepthMain = '5'' 6\"in'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 6 WHERE DepthMain = '5''-6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 6 WHERE DepthMain = '5''-6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 1 WHERE DepthMain = '5''1\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 10 WHERE DepthMain = '5''10'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 10 WHERE DepthMain = '5''10\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 11 WHERE DepthMain = '5''11\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 2 WHERE DepthMain = '5''2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 2 WHERE DepthMain = '5''2\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 3 WHERE DepthMain = '5''3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 3 WHERE DepthMain = '5''3\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 4 WHERE DepthMain = '5''4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 4 WHERE DepthMain = '5''4\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 5 WHERE DepthMain = '5''5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 6 WHERE DepthMain = '5''6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 6 WHERE DepthMain = '5''6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 7 WHERE DepthMain = '5''7\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 8 WHERE DepthMain = '5''8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 9 WHERE DepthMain = '5''9\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 0 WHERE DepthMain = '5.0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 1 WHERE DepthMain = '5.1'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 10 WHERE DepthMain = '5.10'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 11 WHERE DepthMain = '5.11'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 2 WHERE DepthMain = '5.17'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 2 WHERE DepthMain = '5.2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 3 WHERE DepthMain = '5.3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 4 WHERE DepthMain = '5.4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 5 WHERE DepthMain = '5.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 5 WHERE DepthMain = '5.5'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 6 WHERE DepthMain = '5.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 7 WHERE DepthMain = '5.7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 8 WHERE DepthMain = '5.8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 9 WHERE DepthMain = '5.9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 1 WHERE DepthMain = '50'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 1 WHERE DepthMain = '50\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 1 WHERE DepthMain = '51'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 1 WHERE DepthMain = '51\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '52'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '52\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '53'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 2 WHERE DepthMain = '53\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '54'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '54\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 3 WHERE DepthMain = '56\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 4, DepthMainInches = 8 WHERE DepthMain = '57'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 0 WHERE DepthMain = '6'			;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 0 WHERE DepthMain = '6 ft'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 0 WHERE DepthMain = '6\"'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 0 WHERE DepthMain = '6'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 6 WHERE DepthMain = '6'' 6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 6 WHERE DepthMain = '6'' 6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 6 WHERE DepthMain = '6''-6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 1 WHERE DepthMain = '6''1\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 11 WHERE DepthMain = '6''11\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 2 WHERE DepthMain = '6''2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 2 WHERE DepthMain = '6''2\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 3 WHERE DepthMain = '6''3\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 4 WHERE DepthMain = '6''4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 6 WHERE DepthMain = '6''6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 6 WHERE DepthMain = '6''6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 9 WHERE DepthMain = '6''9\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 0 WHERE DepthMain = '6.'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 0 WHERE DepthMain = '6.0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 1 WHERE DepthMain = '6.1'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 10 WHERE DepthMain = '6.10'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 11 WHERE DepthMain = '6.11'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 2 WHERE DepthMain = '6.2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 3 WHERE DepthMain = '6.3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 4 WHERE DepthMain = '6.4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 5 WHERE DepthMain = '6.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 6 WHERE DepthMain = '6.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 8 WHERE DepthMain = '6.8'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 6, DepthMainInches = 9 WHERE DepthMain = '6.9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 5, DepthMainInches = 3 WHERE DepthMain = '63'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 0 WHERE DepthMain = '7'			;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 0 WHERE DepthMain = '7 ft'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 0 WHERE DepthMain = '7'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 6 WHERE DepthMain = '7'' 6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 3 WHERE DepthMain = '7''3\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 6 WHERE DepthMain = '7''6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 6 WHERE DepthMain = '7''6\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 7 WHERE DepthMain = '7''7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 7 WHERE DepthMain = '7''7\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 0 WHERE DepthMain = '7.0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 1 WHERE DepthMain = '7.1'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 10 WHERE DepthMain = '7.10'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 2 WHERE DepthMain = '7.2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 3 WHERE DepthMain = '7.3'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 4 WHERE DepthMain = '7.4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 4 WHERE DepthMain = '7.4'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 5 WHERE DepthMain = '7.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 5 WHERE DepthMain = '7.5'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 6 WHERE DepthMain = '7.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 7 WHERE DepthMain = '7.7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 7, DepthMainInches = 9 WHERE DepthMain = '7.9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 0 WHERE DepthMain = '8'			;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 0 WHERE DepthMain = '8'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 3 WHERE DepthMain = '8''3\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 5 WHERE DepthMain = '8''5\"'	;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 0 WHERE DepthMain = '8.0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 10 WHERE DepthMain = '8.10'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 4 WHERE DepthMain = '8.4'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 5 WHERE DepthMain = '8.5ft'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 6 WHERE DepthMain = '8.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 7 WHERE DepthMain = '8.7'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 8, DepthMainInches = 9 WHERE DepthMain = '8.9'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 9, DepthMainInches = 0 WHERE DepthMain = '9'			;" +
                    "UPDATE [Services] SET DepthMainFeet = 9, DepthMainInches = 0 WHERE DepthMain = '9'''		;" +
                    "UPDATE [Services] SET DepthMainFeet = 9, DepthMainInches = 2 WHERE DepthMain = '9''2'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 9, DepthMainInches = 0 WHERE DepthMain = '9.0'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 9, DepthMainInches = 6 WHERE DepthMain = '9.5'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 9, DepthMainInches = 6 WHERE DepthMain = '9.6'		;" +
                    "UPDATE [Services] SET DepthMainFeet = 9, DepthMainInches = 7 WHERE DepthMain = '9.7'		;",
                REVERT_DEPTH_MAIN =
                    "UPDATE [Services] SET DepthMain = CAST(DepthMainFeet as varchar) + ''' ' + CAST(DepthMainInches as varchar) + '\"' WHERE DepthMainFeet IS NOT NULL",
                UPDATE_WORKISSUEDTO = "update services set WorkIssuedTo = null where rtrim(ltrim(WorkIssuedTo)) = '';" +
                                      "update services set WorkIssuedTo = 'Blenheim Construction' where WorkIssuedTo in ('Blenheim');" +
                                      "update services set WorkIssuedTo = 'BIL-JIM' where WorkIssuedTo in ('BIL- JIM');" +
                                      "update services set WorkIssuedTo = 'Caruso Excavating' where WorkIssuedTo in ('Caruso', 'CARUSO EXCAVATING');" +
                                      "update services set WorkIssuedTo = 'Crosco, Inc.' where WorkIssuedTo in ('CROSCO, INC','CROSCO, INC.');" +
                                      "update services set WorkIssuedTo = 'George Harms Const Co' where WorkIssuedTo = 'George Harms';" +
                                      "update services set WorkIssuedTo = 'Henkels & McCoy, Inc.' where WorkIssuedTo in ('Henkel and McCoy','Henkels & McCoy','Henkels and McCoy','Henkles and McCoy');" +
                                      "update services set WorkIssuedTo = 'Hisko Excavating' where WorkIssuedTo in ('Hisko','Hisko Excavating');" +
                                      "update services set WorkIssuedTo = 'Hoff Brothers' where WorkIssuedTo in ('Hoff','Hoff Brothers');" +
                                      "update services set WorkIssuedTo = 'Ientile, Inc' where WorkIssuedTo in ('IENTILE','IENTILE, INC');" +
                                      "update services set WorkIssuedTo = 'JD Covely' where WorkIssuedTo in ('J.D. Covely','JD Covely');" +
                                      "update services set WorkIssuedTo = 'J.F. Keily Construction Co.' where WorkIssuedTo in ('Keily','Kiely','Kiely Contracting');" +
                                      "update services set WorkIssuedTo = 'Lafayette Utility' where WorkIssuedTo in ('Lafayette','Lafayette Utility');" +
                                      "update services set WorkIssuedTo = 'Montana Construction' where WorkIssuedTo in ('Montana','Montana Construction');" +
                                      "update services set WorkIssuedTo = 'Pillari Bros' where WorkIssuedTo in ('Pillari Bros','Pillari Brothers');" +
                                      "update services set WorkIssuedTo = 'RCW' where WorkIssuedTo in ('R.C.W.','RCW','RCW Contracting');" +
                                      "update services set WorkIssuedTo = 'Renda Contracting' where WorkIssuedTo in ('Renda','Renda Construction','Renda Contracting');" +
                                      "update services set WorkIssuedTo = 'Schilke Contracting' where WorkIssuedTo in ('Schilke','Schilke Contracting');" +
                                      "update services set WorkIssuedTo = 'Spinello' where WorkIssuedTo in ('Spinello','Spiniello');" +
                                      "update services set WorkIssuedTo = 'Valvetek Utility Services' where WorkIssuedTo in ('Valvetek','Valvetek Utility Services');" +
                                      "update services set WorkIssuedTo = 'Vollers E&C, Inc.' where WorkIssuedTo in ('Vollers','VOLLERS E&C,INC','Vollers Excavating');" +
                                      "update services set WorkIssuedTo = 'Creamer' where WorkIssuedTo in ('KRAMER');" +
                                      "update services set WorkIssuedTo = 'R E Pierson' where WorkIssuedTo = 'Pierson';" +
                                      "update services set WorkIssuedTo = 'Perna' where WorkIssuedTo in ('Perna Finigan');" +
                                      "update services set WorkIssuedTo = 'LIEDL & CO.' where WorkIssuedTo in ('Liedl & Co', 'Liedle and Co');" +
                                      "update services set WorkIssuedTo = 'J.H. Reid' where WorkIssuedTo in ('J. H. REID', 'JH REID');" +
                                      "update services set WorkIssuedTo = 'CRJ Contracting Inc' where WorkIssuedTo in ('CRJ Const','CRJ Contracting');" +
                                      "update services set WorkIssuedTo = 'Pioneer Pipe' where WorkIssuedTo in ('PIONEER');" +
                                      "update services set WorkIssuedTo = 'Sambol Contracting' where WorkIssuedTo in ('Sambol','SAMBOL CONTRACTING')";
        }

        #endregion

        public override void Up()
        {
            #region Services

            #region Service Sizes

            Execute.Sql("ALTER TABLE [dbo].[tblNJAWSizeServ] DROP CONSTRAINT [DF_tblNJAWSizeServ_Hyd];" +
                        "ALTER TABLE[dbo].[tblNJAWSizeServ] DROP CONSTRAINT[DF_tblNJAWSizeServ_Lat];" +
                        "ALTER TABLE[dbo].[tblNJAWSizeServ] DROP CONSTRAINT[DF_tblNJAWSizeServ_Main];" +
                        "ALTER TABLE[dbo].[tblNJAWSizeServ] DROP CONSTRAINT[DF_tblNJAWSizeServ_Meter];" +
                        "ALTER TABLE[dbo].[tblNJAWSizeServ] DROP CONSTRAINT[DF_tblNJAWSizeServ_Serv];" +
                        "ALTER TABLE[dbo].[tblNJAWSizeServ] DROP CONSTRAINT[DF_tblNJAWSizeServ_SizeServ];" +
                        "ALTER TABLE[dbo].[tblNJAWSizeServ] DROP CONSTRAINT[DF_tblNJAWSizeServ_Valve];");
            Rename.Table(TableNamesOld.SERVICE_SIZES).To(TableNames.SERVICE_SIZES);
            Rename.Column(ColumnNamesOld.SS_RECID).OnTable(TableNames.SERVICE_SIZES).To(ColumnNames.SS_RECID);
            Rename.Column(ColumnNamesOld.SS_HYDRANT).OnTable(TableNames.SERVICE_SIZES).To(ColumnNames.SS_HYDRANT);
            Rename.Column(ColumnNamesOld.SS_LAT).OnTable(TableNames.SERVICE_SIZES).To(ColumnNames.SS_LAT);
            Rename.Column(ColumnNamesOld.SS_ORDER).OnTable(TableNames.SERVICE_SIZES).To(ColumnNames.SS_ORDER);
            Rename.Column(ColumnNamesOld.SS_SERVICE).OnTable(TableNames.SERVICE_SIZES).To(ColumnNames.SS_SERVICE);
            Rename.Column(ColumnNamesOld.SS_SIZE_SERVICE).OnTable(TableNames.SERVICE_SIZES)
                  .To(ColumnNames.SS_SIZE_SERVICE);
            Alter.Column(ColumnNames.SS_SIZE_SERVICE).OnTable(TableNames.SERVICE_SIZES).AsDecimal(18, 2).NotNullable()
                 .Nullable();
            Rename.Column(ColumnNamesOld.SS_SIZE_SERVICE_DESCRIPTION).OnTable(TableNames.SERVICE_SIZES)
                  .To(ColumnNames.SS_SIZE_SERVICE_DESCRIPTION);
            this.UpdateBooleanOnColumn(TableNames.SERVICE_SIZES, ColumnNames.SS_HYDRANT);
            this.UpdateBooleanOnColumn(TableNames.SERVICE_SIZES, ColumnNames.SS_LAT);
            this.UpdateBooleanOnColumn(TableNames.SERVICE_SIZES, ColumnNames.SS_SERVICE);
            this.UpdateBooleanOnColumn(TableNames.SERVICE_SIZES, ColumnNames.SS_MAIN);
            this.UpdateBooleanOnColumn(TableNames.SERVICE_SIZES, ColumnNames.SS_METER);

            #endregion

            this.CreateDocumentType("Services", "Service", "Service");
            Execute.Sql(Sql.SQL_DROP_STATISTICS);
            Execute.Sql(Sql.UPDATES);

            Rename.Table(TableNamesOld.SERVICE_PRIORITY).To(TableNames.SERVICE_PRIORITY);
            Rename.Column(ColumnNamesOld.ID).OnTable(TableNames.SERVICE_PRIORITY).To(ColumnNames.ID);
            Rename.Column(ColumnNamesOld.PRIORITY).OnTable(TableNames.SERVICE_PRIORITY).To("Description");

            Rename.Table(TableNamesOld.INSTALLATION_PURPOSES).To(TableNames.INSTALLATION_PURPOSES);
            Rename.Column(ColumnNamesOld.ID).OnTable(TableNames.INSTALLATION_PURPOSES).To(ColumnNames.ID);
            Rename.Column("Purpose").OnTable(TableNames.INSTALLATION_PURPOSES).To("Description");

            Rename.Table(TableNamesOld.SERVICES).To(TableNames.SERVICES);
            Delete.Column(ColumnNamesOld.DATE_ADDED).FromTable(TableNames.SERVICES);
            Rename.Column(ColumnNamesOld.ID).OnTable(TableNames.SERVICES).To(ColumnNames.ID);
            Rename.Column(ColumnNamesOld.AMOUNT_RECEIVED).OnTable(TableNames.SERVICES).To(ColumnNames.AMOUNT_RECEIVED);
            Rename.Column(ColumnNamesOld.APPLICATION_RECEIVED)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.APPLICATION_RECEIVED);
            Rename.Column(ColumnNamesOld.APPLICATION_APPROVED)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.APPLICATION_APPROVED);
            Rename.Column(ColumnNamesOld.APPLICATION_SENT).OnTable(TableNames.SERVICES)
                  .To(ColumnNames.APPLICATION_SENT);
            Rename.Column(ColumnNamesOld.APARTMENT_NUMBER).OnTable(TableNames.SERVICES)
                  .To(ColumnNames.APARTMENT_NUMBER);
            Rename.Column(ColumnNamesOld.BUREAU_OF_SAFE_DRINKING_WATER_PERMIT_REQUIRED)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.BUREAU_OF_SAFE_DRINKING_WATER_PERMIT_REQUIRED);
            Rename.Column(ColumnNamesOld.CATEGORY_OF_SERVICE)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.CATEGORY_OF_SERVICE);
            Alter.Table(TableNames.SERVICES).AlterColumn(ColumnNames.CLEANED_COORDINATES).AsBoolean().WithDefault(0);
            Delete.Column(ColumnNamesOld.COUNT_RECORD).FromTable(TableNames.SERVICES);
            //Rename.Column(ColumnNamesOld.CROSS_STREET).OnTable(TableNames.SERVICES).To(ColumnNames.CROSS_STREET);
            Rename.Column(ColumnNamesOld.DEVELOPER_SERVICES_DRIVEN)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.DEVELOPER_SERVICES_DRIVEN);
            Rename.Column(ColumnNamesOld.ISACTIVE).OnTable(TableNames.SERVICES).To(ColumnNames.ISACTIVE);
            Rename.Column(ColumnNamesOld.INSPECTION_DATE).OnTable(TableNames.SERVICES).To(ColumnNames.INSPECTION_DATE);
            Rename.Column(ColumnNamesOld.INSPECTION_STATUS)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.INSPECTION_STATUS);
            Rename.Column(ColumnNamesOld.INSTALLATION_COST)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.INSTALLATION_COST);
            Rename.Column(ColumnNamesOld.INSTALLATION_INVOICE_NUMBER)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.INSTALLATION_INVOICE_NUMBER);
            Rename.Column(ColumnNamesOld.INSTALLATION_INVOICE_DATE)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.INSTALLATION_INVOICE_DATE);
            Rename.Column(ColumnNamesOld.LENGTH_OF_SERVICE)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.LENGTH_OF_SERVICE);
            Alter.Table(TableNames.SERVICES).AlterColumn(ColumnNames.LENGTH_OF_SERVICE).AsDecimal(18, 2).Nullable();
            Rename.Column(ColumnNamesOld.MAILING_PHONE_NUMBER)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.MAILING_PHONE_NUMBER);
            Rename.Column(ColumnNamesOld.MAILING_STREET_NAME)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.MAILING_STREET_NAME);
            Rename.Column(ColumnNamesOld.MAILING_STREET_NUMBER)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.MAILING_STREET_NUMBER);
            Rename.Column(ColumnNamesOld.METER_SETTING_REQUIREMENT)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.METER_SETTING_REQUIREMENT);
            Rename.Column(ColumnNamesOld.OPERATING_CENTER).OnTable(TableNames.SERVICES)
                  .To(ColumnNames.OPERATING_CENTER);
            Delete.Column(ColumnNamesOld.ORD_CREATION_DATE).FromTable(TableNames.SERVICES);
            Rename.Column(ColumnNamesOld.ORIGINAL_INSTALLATION_DATE)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.ORIGINAL_INSTALLATION_DATE);
            Alter.Column(ColumnNames.ORIGINAL_INSTALLATION_DATE).OnTable(TableNames.SERVICES).AsDateTime().Nullable();
            Rename.Column(ColumnNamesOld.PARENT_TASK_NUMBER)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.PARENT_TASK_NUMBER);
            Rename.Column(ColumnNamesOld.PAYMENT_REFERENCE_NUMBER)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.PAYMENT_REFERENCE_NUMBER);
            Rename.Column(ColumnNamesOld.PERMIT_EXPIRATION_DATE)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.PERMIT_EXPIRATION_DATE);
            Rename.Column(ColumnNamesOld.PERMIT_NUMBER).OnTable(TableNames.SERVICES).To(ColumnNames.PERMIT_NUMBER);
            Rename.Column(ColumnNamesOld.PERMIT_RECEIVED_DATE)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.PERMIT_RECEIVED_DATE);
            Rename.Column(ColumnNamesOld.PERMIT_TYPE).OnTable(TableNames.SERVICES).To(ColumnNames.PERMIT_TYPE);
            Rename.Column(ColumnNamesOld.PHONE_NUMBER).OnTable(TableNames.SERVICES).To(ColumnNames.PHONE_NUMBER);
            Rename.Column(ColumnNamesOld.PREMISE_NUMBER).OnTable(TableNames.SERVICES).To(ColumnNames.PREMISE_NUMBER);
            Rename.Column(ColumnNamesOld.PREVIOUS_SERVICE_MATERIAL)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.PREVIOUS_SERVICE_MATERIAL);
            Rename.Column(ColumnNamesOld.PREVIOUS_SERVICE_SIZE)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.PREVIOUS_SERVICE_SIZE);
            Rename.Column(ColumnNamesOld.PRIORITY).OnTable(TableNames.SERVICES).To(ColumnNames.PRIORITY);
            Delete.Column(ColumnNamesOld.REC_ID_PAV).FromTable(TableNames.SERVICES);
            Rename.Column(ColumnNamesOld.PURPOSE_OF_INSTALLATION)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.PURPOSE_OF_INSTALLATION);
            Rename.Column(ColumnNamesOld.RETIRED_DATE).OnTable(TableNames.SERVICES).To(ColumnNames.RETIRED_DATE);
            Rename.Column(ColumnNamesOld.RETIRED_ACCOUNT_NUMBER)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.RETIRED_ACCOUNT_NUMBER);
            Rename.Column(ColumnNamesOld.ROAD_OPENING_FEE).OnTable(TableNames.SERVICES)
                  .To(ColumnNames.ROAD_OPENING_FEE);
            Rename.Column(ColumnNamesOld.SERVICE_INSTALLATION_FEE)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.SERVICE_INSTALLATION_FEE);
            Rename.Column(ColumnNamesOld.SERVICE_MATERIAL).OnTable(TableNames.SERVICES)
                  .To(ColumnNames.SERVICE_MATERIAL);

            Create.Column(ColumnNames.SERVICE_NUMBER).OnTable(TableNames.SERVICES).AsInt64().Nullable();
            Execute.Sql("UPDATE Services SET ServiceNumber = rtrim(ltrim(Str(ServNum)))");
            Delete.Column(ColumnNamesOld.SERVICE_NUMBER).FromTable(TableNames.SERVICES);

            //Rename.Column(ColumnNamesOld.SMART_GROWTH_COST).OnTable(TableNames.SERVICES).To(ColumnNames.SMART_GROWTH_COST);
            Delete.Column(ColumnNamesOld.SMART_GROWTH).FromTable(TableNames.SERVICES);
            Delete.Column(ColumnNamesOld.SMART_GROWTH_COST).FromTable(TableNames.SERVICES);
            Delete.Column(ColumnNamesOld.SMART_GROWTH_METHOD_USED).FromTable(TableNames.SERVICES);

            Rename.Column(ColumnNamesOld.SIZE_OF_MAIN).OnTable(TableNames.SERVICES).To(ColumnNames.SIZE_OF_MAIN);
            Rename.Column(ColumnNamesOld.SIZE_OF_SERVICE).OnTable(TableNames.SERVICES).To(ColumnNames.SIZE_OF_SERVICE);
            Rename.Column(ColumnNamesOld.SIZE_OF_TAP).OnTable(TableNames.SERVICES).To(ColumnNames.SIZE_OF_TAP);
            Rename.Column(ColumnNamesOld.STREET_NAME).OnTable(TableNames.SERVICES).To(ColumnNames.STREET_NAME);
            Rename.Column(ColumnNamesOld.STREET_NUMBER).OnTable(TableNames.SERVICES).To(ColumnNames.STREET_NUMBER);
            Rename.Column(ColumnNamesOld.TAP_ORDER_NOTES).OnTable(TableNames.SERVICES).To(ColumnNames.TAP_ORDER_NOTES);
            Rename.Column(ColumnNamesOld.TASK_NUMBER_1).OnTable(TableNames.SERVICES).To(ColumnNames.TASK_NUMBER_1);
            Rename.Column(ColumnNamesOld.TASK_NUMBER_2).OnTable(TableNames.SERVICES).To(ColumnNames.TASK_NUMBER_2);
            Rename.Column(ColumnNamesOld.TOWN).OnTable(TableNames.SERVICES).To(ColumnNames.TOWN);
            Rename.Column(ColumnNamesOld.TOWN_SECTION).OnTable(TableNames.SERVICES).To(ColumnNames.TOWN_SECTION);
            Rename.Column(ColumnNamesOld.MAIN_TYPE).OnTable(TableNames.SERVICES).To(ColumnNames.MAIN_TYPE);
            // ok, we have two main types table at this point, with differentIds
            // services points at the old one, lets point it at the new one
            // Kill foreign key
            Delete.ForeignKey("FK_tblNJAWService_tblNJAWTypeMain_TypeMain").OnTable("Services");
            // update
            Execute.Sql(
                "update Services set MainTypeId = (select mt.Id from MainTypes mt where mt.Description = (select TypeMain from tblNJAWTypeMain tm where tm.RecID = Services.MainTypeId))");
            // Create new foreign key
            Create.ForeignKey(
                       Utilities.CreateForeignKeyName("Services", "MainTypes", "MainTypeId"))
                  .FromTable("Services")
                  .ForeignColumn("MainTypeId")
                  .ToTable("MainTypes")
                  .PrimaryColumn("Id");
            // kill off the old table
            Delete.Table("tblNJAWTypeMain");

            Rename.Column(ColumnNamesOld.QUESTIONAIRE_SENT_DATE)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.QUESTIONAIRE_SENT_DATE);
            Rename.Column(ColumnNamesOld.QUESTIONAIRE_RECEIVED_DATE)
                  .OnTable(TableNames.SERVICES)
                  .To(ColumnNames.QUESTIONAIRE_RECEIVED_DATE);
            Rename.Column(ColumnNamesOld.FLOWBACK_DEVICE).OnTable(TableNames.SERVICES).To(ColumnNames.FLOWBACK_DEVICE);

            // TODO: CLEANUP AND FOREIGN KEYS
            // Coordinate
            Alter.Table(TableNames.SERVICES).AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID");
            Execute.Sql(Sql.UPDATE_COORDINATES);
            Delete.Column(ColumnNamesOld.LATITUDE).FromTable(TableNames.SERVICES);
            Delete.Column(ColumnNamesOld.LONGITUDE).FromTable(TableNames.SERVICES);

            // TODO: NEW FOREIGN KEYS
            // StateId
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.SERVICES, TableNames.STATES,
                ColumnNames.STATE, ColumnNamesOld.STATE, "StateId", "Abbreviation");
            // CrossStreetId
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.SERVICES, TableNames.STREETS,
                ColumnNames.CROSS_STREET, ColumnNamesOld.CROSS_STREET, "StreetID", "FullStName",
                "AND " + TableNames.STREETS + ".TownID = " + TableNames.SERVICES + ".TownId");
            // SmartGrowthMethodUsed
            //this.CreateLookupTableFromQuery(TableNames.SMART_GROWTH_METHODS,
            //    "SELECT DISTINCT SGMethodUsed FROM Services WHERE ISNULL(SGMethodUsed, '') <> '' ORDER BY 1");
            //this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.SERVICES, TableNames.SMART_GROWTH_METHODS,
            //    ColumnNames.SMART_GROWTH_METHOD_USED, ColumnNamesOld.SMART_GROWTH_METHOD_USED);

            // StreetMaterial
            this.CreateLookupTableFromQuery(TableNames.STREET_MATERIAL,
                "SELECT DISTINCT StreetMatl FROM Services where ISNULL(StreetMatl, '') <> '' ORDER BY 1");
            this.AddLookupForeignKeyUpdateItThenRemoveOldColumn(TableNames.SERVICES, TableNames.STREET_MATERIAL,
                ColumnNames.STREET_MATERIAL, ColumnNamesOld.STREET_MATERIAL);

            // TODO: FIELD CLEAN UP
            // InActiveService - bool - flip meaning
            Execute.Sql(String.Format("UPDATE {0} SET {1} = 0 WHERE isNull({1}, '') <> 'ON';", TableNames.SERVICES,
                ColumnNames.ISACTIVE));
            Execute.Sql(String.Format("UPDATE {0} SET {1} = 1 WHERE isNull({1}, '') = 'ON';", TableNames.SERVICES,
                ColumnNames.ISACTIVE));
            Execute.Sql(String.Format("UPDATE {0} SET {1} = ABS(1-{1})", TableNames.SERVICES, ColumnNames.ISACTIVE));
            Alter.Column(ColumnNames.ISACTIVE).OnTable(TableNames.SERVICES).AsInt32().NotNullable().WithDefaultValue(1);

            // DepthMain - decimal
            Alter.Table(TableNames.SERVICES).AddColumn(ColumnNames.DEPTH_MAIN_FEET).AsInt32().Nullable();
            Alter.Table(TableNames.SERVICES).AddColumn(ColumnNames.DEPTH_MAIN_INCHES).AsInt32().Nullable();
            Execute.Sql(Sql.UPDATE_DEPTH_MAIN);
            Delete.Column(ColumnNamesOld.DEPTH_MAIN).FromTable(TableNames.SERVICES);

            // Initiator - map to user who created
            Alter.Table(TableNames.SERVICES).AddForeignKeyColumn(ColumnNames.INITIATOR, "tblPermissions", "RecId");
            Execute.Sql(
                "UPDATE Services SET InitiatorId = (SELECT Top 1 RecID from tblPermissions where FullName = [Initiator]) WHERE IsNull(Initiator,'') <> '';");
            Delete.Column(ColumnNamesOld.INITIATOR).FromTable(TableNames.SERVICES);
            Execute.Sql(Sql.UPDATE_FOREIGN_KEY_NAMES);

            //Bools
            this.UpdateBooleanColumn(TableNames.SERVICES, ColumnNames.AGREEMENT);
            this.UpdateBooleanColumn(TableNames.SERVICES, ColumnNames.BUREAU_OF_SAFE_DRINKING_WATER_PERMIT_REQUIRED);
            this.UpdateBooleanColumn(TableNames.SERVICES, ColumnNames.DEVELOPER_SERVICES_DRIVEN);
            this.UpdateBooleanColumn(TableNames.SERVICES, ColumnNames.METER_SETTING_REQUIREMENT);
            //this.UpdateBooleanColumn(TableNames.SERVICES, ColumnNames.SMART_GROWTH);

            this.NullDateField(TableNames.SERVICES, ColumnNames.INSPECTION_DATE);
            this.NullDateField(TableNames.SERVICES, ColumnNames.INSTALLATION_INVOICE_DATE);
            this.NullDateField(TableNames.SERVICES, ColumnNames.ORIGINAL_INSTALLATION_DATE);
            this.NullDateField(TableNames.SERVICES, ColumnNames.PERMIT_EXPIRATION_DATE);
            this.NullDateField(TableNames.SERVICES, ColumnNames.PERMIT_RECEIVED_DATE);
            this.NullDateField(TableNames.SERVICES, ColumnNames.PERMIT_SENT_DATE);
            this.NullDateField(TableNames.SERVICES, ColumnNames.QUESTIONAIRE_RECEIVED_DATE);
            this.NullDateField(TableNames.SERVICES, ColumnNames.QUESTIONAIRE_SENT_DATE);
            this.NullDateField(TableNames.SERVICES, ColumnNames.RETIRED_DATE);
            this.NullDateField(TableNames.SERVICES, ColumnNames.APPLICATION_APPROVED);
            this.NullDateField(TableNames.SERVICES, ColumnNames.APPLICATION_RECEIVED);
            this.NullDateField(TableNames.SERVICES, ColumnNames.APPLICATION_SENT);
            this.NullDateField(TableNames.SERVICES, ColumnNames.CONTACT_DATE);
            this.NullDateField(TableNames.SERVICES, ColumnNames.CREATED_ON);
            this.NullDateField(TableNames.SERVICES, ColumnNames.DATE_CLOSED);
            this.NullDateField(TableNames.SERVICES, ColumnNames.DATE_INSTALLED);
            this.NullDateField(TableNames.SERVICES, ColumnNames.DATE_ISSUED_TO_FIELD);
            this.NullDateField(TableNames.SERVICES, ColumnNames.LAST_UPDATED);
            this.NullDateField(TableNames.SERVICES, ColumnNames.DATE_CLOSED);

            #endregion

            #region OperatingCenterServiceCategories

            //Rename.Table(TableNamesOld.OPERATING_CENTERS_SERVICE_CATEGORIES).To(TableNames.OPERATING_CENTERS_SERVICE_CATEGORIES);
            //Rename.Column(ColumnNamesOld.ID).OnTable(TableNames.OPERATING_CENTERS_SERVICE_CATEGORIES).To(ColumnNames.ID);
            //Rename.Column(ColumnNamesOld.CATEGORY_OF_SERVICE).OnTable(TableNames.OPERATING_CENTERS_SERVICE_CATEGORIES).To(ColumnNames.CATEGORY_OF_SERVICE);
            //Rename.Column(ColumnNamesOld.OPERATING_CENTER).OnTable(TableNames.OPERATING_CENTERS_SERVICE_CATEGORIES).To(ColumnNames.OPERATING_CENTER);
            //Rename.Column(ColumnNamesOld.SERVICE_TYPE).OnTable(TableNames.OPERATING_CENTERS_SERVICE_CATEGORIES).To(ColumnNames.SERVICE_TYPE);
            //Delete.Column(ColumnNamesOld.LINK).FromTable(TableNames.OPERATING_CENTERS_SERVICE_CATEGORIES);

            Create.Table(TableNames.SERVICE_TYPES)
                  .WithColumn(ColumnNames.OPERATING_CENTER).AsInt32().NotNullable()
                  .WithColumn(ColumnNames.CATEGORY_OF_SERVICE).AsInt32().NotNullable()
                  .WithColumn("Description").AsAnsiString(50).NotNullable()
                  .WithColumn("CategoryOfServiceGroupId").AsInt32().Nullable();
            Execute.Sql("INSERT INTO [{0}] SELECT OpCntr, CatofService, ServType, CategoryOfServiceGroupId FROM [{1}]",
                TableNames.SERVICE_TYPES, TableNamesOld.OPERATING_CENTERS_SERVICE_CATEGORIES);
            Delete.Table(TableNamesOld.OPERATING_CENTERS_SERVICE_CATEGORIES);

            #endregion

            #region Restorations

            // Restoration Contractors
            Rename.Table(TableNamesOld.SERVICE_RESTORATION_CONTRACTORS).To(TableNames.SERVICE_RESTORATION_CONTRACTORS);
            // Update column names
            Rename.Column("RecID").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS).To("Id");
            Create.PrimaryKey("PK_ServiceRestorationContractors").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS)
                  .Column("Id");
            Delete.Column("Link").FromTable(TableNames.SERVICE_RESTORATION_CONTRACTORS);
            // Fix FK refs
            Execute.Sql(
                "UPDATE {0} SET OpCntr = (SELECT oc.OperatingCenterId from OperatingCenters oc where oc.OperatingCenterCode = OpCntr);",
                TableNames.SERVICE_RESTORATION_CONTRACTORS);
            Rename.Column("OpCntr").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS).To("OperatingCenterId");
            Alter.Table(TableNames.SERVICE_RESTORATION_CONTRACTORS).AlterForeignKeyColumn(ColumnNames.OPERATING_CENTER,
                TableNames.OPERATING_CENTERS, "OperatingCenterID");

            Execute.Sql(Sql.UPDATE_RESTORATIONS);
            Rename.Column("Pav").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS).To("FinalRestoration");
            Rename.Column("Serv").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS).To("PartialRestoration");
            // Fix Booleans
            this.UpdateBooleanColumn(TableNames.SERVICE_RESTORATION_CONTRACTORS, "FinalRestoration");
            this.UpdateBooleanColumn(TableNames.SERVICE_RESTORATION_CONTRACTORS, "PartialRestoration");

            Rename.Table(TableNamesOld.RESTORATIONS).To(TableNames.RESTORATIONS);
            Rename.Column(ColumnNamesOld.R_ID).OnTable(TableNames.RESTORATIONS).To(ColumnNames.R_ID);
            Rename.Column(ColumnNamesOld.R_APPROVED_ON).OnTable(TableNames.RESTORATIONS).To(ColumnNames.R_APPROVED_ON);
            Rename.Column(ColumnNamesOld.R_CANCEL).OnTable(TableNames.RESTORATIONS).To(ColumnNames.R_CANCEL);
            Rename.Column(ColumnNamesOld.R_ESTIMATED_RESTORATION_AMOUNT).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_ESTIMATED_RESTORATION_AMOUNT);
            Rename.Column(ColumnNamesOld.R_FINAL_COMPLETION_BY).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_FINAL_COMPLETION_BY);
            Rename.Column(ColumnNamesOld.R_FINAL_COST).OnTable(TableNames.RESTORATIONS).To(ColumnNames.R_FINAL_COST);
            Rename.Column(ColumnNamesOld.R_FINAL_DATE).OnTable(TableNames.RESTORATIONS).To(ColumnNames.R_FINAL_DATE);
            Rename.Column(ColumnNamesOld.R_FINAL_INVOICE_NUMBER).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_FINAL_INVOICE_NUMBER);
            Rename.Column(ColumnNamesOld.R_FINAL_RESTORATION_METHOD).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_FINAL_RESTORATION_METHOD);
            Rename.Column(ColumnNamesOld.R_FINAL_RESTORATION_AMOUNT).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_FINAL_RESTORATION_AMOUNT);
            Rename.Column(ColumnNamesOld.R_FINAL_TRAFFIC).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_FINAL_TRAFFIC);
            Rename.Column(ColumnNamesOld.R_NOTES).OnTable(TableNames.RESTORATIONS).To(ColumnNames.R_NOTES);
            Rename.Column(ColumnNamesOld.R_PARTIAL_COMPLETION_BY).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_PARTIAL_COMPLETION_BY);
            Rename.Column(ColumnNamesOld.R_PARTIAL_RESTORATION_DATE).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_PARTIAL_RESTORATION_DATE);
            Rename.Column(ColumnNamesOld.R_PARTIAL_INVOICE_NUMBER).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_PARTIAL_INVOICE_NUMBER);
            Rename.Column(ColumnNamesOld.R_PARTIAL_METHOD).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_PARTIAL_METHOD);
            Rename.Column(ColumnNamesOld.R_PARTIAL_RESTORATION_AMOUNT).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_PARTIAL_RESTORATION_AMOUNT);
            Rename.Column(ColumnNamesOld.R_PARTIAL_RESTORATION_COST).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_PARTIAL_RESTORATION_COST);
            Rename.Column(ColumnNamesOld.R_PARTIAL_TRAFFIC_CONTROL_HOURS).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_PARTIAL_TRAFFIC_CONTROL_HOURS);
            Rename.Column(ColumnNamesOld.R_REJECTED_ON).OnTable(TableNames.RESTORATIONS).To(ColumnNames.R_REJECTED_ON);
            Rename.Column(ColumnNamesOld.R_SERVICE_ID).OnTable(TableNames.RESTORATIONS).To(ColumnNames.R_SERVICE_ID);
            Rename.Column(ColumnNamesOld.R_RESTORATION_TYPE).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_RESTORATION_TYPE);
            Rename.Column(ColumnNamesOld.R_PURCHASE_ORDER_NUMBER).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_PURCHASE_ORDER_NUMBER);
            Rename.Column(ColumnNamesOld.R_ESTIMATED_VALUE).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNames.R_ESTIMATED_VALUE);

            Alter.Table(TableNames.RESTORATIONS).AlterForeignKeyColumn(ColumnNames.R_SERVICE_ID, TableNames.SERVICES)
                 .Nullable();
            Alter.Table(TableNames.RESTORATIONS).AlterForeignKeyColumn(ColumnNames.R_PARTIAL_METHOD,
                TableNames.RESTORATION_METHODS, "RestorationMethodID");
            Alter.Table(TableNames.RESTORATIONS).AlterForeignKeyColumn(ColumnNames.R_FINAL_RESTORATION_METHOD,
                TableNames.RESTORATION_METHODS, "RestorationMethodID");

            Alter.Table(TableNames.RESTORATIONS).AlterForeignKeyColumn(ColumnNames.R_FINAL_COMPLETION_BY,
                TableNames.SERVICE_RESTORATION_CONTRACTORS);
            Alter.Table(TableNames.RESTORATIONS).AlterForeignKeyColumn(ColumnNames.R_PARTIAL_COMPLETION_BY,
                TableNames.SERVICE_RESTORATION_CONTRACTORS);

            Alter.Table(TableNames.RESTORATIONS).AlterForeignKeyColumn(ColumnNames.R_RESTORATION_TYPE,
                TableNames.RESTORATION_TYPES, "RestorationTypeID");

            Execute.Sql(String.Format("UPDATE [{0}] SET [{1}] = 1 WHERE [{1}] = 'ON';", TableNames.RESTORATIONS,
                ColumnNames.R_CANCEL));

            Alter.Column(ColumnNames.R_PARTIAL_TRAFFIC_CONTROL_HOURS).OnTable(TableNames.RESTORATIONS).AsAnsiString(20)
                 .Nullable();
            Alter.Column(ColumnNames.R_FINAL_TRAFFIC).OnTable(TableNames.RESTORATIONS).AsAnsiString(20).Nullable();

            Execute.Sql(
                "UPDATE ServiceRestorations SET FinalRestorationTrafficControlHours = '2' where   FinalRestorationTrafficControlHours = '2 HRS';" +
                "UPDATE ServiceRestorations SET FinalRestorationTrafficControlHours = '2' where   FinalRestorationTrafficControlHours = '1  COP @ 2 HRS';" +
                "UPDATE ServiceRestorations SET FinalRestorationTrafficControlHours = '1.5' where FinalRestorationTrafficControlHours = '1 cop @ 1.5 hrs';" +
                "UPDATE ServiceRestorations SET FinalRestorationTrafficControlHours = '1' where   FinalRestorationTrafficControlHours = '1 HR';" +
                "UPDATE ServiceRestorations SET FinalRestorationTrafficControlHours = null where  FinalRestorationTrafficControlHours = '.';" +
                "UPDATE ServiceRestorations SET FinalRestorationTrafficControlHours = null where  FinalRestorationTrafficControlHours = '';" +
                "UPDATE ServiceRestorations SET PartialRestorationTrafficControlHours = null where PartialRestorationTrafficControlHours = '';");
            Alter.Column(ColumnNames.R_PARTIAL_TRAFFIC_CONTROL_HOURS).OnTable(TableNames.RESTORATIONS).AsDecimal(18, 2)
                 .Nullable();
            Alter.Column(ColumnNames.R_FINAL_TRAFFIC).OnTable(TableNames.RESTORATIONS).AsDecimal(18, 2).Nullable();
            this.UpdateBooleanColumn(TableNames.RESTORATIONS, ColumnNames.R_CANCEL);
            this.NullDateField(TableNames.RESTORATIONS, ColumnNames.R_FINAL_DATE);
            this.NullDateField(TableNames.RESTORATIONS, ColumnNames.R_PARTIAL_RESTORATION_DATE);
            this.NullDateField(TableNames.RESTORATIONS, ColumnNames.R_APPROVED_ON);
            this.NullDateField(TableNames.RESTORATIONS, ColumnNames.R_REJECTED_ON);

            // APPROVED BY
            Alter.Table(TableNames.RESTORATIONS)
                 .AddForeignKeyColumn(ColumnNames.R_APPROVED_BY, "tblPermissions", "RecID");
            Execute.Sql(
                "UPDATE ServiceRestorations SET ApprovedById = (SELECT TOP 1 RecID from tblPermissions where Username = IsNull(AppBy,''));");
            Delete.Column("AppBy").FromTable(TableNames.RESTORATIONS);

            // REJECTED BY
            Alter.Table(TableNames.RESTORATIONS)
                 .AddForeignKeyColumn(ColumnNames.R_REJECTED_BY, "tblPermissions", "RecID");
            Execute.Sql(
                "UPDATE ServiceRestorations SET RejectedById = (SELECT TOP 1 RecID from tblPermissions where Username = IsNull(RejBy,''));");
            Delete.Column("RejBy").FromTable(TableNames.RESTORATIONS);

            // INITIATED BY
            Alter.Table(TableNames.RESTORATIONS)
                 .AddForeignKeyColumn(ColumnNames.R_INITIATED_BY, "tblPermissions", "RecID");
            Execute.Sql(
                "UPDATE ServiceRestorations set InitiatedById = (SELECT TOP 1 RecID from tblPermissions where FULLName = IsNull(InitiatedBy,''));");
            Delete.Column(ColumnNamesOld.R_INITIATED_BY).FromTable(TableNames.RESTORATIONS);

            Execute.Sql(
                "IF EXISTS (select 1 from sysobjects where name = 'DF_tblNJAWService_WorkIssuedto') ALTER TABLE Services DROP CONSTRAINT [DF_tblNJAWService_WorkIssuedto];" +
                "UPDATE Services SET WorkIssuedTo = (SELECT src.Id FROM ServiceRestorationContractors src where src.OperatingCenterId = services.OperatingCenterId and services.WorkIssuedTo = src.Contractor)");
            Alter.Table(TableNames.SERVICES)
                 .AlterForeignKeyColumn("WorkIssuedTo", TableNames.SERVICE_RESTORATION_CONTRACTORS);

            #endregion

            #region Service Installation Materials

            Execute.Sql("alter table tblNJAWInstMatl alter column RecOrd char(3);" +
                        "update tblNJAWInstMatl set RecOrd = ASCII(RecOrd) where isNumeric(RecOrd) = 0;");
            Execute.Sql("update tblNJAWInstMatl set RecOrd = 150 where RecOrd = '*';");

            Rename.Table(TableNamesOld.INSTALLATION_MATERIALS).To(TableNames.INSTALLATION_MATERIALS);
            Alter.Table(TableNames.INSTALLATION_MATERIALS)
                 .AddForeignKeyColumn("ServiceCategoryId", TableNames.SERVICE_CATEGORIES, "ServiceCategoryID");
            Execute.Sql(
                "update ServiceInstallationMaterials set ServiceCategoryId = (select ServiceCategoryId from ServiceCategories where Description = CatOfServ)");
            Delete.Column(ColumnNamesOld.P_SERVICE_CATEGORY).FromTable(TableNames.INSTALLATION_MATERIALS);

            Alter.Table(TableNames.INSTALLATION_MATERIALS)
                 .AddForeignKeyColumn("OperatingCenterId", TableNames.OPERATING_CENTERS, "OperatingCenterId");
            Execute.Sql(
                "update ServiceInstallationMaterials set OperatingCenterId = (select OperatingCenterID from operatingCenters where operatingCenterCode = OpCntr)");
            Delete.Column(ColumnNamesOld.P_OPERATING_CENTER).FromTable(TableNames.INSTALLATION_MATERIALS);

            Alter.Table(TableNames.INSTALLATION_MATERIALS)
                 .AddForeignKeyColumn("ServiceSizeId", TableNames.SERVICE_SIZES);
            Execute.Sql(
                "update ServiceInstallationMaterials set ServiceSizeId = (select Id from ServiceSizes where ServiceSizeDescription = SizeOfService)");
            Delete.Column(ColumnNamesOld.P_SIZE_OF_SERVICE).FromTable(TableNames.INSTALLATION_MATERIALS);

            Rename.Column(ColumnNamesOld.P_ID).OnTable(TableNames.INSTALLATION_MATERIALS).To(ColumnNames.P_ID);
            Rename.Column(ColumnNamesOld.P_PART_SIZE).OnTable(TableNames.INSTALLATION_MATERIALS)
                  .To(ColumnNames.P_PART_SIZE);
            Rename.Column(ColumnNamesOld.P_PART_QUANTITY).OnTable(TableNames.INSTALLATION_MATERIALS)
                  .To(ColumnNames.P_PART_QUANTITY);

            Rename.Column(ColumnNamesOld.P_ORDER).OnTable(TableNames.INSTALLATION_MATERIALS).To(ColumnNames.P_ORDER);
            Alter.Column(ColumnNames.P_ORDER).OnTable(TableNames.INSTALLATION_MATERIALS).AsInt32().Nullable();

            Execute.Sql(@"Create View ServicesServiceInstallationMaterials
                            AS
                            select
	                            S.Id as ServiceId, SIM.Id as ServiceInstallationMaterialId
                            from
	                            Services S 
                            LEFT JOIN
	                            ServiceInstallationMaterials SIM
                            ON
	                            SIM.ServiceCategoryId = S.ServiceCategoryId
                            AND
	                            SIM.OperatingCenterId = S.OperatingCenterId
                            AND
	                            SIM.ServiceSizeId = S.ServiceSizeId");

            #endregion

            #region Operating Center Service Materials

            Rename.Table(TableNamesOld.OPERATING_CENTERS_SERVICE_MATERIALS)
                  .To(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS);
            Rename.Column(ColumnNamesOld.OCSM_ID).OnTable(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS)
                  .To(ColumnNames.OCSM_ID);
            Rename.Column(ColumnNamesOld.OCSM_NEW_SERVICE_RECORD)
                  .OnTable(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS).To(ColumnNames.OCSM_NEW_SERVICE_RECORD);
            Rename.Column(ColumnNamesOld.OCSM_OPERATING_CENTER).OnTable(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS)
                  .To(ColumnNames.OCSM_OPERATING_CENTER);
            Rename.Column(ColumnNamesOld.OCSM_SERVICE_MATERIAL).OnTable(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS)
                  .To(ColumnNames.OCSM_SERVICE_MATERIAL);

            this.UpdateBooleanOnColumn(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS,
                ColumnNames.OCSM_NEW_SERVICE_RECORD);

            #endregion
        }

        public override void Down()
        {
            #region Services

            Delete.ForeignKey("FK_Services_ServiceRestorationContractors_WorkIssuedTo").OnTable(TableNames.SERVICES);
            Alter.Table(TableNames.SERVICES).AlterColumn("WorkIssuedTo").AsAnsiString(30).Nullable();
            Execute.Sql(
                "UPDATE Services SET WorkIssuedTo = (SELECT Contractor from ServiceRestorationContractors src where src.Id = WorkIssuedTo)");

            this.DeleteDataType("Services");
            // Initiator - map to user who created
            Alter.Table(TableNames.SERVICES).AddColumn(ColumnNamesOld.INITIATOR).AsAnsiString(StringLengths.INITIATOR)
                 .Nullable();
            Execute.Sql(
                "UPDATE Services Set Initiator = (SELECT Top 1 FullName from TblPermissions where RecID = InitiatorId);");
            Delete.ForeignKeyColumn(TableNames.SERVICES, ColumnNames.INITIATOR, "tblPermissions", "RecId");
            // DepthMain - decimal
            Alter.Table(TableNames.SERVICES).AddColumn(ColumnNamesOld.DEPTH_MAIN).AsAnsiString(StringLengths.DEPTH_MAIN)
                 .Nullable();
            Execute.Sql(Sql.REVERT_DEPTH_MAIN);
            Delete.Column(ColumnNames.DEPTH_MAIN_FEET).FromTable(TableNames.SERVICES);
            Delete.Column(ColumnNames.DEPTH_MAIN_INCHES).FromTable(TableNames.SERVICES);

            // InActiveService - bool - flip meaning
            Execute.Sql(String.Format("UPDATE {0} SET {1} = ABS(1-{1})", TableNames.SERVICES, ColumnNames.ISACTIVE));
            Execute.Sql("ALTER TABLE [dbo].[Services] DROP CONSTRAINT [DF_Services_IsActive];");
            Alter.Table(TableNames.SERVICES).AlterColumn(ColumnNames.ISACTIVE).AsAnsiString(StringLengths.IS_ACTIVE)
                 .NotNullable();
            Execute.Sql(String.Format("UPDATE {0} SET {1} = 'ON' WHERE {1} = '1';" +
                                      "UPDATE {0} SET {1} = ''   WHERE {1} = '0';", TableNames.SERVICES,
                ColumnNames.ISACTIVE));

            this.RemoveLookupAndAdjustColumns(TableNames.SERVICES, TableNames.STREET_MATERIAL,
                ColumnNames.STREET_MATERIAL, ColumnNamesOld.STREET_MATERIAL, StringLengths.STREET_MATERIAL);
            Delete.Table(TableNames.STREET_MATERIAL);
            //this.RemoveLookupAndAdjustColumns(TableNames.SERVICES, TableNames.SMART_GROWTH_METHODS, ColumnNames.SMART_GROWTH_METHOD_USED, ColumnNamesOld.SMART_GROWTH_METHOD_USED, StringLengths.SMART_GROWTH_METHOD_USED);
            //Delete.Table(TableNames.SMART_GROWTH_METHODS);
            this.RemoveLookupAndAdjustColumns(TableNames.SERVICES, TableNames.STREETS, ColumnNames.CROSS_STREET,
                ColumnNamesOld.CROSS_STREET, StringLengths.CROSS_STREET, "StreetID", "FullStName");
            this.RemoveLookupAndAdjustColumns(TableNames.SERVICES, TableNames.STATES, ColumnNames.STATE,
                ColumnNamesOld.STATE, StringLengths.STATE, "StateId", "Abbreviation");

            Alter.Table(TableNames.SERVICES).AddColumn(ColumnNamesOld.LATITUDE).AsAnsiString(StringLengths.LAT)
                 .Nullable();
            Alter.Table(TableNames.SERVICES).AddColumn(ColumnNamesOld.LONGITUDE).AsAnsiString(StringLengths.LON)
                 .Nullable();
            Execute.Sql(Sql.ROLLBACK_SERVICE_COORDINATES);
            Delete.ForeignKeyColumn(TableNames.SERVICES, "CoordinateId", "Coordinates", "CoordinateID");

            Rename.Table(TableNames.SERVICE_PRIORITY).To(TableNamesOld.SERVICE_PRIORITY);
            Rename.Column(ColumnNames.ID).OnTable(TableNamesOld.SERVICE_PRIORITY).To(ColumnNamesOld.ID);
            Rename.Column("Description").OnTable(TableNamesOld.SERVICE_PRIORITY).To(ColumnNamesOld.PRIORITY);

            Rename.Table(TableNames.INSTALLATION_PURPOSES).To(TableNamesOld.INSTALLATION_PURPOSES);
            Rename.Column(ColumnNames.ID).OnTable(TableNamesOld.INSTALLATION_PURPOSES).To(ColumnNamesOld.ID);
            Rename.Column("Description").OnTable(TableNamesOld.INSTALLATION_PURPOSES).To("Purpose");

            Rename.Table(TableNames.SERVICES).To(TableNamesOld.SERVICES);
            Rename.Column(ColumnNames.ID).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.ID);
            Rename.Column(ColumnNames.AMOUNT_RECEIVED).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.AMOUNT_RECEIVED);
            Rename.Column(ColumnNames.APPLICATION_RECEIVED).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.APPLICATION_RECEIVED);
            Rename.Column(ColumnNames.APPLICATION_APPROVED).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.APPLICATION_APPROVED);
            Rename.Column(ColumnNames.APPLICATION_SENT).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.APPLICATION_SENT);
            Rename.Column(ColumnNames.APARTMENT_NUMBER).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.APARTMENT_NUMBER);
            Rename.Column(ColumnNames.BUREAU_OF_SAFE_DRINKING_WATER_PERMIT_REQUIRED).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.BUREAU_OF_SAFE_DRINKING_WATER_PERMIT_REQUIRED);
            Rename.Column(ColumnNames.CATEGORY_OF_SERVICE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.CATEGORY_OF_SERVICE);
            Alter.Table(TableNamesOld.SERVICES).AlterColumn(ColumnNames.CLEANED_COORDINATES).AsBoolean();
            Alter.Table(TableNamesOld.SERVICES).AddColumn(ColumnNamesOld.COUNT_RECORD).AsInt32().Nullable();
            Execute.Sql(String.Format("UPDATE {0} SET {1} = 1", TableNamesOld.SERVICES, ColumnNamesOld.COUNT_RECORD));
            //Rename.Column(ColumnNames.CROSS_STREET).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.CROSS_STREET);
            Rename.Column(ColumnNames.DEVELOPER_SERVICES_DRIVEN).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.DEVELOPER_SERVICES_DRIVEN);
            Rename.Column(ColumnNames.ISACTIVE).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.ISACTIVE);
            Rename.Column(ColumnNames.INSPECTION_DATE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.INSPECTION_DATE);
            Rename.Column(ColumnNames.INSPECTION_STATUS).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.INSPECTION_STATUS);
            Rename.Column(ColumnNames.INSTALLATION_COST).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.INSTALLATION_COST);
            Rename.Column(ColumnNames.INSTALLATION_INVOICE_NUMBER).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.INSTALLATION_INVOICE_NUMBER);
            Rename.Column(ColumnNames.INSTALLATION_INVOICE_DATE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.INSTALLATION_INVOICE_DATE);
            Rename.Column(ColumnNames.LENGTH_OF_SERVICE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.LENGTH_OF_SERVICE);
            Alter.Table(TableNamesOld.SERVICES).AlterColumn(ColumnNamesOld.LENGTH_OF_SERVICE).AsFloat().Nullable();
            Rename.Column(ColumnNames.MAILING_PHONE_NUMBER).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.MAILING_PHONE_NUMBER);
            Rename.Column(ColumnNames.MAILING_STREET_NAME).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.MAILING_STREET_NAME);
            Rename.Column(ColumnNames.MAILING_STREET_NUMBER).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.MAILING_STREET_NUMBER);
            Rename.Column(ColumnNames.METER_SETTING_REQUIREMENT).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.METER_SETTING_REQUIREMENT);
            Rename.Column(ColumnNames.OPERATING_CENTER).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.OPERATING_CENTER);
            Alter.Table(TableNamesOld.SERVICES).AddColumn(ColumnNamesOld.ORD_CREATION_DATE).AsDateTime().Nullable();
            Alter.Column(ColumnNames.ORIGINAL_INSTALLATION_DATE).OnTable(TableNamesOld.SERVICES)
                 .AsCustom("smalldatetime").Nullable();
            Rename.Column(ColumnNames.ORIGINAL_INSTALLATION_DATE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.ORIGINAL_INSTALLATION_DATE);
            Rename.Column(ColumnNames.PARENT_TASK_NUMBER).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.PARENT_TASK_NUMBER);
            Rename.Column(ColumnNames.PAYMENT_REFERENCE_NUMBER).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.PAYMENT_REFERENCE_NUMBER);
            Rename.Column(ColumnNames.PERMIT_EXPIRATION_DATE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.PERMIT_EXPIRATION_DATE);
            Rename.Column(ColumnNames.PERMIT_NUMBER).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.PERMIT_NUMBER);
            Rename.Column(ColumnNames.PERMIT_RECEIVED_DATE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.PERMIT_RECEIVED_DATE);
            Rename.Column(ColumnNames.PERMIT_TYPE).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.PERMIT_TYPE);
            Rename.Column(ColumnNames.PHONE_NUMBER).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.PHONE_NUMBER);
            Rename.Column(ColumnNames.PREMISE_NUMBER).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.PREMISE_NUMBER);
            Rename.Column(ColumnNames.PREVIOUS_SERVICE_MATERIAL).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.PREVIOUS_SERVICE_MATERIAL);
            Rename.Column(ColumnNames.PREVIOUS_SERVICE_SIZE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.PREVIOUS_SERVICE_SIZE);
            Rename.Column(ColumnNames.PRIORITY).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.PRIORITY);
            Rename.Column(ColumnNames.PURPOSE_OF_INSTALLATION).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.PURPOSE_OF_INSTALLATION);
            Alter.Table(TableNamesOld.SERVICES).AddColumn(ColumnNamesOld.REC_ID_PAV).AsInt32().Nullable();
            Rename.Column(ColumnNames.RETIRED_DATE).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.RETIRED_DATE);
            Rename.Column(ColumnNames.RETIRED_ACCOUNT_NUMBER).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.RETIRED_ACCOUNT_NUMBER);
            Rename.Column(ColumnNames.ROAD_OPENING_FEE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.ROAD_OPENING_FEE);
            Rename.Column(ColumnNames.SERVICE_INSTALLATION_FEE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.SERVICE_INSTALLATION_FEE);
            Rename.Column(ColumnNames.SERVICE_MATERIAL).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.SERVICE_MATERIAL);
            Rename.Column(ColumnNames.SERVICE_NUMBER).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.SERVICE_NUMBER);
            Alter.Column(ColumnNamesOld.SERVICE_NUMBER).OnTable(TableNamesOld.SERVICES).AsFloat().Nullable();

            //Rename.Column(ColumnNames.SMART_GROWTH_COST).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.SMART_GROWTH_COST);
            Alter.Table(TableNamesOld.SERVICES)
                 .AddColumn(ColumnNamesOld.SMART_GROWTH).AsBoolean().Nullable()
                 .AddColumn(ColumnNamesOld.SMART_GROWTH_COST).AsDecimal(19, 2).Nullable()
                 .AddColumn(ColumnNamesOld.SMART_GROWTH_METHOD_USED).AsAnsiString(15).Nullable();

            Rename.Column(ColumnNames.SIZE_OF_MAIN).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.SIZE_OF_MAIN);
            Rename.Column(ColumnNames.SIZE_OF_SERVICE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.SIZE_OF_SERVICE);
            Rename.Column(ColumnNames.SIZE_OF_TAP).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.SIZE_OF_TAP);
            Rename.Column(ColumnNames.STREET_NAME).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.STREET_NAME);
            Rename.Column(ColumnNames.STREET_NUMBER).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.STREET_NUMBER);
            Rename.Column(ColumnNames.TAP_ORDER_NOTES).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.TAP_ORDER_NOTES);
            Rename.Column(ColumnNames.TASK_NUMBER_1).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.TASK_NUMBER_1);
            Rename.Column(ColumnNames.TASK_NUMBER_2).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.TASK_NUMBER_2);
            Rename.Column(ColumnNames.TOWN).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.TOWN);
            Rename.Column(ColumnNames.TOWN_SECTION).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.TOWN_SECTION);
            Rename.Column(ColumnNames.MAIN_TYPE).OnTable(TableNamesOld.SERVICES).To(ColumnNamesOld.MAIN_TYPE);
            // restore the shitty old table
            // "exec sp_rename 'FK_Services_tblNJAWTypeMain_MainTypeId','FK_tblNJAWService_tblNJAWTypeMain_TypeMain', 'OBJECT';" +
            Create.Table("tblNJAWTypeMain")
                  .WithColumn("RecID").AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn("TypeMain").AsAnsiString(15).Nullable();
            Delete.ForeignKey(Utilities.CreateForeignKeyName("Services", "MainTypes", "MainTypeId"))
                  .OnTable(TableNamesOld.SERVICES);
            Execute.Sql("SET IDENTITY_INSERT tblNJAWTypeMain ON;" +
                        " INSERT INTO tblNJAWTypeMain(RecID, TypeMain) SELECT Id, Description from MainTypes order by Id;" +
                        "SET IDENTITY_INSERT tblNJAWTypeMain OFF;");
            Create.ForeignKey(
                       "FK_tblNJAWService_tblNJAWTypeMain_TypeMain")
                  .FromTable(TableNamesOld.SERVICES)
                  .ForeignColumn(ColumnNamesOld.MAIN_TYPE)
                  .ToTable("tblNJAWTypeMain")
                  .PrimaryColumn("RecID");

            Rename.Column(ColumnNames.QUESTIONAIRE_SENT_DATE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.QUESTIONAIRE_SENT_DATE);
            Rename.Column(ColumnNames.QUESTIONAIRE_RECEIVED_DATE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.QUESTIONAIRE_RECEIVED_DATE);
            Rename.Column(ColumnNames.FLOWBACK_DEVICE).OnTable(TableNamesOld.SERVICES)
                  .To(ColumnNamesOld.FLOWBACK_DEVICE);

            Alter.Table(TableNamesOld.SERVICES).AddColumn(ColumnNamesOld.DATE_ADDED).AsDateTime().Nullable();
            Execute.Sql("UPDATE tblNJAWService SET DateAdded = CreatedOn, OrdCreationDate = CreatedOn");
            Execute.Sql(
                "ALTER TABLE [dbo].[tblNJAWService] ADD  CONSTRAINT [DF_tblNJAWService_DateAdded]  DEFAULT (getdate()) FOR [DateAdded];");

            //Bools
            Alter.Table(TableNamesOld.SERVICES).AlterColumn(ColumnNamesOld.AGREEMENT).AsBoolean().Nullable();
            Alter.Table(TableNamesOld.SERVICES)
                 .AlterColumn(ColumnNamesOld.BUREAU_OF_SAFE_DRINKING_WATER_PERMIT_REQUIRED).AsBoolean().Nullable();
            Alter.Table(TableNamesOld.SERVICES).AlterColumn(ColumnNamesOld.DEVELOPER_SERVICES_DRIVEN).AsBoolean()
                 .Nullable();
            Alter.Table(TableNamesOld.SERVICES).AlterColumn(ColumnNamesOld.METER_SETTING_REQUIREMENT).AsBoolean()
                 .Nullable();
            //Alter.Table(TableNamesOld.SERVICES).AlterColumn(ColumnNamesOld.SMART_GROWTH).AsBoolean().Nullable();

            // TODO: Cleanup and Foreign Keys
            Execute.Sql(Sql.SQL_RESTORE_STATISTICS);
            Execute.Sql(Sql.ROLLBACK_FOREIGN_KEY_NAMES);

            #endregion

            #region OperatingCenterServiceCategories

            Create.Table(TableNamesOld.OPERATING_CENTERS_SERVICE_CATEGORIES)
                  .WithIdentityColumn(ColumnNamesOld.ID)
                  .WithForeignKeyColumn(ColumnNamesOld.OPERATING_CENTER, TableNames.OPERATING_CENTERS,
                       "OperatingCenterId")
                  .WithForeignKeyColumn(ColumnNamesOld.CATEGORY_OF_SERVICE, TableNames.SERVICE_CATEGORIES,
                       "ServiceCategoryId")
                  .WithForeignKeyColumn("CategoryOfServiceGroupId", "CategoryOfServiceGroups",
                       "CategoryOfServiceGroupId")
                  .WithColumn("ServType").AsAnsiString(20).NotNullable()
                  .WithColumn("Link").AsAnsiString(1).Nullable();
            Execute.Sql(
                "INSERT INTO [{0}] SELECT OperatingCenterId, ServiceCategoryId, CategoryOfServiceGroupId, Description, 'L' FROM [{1}]",
                TableNamesOld.OPERATING_CENTERS_SERVICE_CATEGORIES, TableNames.SERVICE_TYPES);
            Delete.Table(TableNames.SERVICE_TYPES);

            #endregion

            #region Restorations

            #region RESTORATION CONTRACTORS

            Execute.Sql(
                "ALTER TABLE [dbo].[ServiceRestorationContractors] DROP CONSTRAINT [DF_ServiceRestorationContractors_FinalRestoration];");
            Execute.Sql(
                "ALTER TABLE [dbo].[ServiceRestorationContractors] DROP CONSTRAINT [DF_ServiceRestorationContractors_PartialRestoration];");

            // FinalRestoration bool
            Alter.Column("FinalRestoration").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS).AsAnsiString(2)
                 .Nullable();
            Execute.Sql(
                "UPDATE ServiceRestorationContractors SET FinalRestoration = 'ON' where FinalRestoration = '1';");
            Execute.Sql("UPDATE ServiceRestorationContractors SET FinalRestoration = '' where FinalRestoration = '0';");
            // PartialRestoration bool
            Alter.Column("PartialRestoration").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS).AsAnsiString(2)
                 .Nullable();
            Execute.Sql(
                "UPDATE ServiceRestorationContractors SET PartialRestoration = 'ON' where PartialRestoration = '1';");
            Execute.Sql(
                "UPDATE ServiceRestorationContractors SET PartialRestoration = '' where PartialRestoration = '0';");
            // OperatingCenter
            Create.Column("OpCntr").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS).AsAnsiString(4).Nullable();
            Execute.Sql(
                "UPDATE ServiceRestorationContractors SET OpCntr = (SELECT oc.OperatingCenterCode from OperatingCenters oc where oc.OperatingCenterID = ServiceRestorationContractors.OperatingCenterId);");
            Delete.ForeignKeyColumn(TableNames.SERVICE_RESTORATION_CONTRACTORS, "OperatingCenterId", "OperatingCenters",
                "OperatingCenterID");
            // Link
            Alter.Table(TableNames.SERVICE_RESTORATION_CONTRACTORS).AddColumn("Link").AsAnsiString(1).Nullable();
            Execute.Sql("UPDATE ServiceRestorationContractors SET Link = 'L'");
            // Rename Columns Back
            Rename.Column("FinalRestoration").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS).To("Pav");
            Rename.Column("PartialRestoration").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS).To("Serv");

            Rename.Column("Id").OnTable(TableNames.SERVICE_RESTORATION_CONTRACTORS).To("RecID");
            // Rename Table Back
            Rename.Table(TableNames.SERVICE_RESTORATION_CONTRACTORS).To(TableNamesOld.SERVICE_RESTORATION_CONTRACTORS);

            #endregion RESTORATION CONTRACTORS

            // FINAL COMPLETION
            Alter.Table(TableNames.RESTORATIONS).AddColumn(ColumnNamesOld.R_FINAL_COMPLETION_BY).AsAnsiString(30)
                 .Nullable();
            Execute.Sql(
                "UPDATE ServiceRestorations SET FinalCompBy = (SELECT Contractor FROM tblNJAWContractor rc WHERE rc.RecId = FinalRestorationCompletionBy);");
            Delete.ForeignKeyColumn(TableNames.RESTORATIONS, "FinalRestorationCompletionBy",
                TableNames.SERVICE_RESTORATION_CONTRACTORS);
            // PARTIAL COMPLETION
            Alter.Table(TableNames.RESTORATIONS).AddColumn(ColumnNamesOld.R_PARTIAL_COMPLETION_BY).AsAnsiString(30)
                 .Nullable();
            Execute.Sql(
                "UPDATE ServiceRestorations SET PartCompBy = (SELECT Contractor FROM tblNJAWContractor rc WHERE rc.RecId = PartialRestorationCompletionBy);");
            Delete.ForeignKeyColumn(TableNames.RESTORATIONS, "PartialRestorationCompletionBy",
                TableNames.SERVICE_RESTORATION_CONTRACTORS);
            Delete.PrimaryKey("PK_ServiceRestorationContractors")
                  .FromTable(TableNamesOld.SERVICE_RESTORATION_CONTRACTORS);

            //ApprovedBy
            Alter.Table(TableNames.RESTORATIONS).AddColumn("AppBy").AsAnsiString(30).Nullable();
            Execute.Sql(
                "UPDATE ServiceRestorations SET AppBy = (SELECT UserName from tblPermissions where ApprovedById = RecID);");
            Delete.ForeignKeyColumn(TableNames.RESTORATIONS, "ApprovedById", "tblPermissions", "RecID");

            //RejectedBy
            Alter.Table(TableNames.RESTORATIONS).AddColumn("RejBy").AsAnsiString(30).Nullable();
            Execute.Sql(
                "UPDATE ServiceRestorations SET REjBy = (SELECT UserName from tblPermissions where RejectedById = RecID);");
            Delete.ForeignKeyColumn(TableNames.RESTORATIONS, "RejectedById", "tblPermissions", "RecID");

            //Initiator
            Alter.Table(TableNames.RESTORATIONS).AddColumn(ColumnNamesOld.R_INITIATED_BY).AsAnsiString(25).Nullable();
            Execute.Sql(
                "UPDATE ServiceRestorations SET InitiatedBy = (SELECT FullName FROM tblPermissions where RecID = InitiatedById)");
            Delete.ForeignKeyColumn(TableNames.RESTORATIONS, ColumnNames.R_INITIATED_BY, "tblPermissions", "RecID");

            Delete.ForeignKey(Utilities.CreateForeignKeyName(TableNames.RESTORATIONS, TableNames.SERVICES,
                ColumnNames.R_SERVICE_ID)).OnTable(TableNames.RESTORATIONS);

            Delete.ForeignKey(Utilities.CreateForeignKeyName(TableNames.RESTORATIONS, TableNames.RESTORATION_METHODS,
                ColumnNames.R_PARTIAL_METHOD)).OnTable(TableNames.RESTORATIONS);
            Alter.Table(TableNames.RESTORATIONS).AlterColumn(ColumnNames.R_PARTIAL_METHOD).AsAnsiString(30).Nullable();
            Delete.ForeignKey(Utilities.CreateForeignKeyName(TableNames.RESTORATIONS, TableNames.RESTORATION_METHODS,
                ColumnNames.R_FINAL_RESTORATION_METHOD)).OnTable(TableNames.RESTORATIONS);
            Alter.Table(TableNames.RESTORATIONS).AlterColumn(ColumnNames.R_FINAL_RESTORATION_METHOD).AsAnsiString(30)
                 .Nullable();
            Delete.ForeignKey(Utilities.CreateForeignKeyName(TableNames.RESTORATIONS, TableNames.RESTORATION_TYPES,
                ColumnNames.R_RESTORATION_TYPE)).OnTable(TableNames.RESTORATIONS);
            Alter.Table(TableNames.RESTORATIONS).AlterColumn(ColumnNames.R_RESTORATION_TYPE).AsAnsiString(30)
                 .Nullable();
            Alter.Table(TableNames.RESTORATIONS).AlterColumn(ColumnNames.R_FINAL_TRAFFIC).AsAnsiString(20).Nullable();
            Alter.Table(TableNames.RESTORATIONS).AlterColumn(ColumnNames.R_PARTIAL_TRAFFIC_CONTROL_HOURS)
                 .AsAnsiString(20).Nullable();

            Rename.Column(ColumnNames.R_ID).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_ID);
            //Rename.Column(ColumnNames.R_APPROVED_BY).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_APPROVED_BY);
            Rename.Column(ColumnNames.R_APPROVED_ON).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_APPROVED_ON);
            Rename.Column(ColumnNames.R_CANCEL).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_CANCEL);
            Rename.Column(ColumnNames.R_ESTIMATED_RESTORATION_AMOUNT).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_ESTIMATED_RESTORATION_AMOUNT);
            Rename.Column(ColumnNames.R_FINAL_COST).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_FINAL_COST);
            Rename.Column(ColumnNames.R_FINAL_DATE).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_FINAL_DATE);
            Rename.Column(ColumnNames.R_FINAL_INVOICE_NUMBER).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_FINAL_INVOICE_NUMBER);
            Rename.Column(ColumnNames.R_FINAL_RESTORATION_METHOD).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_FINAL_RESTORATION_METHOD);
            Rename.Column(ColumnNames.R_FINAL_RESTORATION_AMOUNT).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_FINAL_RESTORATION_AMOUNT);
            Rename.Column(ColumnNames.R_FINAL_TRAFFIC).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_FINAL_TRAFFIC);
            //Rename.Column(ColumnNames.R_INITIATED_BY).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_INITIATED_BY);
            Rename.Column(ColumnNames.R_NOTES).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_NOTES);
            Rename.Column(ColumnNames.R_PARTIAL_RESTORATION_DATE).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_PARTIAL_RESTORATION_DATE);
            Rename.Column(ColumnNames.R_PARTIAL_INVOICE_NUMBER).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_PARTIAL_INVOICE_NUMBER);
            Rename.Column(ColumnNames.R_PARTIAL_METHOD).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_PARTIAL_METHOD);
            Rename.Column(ColumnNames.R_PARTIAL_RESTORATION_AMOUNT).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_PARTIAL_RESTORATION_AMOUNT);
            Rename.Column(ColumnNames.R_PARTIAL_RESTORATION_COST).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_PARTIAL_RESTORATION_COST);
            Rename.Column(ColumnNames.R_PARTIAL_TRAFFIC_CONTROL_HOURS).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_PARTIAL_TRAFFIC_CONTROL_HOURS);
            //Rename.Column(ColumnNames.R_REJECTED_BY).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_REJECTED_BY);
            Rename.Column(ColumnNames.R_REJECTED_ON).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_REJECTED_ON);
            Rename.Column(ColumnNames.R_SERVICE_ID).OnTable(TableNames.RESTORATIONS).To(ColumnNamesOld.R_SERVICE_ID);
            Rename.Column(ColumnNames.R_RESTORATION_TYPE).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_RESTORATION_TYPE);
            Rename.Column(ColumnNames.R_PURCHASE_ORDER_NUMBER).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_PURCHASE_ORDER_NUMBER);
            Rename.Column(ColumnNames.R_ESTIMATED_VALUE).OnTable(TableNames.RESTORATIONS)
                  .To(ColumnNamesOld.R_ESTIMATED_VALUE);

            // Cancel to bool
            Execute.Sql("ALTER TABLE [dbo].[ServiceRestorations] DROP CONSTRAINT [DF_ServiceRestorations_Cancel]");
            Alter.Column(ColumnNames.R_CANCEL).OnTable(TableNames.RESTORATIONS).AsAnsiString(2).Nullable();
            Execute.Sql(String.Format("UPDATE [{0}] SET [{1}] = 'ON' WHERE [{1}] = 1;", TableNames.RESTORATIONS,
                ColumnNames.R_CANCEL));
            Execute.Sql(
                "ALTER TABLE [dbo].[ServiceRestorations] ADD  CONSTRAINT [DF_tblNJAWRestore_Cancel]  DEFAULT ('') FOR [Cancel];");

            Execute.Sql(Sql.RESTORE_RESTORATIONS);

            Rename.Table(TableNames.RESTORATIONS).To(TableNamesOld.RESTORATIONS);
            Execute.Sql(
                "CREATE NONCLUSTERED INDEX [_dta_index_tblNJAWRestore_15_1453248232__K24_K25_K5_K7_K14_K16_K12_K21] ON [dbo].[tblNJAWRestore] ([ServID] ASC, [TypeRestore] ASC, [FinalCompBy] ASC, [FinalDate] ASC,[PartCompBy] ASC, [PartDate] ASC, [InitiatedBy] ASC, [RecID] ASC)  ON [PRIMARY];");
            Execute.Sql(
                "CREATE STATISTICS [_dta_stat_1453248232_25_24_5_7_14_16_12_21] ON [dbo].[tblNJAWRestore]([TypeRestore], [ServID], [FinalCompBy], [FinalDate], [PartCompBy], [PartDate], [InitiatedBy], [RecID])");

            #endregion

            #region Installation Materials

            Execute.Sql("DROP VIEW ServicesServiceInstallationMaterials;");

            //SizeOfService - 10, CatOfServ 30, OpCntr 4
            Alter.Table(TableNames.INSTALLATION_MATERIALS).AddColumn(ColumnNamesOld.P_SIZE_OF_SERVICE).AsAnsiString(10)
                 .Nullable();
            Execute.Sql(
                "UPDATE ServiceInstallationMaterials SET SizeOfService = (select ServiceSizeDescription from ServiceSizes where Id = ServiceSizeId)");
            Delete.ForeignKeyColumn(TableNames.INSTALLATION_MATERIALS, ColumnNames.P_SIZE_OF_SERVICE,
                TableNames.SERVICE_SIZES);

            Alter.Table(TableNames.INSTALLATION_MATERIALS).AddColumn(ColumnNamesOld.P_OPERATING_CENTER).AsAnsiString(4)
                 .Nullable();
            Execute.Sql(
                "UPDATE ServiceInstallationMaterials SET OpCntr = (select operatingCenterCode from OperatingCenters where OperatingCenters.OperatingCenterId = ServiceInstallationMaterials.OperatingCenterId);");
            Delete.ForeignKeyColumn(TableNames.INSTALLATION_MATERIALS, ColumnNames.P_OPERATING_CENTER,
                TableNames.OPERATING_CENTERS);

            Alter.Table(TableNames.INSTALLATION_MATERIALS).AddColumn(ColumnNamesOld.P_SERVICE_CATEGORY).AsAnsiString(30)
                 .Nullable();
            Execute.Sql(
                "UPDATE ServiceInstallationMaterials SET CatOfServ = (select description from servicecategories where servicecategories.SErviceCategoryId = ServiceInstallationMaterials.ServiceCategoryID)");
            Delete.ForeignKeyColumn(TableNames.INSTALLATION_MATERIALS, ColumnNames.P_SERVICE_CATEGORY,
                TableNames.SERVICE_CATEGORIES);

            Rename.Column(ColumnNames.P_ID).OnTable(TableNames.INSTALLATION_MATERIALS).To(ColumnNamesOld.P_ID);
            Rename.Column(ColumnNames.P_PART_SIZE).OnTable(TableNames.INSTALLATION_MATERIALS)
                  .To(ColumnNamesOld.P_PART_SIZE);
            Rename.Column(ColumnNames.P_PART_QUANTITY).OnTable(TableNames.INSTALLATION_MATERIALS)
                  .To(ColumnNamesOld.P_PART_QUANTITY);
            Rename.Column(ColumnNames.P_ORDER).OnTable(TableNames.INSTALLATION_MATERIALS).To(ColumnNamesOld.P_ORDER);
            Rename.Table(TableNames.INSTALLATION_MATERIALS).To(TableNamesOld.INSTALLATION_MATERIALS);

            Alter.Column(ColumnNamesOld.P_ORDER).OnTable(TableNamesOld.INSTALLATION_MATERIALS).AsString(3);
            Execute.Sql("UPDATE tblNJAWInstMatl SET RecOrd = CHAR(RecOrd) where RecOrd > 9");
            Alter.Column(ColumnNamesOld.P_ORDER).OnTable(TableNamesOld.INSTALLATION_MATERIALS).AsString(1);

            #endregion

            #region Operating Center Service Materials

            Rename.Column(ColumnNames.OCSM_ID).OnTable(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS)
                  .To(ColumnNamesOld.OCSM_ID);
            Rename.Column(ColumnNames.OCSM_NEW_SERVICE_RECORD).OnTable(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS)
                  .To(ColumnNamesOld.OCSM_NEW_SERVICE_RECORD);
            Rename.Column(ColumnNames.OCSM_OPERATING_CENTER).OnTable(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS)
                  .To(ColumnNamesOld.OCSM_OPERATING_CENTER);
            Rename.Column(ColumnNames.OCSM_SERVICE_MATERIAL).OnTable(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS)
                  .To(ColumnNamesOld.OCSM_SERVICE_MATERIAL);
            Rename.Table(TableNames.OPERATING_CENTERS_SERVICE_MATERIALS)
                  .To(TableNamesOld.OPERATING_CENTERS_SERVICE_MATERIALS);

            this.RollbackBooleanOnColumn(TableNamesOld.OPERATING_CENTERS_SERVICE_MATERIALS,
                ColumnNamesOld.OCSM_NEW_SERVICE_RECORD);

            #endregion

            #region ServiceSizes

            this.RollbackBooleanOnColumn(TableNames.SERVICE_SIZES, ColumnNames.SS_HYDRANT);
            this.RollbackBooleanOnColumn(TableNames.SERVICE_SIZES, ColumnNames.SS_LAT);
            this.RollbackBooleanOnColumn(TableNames.SERVICE_SIZES, ColumnNames.SS_SERVICE);
            this.RollbackBooleanOnColumn(TableNames.SERVICE_SIZES, ColumnNames.SS_MAIN);
            this.RollbackBooleanOnColumn(TableNames.SERVICE_SIZES, ColumnNames.SS_METER);

            Rename.Column(ColumnNames.SS_RECID).OnTable(TableNames.SERVICE_SIZES).To(ColumnNamesOld.SS_RECID);
            Rename.Column(ColumnNames.SS_HYDRANT).OnTable(TableNames.SERVICE_SIZES).To(ColumnNamesOld.SS_HYDRANT);
            Rename.Column(ColumnNames.SS_LAT).OnTable(TableNames.SERVICE_SIZES).To(ColumnNamesOld.SS_LAT);
            Rename.Column(ColumnNames.SS_ORDER).OnTable(TableNames.SERVICE_SIZES).To(ColumnNamesOld.SS_ORDER);
            Rename.Column(ColumnNames.SS_SERVICE).OnTable(TableNames.SERVICE_SIZES).To(ColumnNamesOld.SS_SERVICE);
            Alter.Column(ColumnNames.SS_SIZE_SERVICE).OnTable(TableNames.SERVICE_SIZES).AsAnsiString(6).NotNullable()
                 .Nullable();
            Rename.Column(ColumnNames.SS_SIZE_SERVICE).OnTable(TableNames.SERVICE_SIZES)
                  .To(ColumnNamesOld.SS_SIZE_SERVICE);
            Rename.Column(ColumnNames.SS_SIZE_SERVICE_DESCRIPTION).OnTable(TableNames.SERVICE_SIZES)
                  .To(ColumnNamesOld.SS_SIZE_SERVICE_DESCRIPTION);

            Rename.Table(TableNames.SERVICE_SIZES).To(TableNamesOld.SERVICE_SIZES);

            Execute.Sql(
                "ALTER TABLE [dbo].[tblNJAWSizeServ] ADD  CONSTRAINT [DF_tblNJAWSizeServ_Hyd]  DEFAULT ('') FOR [Hyd];" +
                "ALTER TABLE[dbo].[tblNJAWSizeServ] ADD  CONSTRAINT[DF_tblNJAWSizeServ_Lat]  DEFAULT('') FOR[Lat];" +
                "ALTER TABLE[dbo].[tblNJAWSizeServ] ADD  CONSTRAINT[DF_tblNJAWSizeServ_Main]  DEFAULT('') FOR[Main];" +
                "ALTER TABLE[dbo].[tblNJAWSizeServ] ADD  CONSTRAINT[DF_tblNJAWSizeServ_Meter]  DEFAULT('') FOR[Meter];" +
                "ALTER TABLE[dbo].[tblNJAWSizeServ] ADD  CONSTRAINT[DF_tblNJAWSizeServ_Serv]  DEFAULT('') FOR[Serv];" +
                "ALTER TABLE[dbo].[tblNJAWSizeServ] ADD  CONSTRAINT[DF_tblNJAWSizeServ_SizeServ]  DEFAULT('') FOR[SizeServ];" +
                "ALTER TABLE[dbo].[tblNJAWSizeServ] ADD  CONSTRAINT[DF_tblNJAWSizeServ_Valve]  DEFAULT('') FOR[Valve]");

            #endregion
        }
    }
}
