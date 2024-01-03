namespace MapCallImporter.SampleValues
{
    public struct Facilities
    {
        public struct NJSB10
        {
            public const int ID = 10;

            public const string FUNCTIONAL_LOCATION = "NJMM-MM-ABTIC",
                                FACILITY_ID = "NJ7-10";
        }

        public struct NJSB2
        {
            public const int ID = 2;

            public const string FUNCTIONAL_LOCATION = "NJMM-MM-ABTNK",
                                FACILITY_ID = "NJ7-2";
        }

        public struct NJSB11
        {
            public const int ID = 11;

            public static string GetInsertQuery()
                => $"INSERT INTO tblFacilities (PublicWaterSupplyId, StreetNumber, StreetId, Status, PlanningPlantId, CreatedAt, OperatingCenterId, FacilityName, Facility_Ownership, RecordId, DepartmentId, PSM, RMP, SwmStation, WellProd, WellMonitoring, ClearWell, RawWaterIntake, UpdatedAt, Radionuclides, CommunityRightToKnow, IsInVamp, BasicGroundWaterSupply, RawWaterPumpStation, SystemDeliveryTypeId, WaterStress, IgnitionEnterprisePortal, ArcFlashLabelRequired) VALUES (22, '600', 2962, 159, 26, '2019-06-06 14:34:29', 10, 'ARCOLA/TUSCOLA - PUMP STATION', 157, {ID}, 3, 0, 0, 0, 0, 0, 0, 0, '2002-08-27T09:08:00.000', 0, 0, 0, 0, 0, 1, 0, 0, 0);";
        }
    }
}