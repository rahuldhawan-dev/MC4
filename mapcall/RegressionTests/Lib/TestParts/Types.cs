using System;

namespace RegressionTests.Lib.TestParts
{
    public static class Types
    {
        public struct Contractor
        {
            public string Name;
        }

        public struct Grievance
        {
            public string DateReceived;
        }

        public struct Hydrant
        {
            public string PremiseNumber,
                          BillingDate,
                          Town,
                          Street,
                          OpCenterVerify,
                          OpCenterCreate,
                          StreetID,
                          WONum;
        }

        public struct Permit
        {
            public string Municipality,
                          County,
                          FeePermit,
                          FeeInspec,
                          StreetMaterial,
                          BondAmount,
                          FeeRestoration,
                          FeeTotal,
                          DateBegin,
                          DateEnd,
                          EngineerAmount,
                          StatePermitID,
                // TODO: Does not belong here?
                          SuccessText;
        }

        public struct Restoration
        {
            public string OperatingCenter,
                          OperatingCenterID,
                          ServiceNumber,
                          PremiseNumber,
                          TypeRestore, 
                          InvoiceNumber;

        }

        public struct Service
        {
            public string Town,
                          Street,
                          StreetNumber,
                          Lot,
                          Block,
                          CategoryOfService,
                          OperatingCenter,
                          ServNum,
                          PremNum,
                          ServiceSize,
                          PurposeOfInstallation,
                          OperatingCenterID,
                          Material,
                          MainSize,
                          Priority, 
                          MainType,
                          PreviousServiceSize,
                          PreviousServiceMaterial,
                          RetirementDate,
                          OriginalInstallationDate;
        }

        public struct SewerMainCleaning
        {
            public string OpCenterCreate,
                          OpCenterVerify,
                          Date,
                          CompletedDate,
                          Town;
        }

        public struct SewerManhole
        {
            public string Town;
        }

        public struct SickBank
        {
            public string CalendarYear;
        }

        public struct StormwaterAsset
        {
            public string AssetNumber,
                // TODO: What is this here for?
                          User,
                          OpCenterCreate,
                          OpCenterVerify,
                          StormwaterAssetType,
                          Street,
                          Town, 
                          Latitude, 
                          Longitude;
        }

        public struct Street
        {
            public string StreetName,
                State,
                County,
                TownName,
                StreetSuffix;
        }

        public struct TailgateTalk
        {
            public string HeldOn,
                          Topic,
                          Description,
                          PresentedBy,
                          TrainingTimeHours;
        }

        public struct Town
        {
            public string TownshipName,
                          TownName,
                          State,
                          Zip,
                          TownAbbreviation,
                          County;
        }

        public struct TrainingModule
        {
            public string Title, 
                          TotalHours, 
                          TCH_Credit_Value, 
                          OpCode, 
                          IsActive;
        }

        public struct TrainingRecord
        {
            public string HeldOn,
                          TrainingModule,
                          Description, 
                          Instructor,
                          CourseLocation;
        }

        public struct Valve
        {
            public string ValveSize,
                          Street,
                          ValveZone,
                          OperatingCenter,
                          Town;
        }

        public struct Vehicle
        {
            public string VehicleIcon;
        }

        public struct WQComplaint
        {
            public string OrcomOrderNumber;
        }
        [Serializable]
        public struct User
        {
            public string UserName,
                          Password,
                          Region,
                          OpCenter,
                          OpCenterCode,
                          FullName,
                          EmployeeNumber,
                          Email,
                          FORole,
                          EW1BPUGeneralAdd,
                          EW1BPUGeneralDelete,
                          UserType;
        }

        public struct TapImage
        {
            public string town,
                          dated,
                          serviceType,
                          premiseNumber,
                          street,
                          streetNumber,
                          filename,
                          ShortFileName,
                          serviceNumber,
                          State,
                          County,
                          OpCenter;
        }
    }
}
