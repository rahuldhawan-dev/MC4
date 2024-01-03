using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;

namespace MapCallMVC.Tests.Models.ViewModels.Facilities
{
    [TestClass]
    public class FacilityViewModelTest : MapCallMvcInMemoryDatabaseTestBase<Facility>
    {
        #region Fields

        private Facility _entity;
        private FacilityViewModel _target;
        private ViewModelTester<FacilityViewModel, Facility> _vmTester;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authServ;
        public User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetFactory<FacilityFactory>().Create();
            _target = new EditFacility(_container);
            _vmTester = new ViewModelTester<FacilityViewModel, Facility>(_target, _entity);
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = GetEntityFactory<User>().Create();
            _user.Employee = GetEntityFactory<Employee>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _container.Inject(_authServ.Object);

            GetFactory<FacilityConditionFactory>().CreateAll();
            GetFactory<FacilityPerformanceFactory>().CreateAll();
            GetFactory<FacilityLikelihoodOfFailureFactory>().CreateAll();
            GetFactory<FacilityConsequenceOfFailureFactory>().CreateAll();
            GetFactory<FacilityMaintenanceRiskOfFailureFactory>().CreateAll();
            GetFactory<FacilityAssetManagementMaintenanceStrategyTierFactory>().CreateAll();
            GetFactory<SystemDeliveryTypeFactory>().Create();
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMappings()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 13);
            _vmTester.DoesNotMapToEntity(x => x.Id, 31);

            _vmTester.CanMapBothWays(x => x.Administration);
            _vmTester.CanMapBothWays(x => x.BoosterStation);
            _vmTester.CanMapBothWays(x => x.CellularAntenna);
            _vmTester.CanMapBothWays(x => x.ChemicalFeed);
            _vmTester.CanMapBothWays(x => x.ClearWell);
            _vmTester.CanMapBothWays(x => x.HasConfinedSpaceRequirement);
            _vmTester.CanMapBothWays(x => x.CriticalRating);
            _vmTester.CanMapBothWays(x => x.FacilityReliableCapacityMGD);
            _vmTester.CanMapBothWays(x => x.EnvironmentalRegulatorIDNumber);
            _vmTester.CanMapBothWays(x => x.FacilityOperatingCapacityMGD);
            _vmTester.CanMapBothWays(x => x.FacilityRatedCapacityMGD);
            _vmTester.CanMapBothWays(x => x.FacilityAuxPowerCapacityMGD);
            _vmTester.CanMapBothWays(x => x.UsedInProductionCapacityCalculation);
            _vmTester.CanMapBothWays(x => x.DPCC);
            _vmTester.CanMapBothWays(x => x.Dam);
            _vmTester.CanMapBothWays(x => x.DistributivePumping);
            _vmTester.CanMapBothWays(x => x.ElevatedStorage);
            _vmTester.CanMapBothWays(x => x.EmergencyPower);
            _vmTester.CanMapBothWays(x => x.FacilityContactInfo);
            _vmTester.CanMapBothWays(x => x.FacilityInspectionFrequency);
            _vmTester.CanMapBothWays(x => x.FacilityLoopGrouping);
            _vmTester.CanMapBothWays(x => x.FacilityLoopGroupingSub);
            _vmTester.CanMapBothWays(x => x.FacilityLoopSequence);
            _vmTester.CanMapBothWays(x => x.FacilityName);
            _vmTester.CanMapBothWays(x => x.FieldOperations);
            _vmTester.CanMapBothWays(x => x.Filtration);
            _vmTester.CanMapBothWays(x => x.GroundStorage);
            _vmTester.CanMapBothWays(x => x.GroundWaterSupply);
            _vmTester.CanMapBothWays(x => x.Interconnection);
            _vmTester.CanMapBothWays(x => x.FacilityTotalCapacityMGD);
            _vmTester.CanMapBothWays(x => x.DesignationPumpStation);
            _vmTester.CanMapBothWays(x => x.DesignationTreatmentPlant);
            _vmTester.CanMapBothWays(x => x.EntityNotes);
            _vmTester.CanMapBothWays(x => x.OnSiteAnalyticalInstruments);
            _vmTester.CanMapBothWays(x => x.Operations);
            _vmTester.CanMapBothWays(x => x.PSM);
            _vmTester.CanMapBothWays(x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
            _vmTester.CanMapBothWays(x => x.PublicWaterSupplyPressureZone, GetEntityFactory<PublicWaterSupplyPressureZone>().Create());
            _vmTester.CanMapBothWays(x => x.WasteWaterSystem, GetEntityFactory<WasteWaterSystem>().Create());
            _vmTester.CanMapBothWays(x => x.WasteWaterSystemBasin, GetEntityFactory<WasteWaterSystemBasin>().Create());
            _vmTester.CanMapBothWays(x => x.RMP);
            _vmTester.CanMapBothWays(x => x.PointOfEntry);
            _vmTester.CanMapBothWays(x => x.PressureReducing);
            _vmTester.CanMapBothWays(x => x.PropertyOnly);
            _vmTester.CanMapBothWays(x => x.RawWaterIntake);
            _vmTester.CanMapBothWays(x => x.RegionalPlanningArea);
            _vmTester.CanMapBothWays(x => x.Reservoir);
            _vmTester.CanMapBothWays(x => x.ResidualsGeneration);
            _vmTester.CanMapBothWays(x => x.RMPNumber);
            _vmTester.CanMapBothWays(x => x.SCADAIntrusionAlarm);
            _vmTester.CanMapBothWays(x => x.SICNumber);
            _vmTester.CanMapBothWays(x => x.SecurityCategory);
            _vmTester.CanMapBothWays(x => x.SecurityGrouping);
            _vmTester.CanMapBothWays(x => x.SecurityInspectionFrequency);
            _vmTester.CanMapBothWays(x => x.SecurityLoopSequence);
            _vmTester.CanMapBothWays(x => x.SewerLiftStation);
            _vmTester.CanMapBothWays(x => x.SpoilsStaging);
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.SurfaceWaterSupply);
            _vmTester.CanMapBothWays(x => x.SWMStation);
            _vmTester.CanMapBothWays(x => x.System);
            _vmTester.CanMapBothWays(x => x.SampleStation);
            _vmTester.CanMapBothWays(x => x.TReport);
            _vmTester.CanMapBothWays(x => x.WasteWaterTreatmentFacility);
            _vmTester.CanMapBothWays(x => x.WaterShed);
            _vmTester.CanMapBothWays(x => x.WaterTreatmentFacility);
            _vmTester.CanMapBothWays(x => x.WellProd);
            _vmTester.CanMapBothWays(x => x.WellMonitoring);
            _vmTester.CanMapBothWays(x => x.Radionuclides);
            _vmTester.CanMapBothWays(x => x.CommunityRightToKnow);
            _vmTester.CanMapBothWays(x => x.IgnitionEnterprisePortal);
            _vmTester.CanMapBothWays(x => x.ArcFlashLabelRequired);
            _vmTester.CanMapBothWays(x => x.YearInService);
            _vmTester.CanMapBothWays(x => x.ZipCode);
            _vmTester.CanMapBothWays(x => x.IsInVamp);
            _vmTester.CanMapBothWays(x => x.VampUrl);
            _vmTester.CanMapBothWays(x => x.Process, GetEntityFactory<ProcessStage>().Create());
            _vmTester.CanMapBothWays(x => x.SystemDeliveryType,
                GetEntityFactory<SystemDeliveryType>().Create(new { Description = "For the Horde!" }));
            _vmTester.CanMapBothWays(x => x.BasicGroundWaterSupply);
            _vmTester.CanMapBothWays(x => x.RawWaterPumpStation);
            _vmTester.CanMapBothWays(x => x.WaterStress);
            _vmTester.CanMapBothWays(x => x.InsuranceId);
            _vmTester.CanMapBothWays(x => x.InsuranceScore);
            _vmTester.CanMapBothWays(x => x.InsuranceVisitDate);
            _vmTester.CanMapBothWays(x => x.InsuranceScoreQuartile, GetEntityFactory<InsuranceScoreQuartile>().Create( new { Id = 1, Description = "1"}));
        }

        [TestMethod]
        public void TestFacilityViewModelMapSetsPropertiesAndIds()
        {
            var facilityOwner = GetFactory<FacilityOwnerFactory>().Create();
            var facilityStatus = GetFactory<FacilityStatusFactory>().Create();
            var floodRating = GetFactory<FEMAFloodRatingFactory>().Create();
            var department = GetFactory<DepartmentFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var coordinate = GetFactory<CoordinateFactory>().Create();
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var facility = GetFactory<FacilityFactory>().Create(new {
                FacilityOwner = facilityOwner,
                FacilityStatus = facilityStatus,
                FEMAFloodRating = floodRating,
                Department = department,
                Town = town,
                Coordinate = coordinate,
                OperatingCenter = operatingCenter
            });

            var target = new FacilityViewModel(_container);

            Assert.IsNotNull(facility.Town);
            target.Map(facility);
            Assert.AreEqual(facilityOwner.Id, target.FacilityOwner);
            Assert.AreEqual(facilityStatus.Id, target.FacilityStatus);
            Assert.AreEqual(floodRating.Id, target.FEMAFloodRating);
            Assert.AreEqual(department.Id, target.Department);
            Assert.AreEqual(town.Id, target.Town);
            Assert.AreEqual(coordinate.Id, target.CoordinateId);
            Assert.AreEqual(operatingCenter.Id, target.OperatingCenter);
        }

        [TestMethod]
        public void TestFacilityViewModelMapToEntitySetsProperties()
        {
            var facilityOwner = GetFactory<AWFacilityOwnerFactory>().Create();
            var facilityStatus = GetFactory<FacilityStatusFactory>().Create();
            var floodRating = GetFactory<FEMAFloodRatingFactory>().Create();
            var department = GetFactory<DepartmentFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var coordinate = GetFactory<CoordinateFactory>().Create();
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var target = new FacilityViewModel(_container) {
                FacilityOwner = facilityOwner.Id,
                FacilityStatus = facilityStatus.Id,
                FEMAFloodRating = floodRating.Id,
                Department = department.Id,
                Town = town.Id,
                CoordinateId = coordinate.Id,
                OperatingCenter = operatingCenter.Id
            };
            var entity = new Facility();

            target.MapToEntity(entity);

            Assert.AreEqual(facilityOwner.Id, entity.FacilityOwner.Id);
            Assert.AreEqual(facilityStatus.Id, entity.FacilityStatus.Id);
            Assert.AreEqual(floodRating.Id, entity.FEMAFloodRating.Id);
            Assert.AreEqual(department.Id, entity.Department.Id);
            Assert.AreEqual(town.Id, entity.Town.Id);
            Assert.AreEqual(coordinate.Id, entity.Coordinate.Id);
            Assert.AreEqual(operatingCenter.Id, entity.OperatingCenter.Id);
        }

        [TestMethod]
        public void TestValidatingOnAllOfTheFacilityViewModelsNeedsToEnsureThatEachForeignKeyPropertyWouldMapToAnExistingObject()
        {
            ValidationAssert.EntityMustExist(_target, x => x.FacilityOwner,
                GetFactory<FacilityOwnerFactory>().Create());
            ValidationAssert.EntityMustExist(_target, x => x.FacilityStatus,
                GetFactory<FacilityStatusFactory>().Create());
            ValidationAssert.EntityMustExist(_target, x => x.FEMAFloodRating,
                GetFactory<FEMAFloodRatingFactory>().Create());
            ValidationAssert.EntityMustExist(_target, x => x.Department, GetFactory<DepartmentFactory>().Create());
            ValidationAssert.EntityMustExist(_target, x => x.Town, GetFactory<TownFactory>().Create());
            ValidationAssert.EntityMustExist(_target, x => x.OperatingCenter,
                GetFactory<OperatingCenterFactory>().Create());
            ValidationAssert.EntityMustExist(_target, x => x.CoordinateId, GetFactory<CoordinateFactory>().Create());
        }

        [TestMethod]
        public void TestForeignKeyPropertyThatAreNullableAllowNulls()
        {
            _target.FacilityOwner = null;
            _target.FacilityStatus = null;
            _target.FEMAFloodRating = null;
            _target.Town = null;
            _target.PublicWaterSupply = null;
            _target.PublicWaterSupplyPressureZone = null;
            _target.WasteWaterSystemBasin = null;
            _target.WasteWaterSystem = null;
            _target.InsuranceScoreQuartile = null;

            ValidationAssert.ModelStateIsValid(_target, "FacilityOwnerId");
            ValidationAssert.ModelStateIsValid(_target, "FacilityStatusId");
            ValidationAssert.ModelStateIsValid(_target, "FEMAFloodRatingId");
            ValidationAssert.ModelStateIsValid(_target, "DepartmentId");
            ValidationAssert.ModelStateIsValid(_target, "TownId");
            ValidationAssert.ModelStateIsValid(_target, "PublicWaterSupplyId");
            ValidationAssert.ModelStateIsValid(_target, "PublicWaterSupplyPressureZoneId");
            ValidationAssert.ModelStateIsValid(_target, "WasteWaterSystemBasinId");
            ValidationAssert.ModelStateIsValid(_target, "WasteWaterSystemId");
            ValidationAssert.ModelStateIsValid(_target, "InsuranceScoreQuartile");
        }

        [TestMethod]
        public void TestMaxLengthOnTheStringProperties()
        {
            // setting values up here so Regex validation doesn't show up during model state check
            var i = 0;
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.EnvironmentalRegulatorIDNumber,
                Facility.StringLengths.ENVIRONMENTAL_REGULATOR_ID_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.FacilityContactInfo,
                Facility.StringLengths.FACILITY_CONTACT_INFO);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.FacilityInspectionFrequency,
                Facility.StringLengths.FACILITY_INSPECTION_FREQUENCY);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.FacilityLoopGrouping,
                Facility.StringLengths.FACILITY_LOOP_GROUPING);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.FacilityLoopGroupingSub,
                Facility.StringLengths.FACILITY_LOOP_GROUPING_SUB);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.FacilityName,
                Facility.StringLengths.FACILITY_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.DesignationPumpStation,
                Facility.StringLengths.DESIGNATION_PUMP_STATION);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.DesignationTreatmentPlant,
                Facility.StringLengths.DESIGNATION_TREATMENT_PLANT);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.EntityNotes, Facility.StringLengths.NOTES,
                error: "The field Notes must be a string with a maximum length of 255.");
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.Operations, Facility.StringLengths.OPERATIONS);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.RegionalPlanningArea,
                Facility.StringLengths.REGIONAL_PLANNING_AREA);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.SecurityCategory,
                Facility.StringLengths.SECURITY_CATEGORY);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.SecurityGrouping,
                Facility.StringLengths.SECURITY_GROUPING);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.SecurityInspectionFrequency,
                Facility.StringLengths.SECURITY_INSPECTION_FREQUENCY);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.SecurityLoopSequence,
                Facility.StringLengths.SECURITY_LOOP_SEQUENCE);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.StreetNumber,
                Facility.StringLengths.STREET_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.System, Facility.StringLengths.SYSTEM);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.WaterShed, Facility.StringLengths.WATER_SHED);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.YearInService,
                Facility.StringLengths.YEAR_IN_SERVICE);
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.ZipCode, Facility.StringLengths.ZIP_CODE);

            // MaxStringLength generates random string
            // Need a valid url format otherwise test fails in url format validation
            var url = "http://mapcall.amwater.com/";
            url = url + url.PadRight(Facility.StringLengths.VAMP_URL - url.Length, 'x');
            _target.VampUrl = url;
            ValidationAssert.PropertyHasMaxStringLength(_target, x => x.VampUrl, Facility.StringLengths.VAMP_URL, useCurrentPropertyValue: true);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_target, x => x.OperatingCenter, "The Operating Center field is required.");
            ValidationAssert.PropertyIsNotRequired(_target, x => x.RMPNumber);

            _target.RMP = true;

            ValidationAssert.PropertyIsRequiredWhen(_target, x => x.RMPNumber, (long)121231233, x => x.RMP, true);
            ValidationAssert.PropertyIsRequiredWhen(_target, x => x.SystemDeliveryType, 1, x => x.PointOfEntry, true);
        }

        [TestMethod]
        public void TestLikelihoodOfFailureIsCalculatedFromConditionAndPerformance()
        {
            var matrix = new[] {
                // 1 1 1
                (condition: FacilityCondition.Indices.GOOD,
                    performance: FacilityPerformance.Indices.GOOD,
                    likelihood: FacilityLikelihoodOfFailure.Indices.LOW),
                // 2 1 1
                (condition: FacilityCondition.Indices.AVERAGE,
                    performance: FacilityPerformance.Indices.GOOD,
                    likelihood: FacilityLikelihoodOfFailure.Indices.LOW),
                // 3 1 2
                (condition: FacilityCondition.Indices.POOR,
                    performance: FacilityPerformance.Indices.GOOD,
                    likelihood: FacilityLikelihoodOfFailure.Indices.MEDIUM),
                // 1 2 1
                (condition: FacilityCondition.Indices.GOOD,
                    performance: FacilityPerformance.Indices.AVERAGE,
                    likelihood: FacilityLikelihoodOfFailure.Indices.LOW),
                // 2 2 2
                (condition: FacilityCondition.Indices.AVERAGE,
                    performance: FacilityPerformance.Indices.AVERAGE,
                    likelihood: FacilityLikelihoodOfFailure.Indices.MEDIUM),
                // 3 2 3
                (condition: FacilityCondition.Indices.POOR,
                    performance: FacilityPerformance.Indices.AVERAGE,
                    likelihood: FacilityLikelihoodOfFailure.Indices.HIGH),
                // 1 3 2
                (condition: FacilityCondition.Indices.GOOD,
                    performance: FacilityPerformance.Indices.POOR,
                    likelihood: FacilityLikelihoodOfFailure.Indices.MEDIUM),
                // 2 3 3
                (condition: FacilityCondition.Indices.AVERAGE,
                    performance: FacilityPerformance.Indices.POOR,
                    likelihood: FacilityLikelihoodOfFailure.Indices.HIGH),
                // 3 3 3
                (condition: FacilityCondition.Indices.POOR,
                    performance: FacilityPerformance.Indices.POOR,
                    likelihood: FacilityLikelihoodOfFailure.Indices.HIGH)
            };

            foreach (var tup in matrix)
            {
                _target.Condition = tup.condition;
                _target.Performance = tup.performance;

                var entity = _target.MapToEntity(new Facility());

                Assert.AreEqual(tup.likelihood, entity.LikelihoodOfFailure.Id);
            }
        }

        [TestMethod]
        public void TestRiskOfFailureScoreIsCalculatedFromLikelihoodOfFailureAndConsequenceOfFailure()
        {
            var matrix = new[] {
                (condition: FacilityCondition.Indices.GOOD,
                    performance: FacilityPerformance.Indices.GOOD,
                    consequence: FacilityConsequenceOfFailure.Indices.LOW,
                    risk: 1),
                (condition: FacilityCondition.Indices.AVERAGE,
                    performance: FacilityPerformance.Indices.AVERAGE,
                    consequence: FacilityConsequenceOfFailure.Indices.LOW,
                    risk: 2),
                (condition: FacilityCondition.Indices.POOR,
                    performance: FacilityPerformance.Indices.POOR,
                    consequence: FacilityConsequenceOfFailure.Indices.LOW,
                    risk: 3),
                (condition: FacilityCondition.Indices.GOOD,
                    performance: FacilityPerformance.Indices.GOOD,
                    consequence: FacilityConsequenceOfFailure.Indices.MEDIUM,
                    risk: 2),
                (condition: FacilityCondition.Indices.AVERAGE,
                    performance: FacilityPerformance.Indices.AVERAGE,
                    consequence: FacilityConsequenceOfFailure.Indices.MEDIUM,
                    risk: 4),
                (condition: FacilityCondition.Indices.POOR,
                    performance: FacilityPerformance.Indices.POOR,
                    consequence: FacilityConsequenceOfFailure.Indices.MEDIUM,
                    risk: 6),
                (condition: FacilityCondition.Indices.GOOD,
                    performance: FacilityPerformance.Indices.GOOD,
                    consequence: FacilityConsequenceOfFailure.Indices.HIGH,
                    risk: 3),
                (condition: FacilityCondition.Indices.AVERAGE,
                    performance: FacilityPerformance.Indices.AVERAGE,
                    consequence: FacilityConsequenceOfFailure.Indices.HIGH,
                    risk: 6),
                (condition: FacilityCondition.Indices.POOR,
                    performance: FacilityPerformance.Indices.POOR,
                    consequence: FacilityConsequenceOfFailure.Indices.HIGH,
                    risk: 9),
            };

            foreach (var tup in matrix)
            {
                _target.Condition = tup.condition;
                _target.Performance = tup.performance;
                _target.ConsequenceOfFailure = tup.consequence;

                var entity = _target.MapToEntity(new Facility {
                    MaintenanceRiskOfFailure = new FacilityMaintenanceRiskOfFailure()
                });
                Assert.AreEqual((short)tup.risk, entity.MaintenanceRiskOfFailure.RiskScore,
                    $"Risk: {tup.risk} failed to match up");
            }
        }

        [TestMethod]
        public void TestStrategyTierIsDeterminedFromRiskOfFailureScore()
        {
            var matrix = new[] {
                (Consequence: FacilityConsequenceOfFailure.Indices.LOW,
                    Condition: FacilityCondition.Indices.GOOD,
                    Performance: FacilityPerformance.Indices.GOOD,
                    ExpectedTier: FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_3),
                (Consequence: FacilityConsequenceOfFailure.Indices.LOW,
                    Condition: FacilityCondition.Indices.AVERAGE,
                    Performance: FacilityPerformance.Indices.AVERAGE,
                    ExpectedTier: FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_3),
                (Consequence: FacilityConsequenceOfFailure.Indices.LOW,
                    Condition: FacilityCondition.Indices.POOR,
                    Performance: FacilityPerformance.Indices.POOR,
                    ExpectedTier: FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_2),

                (Consequence: FacilityConsequenceOfFailure.Indices.MEDIUM,
                    Condition: FacilityCondition.Indices.GOOD,
                    Performance: FacilityPerformance.Indices.GOOD,
                    ExpectedTier: FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_3),
                (Consequence: FacilityConsequenceOfFailure.Indices.MEDIUM,
                    Condition: FacilityCondition.Indices.AVERAGE,
                    Performance: FacilityPerformance.Indices.AVERAGE,
                    ExpectedTier: FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_2),
                (Consequence: FacilityConsequenceOfFailure.Indices.MEDIUM,
                    Condition: FacilityCondition.Indices.POOR,
                    Performance: FacilityPerformance.Indices.POOR,
                    ExpectedTier: FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_1),

                (Consequence: FacilityConsequenceOfFailure.Indices.HIGH,
                    Condition: FacilityCondition.Indices.GOOD,
                    Performance: FacilityPerformance.Indices.GOOD,
                    ExpectedTier: FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_2),
                (Consequence: FacilityConsequenceOfFailure.Indices.HIGH,
                    Condition: FacilityCondition.Indices.AVERAGE,
                    Performance: FacilityPerformance.Indices.AVERAGE,
                    ExpectedTier: FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_1),
                (Consequence: FacilityConsequenceOfFailure.Indices.HIGH,
                    Condition: FacilityCondition.Indices.POOR,
                    Performance: FacilityPerformance.Indices.POOR,
                    ExpectedTier: FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_1),
            };

            foreach (var tup in matrix)
            {
                _target.Condition = tup.Condition;
                _target.Performance = tup.Performance;
                _target.ConsequenceOfFailure = tup.Consequence;

                var entity = _target.MapToEntity(new Facility());
                Assert.AreEqual(tup.ExpectedTier, entity.StrategyTier.Id);
            }
        }

        [TestMethod]
        public void TestConsequenceOfFailureFactorDoesNotFailWithOnlyOnePWSID()
        {
            var state = GetEntityFactory<State>().Create();
            var pwsid = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 100,
                State = state
            });
            var entity = GetEntityFactory<Facility>().Create(new {
                PublicWaterSupply = pwsid,
            });

            _target.Map(entity);
            entity = _target.MapToEntity(entity);

            Assert.IsNull(entity.ConsequenceOfFailureFactor);
        }

        [TestMethod]
        public void TestConsequenceOfFailureFactorIsDeterminedFromOtherPWSIDs()
        {
            var state = GetEntityFactory<State>().Create();
            var largestPWSID = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 100,
                State = state
            });
            var smallestPWSID = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 25,
                State = state
            });
            var someOtherPWSID = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 60,
                State = state
            });
            var thisPWSID = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 75,
                State = state
            });
            var otherStateLargestPWSID = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 1000,
            });
            var otherStateSmallestPWSID = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 10,
            });
            var entity = GetEntityFactory<Facility>().Create(new {
                PublicWaterSupply = thisPWSID,
            });

            _target.Map(entity);
            entity = _target.MapToEntity(entity);

            Assert.AreEqual(0.8333333f, entity.ConsequenceOfFailureFactor);
        }

        [TestMethod]
        public void TestWeightedRiskOfFailureScoreIsCalculated()
        {
            var state = GetEntityFactory<State>().Create();
            var largestPWSID = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 100,
                State = state
            });
            var smallestPWSID = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 25,
                State = state
            });
            var someOtherPWSID = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 60,
                State = state
            });
            var thisPWSID = GetEntityFactory<PublicWaterSupply>().Create(new {
                UsageLastYear = 75,
                State = state
            });
            var entity = GetEntityFactory<Facility>().Create(new {
                PublicWaterSupply = thisPWSID,
            });

            _target.Map(entity);
            _target.Condition = 2;
            _target.Performance = 2;
            _target.ConsequenceOfFailure = 2;
            entity = _target.MapToEntity(new Facility { MaintenanceRiskOfFailure = GetFactory<FacilityModHighMaintenanceRiskOfFailureFactory>().Create()});

            Assert.AreEqual(3.333333, entity.WeightedRiskOfFailureScore);
        }

        [TestMethod]
        public void TestRiskOfFailureIsRecalculatedOnChildEquipment()
        {
            var entity = new Facility {
                Equipment = new List<Equipment> {
                    new Equipment {
                        ConsequenceOfFailure = new EquipmentConsequencesOfFailureRating {Id = EquipmentConsequencesOfFailureRating.Indices.LOW},
                        LikelyhoodOfFailure = new EquipmentLikelyhoodOfFailureRating {Id = EquipmentLikelyhoodOfFailureRating.Indices.LOW}
                    }
                },
                ConsequenceOfFailure = new FacilityConsequenceOfFailure {Id = FacilityConsequenceOfFailure.Indices.LOW},
                MaintenanceRiskOfFailure = GetFactory<FacilityLowMaintenanceRiskOfFailureFactory>().Create(),
                Condition = new FacilityCondition {Id = FacilityCondition.Indices.GOOD},
                Performance = new FacilityPerformance {Id = FacilityPerformance.Indices.GOOD}
            };

            _target.Map(entity);
            entity = _target.MapToEntity(entity);

            Assert.AreEqual(EquipmentFailureRiskRating.Indices.LOW, entity.Equipment[0].RiskOfFailure.Id);
        }

        [TestMethod]
        public void TestSetEquipmentRiskCharacteristicsLastUpdatedIfEquipmentRiskCharacteristicsHaveBeenModified()
        {
            var now = DateTime.Now;
            _dateTimeProvider
               .Setup(dt => dt.GetCurrentDate())
               .Returns(now);
            
            var entity = new Facility {
                Equipment = new List<Equipment> {
                    new Equipment {
                        ConsequenceOfFailure = new EquipmentConsequencesOfFailureRating {Id = EquipmentConsequencesOfFailureRating.Indices.LOW},
                        LikelyhoodOfFailure = new EquipmentLikelyhoodOfFailureRating {Id = EquipmentLikelyhoodOfFailureRating.Indices.LOW}
                    }
                },
                ConsequenceOfFailure = new FacilityConsequenceOfFailure {Id = FacilityConsequenceOfFailure.Indices.LOW},
                MaintenanceRiskOfFailure = GetFactory<FacilityLowMaintenanceRiskOfFailureFactory>().Create(),
                Condition = new FacilityCondition {Id = FacilityCondition.Indices.GOOD},
                Performance = new FacilityPerformance {Id = FacilityPerformance.Indices.GOOD}
            };
            
            Assert.IsNull(entity.Equipment[0].RiskCharacteristicsLastUpdatedBy);

            _target.Map(entity);
            entity = _target.MapToEntity(entity);

            var riskCharacteristicsLastUpdated = 
                entity.Equipment[0].RiskCharacteristicsLastUpdatedOn ?? DateTime.MinValue;
            
            Assert.AreEqual(entity.Equipment[0].RiskCharacteristicsLastUpdatedBy, _user);
            Assert.AreEqual(riskCharacteristicsLastUpdated, now);
        }

        #endregion
    }
}
