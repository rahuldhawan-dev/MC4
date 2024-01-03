using MapCall.Common.Model.Entities;
using MapCallImporter.Library.ClassExtensions;
using MapCallImporter.Library.Testing;
using MapCallImporter.Models.Import;
using MapCallImporter.SampleValues;
using MapCallImporter.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MapCallImporter.Tests.Models.Import
{
    [TestClass]
    public class SewerOpeningExcelRecordTest : ExcelRecordTestBase<SewerOpening, MyCreateSewerOpening, SewerOpeningExcelRecord>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TestDataHelper.CreateStuffForSewerOpeningsInAberdeenNJ(_container);
        }

        #endregion

        protected override SewerOpeningExcelRecord CreateTarget()
        {
            return new SewerOpeningExcelRecord {
                OperatingCenterID = OperatingCenters.NJ7.ID,
                TownID = AberdeenMonmouthNJTown.ID,
                StreetID = AberdeenMonmouthNJStreets.ChurchStreet.ID,
                IntersectingStreetID = AberdeenMonmouthNJStreets.IdlewildLane.ID,
                SewerOpeningTypeID = SewerOpeningType.Indices.CATCH_BASIN,
                Latitude = 1,
                Longitude = 2,
                FunctionalLocationID = FunctionalLocations.NJMM_AB_VALVE,
                AssetStatusID = AssetStatus.Indices.ACTIVE,
                OpeningSuffix = 3,
                OpeningNumber = "MAB-3",
                TaskNumber = "workOrderNumber",
            };
        }

        protected override void InnerTestMappings(ExcelRecordMappingTester<SewerOpening, MyCreateSewerOpening, SewerOpeningExcelRecord> test)
        {
            test.RequiredEntityRef(x => x.OperatingCenterID, x => x.OperatingCenter);
            test.RequiredEntityRef(x => x.TownID, x => x.Town);
            test.RequiredEntityRef(x => x.StreetID, x => x.Street);
            test.RequiredEntityRef(x => x.IntersectingStreetID, x => x.IntersectingStreet);
            test.RequiredEntityRef(x => x.AssetStatusID, x => x.Status);
            test.RequiredEntityRef(x => x.SewerOpeningTypeID, x => x.SewerOpeningType);

            test.EntityRef(x => x.PDESSystem, x => x.WasteWaterSystem);
            test.EntityRef(x => x.TownSection, x => x.TownSection);
            test.EntityRef(x => x.SewerOpeningMaterialID, x => x.SewerOpeningMaterial);
            test.EntityRef(x => x.InspectionFrequencyUnitId, x => x.InspectionFrequencyUnit);

            test.String(x => x.OldNumber, x => x.OldNumber);
            test.String(x => x.StreetNumber, x => x.StreetNumber);
            test.String(x => x.MapPage, x => x.MapPage);
            test.String(x => x.DistanceFromCrossStreet, x => x.DistanceFromCrossStreet);
            test.String(x => x.TaskNumber, x => x.TaskNumber);
            test.String(x => x.SAPErrorCode, x => x.SAPErrorCode);

            test.Int(x => x.SAPEquipmentID, x => x.SAPEquipmentId);
            test.Int(x => x.OpeningSuffix, x => x.OpeningSuffix);
            test.Int(x => x.Route, x => x.Route);
            test.Int(x => x.Stop, x => x.Stop);
            test.Int(x => x.InspectionFrequency, x => x.InspectionFrequency);

            test.Decimal(x => x.DepthToInvert, x => x.DepthToInvert);
            test.Decimal(x => x.RimElevation, x => x.RimElevation);
            
            test.DateTime(x => x.DateInstalled, x => x.DateInstalled);
            test.TestedElsewhere(x => x.DateRetired);

            test.Boolean(x => x.IsEpoxyCoated, x => x.IsEpoxyCoated);
            test.Boolean(x => x.IsDoghouseOpening, x => x.IsDoghouseOpening);

            test.TestedElsewhere(x => x.OpeningNumber);
            test.TestedElsewhere(x => x.Latitude);
            test.TestedElsewhere(x => x.Longitude);
            test.TestedElsewhere(x => x.FunctionalLocationID);
            test.TestedElsewhere(x => x.Critical);
            test.TestedElsewhere(x => x.CriticalNotes);
            test.TestedElsewhere(x => x.GeoEFunctionalLocation);

            test.NotMapped(x => x.Notes);
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

            _target.GeoEFunctionalLocation = GenerateIntString(19);

            WithUnitOfWork(uow => ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper),
                "String of 19 chars was accepted"));
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
        public void TestProvidedSuffixAndSewerOpeningNumberAreKept()
        {
            WithUnitOfWork(uow => {
                var result = _target.MapToEntity(uow, 1, MappingHelper);

                Assert.AreEqual(_target.OpeningSuffix, result.OpeningSuffix);
                Assert.AreEqual(_target.OpeningNumber, result.OpeningNumber);
            });
        }

        [TestMethod]
        public void TestFunctionalLocationIsRequiredSometimes()
        {
            WithMappingTester(t => {
                t.RequiredEntityRef(x => x.FunctionalLocationID, x => x.FunctionalLocation);
            });

            WithUnitOfWork(uow => {
                var operatingCenter = uow.Find<OperatingCenter>(OperatingCenters.NJ7.ID);
                operatingCenter.SAPEnabled = false;
                _target.FunctionalLocationID = null;

                ExpectNoMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
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
        public void TestOpeningStatusMustBeEntered()
        {
            _target.AssetStatusID = null;
            
            WithUnitOfWork(uow => {
                ExpectMappingFailure(() => _target.MapToEntity(uow, 1, MappingHelper));
            });
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
        public void TestRetiredDateMapsValueWhenStatusIsRetiredAndIsNullForAnyOtherStatus()
        {
            SewerOpening entity = null;
            var dt = DateTime.Now;

            _target.AssetStatusID = AssetStatus.Indices.RETIRED;
            _target.DateRetired = dt;

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.AreEqual(dt, entity.DateRetired);

            _target.AssetStatusID = AssetStatus.Indices.ACTIVE;

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.IsNull(entity.DateRetired);

            _target.AssetStatusID = AssetStatus.Indices.REQUEST_RETIREMENT;

            WithUnitOfWork(uow => entity = _target.MapToEntity(uow, 1, MappingHelper));

            Assert.IsNull(entity.DateRetired);
        }
    }
}
