using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using StructureMap;

namespace MapCallImporter.Tests.Models.Import
{
    [TestClass]
    public class HydrantExcelRecordTest : ExcelRecordTestBase<Hydrant, MyCreateHydrant, HydrantExcelRecord>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override void ImportTestData()
        {
            base.ImportTestData();

            TestDataHelper.CreateStuffForHydrantsInAberdeenNJ(_container);
        }

        protected override HydrantExcelRecord CreateTarget()
        {
            return new HydrantExcelRecord {
                Town = AberdeenMonmouthNJTown.ID,
                StreetId = AberdeenMonmouthNJStreets.ChurchStreet.ID,
                FireDistrictID = FireDistricts.ABERDEEN,
                HydrantStatusId = AssetStatus.Indices.ACTIVE,
                InspectionFrequencyUnitId = RecurringFrequencyUnit.Indices.YEAR,
                OperatingCenterId = OperatingCenters.NJ7.ID,
                HydrantBillingId = HydrantBilling.Indices.PUBLIC,
                WaterSystemId = WaterSystems.NJMM,
                FunctionalLocationId = FunctionalLocations.NJMM_AB_HYDRT,
                BillingDate = _now,
                DateInstalled = _now,
                Latitude = 1,
                Longitude = 2,
                HydrantSuffix = 3,
                HydrantNumber = "HAB-3",
                GeoEFunctionalLocation = GenerateIntString(18),
                WorkOrderNumber = "workOrderNumber",
                HydrantMainSizeId = 23,
                YearManufactured = 2020
            };
        }

        #endregion

        protected override void InnerTestMappings(ExcelRecordMappingTester<Hydrant, MyCreateHydrant, HydrantExcelRecord> test)
        {
            test.RequiredEntityRef(h => h.StreetId, h => h.Street);
            test.RequiredEntityRef(h => h.Town, h => h.Town);
            test.RequiredEntityRef(h => h.FireDistrictID, h => h.FireDistrict);
            test.RequiredEntityRef(h => h.HydrantStatusId, h => h.Status);
            test.RequiredEntityRef(h => h.OperatingCenterId, h => h.OperatingCenter);
            test.RequiredEntityRef(h => h.HydrantBillingId, h => h.HydrantBilling);
            test.RequiredEntityRef(h => h.WaterSystemId, h => h.WaterSystem);
            test.RequiredEntityRef(h => h.FunctionalLocationId, h => h.FunctionalLocation);
            test.RequiredEntityRef(h => h.HydrantMainSizeId, h => h.HydrantMainSize);

            test.RequiredDateTime(h => h.BillingDate, h => h.BillingDate);
            test.RequiredGreaterThanZeroInt(h => h.HydrantSuffix, h => h.HydrantSuffix);

            test.EntityRef(h => h.InspectionFrequencyUnitId, h => h.InspectionFrequencyUnit);
            test.EntityRef(h => h.FacilityId, h => h.Facility);
            test.EntityRef(h => h.TownSectionId, h => h.TownSection);
            test.EntityRef(h => h.MainTypeId, h => h.MainType);
            test.EntityRef(h => h.HydrantThreadTypeId, h => h.HydrantThreadType);
            test.EntityRef(h => h.InitiatorId, h => h.Initiator);
            test.EntityRef(h => h.HydrantTagStatusID, h => h.HydrantTagStatus);
            test.EntityRef(h => h.ManufacturerID, h => h.HydrantManufacturer);
            test.EntityRef(h => h.HydrantModelID, h => h.HydrantModel);
            test.EntityRef(h => h.LateralSizeId, h => h.LateralSize);
            test.EntityRef(h => h.CrossStreetId, h => h.CrossStreet);
            test.EntityRef(h => h.OpensDirectionId, h => h.OpenDirection);
            test.EntityRef(h => h.GradientId, h => h.Gradient);
            test.EntityRef(h => h.HydrantSizeId, h => h.HydrantSize);

            test.Int(h => h.BranchLengthFeet, h => h.BranchLengthFeet);
            test.Int(h => h.BranchLengthInches, h => h.BranchLengthFeet);
            test.DateTime(h => h.DateAdded, h => h.CreatedAt);
            test.TestedElsewhere(h => h.DateRetired);
            test.DateTime(h => h.DateTested, h => h.DateTested);
            test.Int(h => h.DepthBuryFeet, h => h.DepthBuryFeet);
            test.Int(h => h.DepthBuryInches, h => h.DepthBuryInches);
            test.Decimal(h => h.Elevation, h => h.Elevation);
            test.Int(h => h.FLRouteNumber, h => h.FLRouteNumber);
            test.Int(h => h.FLRouteSequence, h => h.FLRouteSequence);
            test.Int(h => h.InspectionFrequency, h => h.InspectionFrequency);
            test.Boolean(h => h.IsDeadEndMain, h => h.IsDeadEndMain);
            test.Boolean(h => h.IsNonBPUKPI, h => h.IsNonBPUKPI);
            test.String(h => h.Location, h => h.Location);
            test.String(h => h.MapPage, h => h.MapPage);
            test.Int(h => h.ObjectID, h => h.ObjectID);
            test.Int(h => h.Route, h => h.Route);
            test.Int(h => h.SAPEquipmentID, h => h.SAPEquipmentId);
            test.Decimal(h => h.Stop, h => h.Stop);
            test.String(h => h.StreetNumber, h => h.StreetNumber);
            test.String(h => h.ValveLocation, h => h.ValveLocation);
            test.String(h => h.WorkOrderNumber, h => h.WorkOrderNumber);
            test.IntWithinRange(h => h.YearManufactured, h => h.YearManufactured, 1850, DateTime.Now.Year);
            test.String(x => x.LegacyID, x => x.LegacyId);
            test.String(x => x.GeoEFunctionalLocation, x => x.GISUID);
            test.Int(x => x.Zone, x => x.Zone);

            test.TestedElsewhere(h => h.ClowTagged);
            test.TestedElsewhere(h => h.CreatedOn);
            test.TestedElsewhere(h => h.Critical);
            test.TestedElsewhere(h => h.CriticalNotes);
            test.TestedElsewhere(h => h.DateInstalled);
            test.TestedElsewhere(h => h.GeoEFunctionalLocation);
            test.TestedElsewhere(h => h.HydrantNumber);
            test.TestedElsewhere(h => h.LateralValveId);
            test.TestedElsewhere(h => h.Latitude);
            test.TestedElsewhere(h => h.Longitude);
            test.TestedElsewhere(h => h.LastUpdated);

            test.NotMapped(h => h.CoordinateId);
            test.NotMapped(h => h.Id);
            test.NotMapped(h => h.LateralValve);
            test.NotMapped(h => h.LateralValveRemove);
            test.NotMapped(h => h.SAPerrorCode);
        }

        [TestMethod]
        public void TestCoordinateIsCreatedFromLatAndLng()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(1, result.Coordinate.Latitude);
                Assert.AreEqual(2, result.Coordinate.Longitude);
            });
        }

        [TestMethod]
        public void TestProvidedSuffixAndHydrantNumberAreKept()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(_target.HydrantSuffix, result.HydrantSuffix);
                Assert.AreEqual(_target.HydrantNumber, result.HydrantNumber);
            });
        }

        [TestMethod]
        public void TestLateralValveIsLookedUpFromLateralValveId()
        {
            _target.LateralValveId = "VAB-6666";

            WithUnitOfWork(uow => {
                var expected = uow.Where<Valve>(v => v.ValveNumber == _target.LateralValveId).Single();

                Assert.AreEqual(expected.Id, _target.MapToEntity(uow, 1, MappingHelper).LateralValve.Id);
            });
        }

        [TestMethod]
        public void TestClowTaggedIsSetToFalse()
        {
            WithUnitOfWork(uow => {
                foreach (var value in new[] {"yes", "true", "t", "1", true.ToString()})
                {
                    _target.ClowTagged = value;

                    Assert.IsFalse(_target.MapToEntity(uow, 1, MappingHelper).ClowTagged);
                }
            });
        }

        [TestMethod]
        public void TestCriticalMustBeTrueWhenEnteringCriticalNotes()
        {
            _target.CriticalNotes = "blah";
            _target.Critical = false;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestCriticalNotesMustBeEnteredWhenCriticalIsTrue()
        {
            _target.CriticalNotes = null;
            _target.Critical = true;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestDateInstalledIsRequiredForActiveHydrants()
        {
            _target.DateInstalled = null;
            _target.HydrantStatusId = AssetStatus.Indices.ACTIVE;

            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestDateInstalledIsNotRequiredForInactiveHydrants()
        {
            _target.DateInstalled = null;
            _target.HydrantStatusId = AssetStatus.Indices.INACTIVE;

            WithUnitOfWork(uow => {
                ExpectNoMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
        }

        [TestMethod]
        public void TestGeoEFunctionalLocationMustBeBetween18And40CharsLongIfSet()
        {
            _target.GeoEFunctionalLocation = GenerateIntString(17);

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper),
                "String of only 17 chars was accepted"));

            _target.GeoEFunctionalLocation = GenerateIntString(18);

            WithUnitOfWork(uow => ExpectNoMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper),
                "String of 18 chars was not accepted"));

            _target.GeoEFunctionalLocation = GenerateIntString(40);

            WithUnitOfWork(uow => ExpectNoMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper),
                "String of 40 chars was not accepted"));

            _target.GeoEFunctionalLocation = GenerateIntString(41);

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper),
                "String of 41 chars was accepted"));
        }

        [TestMethod]
        public void TestHydrantNumberAllowsLettersInSuffix()
        {
            Hydrant entity = null;
            _target.HydrantNumber = "HAB-3A";

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.AreEqual("HAB-3A", entity.HydrantNumber);
            Assert.AreEqual(3, entity.HydrantSuffix);
        }

        [TestMethod]
        public void TestDoesNotChokeWhenTownIsNotInOperatingCenter()
        {
            var town = GetEntityFactory<Town>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();

            _target.OperatingCenterId = operatingCenter.Id;
            _target.Town = town.Id;

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper)));
        }

        [TestMethod]
        public void TestSAPErrorCodeIsSet()
        {
            WithUnitOfWork(uow => {
                Assert.AreEqual(HydrantExcelRecord.SAP_RETRY_ERROR_CODE,
                    _target.MapToEntity(uow, 1, MappingHelper).SAPErrorCode);
            });
        }

        [TestMethod]
        public void TestRetiredDateMapsValueWhenStatusIsRetiredAndIsNullForAnyOtherStatus()
        {
            Hydrant entity = null;
            var dt = DateTime.Now;

            _target.HydrantStatusId = AssetStatus.Indices.RETIRED;
            _target.DateRetired = dt;

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.AreEqual(dt, entity.DateRetired);

            _target.HydrantStatusId = AssetStatus.Indices.ACTIVE;

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.IsNull(entity.DateRetired);

            _target.HydrantStatusId = AssetStatus.Indices.REQUEST_RETIREMENT;

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.IsNull(entity.DateRetired);
        }

        /*
         *  Per MC-1059, Data Import overrides any existing view model behavior defined for the UI... put simply,
         *  whatever is in the import file, needs to put put on the asset.
         */
        [TestMethod]
        public void TestInspectionFrequencyIsMappedWhenEmpty()
        {
            WithUnitOfWork(uow => {
                _target.InspectionFrequency = null;
                _target.InspectionFrequencyUnitId = null;

                var entity = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.IsNull(entity.InspectionFrequency);
                Assert.IsNull(entity.InspectionFrequencyUnit);
            });
        }

        /*
         *  Per MC-1059, Data Import overrides any existing view model behavior defined for the UI... put simply,
         *  whatever is in the import file, needs to put put on the asset.
         */
        [TestMethod]
        public void TestInspectionFrequencyIsMappedWhenNotEmpty()
        {
            WithUnitOfWork(uow => {
                const int expectedInspectionFrequency = 30;
                const int expectedInspectedFrequencyUnitId = RecurringFrequencyUnit.Indices.DAY;

                _target.InspectionFrequency = expectedInspectionFrequency;
                _target.InspectionFrequencyUnitId = expectedInspectedFrequencyUnitId;

                var entity = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(entity.InspectionFrequency, expectedInspectionFrequency);
                Assert.AreEqual(entity.InspectionFrequencyUnit.Id, expectedInspectedFrequencyUnitId);
            });
        }

        [TestMethod]
        public override void TestMappings()
        {
            base.TestMappings();
        }
    }
}
