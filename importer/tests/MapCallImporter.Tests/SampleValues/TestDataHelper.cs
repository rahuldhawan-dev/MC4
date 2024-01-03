using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings.Users;
using MapCallImporter.Library.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.NHibernate.V2;
using NHibernate;
using StructureMap;

// ReSharper disable once CheckNamespace
namespace MapCallImporter.SampleValues
{
    public static class TestDataHelper
    {
        #region Private Methods

        private static int ExecuteSql(IStatelessSession session, params string[] queries)
        {
            return queries.Where(q => q != null).Sum(query => session.CreateSQLQuery(query).ExecuteUpdate());
        }

        private static int CreateCommonStuffForAssetsInAberdeenNJ(IContainer container)
        {
            return CreateAberdeenNJWithCountyAndStateAndSomeStreets(container) + ExecuteSql(
                container.GetInstance<IStatelessSession>(),

                // Recurring Frequency Units
                $@"
                INSERT INTO RecurringFrequencyUnits (Id, Description) VALUES ({RecurringFrequencyUnit.Indices.DAY}, 'Day');
                INSERT INTO RecurringFrequencyUnits (Id, Description) VALUES ({RecurringFrequencyUnit.Indices.MONTH}, 'Month');
                INSERT INTO RecurringFrequencyUnits (Id, Description) VALUES ({RecurringFrequencyUnit.Indices.WEEK}, 'Week');
                INSERT INTO RecurringFrequencyUnits (Id, Description) VALUES ({RecurringFrequencyUnit.Indices.YEAR}, 'Year');",

                // Asset Statuses
                $@"
                INSERT INTO AssetStatuses (AssetStatusId, Description, IsUserAdminOnly) VALUES ({AssetStatus.Indices.ACTIVE},'ACTIVE', 1);
                INSERT INTO AssetStatuses (AssetStatusId, Description, IsUserAdminOnly) VALUES ({AssetStatus.Indices.CANCELLED},'CANCELLED', 1);
                INSERT INTO AssetStatuses (AssetStatusId, Description, IsUserAdminOnly) VALUES ({AssetStatus.Indices.PENDING},'PENDING', 1);
                INSERT INTO AssetStatuses (AssetStatusId, Description, IsUserAdminOnly) VALUES ({AssetStatus.Indices.RETIRED},'RETIRED', 1);
                INSERT INTO AssetStatuses (AssetStatusId, Description, IsUserAdminOnly) VALUES ({AssetStatus.Indices.REMOVED},'REMOVED', 1);
                INSERT INTO AssetStatuses (AssetStatusId, Description, IsUserAdminOnly) VALUES ({AssetStatus.Indices.INACTIVE},'INACTIVE', 1);
                INSERT INTO AssetStatuses (AssetStatusId, Description, IsUserAdminOnly) VALUES ({AssetStatus.Indices.INSTALLED},'INSTALLED', 0);
                INSERT INTO AssetStatuses (AssetStatusId, Description, IsUserAdminOnly) VALUES ({AssetStatus.Indices.REQUEST_RETIREMENT},'REQUEST RETIREMENT', 0);
                INSERT INTO AssetStatuses (AssetStatusId, Description, IsUserAdminOnly) VALUES ({AssetStatus.Indices.REQUEST_CANCELLATION},'REQUEST CANCELLATION', 0);",

                // WBS Numbers
                "INSERT INTO WBSNumbers (Id, Description) VALUES (1, 'B18-02-0001')",

                // Coordinates
                "INSERT INTO Coordinates (CoordinateID, Latitude, Longitude) VALUES (2329095, 40.3224, -74.1481);",

                // Operating Centers
                $"INSERT INTO OperatingCenters (OperatingCenterID, CoInfo, CSNum, FaxNum, MailAdd, MailCo, MailCSZ, OperatingCenterCode, OperatingCenterName, ServContactNum, WorkOrdersEnabled, PermitsOMUserName, PermitsCapitalUserName, RSADivisionNumber, StateId, IsActive, StateRegionId, HydrantInspectionFrequency, HydrantInspectionFrequencyUnitId, SewerOpeningInspectionFrequency, SewerOpeningInspectionFrequencyUnitId, LargeValveInspectionFrequency, LargeValveInspectionFrequencyUnitId, SmallValveInspectionFrequency, SmallValveInspectionFrequencyUnitId, MaximumOverflowGallons, DefaultServiceReplacementWBSNumberId, InfoMasterMapId, InfoMasterMapLayerName, HasWorkOrderInvoicing, SAPEnabled, IsContractedOperations, SAPWorkOrdersEnabled, OperatedByOperatingCenterId, UsesValveInspectionFrequency, CoordinateId, MapId, MarkoutsEditable) VALUES ({OperatingCenters.NJ7.ID}, 'New Jersey American Water, Shrewsbury Operating Center', '800-652-6987', '732-842-0866', '661 Shrewsbury Ave', 'New Jersey American Water', 'Shrewsbury, NJ 07702', '{OperatingCenters.NJ7.CODE}', '{OperatingCenters.NJ7.NAME}', '732-933-5911', 1, 'permits-nj7-om@mapcall.com', 'permits-nj7-cap@mapcall.com', NULL, 1, 1, NULL, 1, 4, 1, 4, 4, 4, 4, 4, NULL, 1, '525f43d20acc43899b6dc7ec3afc6b53', 'NJ4_NJ7_wPressurizedMain_2017_6442', 0, 1, 0, 1, NULL, 0, 2329095, '8826a27b17274ab284f0fb39a3b6ecc1', 0);",

                // Operating Centers
                $"INSERT INTO OperatingCenters (OperatingCenterID, CoInfo, CSNum, FaxNum, MailAdd, MailCo, MailCSZ, OperatingCenterCode, OperatingCenterName, ServContactNum, WorkOrdersEnabled, PermitsOMUserName, PermitsCapitalUserName, RSADivisionNumber, StateId, IsActive, StateRegionId, HydrantInspectionFrequency, HydrantInspectionFrequencyUnitId, SewerOpeningInspectionFrequency, SewerOpeningInspectionFrequencyUnitId, LargeValveInspectionFrequency, LargeValveInspectionFrequencyUnitId, SmallValveInspectionFrequency, SmallValveInspectionFrequencyUnitId, MaximumOverflowGallons, DefaultServiceReplacementWBSNumberId, InfoMasterMapId, InfoMasterMapLayerName, HasWorkOrderInvoicing, SAPEnabled, IsContractedOperations, SAPWorkOrdersEnabled, OperatedByOperatingCenterId, UsesValveInspectionFrequency, CoordinateId, MapId, MarkoutsEditable) VALUES ({OperatingCenters.NJ4.ID}, 'New Jersey American Water, Lakewood Operating Center', '800-652-6987', '732-842-0866', '661 Lakewood Ave', 'New Jersey American Water', 'Lakewood, NJ 07702', '{OperatingCenters.NJ4.CODE}', '{OperatingCenters.NJ4.NAME}', '732-933-5911', 1, 'permits-nj7-om@mapcall.com', 'permits-nj7-cap@mapcall.com', NULL, 1, 1, NULL, 1, 4, 1, 4, 4, 4, 4, 4, NULL, 1, '525f43d20acc43899b6dc7ec3afc6b53', 'NJ4_NJ7_wPressurizedMain_2017_6442', 0, 1, 0, 1, NULL, 1, 2329095, '8826a27b17274ab284f0fb39a3b6ecc1', 0);",

                // Operating Center Towns
                "INSERT INTO OperatingCentersTowns (OperatingCenterId, TownId, Abbreviation, Id) VALUES (10, 41, 'AB', 1)",

                // Operating Center Towns
                "INSERT INTO OperatingCentersTowns (OperatingCenterId, TownId, Abbreviation, Id) VALUES (14, 41, 'AB', 2)",

                // Water Systems
                $"INSERT INTO WaterSystems (Id, Description) VALUES ({WaterSystems.NJMM}, 'NJMM');",

                // Commercial Drivers License Program Statuses
                "INSERT INTO CommercialDriversLicenseProgramStatuses (Id, Description) VALUES (1, 'MEH');",

                // tblEmployee
                "INSERT INTO tblEmployee (tblEmployeeID, EmployeeId, CommercialDriversLicenseProgramStatusId) VALUES (1, '99999999', 1);",

                // User Types
                "INSERT INTO UserTypes (Id, Description) VALUES (1, 'Internal');",

                // tblPermissions
                $"INSERT INTO {UserMap.TABLE_NAME} (RecId, UserName, EmployeeId, DefaultOperatingCenterId, HasAccess, IsSiteAdministrator, UserTypeId, IsUserAdministrator) VALUES (402, 'mcadmin', 1, {OperatingCenters.NJ7.ID}, 1, 1, 1, 1)",

                // Map Icon Offsets
                "INSERT INTO MapIconOffsets (Id, Description) VALUES (8, 'bottom-center')",

                // Map Icon
                "INSERT INTO MapIcon (iconID, iconURL, width, height, OffsetId) VALUES (8, 'MapIcons/pin_blue.gif', 16, 27, 8);",

                // Coordinates
                @"
                INSERT INTO Coordinates (CoordinateID, Latitude, Longitude, IconID) VALUES (810064, 40.393989, -74.211844, 8);
                INSERT INTO Coordinates (CoordinateID, Latitude, Longitude, IconID) VALUES (1445919, 40.40712, -74.22426, 8);",

                // Departments
                $"INSERT INTO Departments (DepartmentID, Code, Description) VALUES ({ProductionDepartment.ID}, '{ProductionDepartment.CODE}', '{ProductionDepartment.DESCRIPTION}');",

                // Facility Owners
                $"INSERT INTO FacilityOwners (FacilityOwnerID, Description) VALUES ({AmericanWaterFacilityOwner.ID}, '{AmericanWaterFacilityOwner.DESCRIPTION}');",

                // Planning Plants
                $"INSERT INTO PlanningPlants (Id, Code, Description, OperatingCenterId) VALUES ({P218PlanningPlant.ID}, '{P218PlanningPlant.CODE}', '{P218PlanningPlant.DESCRIPTION}', {P218PlanningPlant.OPERATING_CENTER_ID});",

                // Facility Statuses
                @"
                INSERT INTO FacilityStatuses (FacilityStatusId, Description) VALUES (159, 'Active');
                INSERT INTO FacilityStatuses (FacilityStatusId, Description) VALUES (160, 'Inactive');",

                // Asset Types
                "INSERT INTO AssetTypes (AssetTypeId, Description) VALUES (1, 'Valve');",

                // Functional Locations
                $@"
                INSERT INTO FunctionalLocations (FunctionalLocationID, Description, TownID, AssetTypeID, IsActive) VALUES ({
                    FunctionalLocations.NJMM_AB_VALVE}, 'NJMM-AB-VALVE', 41, 1, 1);
                INSERT INTO FunctionalLocations (FunctionalLocationID, Description, TownID, AssetTypeID, IsActive) VALUES ({
                    FunctionalLocations.NJMM_AB_HYDRT}, 'NJMM-AB-HYDRT', 41, 1, 1);",

                // Arc Flash Statuses
                "INSERT INTO ArcFlashStatuses (Id, Description) VALUES (2, 'Pending');",

                // Company Subsidiaries
                $"INSERT INTO CompanySubsidiaries (Id, Description) VALUES ({CompanySubsidiaries.NJAW.ID}, '{CompanySubsidiaries.NJAW.DESCRIPTION}');",

                // Public Water Supplies
                $"INSERT INTO PublicWaterSupplies (Id, PWSID, FreeChlorineReported, TotalChlorineReported, UpdatedAt) VALUES ({PublicWaterSupplies._1345001.ID}, '{PublicWaterSupplies._1345001.PWSID}', 0, 0, '2022-11-21');",

                // Facility Stuff
                $"INSERT INTO FacilityAssetManagementMaintenanceStrategyTiers (Id, Description) VALUES (1, 'Tier 1');",
                $"INSERT INTO FacilityAssetManagementMaintenanceStrategyTiers (Id, Description) VALUES (2, 'Tier 2');",
                $"INSERT INTO FacilityAssetManagementMaintenanceStrategyTiers (Id, Description) VALUES (3, 'Tier 3');",
                $"INSERT INTO FacilityConsequencesOfFailure (Id, Description) VALUES ({FacilityConsequenceOfFailure.Indices.LOW}, 'Low');",
                $"INSERT INTO FacilityConsequencesOfFailure (Id, Description) VALUES ({FacilityConsequenceOfFailure.Indices.MEDIUM}, 'Medium');",
                $"INSERT INTO FacilityConsequencesOfFailure (Id, Description) VALUES ({FacilityConsequenceOfFailure.Indices.HIGH}, 'High');",
                $"INSERT INTO FacilityConditions (Id, Description) VALUES ({FacilityCondition.Indices.POOR}, 'Poor');",
                $"INSERT INTO FacilityConditions (Id, Description) VALUES ({FacilityCondition.Indices.AVERAGE}, 'Average');",
                $"INSERT INTO FacilityConditions (Id, Description) VALUES ({FacilityCondition.Indices.GOOD}, 'Good');",
                $"INSERT INTO FacilityLikelihoodsOfFailure (Id, Description) VALUES (1, 'Low');",
                $"INSERT INTO FacilityLikelihoodsOfFailure (Id, Description) VALUES (2, 'Medium');",
                $"INSERT INTO FacilityLikelihoodsOfFailure (Id, Description) VALUES (3, 'High');",
                $"INSERT INTO FacilityPerformances (Id, Description) VALUES ({FacilityPerformance.Indices.POOR}, 'Poor');",
                $"INSERT INTO FacilityPerformances (Id, Description) VALUES ({FacilityPerformance.Indices.AVERAGE}, 'Average');",
                $"INSERT INTO FacilityPerformances (Id, Description) VALUES ({FacilityPerformance.Indices.GOOD}, 'Good');",

                // Facility Maintenance Risks Of Failure
                $"INSERT INTO FacilityMaintenanceRiskOfFailures(Id, RiskScore, Description) Values({FacilityMaintenanceRiskOfFailure.Indices.LOW}, 1, '1 - Low Risk Control');",
                $"INSERT INTO FacilityMaintenanceRiskOfFailures(Id, RiskScore, Description) Values({FacilityMaintenanceRiskOfFailure.Indices.LOW_MODERATE}, 2, '2 - Low-Moderate Risk');",
                $"INSERT INTO FacilityMaintenanceRiskOfFailures(Id, RiskScore, Description) Values({FacilityMaintenanceRiskOfFailure.Indices.MODERATE}, 3, '3 - Moderate Risk');",
                $"INSERT INTO FacilityMaintenanceRiskOfFailures(Id, RiskScore, Description) Values({FacilityMaintenanceRiskOfFailure.Indices.MODERATE_HIGH}, 4, '4 - Moderate-High Risk');",
                $"INSERT INTO FacilityMaintenanceRiskOfFailures(Id, RiskScore, Description) Values({FacilityMaintenanceRiskOfFailure.Indices.HIGH}, 6, '6 - High Risk');",
                $"INSERT INTO FacilityMaintenanceRiskOfFailures(Id, RiskScore, Description) Values({FacilityMaintenanceRiskOfFailure.Indices.CRITICAL}, 9, '9 - High-Critical Risk');",

                // System Delivery Types
                $@"
                INSERT INTO SystemDeliveryTypes (Id, Description) 
                VALUES ({SystemDeliveryTypes.Water.ID}, '{SystemDeliveryTypes.Water.DESCRIPTION}');",

                // System Delivery Types
                $@"
                INSERT INTO SystemDeliveryTypes (Id, Description) 
                VALUES ({SystemDeliveryTypes.WasteWater.ID}, '{SystemDeliveryTypes.WasteWater.DESCRIPTION}');",

                // tblFacilities
                $@"
                INSERT INTO tblFacilities (RecordId, CreatedAt, PSM, RMP, DepartmentID, OperatingCenterID, SwmStation, WellProd, WellMonitoring, ClearWell, RawWaterIntake, UpdatedAt, CompanySubsidiaryId, PublicWaterSupplyId, FunctionalLocationId, Radionuclides, CommunityRightToKnow, IgnitionEnterprisePortal, ArcFlashLabelRequired, IsInVamp, BasicGroundWaterSupply, RawWaterPumpStation, SystemDeliveryTypeId, WaterStress)
                VALUES ({Facilities.NJSB10.ID}, '2002-07-27 09:16:00.000', 0, 0, 3, 10, 0, 0, 0, 0, 0, '2002-07-27 09:16:00.000', {CompanySubsidiaries.NJAW.ID},
                        {PublicWaterSupplies._1345001.ID}, '{Facilities.NJSB10.FUNCTIONAL_LOCATION}', 0, 0, 0, 0, 0, 0, 0, {SystemDeliveryTypes.Water.ID}, 0);
                INSERT INTO tblFacilities (RecordId, CreatedAt, PSM, RMP, DepartmentID, OperatingCenterID, SwmStation, WellProd, WellMonitoring, ClearWell, RawWaterIntake, UpdatedAt, CompanySubsidiaryId, PublicWaterSupplyId, FunctionalLocationId, Radionuclides, CommunityRightToKnow, IgnitionEnterprisePortal, ArcFlashLabelRequired, IsInVamp, BasicGroundWaterSupply, RawWaterPumpStation, SystemDeliveryTypeId, WaterStress)
                VALUES ({Facilities.NJSB2.ID}, '2002-07-27 09:08:00.000', 0, 0, 3, 10, 0, 0, 0, 0, 0, '2002-07-27 09:08:00.000', {CompanySubsidiaries.NJAW.ID},
                        {PublicWaterSupplies._1345001.ID}, '{Facilities.NJSB2.FUNCTIONAL_LOCATION}', 0, 0, 0, 0, 0, 0, 0, {SystemDeliveryTypes.Water.ID}, 0);",

                // ABC Indicators
                @"
                INSERT INTO ABCIndicators (Id, Description) VALUES (1, 'Low');
                INSERT INTO ABCIndicators (Id, Description) VALUES (2, 'Medium');
                INSERT INTO ABCIndicators (Id, Description) VALUES (3, 'High');",

                // Public Water Supply Pressure Zones
                $@"
                INSERT INTO PublicWaterSupplyPressureZones (Id, PublicWaterSupplyId, HydraulicModelName, HydraulicGradientMin, HydraulicGradientMax) 
                VALUES ({PublicWaterSupplyPressureZones.ID}, {PublicWaterSupplies._1345001.ID}, '{PublicWaterSupplyPressureZones.HYDRAULIC_MODEL_NAME}', 1, 2);",

                // Waste Water Systems
                $@"
                INSERT INTO WasteWaterSystems (Id, OperatingCenterId, WasteWaterSystemName, PermitNumber) 
                VALUES ({WasteWaterSystems.ID}, {OperatingCenters.NJ7.ID}, '{WasteWaterSystems.WASTE_WATER_SYSTEM_NAME}', '{WasteWaterSystems.PERMIT_NUMBER}');",

                // Waste Water System Basins
                $@"
                INSERT INTO WasteWaterSystemBasins (Id, WasteWaterSystemId, BasinName) 
                VALUES ({WasteWaterSystemBasins.ID}, {WasteWaterSystems.ID}, '{WasteWaterSystemBasins.BASIN_NAME}');"
            );
        }

        private static TestDataFactory<TEntity> GetEntityFactory<TEntity>(IContainer container) where TEntity : class, new() =>
            new TestDataFactoryService(container, typeof(TestDataHelper).Assembly).GetEntityFactory<TEntity>();

        #endregion

        #region Exposed Methods

        public static int CreateAberdeenNJWithCountyAndStateAndSomeStreets(IContainer container)
        {
            return ExecuteSql(container.GetInstance<IStatelessSession>(),
                @"
                INSERT INTO AbbreviationTypes (AbbreviationTypeID, Description) VALUES (1, 'Town');
                INSERT INTO AbbreviationTypes (AbbreviationTypeID, Description) VALUES (2, 'Town Section');
                INSERT INTO AbbreviationTypes (AbbreviationTypeID, Description) VALUES (3, 'Fire District');",

                // States, Counties, and Towns
                $"INSERT INTO States (stateID, Name, Abbreviation) VALUES ({NJState.ID}, '{NJState.NAME}', '{NJState.ABBREVIATION}');",
                $"INSERT INTO Counties (CountyID, Name, stateID) VALUES ({MonmouthNJCounty.ID}, '{MonmouthNJCounty.NAME}', {MonmouthNJCounty.STATE_ID});",
                $"INSERT INTO Towns (TownID, Town, StateID, CountyID, AbbreviationTypeId) VALUES ({AberdeenMonmouthNJTown.ID}, '{AberdeenMonmouthNJTown.TOWN}', {AberdeenMonmouthNJTown.STATE_ID}, {AberdeenMonmouthNJTown.COUNTY_ID}, 1)",

                // Street Prefixes
                $"INSERT INTO StreetPrefixes (Id, Description) VALUES ({StreetPrefixes.South.ID}, '{StreetPrefixes.South.DESCRIPTION}');",
                $"INSERT INTO StreetPrefixes (Description) VALUES ('POSITIVELY');",

                // Street Suffixes
                $"INSERT INTO StreetSuffixes (Id, Description) VALUES ({StreetSuffixes.Avenue.ID}, '{StreetSuffixes.Avenue.DESCRIPTION}');",
                $"INSERT INTO StreetSuffixes (Id, Description) VALUES ({StreetSuffixes.Lane.ID}, '{StreetSuffixes.Lane.DESCRIPTION}');",
                $"INSERT INTO StreetSuffixes (Id, Description) VALUES ({StreetSuffixes.Road.ID}, '{StreetSuffixes.Road.DESCRIPTION}');",
                $"INSERT INTO StreetSuffixes (Id, Description) VALUES ({StreetSuffixes.Street.ID}, '{StreetSuffixes.Street.DESCRIPTION}');",

                // Streets
                $@"
                INSERT INTO Streets (StreetID, FullStName, PrefixId, StreetName, SuffixId, TownID) VALUES ({
                    AberdeenMonmouthNJStreets.IdlewildLane.ID}, '{AberdeenMonmouthNJStreets.IdlewildLane.NAME}', NULL, 'IDLEWILD', {AberdeenMonmouthNJStreets.IdlewildLane.SUFFIX_ID}, {AberdeenMonmouthNJTown.ID});
                INSERT INTO Streets (StreetID, FullStName, PrefixId, StreetName, SuffixId, TownID) VALUES ({
                    AberdeenMonmouthNJStreets.ChurchStreet.ID}, '{AberdeenMonmouthNJStreets.ChurchStreet.NAME}', NULL, 'CHURCH', {AberdeenMonmouthNJStreets.ChurchStreet.SUFFIX_ID}, {AberdeenMonmouthNJTown.ID});
                INSERT INTO Streets (StreetID, FullStName, PrefixId, StreetName, SuffixId, TownID) VALUES ({
                    AberdeenMonmouthNJStreets.SouthAtlanticAvenue.ID}, '{AberdeenMonmouthNJStreets.SouthAtlanticAvenue.NAME}', {AberdeenMonmouthNJStreets.SouthAtlanticAvenue.PREFIX_ID}, 'ATLANTIC', {AberdeenMonmouthNJStreets.SouthAtlanticAvenue.SUFFIX_ID}, {AberdeenMonmouthNJTown.ID});");
        }

        public static int CreateStuffForArcFlashSudiesInAberdeenNJ(IContainer container)
        {
            return CreateSomeFacilitiesInAberdeenNJ(container) + ExecuteSql(container.GetInstance<IStatelessSession>(),
                $"INSERT INTO UtilityCompanies(Description, StateId) VALUES ( 'Jersey Central Power & Light', {NJState.ID});",
                
                // FacilitySize
                "INSERT INTO FacilitySizes(Id, Description) VALUES(3, 'Large');", 
                "INSERT INTO FacilitySizes(Id, Description) VALUES(2, 'Medium');",
                "INSERT INTO FacilitySizes(Id, Description) VALUES(1, 'Small');",

                // ArcFlashAnalysisType
                "INSERT INTO ArcFlashAnalysisTypes (Id, Description) VALUES (1, 'Utility Data');",
                "INSERT INTO ArcFlashAnalysisTypes (Id, Description) VALUES (2, 'Infinite Bus');",

                // ArcFlashLabelType
                "INSERT INTO ArcFlashLabelTypes (Id, Description) VALUES (1, 'Standard Label');",
                "INSERT INTO ArcFlashLabelTypes (Id, Description) VALUES (2, 'Custom Label');",

                // UtilityTransformerKVARating
                "INSERT INTO UtilityTransformerKVARatings (Id, Description) VALUES (1, '1');",
                "INSERT INTO UtilityTransformerKVARatings (Id, Description) VALUES (2, '1.5');",
                "INSERT INTO UtilityTransformerKVARatings (Id, Description) VALUES (9, '10');",
                "INSERT INTO UtilityTransformerKVARatings (Id, Description) VALUES (17, '100');",
                "INSERT INTO UtilityTransformerKVARatings (Id, Description) VALUES (29, '1000');",
                "INSERT INTO UtilityTransformerKVARatings (Id, Description) VALUES (3, '2');",
                "INSERT INTO UtilityTransformerKVARatings (Id, Description) VALUES (35, '20');",
                "INSERT INTO UtilityTransformerKVARatings (Id, Description) VALUES (22, '200');",
                "INSERT INTO UtilityTransformerKVARatings (Id, Description) VALUES (23, '225');",
                "INSERT INTO UtilityTransformerKVARatings (Id, Description) VALUES (11, '25');",

                // Voltage
                "INSERT INTO Voltages (Id, Description) VALUES (3, '120/208');",
                "INSERT INTO Voltages (Id, Description) VALUES (1, '120/240');",
                "INSERT INTO Voltages (Id, Description) VALUES (4, '240');",
                "INSERT INTO Voltages (Id, Description) VALUES (6, '2400');",
                "INSERT INTO Voltages (Id, Description) VALUES (2, '277/480');",
                "INSERT INTO Voltages (Id, Description) VALUES (7, '4160');",
                "INSERT INTO Voltages (Id, Description) VALUES (5, '480');",

                // PowerPhase
                "INSERT INTO PowerPhases (Id, Description) VALUES (3, 'Other');",
                "INSERT INTO PowerPhases (Id, Description) VALUES (1, 'Single');",
                "INSERT INTO PowerPhases (Id, Description) VALUES (2, 'Three');",

                // FacilityTransformerWiringType
                "INSERT INTO FacilityTransformerWiringTypes (Id, Description) VALUES (2, 'Delta');",
                "INSERT INTO FacilityTransformerWiringTypes (Id, Description) VALUES (1, 'Wye');"
            );
        }

        public static int CreateStuffForValvesInAberdeenNJ(IContainer container)
        {
            return CreateCommonStuffForAssetsInAberdeenNJ(container) + ExecuteSql(container.GetInstance<IStatelessSession>(),

                // Valve Zones
                "INSERT INTO ValveZones (Id, Description) VALUES (1, '1');",

                // Valve Billings
                $"INSERT INTO ValveBillings (Id, Description) VALUES ({ValveBilling.Indices.PUBLIC}, 'PUBLIC');",

                // Valve Controls
                $"INSERT INTO ValveControls (Id, Description) VALUES ({ValveControl.Indices.HYDRANT}, 'HYDRANT');",

                // Valve Normal Positions
                @"
                INSERT INTO ValveNormalPositions (Id, Description) VALUES (2, 'Closed');
                INSERT INTO ValveNormalPositions (Id, Description) VALUES (1, 'Open');",

                // Valve Open Directions
                $@"
                INSERT INTO ValveOpenDirections (Id, Description) VALUES ({ValveOpenDirection.Indices.LEFT}, 'Left');
                INSERT INTO ValveOpenDirections (Id, Description) VALUES ({ValveOpenDirection.Indices.RIGHT}, 'Right');",

                // Valve Sizes
                "INSERT INTO ValveSizes (Id, Size) VALUES (33, CAST(6.000 AS Decimal(5, 3)));",
                "INSERT INTO ValveSizes VALUES(1, 0.750)",

                // Valve Types
                "INSERT INTO ValveTypes (Id, Description, SAPCode) VALUES (5, 'GATE', 'GATE');"
            );
        }

        public static int CreateStuffForHydrantsInAberdeenNJ(IContainer container)
        {
            return CreateStuffForValvesInAberdeenNJ(container) +
                   CreateValidValvesLikeTheValidValvesInTheValidValvesFile(container) +
                   ExecuteSql(container.GetInstance<IStatelessSession>(),

                       // Fire District
                       "INSERT INTO FireDistricts (Id, AddressCity, DistrictName, StateId) VALUES (141, 'Aberdeen', 'ABERDEEN', 1);",

                       // Hydrant Manufacturers
                       @"INSERT INTO HydrantManufacturers (Id, Description) VALUES (18, 'MUELLER');
                         INSERT INTO HydrantManufacturers (Id, Description) VALUES (31, 'TRAVERSE CITY IRON WORKS');",

                       // Misc. Hydrant Stuff
                       "INSERT INTO LateralSizes (Id, Description, Size, SortOrder) VALUES (2, 6, 6.000, 8);",
                       "INSERT INTO HydrantDirections (Id, Description) VALUES (2, 'RIGHT');",
                       "INSERT INTO HydrantSizes (Id, Description, Size, SortOrder) VALUES (7, '5 1/4', 5.250, 23);",
                       "INSERT INTO HydrantMainSizes (Id, Description, Size, SortOrder) VALUES (23, 6, 6.000, 8);",
                       "INSERT INTO HydrantThreadTypes (Id, Description) VALUES (3, 'NST');",
                       "INSERT INTO HydrantBillings (Id, Description) VALUES (2, 'Public');",

                       // Hydrant Inspection Types
                       @"
                        INSERT INTO HydrantInspectionTypes (Id, Description) VALUES (1, 'FLUSH');
                        INSERT INTO HydrantInspectionTypes (Id, Description) VALUES (2, 'INSPECT');
                        INSERT INTO HydrantInspectionTypes (Id, Description) VALUES (3, 'INSPECT/FLUSH');
                        INSERT INTO HydrantInspectionTypes (Id, Description) VALUES (4, 'WATER QUALITY');",
                       
                       // No Read Reasons
                       $@"
                        INSERT INTO NoReadReasons (Id, Description) VALUES ({NoReadReason.Indices.KIT_NOT_AVAILABLE}, 'Kit Not Available');
                        INSERT INTO NoReadReasons (Id, Description) VALUES ({NoReadReason.Indices.NOT_DIRECTED_BY_MANAGER}, 'Not Directed By Manager');
                        INSERT INTO NoReadReasons (Id, Description) VALUES ({NoReadReason.Indices.INSPECT_ONLY}, 'Inspect Only');"
                   );
        }

        public static int CreateStuffForSewerOpeningsInAberdeenNJ(IContainer container)
        {
            return CreateCommonStuffForAssetsInAberdeenNJ(container) +
                   ExecuteSql(container.GetInstance<IStatelessSession>(),

                       // Sewer Opening Materials
                       "INSERT INTO SewerOpeningMaterials (Id, Description, SAPCode) VALUES (2, 'CONCRETE', 'PRECAST CONCRETE');",
                       "INSERT INTO SewerOpeningMaterials (Id, Description, SAPCode) VALUES (3, 'PVC', 'PVC');",
                       "INSERT INTO SewerOpeningMaterials (Id, Description, SAPCode) VALUES (6, 'BRICK', 'BRICK');",
                       "INSERT INTO SewerOpeningMaterials (Id, Description, SAPCode) VALUES (7, 'BLOCK', 'OTHER');",

                       // Sewer Opening Types
                       "INSERT INTO SewerOpeningTypes(Id, Description) Values(1, 'Catch Basin');",
                       "INSERT INTO SewerOpeningTypes(Id, Description) Values(2, 'Cleanout');",
                       "INSERT INTO SewerOpeningTypes(Id, Description) Values(3, 'Lamphole');",
                       "INSERT INTO SewerOpeningTypes(Id, Description) Values(4, 'Manhole');",
                       
                       // Waste Water Systems
                       $"INSERT INTO WasteWaterSystems (Id, OperatingCenterId, WasteWaterSystemName, PermitNumber) VALUES (1, {OperatingCenters.NJ7.ID}, 'Whatever', '655321')"
                   );
        }

        public static int CreateStuffForServicesInAberdeenNJ(IContainer container)
        {
            return CreateStuffForValvesInAberdeenNJ(container) +
                   ExecuteSql(container.GetInstance<IStatelessSession>(),

                       // Service Categories
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (1, 'Fire Retire Service Only');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (2, 'Fire Service Installation');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (3, 'Fire Service Renewal');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (4, 'Install Meter Set');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (5, 'Irrigation New');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (6, 'Irrigation Renewal');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (7, 'Replace Meter Set');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (8, 'Sewer Measurement Only');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (9, 'Sewer Reconnect');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (10, 'Sewer Retire Service Only');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (11, 'Sewer Service Increase Size');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (12, 'Sewer Service New');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (13, 'Sewer Service Renewal');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (14, 'Sewer Service Split');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (15, 'Water Measurement Only');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (16, 'Water Reconnect');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (17, 'Water Relocate Meter Set');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (18, 'Water Retire Meter Set Only');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (19, 'Water Retire Service Only');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (20, 'Water Service Increase Size');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (21, 'Water Service New Commercial');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (22, 'Water Service New Domestic');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (23, 'Water Service Renewal');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (24, 'Water Service Split');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (25, 'Sewer Install Clean Out');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (26, 'Sewer Replace Clean Out');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (27, 'Water Service Renewal Cust Side');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (28, 'Water Commercial Record Import');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (29, 'Water Domestic Record Import');",
                       "INSERT INTO ServiceCategories (ServiceCategoryId, Description) VALUES (30, 'Fire Service Record Import');",

                       // Service Materials
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (1, 'AC', 'AC', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (2, 'Carlon', 'CL', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (3, 'Cast Iron', 'CI', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (4, 'Copper', 'C', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (5, 'Ductile', 'D', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (6, 'Galvanized', 'G', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (7, 'Lead', 'L', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (8, 'Plastic', 'P', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (9, 'Transite', 'TR', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (10, 'Tubeloy', 'L', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (11, 'Unknown', 'U', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (12, 'Vitrified Clay', 'VC', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (13, 'WICL', 'WC', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (14, 'Not Present', 'U', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (15, 'Galvanized with Lead Gooseneck', 'L', true);",
                       "INSERT INTO ServiceMaterials (ServiceMaterialId, Description, Code, IsEditEnabled) VALUES (16, 'Other with Lead Gooseneck', 'L', true);",

                       // WBS Numbers
                       "INSERT INTO WBSNumbers (Id, Description) VALUES (2, 'B18-03-0001');",
                       "INSERT INTO WBSNumbers (Id, Description) VALUES (3, 'B18-05-0001');",
                       "INSERT INTO WBSNumbers (Id, Description) VALUES (4, 'B18-06-0001');",

                       // Service Sizes
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (7, 0, 0, 0, 0, 1, 0, '1');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (8, 0, 0, 0, 0, 1, 0, '3/4');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (9, 0, 0, 0, 0, 1, 0, '2');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (10, 0, 0, 0, 0, 1, 0, '4');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (11, 0, 0, 0, 0, 1, 0, '6');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (12, 0, 0, 0, 0, 1, 0, '8');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (13, 0, 0, 0, 0, 1, 0, '10');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (14, 0, 0, 0, 0, 1, 0, '12');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (15, 0, 0, 0, 0, 1, 0, '1/2');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (16, 0, 0, 0, 0, 1, 0, '16');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (17, 0, 0, 0, 0, 1, 0, '20');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (18, 0, 0, 0, 0, 1, 0, '24');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (19, 0, 0, 0, 0, 1, 0, '30');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (20, 0, 0, 0, 0, 1, 0, '36');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (21, 0, 0, 0, 0, 1, 0, '2 1/4');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (22, 0, 0, 0, 0, 1, 0, '2 1/2');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (23, 0, 0, 0, 0, 1, 0, '5/8');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (24, 0, 0, 0, 0, 1, 0, '0');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (26, 0, 0, 0, 0, 1, 0, '1 1/2');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (27, 0, 0, 0, 0, 1, 0, '18');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (28, 0, 0, 0, 0, 1, 0, '3');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (29, 0, 0, 0, 0, 1, 0, '18');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (30, 0, 0, 0, 0, 1, 0, '4 1/2');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (31, 0, 0, 0, 0, 1, 0, '4 1/4');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (32, 0, 0, 0, 0, 1, 0, '5 1/4');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (35, 0, 0, 0, 0, 1, 0, '5 1/2');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (36, 0, 0, 0, 0, 1, 0, '1 1/4');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (38, 0, 0, 0, 0, 1, 0, '4 3/4');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (39, 0, 0, 0, 0, 1, 0, '48');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (40, 0, 0, 0, 0, 1, 0, '40');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (42, 0, 0, 0, 0, 1, 0, '4 3/4');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (43, 0, 0, 0, 0, 1, 0, '5');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (45, 0, 0, 0, 0, 1, 0, '54');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (46, 0, 0, 0, 0, 1, 0, '14');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (47, 0, 0, 0, 0, 1, 0, '60');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (48, 0, 0, 0, 0, 1, 0, '42');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (49, 0, 0, 0, 0, 1, 0, '15');",
                       "INSERT INTO ServiceSizes (Id, Hydrant, Lateral, Main, Meter, Service, Size, ServiceSizeDescription) VALUES (51, 0, 0, 0, 0, 1, 0, '72');",

                       // Customer Side SL Replacers
                       "INSERT INTO CustomerSideSLReplacers (Id, Description) VALUES (1, 'Company Forces');",
                       "INSERT INTO CustomerSideSLReplacers (Id, Description) VALUES (2, 'Contractor');",

                       // Contractors
                       "INSERT INTO Contractors (ContractorId, Name, IsUnionShop, IsBCPPartner, IsActive, CreatedBy, CreatedAt, ContractorsAccess) VALUES (1, 'Dave', 0, 0, 1, 'Dave', '1976-07-04', 1)",

                       // Customer Side SL Replacement Offer Statuses
                       "INSERT INTO CustomerSideSLReplacementOfferStatuses (Id, Description) VALUES (1, 'No');",
                       "INSERT INTO CustomerSideSLReplacementOfferStatuses (Id, Description) VALUES (2, 'Offered-Accepted');",
                       "INSERT INTO CustomerSideSLReplacementOfferStatuses (Id, Description) VALUES (3, 'Offered-Rejected');",

                       // Service Installation Purposes
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (1, 'Standard Renewal');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (2, 'Make Exterior Setting');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (3, 'New Service');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (4, 'Main Replacement');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (5, 'Service Line Leak');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (6, 'Measurement Only');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (7, 'Retirement Only');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (8, 'Main Extension');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (9, 'Customer Request');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (10, 'Sample Site');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (11, 'Main Cleaning and Lining');",
                       "INSERT INTO ServiceInstallationPurposes (Id, Description) VALUES (12, 'Material Verification');",

                       // Service Priorities
                       "INSERT INTO ServicePriorities (Id, Description) VALUES (1, 'Emergency');",
                       "INSERT INTO ServicePriorities (Id, Description) VALUES (2, 'Rush / Three Day');",
                       "INSERT INTO ServicePriorities (Id, Description) VALUES (3, 'Routine');",

                       // Service Restoration Contractors
                       $"INSERT INTO ServiceRestorationContractors (Id, Contractor, OperatingCenterId, FinalRestoration, PartialRestoration) VALUES (1, 'Dave', {OperatingCenters.NJ7.ID}, 1, 1);",
                       $"INSERT INTO ServiceRestorationContractors (Id, Contractor, OperatingCenterId, FinalRestoration, PartialRestoration) VALUES (2, 'COMPANY FORCES', {OperatingCenters.NJ7.ID}, 1, 1);",
                       $"INSERT INTO ServiceRestorationContractors (Id, Contractor, OperatingCenterId, FinalRestoration, PartialRestoration) VALUES (3, 'Dave', {OperatingCenters.NJ4.ID}, 1, 1);",

                       // Flushing Of Customer Plumbing Instructions
                       "INSERT INTO FlushingOfCustomerPlumbingInstructions (Id, Description) VALUES (1, 'Standard Flushing Instructions');",
                       "INSERT INTO FlushingOfCustomerPlumbingInstructions (Id, Description) VALUES (2, 'Extended Flushing Instructions');",

                       // Permit Types
                       "INSERT INTO PermitTypes (PermitTypeId, Description) VALUES (1, 'County');",
                       "INSERT INTO PermitTypes (PermitTypeId, Description) VALUES (2, 'State');",
                       "INSERT INTO PermitTypes (PermitTypeId, Description) VALUES (3, 'Town');",
                       "INSERT INTO PermitTypes (PermitTypeId, Description) VALUES (4, 'NJDEP CP1 Permit - Major');",
                       "INSERT INTO PermitTypes (PermitTypeId, Description) VALUES (5, 'NJDEP CP1 Permit - Minor');",
                       "INSERT INTO PermitTypes (PermitTypeId, Description) VALUES (6, 'NJDEP Stream Encroachment Permit');",
                       "INSERT INTO PermitTypes (PermitTypeId, Description) VALUES (7, 'NJDEP Wetlands Permit - Major');",
                       "INSERT INTO PermitTypes (PermitTypeId, Description) VALUES (8, 'NJDEP Wetlands Permit - Minor');",
                       "INSERT INTO PermitTypes (PermitTypeId, Description) VALUES (9, 'De-watering Permit');",
                       "INSERT INTO PermitTypes (PermitTypeId, Description) VALUES (10, 'Soil Erosion Permit');",

                       // Premise Types
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (1, 'ACT54', 'ACT 54/LANDLORD Resp');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (2, 'APT5&UP', 'Apartments (5 units & up)');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (3, 'BULK', 'Bulk Water');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (4, 'CAR WASH', 'Car Wash');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (5, 'CI', 'Correctional Facilities/Institutions');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (6, 'COMM/IND', 'Commercial/Industrial Bldg');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (7, 'COMPANY', 'Company Facility');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (8, 'CONDO', 'Condominium');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (9, 'DAYCARE', 'Day Care Center');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (10, 'DEFAULT', 'Default Value');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (11, 'DIVEST', 'Divestiture Service/Sold');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (12, 'ELDERCTR', 'Nursing Homes/Senior Centers');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (13, 'EMS', 'First Response - Fire/Police');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (14, 'FARM', 'Agriculture/Farm');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (15, 'FOOD', 'Restaurant/Grocery/Food Procss');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (16, 'GOLF', 'Golf Course');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (17, 'HEALTH', 'Health Care Facilities');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (18, 'LAUNDRY', 'Laundromat');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (19, 'LG/USER', 'Large User');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (20, 'MULTICOM', 'Multi Commercial');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (21, 'MULTIFML', 'Multi-Family (2 to 4 units)');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (22, 'OPA/GOV', 'OPA and Government Offices');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (23, 'REC', 'Park/Pool/Ballpark');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (24, 'SALON', 'Barber and Beauty Salons');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (25, 'SEASONAL', 'Seasonal');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (26, 'SINGLE', 'Single Family');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (27, 'SPECIAL', 'Special Request/Critical Care');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (28, 'T/L', 'Tenant/Landlord');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (29, 'UNIMPRV', 'Unimproved Lot');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (30, 'UNKNOWN', 'Unknown');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (31, 'UNV/SCH', 'Universities and Schools');",
                       "INSERT INTO PremiseTypes (PremiseTypeId, Abbreviation, Description) VALUES (32, 'VET/PET', 'Veterinarian/Pet Shop/Aquarium');",

                       // Service Side Types
                       "INSERT INTO ServiceSideTypes (Id, Description) VALUES (1, 'Short Side');",
                       "INSERT INTO ServiceSideTypes (Id, Description) VALUES (2, 'Long Side');",

                       // Street Materials
                       "INSERT INTO StreetMaterials (Id, Description) VALUES (1, 'Black Top');",
                       "INSERT INTO StreetMaterials (Id, Description) VALUES (2, 'Brick');",
                       "INSERT INTO StreetMaterials (Id, Description) VALUES (3, 'Concrete');",
                       "INSERT INTO StreetMaterials (Id, Description) VALUES (4, 'Dirt');",
                       "INSERT INTO StreetMaterials (Id, Description) VALUES (5, 'Grass');",
                       "INSERT INTO StreetMaterials (Id, Description) VALUES (6, 'Gravel');",

                       // Main Types
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (1, '3M structural', 'OTH');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (2, 'Cast Iron', 'CI');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (3, 'Cast Iron Lined', 'CIL');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (4, 'Cast Iron Unline', 'CIU');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (5, 'Cement', 'PCCP');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (6, 'CIPP-epoxy sock', 'OTH');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (7, 'Copper', 'CU');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (8, 'Ductile Iron', 'DI');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (9, 'Galvanized', 'GV');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (10, 'HDPE', 'HDPE');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (11, 'Lock Joint', 'CEM');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (12, 'Plastic', 'PVC');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (13, 'Pre St Concrete', 'PCCP');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (14, 'Steel', 'ST');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (15, 'Transite', 'AC');",
                       "INSERT INTO MainTypes (Id, Description, SAPCode) VALUES (16, 'Vitrified Clay', 'VC');"
                   );
        }

        public static int CreateStuffForMaintenancePlansInAberdeenNJ(IContainer container)
        {
            return CreateSomeAdjustableSpeedDrivesInAberdeenNJ(container) +
                   CreateSomeFacilitiesAndAreasInAberdeenNJAndNothingElse(container) +
                   ExecuteSql(container.GetInstance<IStatelessSession>(), 
                       TaskGroups.GetInsertQuery(),
                       "INSERT INTO PlantMaintenanceActivityTypes (Id, Description, Code, OrderTypeId)  VALUES (1, 'Blanket: Mains - Replace', 'BRB', NULL);",
                      
                       // OrderTypes
                       "INSERT INTO OrderTypes (Id, Description, SAPCode, IsSAPEnabled) VALUES (1, 'Operational Activity', '0010', 1);",
                       "INSERT INTO OrderTypes (Id, Description, SAPCode, IsSAPEnabled) VALUES (2, 'PM Work Order', '0011', 1);",
                       "INSERT INTO OrderTypes (Id, Description, SAPCode, IsSAPEnabled) VALUES (3, 'Corrective Action', '0020', 1);",
                       "INSERT INTO OrderTypes (Id, Description, SAPCode, IsSAPEnabled) VALUES (4, 'RP Capital', '0040', 1);",
                       "INSERT INTO OrderTypes (Id, Description, SAPCode, IsSAPEnabled) VALUES (5, 'Routine', '0013', 0);",

                       // ProductionSkillSets
                       "INSERT INTO ProductionSkillSets (Id, Description) VALUES (6, 'B');",
                       "INSERT INTO ProductionSkillSets (Id, Description) VALUES (7, 'CON01');",
                       "INSERT INTO ProductionSkillSets (Id, Description) VALUES (8, 'CON02');",
                       "INSERT INTO ProductionSkillSets (Id, Description) VALUES (9, 'CON03');",
                       "INSERT INTO ProductionSkillSets (Id, Description) VALUES (10, 'CON04');",

                       // SkillSets
                       "INSERT INTO SkillSets (Id, Name, Abbreviation, IsActive, Description) VALUES (1, 'Maintenance Services - Specialist', 'MMS', 1, '');",
                       "INSERT INTO SkillSets (Id, Name, Abbreviation, IsActive, Description) VALUES (2, 'Maintenance Services - Mechanic', 'MSM', 1, '');",
                       "INSERT INTO SkillSets (Id, Name, Abbreviation, IsActive, Description) VALUES (3, 'Electrician - Hi Voltage', 'EHV', 1, '');",
                       "INSERT INTO SkillSets (Id, Name, Abbreviation, IsActive, Description) VALUES (4, 'Mechanic', 'MEC', 1, '');",

                       // PWD
                       ProductionWorkDescriptions.GetInsertQuery(),
                       
                       // TaskGroupCategories
                       "INSERT INTO TaskGroupCategories (Id, Description, Type, Abbreviation, IsActive) VALUES (1, 'All task associated to Chemical Assets', 'CHEMICAL', 'CHEM', 0);",
                       "INSERT INTO TaskGroupCategories (Id, Description, Type, Abbreviation, IsActive) VALUES (2, 'All task associated to Electrical assets', 'ELECTRICAL', 'ELEC', 0);",
                       "INSERT INTO TaskGroupCategories (Id, Description, Type, Abbreviation, IsActive) VALUES (3, 'All task associated to Instruments', 'INSTRUMENTATION', 'INST', 0);",
                       "INSERT INTO TaskGroupCategories (Id, Description, Type, Abbreviation, IsActive) VALUES (4, 'All task associated to Mechanical Assets', 'MECHANICAL', 'MECH', 0);",
                       "INSERT INTO TaskGroupCategories (Id, Description, Type, Abbreviation, IsActive) VALUES (5, 'All task associated to Operational Activities', 'OPERATIONAL', 'OPER', 0);",
                       "INSERT INTO TaskGroupCategories (Id, Description, Type, Abbreviation, IsActive) VALUES (6, 'All task associated to Safety related activities', 'SAFETY', 'SAFE', 0);",
                       
                       // Frequency
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (1, 'Daily', 'D', 'Work order would be generated daily', 1, 1);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (2, 'Weekly', 'W', 'Work order would be generated every Sunday', 2, 1);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (3, 'Twice Per Month', 'BM', 'Work order would be generated on the 1st and 15th of every month', 3, 1);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (4, 'Monthly', '1M', 'Work order would be generated on the 1st of every month', 4, 1);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (5, 'Quarterly', '3M', 'Work order would be generated on the 1st day of each quarter month (Jan, Apr, Jul, and Oct)', 6, 1);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (6, 'Every Four Months', '4M', 'Work order would be generated on the 1st day of these months (Jan, May, Sept)', 7, 1);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (7, 'Every Six Months', '6M', 'Work order would be generated on January 1st and July 1st', 8, 1);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (8, 'Annual', '1Y', 'Work order would be generated once a year on Jan 1st', 9, 1);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (9, 'Every Two Years', '2Y', 'Work order would be generated every two years on Jan 1st', 10, 2);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (10, 'Every Three Years', '3Y', 'Work order would be generated every three years on Jan 1st', 11, 3);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (11, 'Every Four Years', '4Y', 'Work order would be generated every four years on Jan 1st', 12, 4);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (12, 'Every Five Years', '5Y', 'Work order would be generated every five years on Jan 1st', 13, 5);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (13, 'Every Ten Years', '10Y', 'Work order would be generated every ten years on Jan 1st', 14, 10);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (14, 'Every Fifteen Years', '15Y', 'Work order would be generated every fifteen years on Jan 1st', 15, 15);",
                       "INSERT INTO ProductionWorkOrderFrequencies (Id, Name, Abbreviation, Description, SortOrder, ForecastYearSpan) VALUES (15, 'Every Two Months', '2M', 'Work order would be generated on the 1st of every other month (Jan, Mar, May, Jul, Sep, and Nov)', 5, 1);",
                       
                       // EquipmentLifespans
                       "INSERT INTO EquipmentLifespans (Id, Description, EstimatedLifespan) VALUES (1, 'Chemical Feed Dry', 25);",
                       "INSERT INTO EquipmentLifespans (Id, Description, EstimatedLifespan) VALUES (3, 'Chemical Tank', 25);",
                       "INSERT INTO EquipmentLifespans (Id, Description, EstimatedLifespan) VALUES (2, 'Chemical Feed Liquid', 15);"
                   );
        }

        public static int CreateStuffForFacilitiesInAberdeenNJ(IContainer container) => CreateCommonStuffForAssetsInAberdeenNJ(container);

        public static int CreateValidValvesLikeTheValidValvesInTheValidValvesFile(IContainer container)
        {
            using (var uow = new TestUnitOfWork(container.GetNestedContainer(), container.GetInstance<ISession>()))
            {
                var aberdeen = uow.Find<Town>(AberdeenMonmouthNJTown.ID);
                var status = uow.Where<AssetStatus>(vs => vs.Description == "ACTIVE").SingleOrDefault();
                var nj7 = uow.Where<OperatingCenter>(oc => oc.OperatingCenterCode == "NJ7").SingleOrDefault();
                var billing = uow.Where<ValveBilling>(_ => true).First();

                void CreateValve(int valveSuffix, int sapEquipmentId)
                {
                    GetEntityFactory<Valve>(container).Create(new {
                        Town = aberdeen,
                        ValveNumber = $"VAB-{valveSuffix}",
                        ValveSuffix = valveSuffix,
                        Status = status,
                        OperatingCenter = nj7,
                        ValveBilling = billing,
                        SAPEquipmentId = sapEquipmentId
                    });
                }

                CreateValve(6666, 10278997);
                CreateValve(6667, 10284540);
                CreateValve(6668, 10284809);
                CreateValve(6669, 10284852);

                uow.Commit();
            }

            return 4;
        }

        public static int CreateStuffForEquipmentInAberdeenNJ(IContainer container, string equipmentType)
        {
            return CreateCommonStuffForAssetsInAberdeenNJ(container) + ExecuteSql(container.GetInstance<IStatelessSession>(),

                // Asset Types
                "INSERT INTO AssetTypes (AssetTypeId, Description) VALUES (9, 'Equipment');",

                // Production Prerequisites
                @"
                INSERT INTO ProductionPrerequisites (Id, Description) VALUES (1, 'Has Lockout Requirement');
                INSERT INTO ProductionPrerequisites (Id, Description) VALUES (2, 'Is Confined Space');
                INSERT INTO ProductionPrerequisites (Id, Description) VALUES (3, 'Job Safety Checklist');
                INSERT INTO ProductionPrerequisites (Id, Description) VALUES (4, 'Air Permit');
                INSERT INTO ProductionPrerequisites (Id, Description) VALUES (5, 'Hot Work');
                INSERT INTO ProductionPrerequisites (Id, Description) VALUES (6, 'Pre Job Safety Brief');",
  
                // Equipment Statuses
                @"
                INSERT INTO EquipmentStatuses (EquipmentStatusId, Description) VALUES (1, 'In Service');
                INSERT INTO EquipmentStatuses (EquipmentStatusId, Description) VALUES (2, 'Out of Service');
                INSERT INTO EquipmentStatuses (EquipmentStatusId, Description) VALUES (3, 'Pending');
                INSERT INTO EquipmentStatuses (EquipmentStatusId, Description) VALUES (4, 'Retired');
                INSERT INTO EquipmentStatuses (EquipmentStatusId, Description) VALUES (5, 'Pending Retirement');
                INSERT INTO EquipmentStatuses (EquipmentStatusId, Description) VALUES (6, 'Cancelled');
                INSERT INTO EquipmentStatuses (EquipmentStatusId, Description) VALUES (7, 'Field Installed');",

                // Equipment Characteristic Field Types
                @"
                INSERT INTO EquipmentCharacteristicFieldTypes (Id, DataType, Regex) VALUES (1, 'String', '^(.+)$');
                INSERT INTO EquipmentCharacteristicFieldTypes (Id, DataType, Regex) VALUES (2, 'Number', '^-?(\d+(?:\.\d+)?)$');
                INSERT INTO EquipmentCharacteristicFieldTypes (Id, DataType, Regex) VALUES (3, 'Currency', '^-?(\d+(?:\.\d+)?)$');
                INSERT INTO EquipmentCharacteristicFieldTypes (Id, DataType, Regex) VALUES (4, 'Date', '^(0?[1-9]|1[012])/(0?[1-9]|[12][0-9]|3[01])/(19|20)\d\d$');
                INSERT INTO EquipmentCharacteristicFieldTypes (Id, DataType, Regex) VALUES (5, 'DropDown', '^(\d+)$');",

                // Equipment Conditions
                $"INSERT INTO EquipmentConditions (Id, Description) VALUES ({EquipmentCondition.Indices.POOR}, 'Poor');",
                $"INSERT INTO EquipmentConditions (Id, Description) VALUES ({EquipmentCondition.Indices.AVERAGE}, 'Average');",
                $"INSERT INTO EquipmentConditions (Id, Description) VALUES ({EquipmentCondition.Indices.GOOD}, 'Good');",

                // Equipment Consequences Of Failure Ratings
                $"INSERT INTO EquipmentConsequencesOfFailureRatings (Id, Description) VALUES ({EquipmentConsequencesOfFailureRating.Indices.LOW}, 'Low');",
                $"INSERT INTO EquipmentConsequencesOfFailureRatings (Id, Description) VALUES ({EquipmentConsequencesOfFailureRating.Indices.MEDIUM}, 'Medium');",
                $"INSERT INTO EquipmentConsequencesOfFailureRatings (Id, Description) VALUES ({EquipmentConsequencesOfFailureRating.Indices.HIGH}, 'High');",

                // Equipment Failure Risk Ratings
                $"INSERT INTO EquipmentFailureRiskRatings (Id, Description) VALUES ({EquipmentFailureRiskRating.Indices.LOW}, 'Low');",
                $"INSERT INTO EquipmentFailureRiskRatings (Id, Description) VALUES ({EquipmentFailureRiskRating.Indices.MEDIUM}, 'Medium');",
                $"INSERT INTO EquipmentFailureRiskRatings (Id, Description) VALUES ({EquipmentFailureRiskRating.Indices.HIGH}, 'High');",

                // Equipment Likelyhood Of Failure Ratings
                $"INSERT INTO EquipmentLikelyhoodOfFailureRatings (Id, Description) VALUES ({EquipmentLikelyhoodOfFailureRating.Indices.LOW}, 'Low');",
                $"INSERT INTO EquipmentLikelyhoodOfFailureRatings (Id, Description) VALUES ({EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM}, 'Medium');",
                $"INSERT INTO EquipmentLikelyhoodOfFailureRatings (Id, Description) VALUES ({EquipmentLikelyhoodOfFailureRating.Indices.HIGH}, 'High');",

                // Equipment Performance Ratings
                $"INSERT INTO EquipmentPerformanceRatings (Id, Description) VALUES ({EquipmentPerformanceRating.Indices.POOR}, 'Poor');",
                $"INSERT INTO EquipmentPerformanceRatings (Id, Description) VALUES ({EquipmentPerformanceRating.Indices.AVERAGE}, 'Average');",
                $"INSERT INTO EquipmentPerformanceRatings (Id, Description) VALUES ({EquipmentPerformanceRating.Indices.GOOD}, 'Good');",

                // Equipment Reliability Ratings
                $"INSERT INTO EquipmentReliabilityRatings (Id, Description) VALUES ({EquipmentReliabilityRating.Indices.LOW}, 'Low');",
                $"INSERT INTO EquipmentReliabilityRatings (Id, Description) VALUES ({EquipmentReliabilityRating.Indices.MEDIUM}, 'Medium');",
                $"INSERT INTO EquipmentReliabilityRatings (Id, Description) VALUES ({EquipmentReliabilityRating.Indices.HIGH}, 'High');",

                // Equipment Static Dynamic Types
                $"INSERT INTO EquipmentStaticDynamicTypes (Id, Description) VALUES ({EquipmentStaticDynamicType.Indices.DYNAMIC}, 'Dynamic');",
                $"INSERT INTO EquipmentStaticDynamicTypes (Id, Description) VALUES ({EquipmentStaticDynamicType.Indices.STATIC}, 'Static');",

                // Get Stuff
                EquipmentTypes.GetInsertQuery(equipmentType),
                EquipmentPurposes.GetInsertQuery(equipmentType),
                EquipmentCharacteristicFields.GetInsertQuery(equipmentType),
                EquipmentCharacteristicDropDownValues.GetInsertQuery(equipmentType),
                EquipmentManufacturers.GetInsertQuery(equipmentType),
                EquipmentModels.GetInsertQuery(equipmentType)
            );
        }

        public static int CreateSomeAdjustableSpeedDrivesInAberdeenNJ(IContainer container)
        {
            return CreateStuffForEquipmentInAberdeenNJ(container, "ADJUSTABLE SPEED DRIVE") + ExecuteSql(
                container.GetInstance<IStatelessSession>(),

                // Equipment
                $@"
                INSERT INTO Equipment (FacilityId, PurposeId, ABCIndicatorId, CreatedById, IsReplacement, EquipmentID, StatusId, EquipmentManufacturerId, Description, FunctionalLocationId, Number, EquipmentTypeId, CreatedAt, SAPEquipmentId, PSMTCPA, Portable, UpdatedAt) 
                VALUES ( 2, 310, 3, 402, 0, 1, 1, 1951, 'MATS-FBE-MCC HS 6 /ADJSPD', 'NJMM-MM-ABTNK-MCC', 1, 121, '2019-06-06 11:25:48', 5017540, 0, 0, '2002-08-27T09:08:00.000');
                INSERT INTO Equipment (FacilityId, PurposeId, ABCIndicatorId, CreatedById, IsReplacement, EquipmentID, StatusId, EquipmentManufacturerId, Description, FunctionalLocationId, Number, EquipmentTypeId, CreatedAt, SAPEquipmentId, PSMTCPA, Portable, UpdatedAt) 
                VALUES ( 2, 310, 3, 402, 0, 2, 1, 1951, 'MATS-FBE-MCC HS 7 /ADJSPD', 'NJMM-MM-ABTNK-MCC', 2, 121, '2019-06-06 11:25:48', 5017541, 0, 0, '2002-08-27T09:08:00.000');
                INSERT INTO Equipment (FacilityId, PurposeId, ABCIndicatorId, CreatedById, IsReplacement, EquipmentID, StatusId, EquipmentManufacturerId, Description, FunctionalLocationId, Number, EquipmentTypeId, CreatedAt, SAPEquipmentId, PSMTCPA, Portable, UpdatedAt) 
                VALUES ( 2, 310, 3, 402, 0, 3, 1, 1951, 'MATS-FBE-MCC HS 8 /ADJSPD', 'NJMM-MM-ABTNK-MCC', 3, 121, '2019-06-06 11:25:48', 5017542, 0, 0, '2002-08-27T09:08:00.000');
                INSERT INTO Equipment (FacilityId, PurposeId, ABCIndicatorId, CreatedById, IsReplacement, EquipmentID, StatusId, EquipmentManufacturerId, Description, FunctionalLocationId, Number, EquipmentTypeId, CreatedAt, SAPEquipmentId, PSMTCPA, Portable, UpdatedAt) 
                VALUES ( 2, 310, 3, 402, 0, 4, 1, 1951, 'MATS-FBE-MCC HS 8 /ADJSPD', 'NJMM-MM-ABTNK-MCC', 4, 121, '2019-06-06 11:25:48', 5017543, 0, 0, '2002-08-27T09:08:00.000');
                INSERT INTO Equipment (FacilityId, PurposeId, ABCIndicatorId, CreatedById, IsReplacement, EquipmentID, StatusId, EquipmentManufacturerId, Description, FunctionalLocationId, Number, EquipmentTypeId, CreatedAt, SAPEquipmentId, PSMTCPA, Portable, UpdatedAt) 
                VALUES ( 2, 310, 3, 402, 0, 5, 1, 1951, 'MATS-FBE-MCC HS 6 /ADJSPD', 'NJMM-MM-ABTNK-MCC', 5, 121, '2019-06-06 11:25:48', NULL, 0, 0, '2002-08-27T09:08:00.000');
                INSERT INTO Equipment (FacilityId, PurposeId, ABCIndicatorId, CreatedById, IsReplacement, EquipmentID, StatusId, EquipmentManufacturerId, Description, FunctionalLocationId, Number, EquipmentTypeId, CreatedAt, SAPEquipmentId, PSMTCPA, Portable, UpdatedAt) 
                VALUES ( 2, 310, 3, 402, 0, 6, 1, 1951, 'MATS-FBE-MCC HS 7 /ADJSPD', 'NJMM-MM-ABTNK-MCC', 6, 121, '2019-06-06 11:25:48', NULL, 0, 0, '2002-08-27T09:08:00.000');
                INSERT INTO Equipment (FacilityId, PurposeId, ABCIndicatorId, CreatedById, IsReplacement, EquipmentID, StatusId, EquipmentManufacturerId, Description, FunctionalLocationId, Number, EquipmentTypeId, CreatedAt, SAPEquipmentId, PSMTCPA, Portable, UpdatedAt) 
                VALUES ( 2, 310, 3, 402, 0, 7, 1, 1951, 'MATS-FBE-MCC HS 8 /ADJSPD', 'NJMM-MM-ABTNK-MCC', 7, 121, '2019-06-06 11:25:48', NULL, 0, 0, '2002-08-27T09:08:00.000');
                INSERT INTO Equipment (FacilityId, PurposeId, ABCIndicatorId, CreatedById, IsReplacement, EquipmentID, StatusId, EquipmentManufacturerId, Description, FunctionalLocationId, Number, EquipmentTypeId, CreatedAt, SAPEquipmentId, PSMTCPA, Portable, UpdatedAt) 
                VALUES ( 2, 310, 3, 402, 0, 8, 1, 1951, 'MATS-FBE-MCC HS 8 /ADJSPD', 'NJMM-MM-ABTNK-MCC', 8, 121, '2019-06-06 11:25:48', NULL, 0, 0, '2002-08-27T09:08:00.000');"
            );
        }

        public static int CreateSomeFacilitiesAndAreasInAberdeenNJAndNothingElse(IContainer container)
        {
            return ExecuteSql(container.GetInstance<IStatelessSession>(),
                Facilities.NJSB11.GetInsertQuery() +
                @"
                INSERT INTO tblFacilities (PublicWaterSupplyId, StreetNumber, StreetId, Status, PlanningPlantId, CreatedAt, OperatingCenterId, FacilityName, Facility_Ownership, RecordId, DepartmentId, PSM, RMP, SwmStation, WellProd, WellMonitoring, ClearWell, RawWaterIntake, UpdatedAt, Radionuclides, CommunityRightToKnow, IsInVamp, BasicGroundWaterSupply, RawWaterPumpStation, SystemDeliveryTypeId, WaterStress, IgnitionEnterprisePortal, ArcFlashLabelRequired) VALUES (22, '2902', 4733, 159, 26, '2019-06-06 14:34:29', 10, 'DUNCAN ELEVATED TANK', 157, 12, 3, 0, 0, 0, 0, 0, 0, 0, '2002-08-27T09:08:00.000', 0, 0, 0, 0, 0, 1, 0, 0, 0);
                INSERT INTO tblFacilities (PublicWaterSupplyId, StreetNumber, StreetId, Status, PlanningPlantId, CreatedAt, OperatingCenterId, FacilityName, Facility_Ownership, RecordId, DepartmentId, PSM, RMP, SwmStation, WellProd, WellMonitoring, ClearWell, RawWaterIntake, UpdatedAt, Radionuclides, CommunityRightToKnow, IsInVamp, BasicGroundWaterSupply, RawWaterPumpStation, SystemDeliveryTypeId, WaterStress, IgnitionEnterprisePortal, ArcFlashLabelRequired) VALUES (22, '', NULL, 159, 26, '2019-06-06 14:34:29', 10, 'EMBARRAS WD -SALE/RESALE', 157, 13, 3, 0, 0, 0, 0, 0, 0, 0, '2002-08-27T09:08:00.000', 0, 0, 0, 0, 0, 1, 0, 0, 0);               
                INSERT INTO tblFacilities (PublicWaterSupplyId, StreetNumber, StreetId, Status, PlanningPlantId, CreatedAt, OperatingCenterId, FacilityName, Facility_Ownership, RecordId, DepartmentId, PSM, RMP, SwmStation, WellProd, WellMonitoring, ClearWell, RawWaterIntake, UpdatedAt, Radionuclides, CommunityRightToKnow, IsInVamp, BasicGroundWaterSupply, RawWaterPumpStation, SystemDeliveryTypeId, WaterStress, IgnitionEnterprisePortal, ArcFlashLabelRequired) VALUES (22, '1601', 6453, 159, 26, '2019-06-06 14:34:29', 10, 'MARKET STREET - BOOSTER STATION', 157, 14, 3, 0, 0, 0, 0, 0, 0, 0, '2002-08-27T09:08:00.000', 0, 0, 0, 0, 0, 1, 0, 0, 0);
                
                INSERT INTO FacilityAreas (Id, Description) VALUES (1, 'Filter');
                INSERT INTO FacilityAreas (Id, Description) VALUES (2, 'Generator');
                INSERT INTO FacilityAreas (Id, Description) VALUES (3, 'Basin');
                INSERT INTO FacilityAreas (Id, Description) VALUES (4, 'Operational');
                INSERT INTO FacilityAreas (Id, Description) VALUES (5, 'Piping');
                INSERT INTO FacilityAreas (Id, Description) VALUES (6, 'Vault');
              
                INSERT INTO FacilitiesFacilityAreas (Id, FacilityId, FacilityAreaId) VALUES (1, 11, 1);
                INSERT INTO FacilitiesFacilityAreas (Id, FacilityId, FacilityAreaId) VALUES (2, 11, 2);
                INSERT INTO FacilitiesFacilityAreas (Id, FacilityId, FacilityAreaId) VALUES (3, 11, 3);
                INSERT INTO FacilitiesFacilityAreas (Id, FacilityId, FacilityAreaId) VALUES (4, 12, 5);
                INSERT INTO FacilitiesFacilityAreas (Id, FacilityId, FacilityAreaId) VALUES (5, 13, 6);
                INSERT INTO FacilitiesFacilityAreas (Id, FacilityId, FacilityAreaId) VALUES (6, 14, 6);
                INSERT INTO FacilitiesFacilityAreas (Id, FacilityId, FacilityAreaId) VALUES (7, 14, 1);
                INSERT INTO FacilitiesFacilityAreas (Id, FacilityId, FacilityAreaId) VALUES (8, 14, 1);
            ");
        }

        public static int CreateSomeFacilitiesInAberdeenNJ(IContainer container)
        {
            return CreateStuffForFacilitiesInAberdeenNJ(container) + ExecuteSql(
                container.GetInstance<IStatelessSession>(),

                // tblFacilities
                @"
                INSERT INTO tblFacilities (PublicWaterSupplyId, StreetNumber, StreetId, Status, PlanningPlantId, CreatedAt, OperatingCenterId, FacilityName, Facility_Ownership, RecordId, DepartmentId, PSM, RMP, SwmStation, WellProd, WellMonitoring, ClearWell, RawWaterIntake, UpdatedAt, Radionuclides, CommunityRightToKnow, IgnitionEnterprisePortal, ArcFlashLabelRequired, IsInVamp, BasicGroundWaterSupply, RawWaterPumpStation, SystemDeliveryTypeId, WaterStress) VALUES (22, '600', 2962, 159, 26, '2019-06-06 14:34:29', 10, 'ARCOLA/TUSCOLA - PUMP STATION', 157, 11, 3, 0, 0, 0, 0, 0, 0, 0, '2002-08-27T09:08:00.000', 0, 0, 0, 0, 0, 0, 0, 1, 0);
                INSERT INTO tblFacilities (PublicWaterSupplyId, StreetNumber, StreetId, Status, PlanningPlantId, CreatedAt, OperatingCenterId, FacilityName, Facility_Ownership, RecordId, DepartmentId, PSM, RMP, SwmStation, WellProd, WellMonitoring, ClearWell, RawWaterIntake, UpdatedAt, Radionuclides, CommunityRightToKnow, IgnitionEnterprisePortal, ArcFlashLabelRequired, IsInVamp, BasicGroundWaterSupply, RawWaterPumpStation, SystemDeliveryTypeId, WaterStress) VALUES (22, '2902', 4733, 159, 26, '2019-06-06 14:34:29', 10, 'DUNCAN ELEVATED TANK', 157, 12, 3, 0, 0, 0, 0, 0, 0, 0, '2002-08-27T09:08:00.000', 0, 0, 0, 0, 0, 0, 0, 1, 0);
                INSERT INTO tblFacilities (PublicWaterSupplyId, StreetNumber, StreetId, Status, PlanningPlantId, CreatedAt, OperatingCenterId, FacilityName, Facility_Ownership, RecordId, DepartmentId, PSM, RMP, SwmStation, WellProd, WellMonitoring, ClearWell, RawWaterIntake, UpdatedAt, Radionuclides, CommunityRightToKnow, IgnitionEnterprisePortal, ArcFlashLabelRequired, IsInVamp, BasicGroundWaterSupply, RawWaterPumpStation, SystemDeliveryTypeId, WaterStress) VALUES (22, '', NULL, 159, 26, '2019-06-06 14:34:29', 10, 'EMBARRAS WD -SALE/RESALE', 157, 13, 3, 0, 0, 0, 0, 0, 0, 0, '2002-08-27T09:08:00.000', 0, 0, 0, 0, 0, 0, 0, 1, 0);
                INSERT INTO tblFacilities (PublicWaterSupplyId, StreetNumber, StreetId, Status, PlanningPlantId, CreatedAt, OperatingCenterId, FacilityName, Facility_Ownership, RecordId, DepartmentId, PSM, RMP, SwmStation, WellProd, WellMonitoring, ClearWell, RawWaterIntake, UpdatedAt, Radionuclides, CommunityRightToKnow, IgnitionEnterprisePortal, ArcFlashLabelRequired, IsInVamp, BasicGroundWaterSupply, RawWaterPumpStation, SystemDeliveryTypeId, WaterStress) VALUES (22, '1601', 6453, 159, 26, '2019-06-06 14:34:29', 10, 'MARKET STREET - BOOSTER STATION', 157, 14, 3, 0, 0, 0, 0, 0, 0, 0, '2002-08-27T09:08:00.000', 0, 0, 0, 0, 0, 0, 0, 1, 0);"
            );
        }

        public static int CreateValidHydrantsLikeTheValidHydrantsInTheValidHydrantsFile(IContainer container)
        {
            using (var uow = new TestUnitOfWork(container.GetNestedContainer(), container.GetInstance<ISession>()))
            {
                var aberdeen = uow.Find<Town>(AberdeenMonmouthNJTown.ID);
                var status = uow.Where<AssetStatus>(vs => vs.Description == "ACTIVE").SingleOrDefault();
                var nj7 = uow.Where<OperatingCenter>(oc => oc.OperatingCenterCode == "NJ7").SingleOrDefault();
                var billing = uow.Where<HydrantBilling>(_ => true).First();

                void CreateHydrant(int hydrantSuffix, int sapEquipmentId)
                {
                    GetEntityFactory<Hydrant>(container).Create(new {
                        Town = aberdeen,
                        HydrantNumber = $"HAB-{hydrantSuffix}",
                        HydrantSuffix = hydrantSuffix,
                        Status = status,
                        OperatingCenter = nj7,
                        HydrantBilling = billing,
                        SAPEquipmentId = sapEquipmentId
                    });
                }
                
                CreateHydrant(6666, 20072439);
                CreateHydrant(6667, 20073578);
                CreateHydrant(6668, 20073544);
                CreateHydrant(6669, 20073608);

                uow.Commit();
            }

            return 4;
        }

        public static int CreateStuffForContractorOverrideLaborCostInAberdeenNJ(IContainer container)
        {
            return CreateCommonStuffForAssetsInAberdeenNJ(container) + ExecuteSql(
                container.GetInstance<IStatelessSession>(),
                $@"
                INSERT INTO ContractorLaborCosts (Id, StockNumber, Unit, JobDescription, SubDescription, Cost, Percentage) VALUES (7, 'AQ100', 'Thing1', 'Look after Thing2', NULL, NULL, 67);
                INSERT INTO Contractors (ContractorId, Name, IsUnionShop, IsBCPPartner, IsActive, CreatedBy, CreatedAt, ContractorsAccess) VALUES (1, 'Craig', 1, 0, 0, 'Craig', '2022-07-04', 1);
                INSERT INTO Contractors (ContractorId, Name, IsUnionShop, IsBCPPartner, IsActive, CreatedBy, CreatedAt, ContractorsAccess) VALUES (14, 'Dave', 1, 1, 1, 'Dave', '1976-07-04', 1);"
            );
        }
        
        public static int CreateSomeServicesInAberdeenNJ(IContainer container)
        {
            return CreateStuffForServicesInAberdeenNJ(container) + ExecuteSql(
                container.GetInstance<IStatelessSession>(),

                // tblFacilities
                $@"
                INSERT INTO Services (Id, Agreement, BureauOfSafeDrinkingWaterPermitRequired, DeveloperServicesDriven, IsActive, MeterSettingRequirement, OperatingCenterId, CreatedAt, UpdatedAt, CleanedCoordinates, TownId, PremiseNumber, NeedsToSync, Block) VALUES (1, 0, 0, 0, 1, 0, 10, '2023-07-27 09:16:00.000', '2023-07-27 09:16:00.000', 0, {AberdeenMonmouthNJTown.ID}, '9180752554', 0, '6');
                INSERT INTO Services (Id, Agreement, BureauOfSafeDrinkingWaterPermitRequired, DeveloperServicesDriven, IsActive, MeterSettingRequirement, OperatingCenterId, CreatedAt, UpdatedAt, CleanedCoordinates, TownId, PremiseNumber, NeedsToSync, Block) VALUES (2, 0, 0, 0, 1, 0, 10, '2023-07-27 09:16:00.000', '2023-07-27 09:16:00.000', 0, {AberdeenMonmouthNJTown.ID}, '9180752552', 0, '6');
                INSERT INTO Services (Id, Agreement, BureauOfSafeDrinkingWaterPermitRequired, DeveloperServicesDriven, IsActive, MeterSettingRequirement, OperatingCenterId, CreatedAt, UpdatedAt, CleanedCoordinates, TownId, PremiseNumber, NeedsToSync, Block) VALUES (3, 0, 0, 0, 1, 0, 10, '2023-07-27 09:16:00.000', '2023-07-27 09:16:00.000', 0, {AberdeenMonmouthNJTown.ID}, '9180754547', 0, '301');
                INSERT INTO Services (Id, Agreement, BureauOfSafeDrinkingWaterPermitRequired, DeveloperServicesDriven, IsActive, MeterSettingRequirement, OperatingCenterId, CreatedAt, UpdatedAt, CleanedCoordinates, TownId, PremiseNumber, NeedsToSync, Block) VALUES (4, 0, 0, 0, 1, 0, 10, '2023-07-27 09:16:00.000', '2023-07-27 09:16:00.000', 0, {AberdeenMonmouthNJTown.ID}, '9180754548', 0, '301');"
            );
        }
        
        public static int CreateValvesForInspectionFrequenciesInAberdeenNJ(IContainer container)
        {
            return CreateStuffForValvesInAberdeenNJ(container) +
                   ExecuteSql(container.GetInstance<IStatelessSession>(),
                       $@"INSERT INTO Valves (Id, BPUKPI, Critical, Town, Traffic, ValveSuffix, ValveBillingId, OperatingCenterId, ControlsCrossing, ValveNumber, CreatedAt, UpdatedAt, AssetStatusId, InspectionFrequency) VALUES (752600, 0, 0, {AberdeenMonmouthNJTown.ID}, 0, 999, {ValveBilling.Indices.PUBLIC}, 10, 0, 'GRX-1', '2023-07-27 09:16:00.000', '2023-07-27 09:16:00.000', {AssetStatus.Indices.ACTIVE}, 5);
                                    INSERT INTO Valves (Id, BPUKPI, Critical, Town, Traffic, ValveSuffix, ValveBillingId, OperatingCenterId, ControlsCrossing, ValveNumber, CreatedAt, UpdatedAt, AssetStatusId, InspectionFrequency) VALUES (2, 0, 0, {AberdeenMonmouthNJTown.ID}, 0, 999, {ValveBilling.Indices.PUBLIC}, 10, 0, 'GRX-2', '2023-07-27 09:16:00.000', '2023-07-27 09:16:00.000', {AssetStatus.Indices.ACTIVE}, 1);"
                   );
        }

        #endregion
    }
}
