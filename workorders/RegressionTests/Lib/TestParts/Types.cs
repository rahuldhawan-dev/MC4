namespace RegressionTests.Lib.TestParts
{
    public static class Types
    {
        public struct WorkOrder
        {
            public string WorkOrderID,
                OperatingCenter,
                OperatingCenterName,
                Town,
                TownSection,
                StreetNumber,
                Street,
                NearestCrossStreet,
                ZipCode,
                AssetType,
                RequestedBy,
                Purpose,
                Priority,
                DescriptionOfWork,
                MarkoutRequirement,
                Notes,
                CreatedBy,
                DateReceived,
                AccountCharged,
                DateCompleted,
                ValveID,
                HydrantID,
                SewerOpeningID,
                PremiseNumber,
                ServiceNumber,
                CustomerImpactRange,
                RepairTimeRange,
                AlertIssued,
                SignificantTrafficImpact,
                LostWater,
                EquipmentID,
                Latitude,
                Longitude,
                StreetOpeningPermitRequired,
                SAPWorkOrderNumber, 
                SAPNotificationNumber,
                ApartmentAddtl;

            public string GetAddress()
            {
                return (TownSection == null) ?
                    string.Format("{0} {1} {2} NJ", StreetNumber, Street, Town) :
                    string.Format("{0} {1} {2} {3} NJ", StreetNumber, Street,
                                  TownSection, Town);
            }
        }

        public struct CrewAssignment
        {
            public string Crew,
                          CrewID,
                          AssignedDate,
                          DateStarted,
                          DateEnded,
                          EmployeesOnCrew;
        }

        public struct MaterialsUsed
        {
            public string Quantity, PartNumber, StockLocation, Description;
        }

        public struct MainBreak
        {
            public string Material,
                          MainCondition,
                          FailureType,
                          Depth,
                          SoilCondition,
                          CustomersAffected,
                          ShutDownTime,
                          DisinfectionMethod,
                          FlushMethod,
                          ChlorineResidual,
                          Size;
        }

        public struct Markout
        {
            public string MarkoutNumber,
                MarkoutType,
                DateOfRequest,
                ReadyDate,
                ExpirationDate,
                Notes;
        }
    }
}
