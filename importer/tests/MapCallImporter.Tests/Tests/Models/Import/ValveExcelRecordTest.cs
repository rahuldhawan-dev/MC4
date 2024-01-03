using MapCall.Common.Model.Entities;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MapCallImporter.Tests.Models.Import
{
    [TestClass]
    public class ValveExcelRecordTest : ExcelRecordTestBase<Valve, MyCreateValve, ValveExcelRecord>
    {
        protected override ValveExcelRecord CreateTarget()
        {
            return new ValveExcelRecord {
                DateInstalled = _now,
                FunctionalLocationId = FunctionalLocations.NJMM_AB_VALVE,
                StreetId = AberdeenMonmouthNJStreets.ChurchStreet.ID,
                Town = AberdeenMonmouthNJTown.ID,
                ValveBillingId = ValveBilling.Indices.PUBLIC,
                OperatingCenterId = OperatingCenters.NJ7.ID,
                ValveControlsId = ValveControl.Indices.HYDRANT,
                ValveNumber = "VAB-3",
                ValveStatusId = AssetStatus.Indices.ACTIVE,
                ValveSuffix = 3,
                ValveZoneId = 1,
                WaterSystemId = WaterSystems.NJMM,
                CrossStreetId = AberdeenMonmouthNJStreets.IdlewildLane.ID,
                GeoEFunctionalLocation = GenerateIntString(18),
                NormalPositionId = ValveNormalPosition.Indices.OPEN,
                OpensId = ValveOpenDirection.Indices.RIGHT,
                Latitude = 1,
                Longitude = 2,
                ValveSizeId = 1,
                WorkOrderNumber = "workOrderNumber",
                ValveTypeId = 5,
                Turns = 4,
            };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<Valve, MyCreateValve, ValveExcelRecord> test)
        {
            test.RequiredEntityRef(v => v.StreetId, v => v.Street);
            test.RequiredEntityRef(v => v.Town, v => v.Town);
            test.RequiredEntityRef(v => v.ValveBillingId, v => v.ValveBilling);
            test.RequiredEntityRef(v => v.OperatingCenterId, v => v.OperatingCenter);
            test.RequiredEntityRef(v => v.ValveControlsId, v => v.ValveControls);
            test.RequiredEntityRef(v => v.ValveStatusId, v => v.Status);
            test.RequiredEntityRef(v => v.ValveZoneId, v => v.ValveZone);
            test.RequiredEntityRef(v => v.WaterSystemId, v => v.WaterSystem);
            test.RequiredEntityRef(v => v.FunctionalLocationId, v => v.FunctionalLocation);
            test.RequiredEntityRef(v => v.ValveSizeId, v => v.ValveSize);
            test.RequiredEntityRef(v => v.NormalPositionId, v => v.NormalPosition);
            test.RequiredEntityRef(v => v.ValveTypeId, v => v.ValveType);

            test.RequiredGreaterThanZeroInt(v => v.ValveSuffix, v => v.ValveSuffix);

            test.EntityRef(v => v.InspectionFrequencyUnitId, v => v.InspectionFrequencyUnit);
            test.EntityRef(v => v.CrossStreetId, v => v.CrossStreet);
            test.EntityRef(v => v.OpensId, v => v.OpenDirection);
            test.EntityRef(v => v.TownSectionId, v => v.TownSection);
            test.EntityRef(v => v.MainTypeId, v => v.MainType);
            test.EntityRef(v => v.ValveMakeId, v => v.ValveMake);
            test.EntityRef(v => v.InitiatorId, v => v.Initiator);
            test.EntityRef(v => v.FacilityId, v => v.Facility);
            test.EntityRef(v => v.CrossStreetId, v => v.CrossStreet);

            test.DateTime(v => v.DateAdded, v => v.CreatedAt);
            test.DateTime(v => v.DateInstalled, v => v.DateInstalled);
            test.TestedElsewhere(v => v.DateRetired);
            test.DateTime(v => v.DateTested, v => v.DateTested);
            test.Decimal(v => v.Elevation, v => v.Elevation);
            test.Int(v => v.InspectionFrequency, v => v.InspectionFrequency);
            test.String(v => v.MapPage, v => v.MapPage);
            test.Int(v => v.ObjectID, v => v.ObjectID);
            test.Int(v => v.Route, v => v.Route);
            test.Int(v => v.SAPEquipmentID, v => v.SAPEquipmentId);
            test.Decimal(v => v.Stop, v => v.Stop);
            test.String(v => v.StreetNumber, v => v.StreetNumber);
            test.String(v => v.ValveLocation, v => v.ValveLocation);
            test.String(v => v.WorkOrderNumber, v => v.WorkOrderNumber);
            test.Boolean(v => v.BPUKPI, v => v.BPUKPI);
            test.String(v => v.SketchNumber, v => v.SketchNumber);
            test.Boolean(v => v.Traffic, v => v.Traffic);
            test.Decimal(v => v.Turns, v => v.Turns);
            test.String(x => x.LegacyID, x => x.LegacyId);
            test.String(x => x.GeoEFunctionalLocation, x => x.GISUID);

            test.TestedElsewhere(v => v.Critical);
            test.TestedElsewhere(v => v.CriticalNotes);
            test.TestedElsewhere(v => v.Latitude);
            test.TestedElsewhere(v => v.Longitude);
            test.TestedElsewhere(v => v.LastUpdated);
            test.TestedElsewhere(v => v.ValveNumber);
            test.TestedElsewhere(v => v.VlvTopValveNutDepth);

            test.NotMapped(v => v.CoordinateId);
            test.NotMapped(v => v.Id);
            test.NotMapped(v => v.SAPerrorCode);
            test.NotMapped(v => v.ImageActionID);
            test.NotMapped(v => v.ControlsCrossing);
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
        public void TestProvidedSuffixAndValveNumberAreKept()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(_target.ValveSuffix, result.ValveSuffix);
                Assert.AreEqual(_target.ValveNumber, result.ValveNumber);
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
        public void TestValveNumberAllowsLettersInSuffix()
        {
            Valve entity = null;
            _target.ValveNumber = "VAB-3A";

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.AreEqual("VAB-3A", entity.ValveNumber);
            Assert.AreEqual(3, entity.ValveSuffix);
        }

        [TestMethod]
        public void TestValveNumberWithLetterNotConsideredDuplicateOfSameNumberWithoutLetter()
        {
            WithUnitOfWork(uow => {
                var existing = CreateTarget();
                existing.ValveNumber = "VAB-3";

                uow.GetRepository<Valve>().Insert(existing.MapToEntity(uow, 1, MappingHelper));

                _target.ValveNumber = "VAB-3A";
                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(_target.ValveNumber, result.ValveNumber);
            });
        }

        /*
         *  Per MC-1059, Data Import overrides any existing view model behavior defined for the UI... put simply,
         *  whatever is in the import file, needs to put put on the asset.
         */
        [TestMethod]
        public void TestInspectionFrequencyIsMappedWhenEmptyAndOperatingCenterHasUsesValveInspectionFrequencyIsFalse()
        {
            WithUnitOfWork(uow => {
                _target.OperatingCenterId = OperatingCenters.NJ7.ID;
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
        public void TestInspectionFrequencyIsMappedWhenNotEmptyAndOperatingCenterHasUsesValveInspectionFrequencyIsFalse()
        {
            WithUnitOfWork(uow => {
                const int expectedInspectionFrequency = 30;
                const int expectedInspectedFrequencyUnitId = RecurringFrequencyUnit.Indices.DAY;

                _target.OperatingCenterId = OperatingCenters.NJ7.ID;
                _target.InspectionFrequency = expectedInspectionFrequency;
                _target.InspectionFrequencyUnitId = expectedInspectedFrequencyUnitId;

                var entity = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(entity.InspectionFrequency, expectedInspectionFrequency);
                Assert.AreEqual(entity.InspectionFrequencyUnit.Id, expectedInspectedFrequencyUnitId);
            });
        }

        /*
         *  Per MC-1059, Data Import overrides any existing view model behavior defined for the UI... put simply,
         *  whatever is in the import file, needs to put put on the asset.
         *
         *  If you are here because this test is failing, it's most likely because the INSERT sql
         *  for OperatingCenters.NJ4 has been regenerated. This operating center has a value of
         *  false for UsesValveInspectionFrequency in the database... and therefore when regenerating
         *  it's value is false. When this test came to be, we decided to explicitly set one of the
         *  NJ operating center UsesValveInspectionFrequency values to true so that we could 
         *  ensure the logic for assigning inspection frequency works properly (it has different
         *  logic depending on the value of this boolean). The simple fix, is to explicitly go
         *  change that insert value for operating center NJ4 to 1 instead of 0.
         * 
         *  Additionally, you may need to associate Aberdeen with the operating center that had
         *  UsesValveInspectionFrequency toggled to true:
         *      INSERT INTO OperatingCentersTowns (OperatingCenterId, TownId, Abbreviation, Id) VALUES (14, 41, 'AB', 2)
         */
        [TestMethod]
        public void TestInspectionFrequencyIsMappedWhenEmptyAndOperatingCenterHasUsesValveInspectionFrequencyIsTrue()
        {
            WithUnitOfWork(uow => {
                _target.OperatingCenterId = OperatingCenters.NJ4.ID;
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
         *
         *  If you are here because this test is failing, it's most likely because the INSERT sql
         *  for OperatingCenters.NJ4 has been regenerated. This operating center has a value of
         *  false for UsesValveInspectionFrequency in the database... and therefore when regenerating
         *  it's value is false. When this test came to be, we decided to explicitly set one of the
         *  NJ operating center UsesValveInspectionFrequency values to true so that we could 
         *  ensure the logic for assigning inspection frequency works properly (it has different
         *  logic depending on the value of this boolean). The simple fix, is to explicitly go
         *  change that insert value for operating center NJ4 to 1 instead of 0.
         * 
         *  Additionally, you may need to associate Aberdeen with the operating center that was toggled:
         *      INSERT INTO OperatingCentersTowns (OperatingCenterId, TownId, Abbreviation, Id) VALUES (14, 41, 'AB', 2)
         */
        [TestMethod]
        public void TestInspectionFrequencyIsMappedWhenNotEmptyAndOperatingCenterHasUsesValveInspectionFrequencyIsTrue()
        {
            WithUnitOfWork(uow => {
                const int expectedInspectionFrequency = 30;
                const int expectedInspectedFrequencyUnitId = RecurringFrequencyUnit.Indices.DAY;

                _target.OperatingCenterId = OperatingCenters.NJ4.ID;
                _target.InspectionFrequency = expectedInspectionFrequency;
                _target.InspectionFrequencyUnitId = expectedInspectedFrequencyUnitId;

                var entity = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(entity.InspectionFrequency, expectedInspectionFrequency);
                Assert.AreEqual(entity.InspectionFrequencyUnit.Id, expectedInspectedFrequencyUnitId);
            });
        }

        [TestMethod]
        public void TestDepthFeetIsSetFromVlvTopValveNutDepth()
        {
            Valve entity = null;

            void Test(string excelValue, int? expectedFeet)
            {
                _target.VlvTopValveNutDepth = excelValue;

                WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

                Assert.AreEqual(expectedFeet, entity.DepthFeet);
            }

            Test("10FT", 10);
            Test("10FT 20IN", 10);
            Test("10IN", null);
        }

        [TestMethod]
        public void TestDepthInchesIsSetFromVlvTopValveNutDepth()
        {
            Valve entity = null;

            void Test(string excelValue, int? expectedInches)
            {
                _target.VlvTopValveNutDepth = excelValue;

                WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

                Assert.AreEqual(expectedInches, entity.DepthInches);
            }

            Test("10FT", null);
            Test("20FT 10IN", 10);
            Test("10IN", 10);
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
            Valve entity = null;
            var dt = DateTime.Now;

            _target.ValveStatusId = AssetStatus.Indices.RETIRED;
            _target.DateRetired = dt;

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.AreEqual(dt, entity.DateRetired);

            _target.ValveStatusId = AssetStatus.Indices.ACTIVE;

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.IsNull(entity.DateRetired);

            _target.ValveStatusId = AssetStatus.Indices.REQUEST_RETIREMENT;

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.IsNull(entity.DateRetired);
        }

        [TestMethod]
        public override void TestMappings()
        {
            base.TestMappings();
        }

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateStuffForValvesInAberdeenNJ(_container);
        }

        #endregion
    }
}
