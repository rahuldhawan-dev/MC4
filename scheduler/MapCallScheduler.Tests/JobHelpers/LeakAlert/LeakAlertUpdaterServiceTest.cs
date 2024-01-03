using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallScheduler.JobHelpers.LeakAlert;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.LeakAlert
{
    [TestClass]
    public class LeakAlertUpdaterServiceTest : SapEntityUpdaterServiceTestBase<LeakAlertFileRecord, ILeakAlertFileParser, EchoshoreLeakAlert, IRepository<EchoshoreLeakAlert>, LeakAlertUpdaterService>
    {
        #region Constants

        private const decimal LATITUDE = 40.7001734m, LONGITUDE = -74.21924929m, DISTANCE1 = 287.785153m, DISTANCE2 = 28.18123639m;

        #endregion

        #region Private Members

        private PointOfInterestStatus _fieldInvestigationrecommendedPointOfInterestStatus;
        private OperatingCenter _operatingCenter;
        private Town _town;
        private EchoshoreSite _echoshoreSite;
        private Hydrant _hyd1, _hyd2;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _fieldInvestigationrecommendedPointOfInterestStatus = GetFactory<FieldInvestigationRecommendedPointOfInterestFactory>().Create();
            _operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            _town = GetEntityFactory<Town>().Create();
            _echoshoreSite = GetEntityFactory<EchoshoreSite>().Create(new { Town = _town, Description = "_townname", OperatingCenter = _operatingCenter });
            _hyd1 = GetEntityFactory<Hydrant>().Create(new { HydrantNumber = "HAB-1", Town = _town, OperatingCenter = _operatingCenter });
            _hyd2 = GetEntityFactory<Hydrant>().Create(new { HydrantNumber = "HAB-2", Town = _town, OperatingCenter = _operatingCenter });
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
            i.For<IDateTimeProvider>().Use(new TestDateTimeProvider());
        }

        [TestMethod]
        public void TestProcessCreatesNewLeakAlert()
        {
            var file = SetupFileAndRecords(new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "2018-10-01T08:36:27Z",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z",
                POIStatusId = _fieldInvestigationrecommendedPointOfInterestStatus.Id.ToString(),
                POIStatus = _fieldInvestigationrecommendedPointOfInterestStatus.Description,
                Latitude = LATITUDE.ToString(),
                Longitude = LONGITUDE.ToString(),
                SocketID1 = _hyd1.HydrantNumber,
                SocketID2 = _hyd2.HydrantNumber,
                DistanceFrom1 = DISTANCE1.ToString(),
                DistanceFrom2 = DISTANCE2.ToString(),
                Note = "This is a note",
                SiteName = _echoshoreSite.Description,
            });
            
            MyAssert.CausesIncrease(() => _target.Process(file), () => Repository.GetAll().Count());
            var entity = Repository.GetAll().ToList().Last();

            Assert.AreEqual(2, entity.PersistedCorrelatedNoiseId);
            Assert.AreEqual(_fieldInvestigationrecommendedPointOfInterestStatus.Id, entity.PointOfInterestStatus.Id);
            // This has to convert for UTC/EST which will change dependent upon DST
            Assert.AreEqual(new DateTime(2018,10,1,4,36,27), entity.DatePCNCreated);
            Assert.AreEqual(LATITUDE, entity.Coordinate.Latitude);
            Assert.AreEqual(LONGITUDE, entity.Coordinate.Longitude);
            Assert.AreEqual(_hyd1.Id, entity.Hydrant1.Id);
            Assert.AreEqual(_hyd2.Id, entity.Hydrant2.Id);
            Assert.AreEqual(_hyd1.HydrantNumber, entity.Hydrant1Text);
            Assert.AreEqual(_hyd2.HydrantNumber, entity.Hydrant2Text);
            Assert.AreEqual(DISTANCE1, entity.DistanceFromHydrant1);
            Assert.AreEqual(DISTANCE2, entity.DistanceFromHydrant2);
            Assert.AreEqual("This is a note", entity.Note);
            Assert.AreEqual(_echoshoreSite.Id, entity.EchoshoreSite.Id);
            Assert.AreEqual(new DateTime(2022, 10, 1, 4, 36, 27), entity.FieldInvestigationRecommendedOn);
        }

        [TestMethod]
        public void TestProcessThrowsExceptionIfDateIsInvalid()
        {
            var file = SetupFileAndRecords(new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "9999-99-99T08:36:27Z",
                FieldInvestigationRecommendedOn = "9999-99-98T08:36:27Z",
                POIStatusId = _fieldInvestigationrecommendedPointOfInterestStatus.Id.ToString(),
                POIStatus = _fieldInvestigationrecommendedPointOfInterestStatus.Description,
                Latitude = LATITUDE.ToString(),
                Longitude = LONGITUDE.ToString(),
                SocketID1 = _hyd1.HydrantNumber,
                SocketID2 = _hyd2.HydrantNumber,
                DistanceFrom1 = DISTANCE1.ToString(),
                DistanceFrom2 = DISTANCE2.ToString(),
                Note = "This is a note",
                SiteName = _echoshoreSite.Description,
            });

            MyAssert.Throws<FormatException>(() =>
                _target.Process(file));
        }

        [TestMethod]
        public void TestProcessThrowsExceptionWhenEchoshoreSiteDoesNotExist()
        {
            var file = SetupFileAndRecords(new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "2018-10-01T08:36:27Z",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z",
                POIStatusId = _fieldInvestigationrecommendedPointOfInterestStatus.Id.ToString(),
                POIStatus = _fieldInvestigationrecommendedPointOfInterestStatus.Description,
                Latitude = LATITUDE.ToString(),
                Longitude = LONGITUDE.ToString(),
                SocketID1 = _hyd1.HydrantNumber,
                SocketID2 = _hyd2.HydrantNumber,
                DistanceFrom1 = DISTANCE1.ToString(),
                DistanceFrom2 = DISTANCE2.ToString(),
                Note = "This is a note",
                SiteName = "Site that doesn't exist"
            });

            MyAssert.Throws<KeyNotFoundException>(() =>
                _target.Process(file));
        }

        [TestMethod]
        public void TestProcessThrowsExceptionWhenEchoshoreSiteIsBlank()
        {
            var file = SetupFileAndRecords(new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "2018-10-01T08:36:27Z",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z",
                POIStatusId = _fieldInvestigationrecommendedPointOfInterestStatus.Id.ToString(),
                POIStatus = _fieldInvestigationrecommendedPointOfInterestStatus.Description,
                Latitude = LATITUDE.ToString(),
                Longitude = LONGITUDE.ToString(),
                SocketID1 = _hyd1.HydrantNumber,
                SocketID2 = _hyd2.HydrantNumber,
                DistanceFrom1 = DISTANCE1.ToString(),
                DistanceFrom2 = DISTANCE2.ToString(),
                Note = "This is a note",
                SiteName = ""
            });

            MyAssert.Throws<KeyNotFoundException>(() =>
                _target.Process(file));
        }

        [TestMethod]
        public void TestProcessThrowsExceptionWhenPointOfInterestStatusDoesNotExist()
        {
            var file = SetupFileAndRecords(new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "2018-10-01T08:36:27Z",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z",
                POIStatusId = "1",
                POIStatus = "some description",
                Latitude = LATITUDE.ToString(),
                Longitude = LONGITUDE.ToString(),
                SocketID1 = _hyd1.HydrantNumber,
                SocketID2 = _hyd2.HydrantNumber,
                DistanceFrom1 = DISTANCE1.ToString(),
                DistanceFrom2 = DISTANCE2.ToString(),
                Note = "This is a note",
                SiteName = _echoshoreSite.Description,
            });

            MyAssert.Throws<KeyNotFoundException>(() =>
                _target.Process(file));
        }

        [TestMethod]
        public void TestProcessThrowsExceptionWhenPointOfInterestStatusIsBlank()
        {
            var file = SetupFileAndRecords(new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "2018-10-01T08:36:27Z",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z",
                POIStatusId = "1",
                POIStatus = "",
                Latitude = LATITUDE.ToString(),
                Longitude = LONGITUDE.ToString(),
                SocketID1 = _hyd1.HydrantNumber,
                SocketID2 = _hyd2.HydrantNumber,
                DistanceFrom1 = DISTANCE1.ToString(),
                DistanceFrom2 = DISTANCE2.ToString(),
                Note = "This is a note",
                SiteName = _echoshoreSite.Description,
            });

            MyAssert.Throws<KeyNotFoundException>(() =>
                _target.Process(file));
        }

        [TestMethod]
        public void TestProcessThrowsWhenHydrant1IsBlank()
        {
            var file = SetupFileAndRecords(new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "2018-10-01T08:36:27Z",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z",
                POIStatusId = _fieldInvestigationrecommendedPointOfInterestStatus.Id.ToString(),
                POIStatus = _fieldInvestigationrecommendedPointOfInterestStatus.Description,
                Latitude = LATITUDE.ToString(),
                Longitude = LONGITUDE.ToString(),
                SocketID1 = "",
                SocketID2 = _hyd2.HydrantNumber,
                DistanceFrom1 = DISTANCE1.ToString(),
                DistanceFrom2 = DISTANCE2.ToString(),
                Note = "This is a note",
                SiteName = _echoshoreSite.Description,
            });

            MyAssert.Throws<KeyNotFoundException>(() =>
                _target.Process(file));
        }

        [TestMethod]
        public void TestProcessThrowsWhenHydrant1LengthIsTooLong()
        {
            var file = SetupFileAndRecords(new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "2018-10-01T08:36:27Z",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z",
                POIStatusId = _fieldInvestigationrecommendedPointOfInterestStatus.Id.ToString(),
                POIStatus = _fieldInvestigationrecommendedPointOfInterestStatus.Description,
                Latitude = LATITUDE.ToString(),
                Longitude = LONGITUDE.ToString(),
                SocketID1 = "HYD-12345678901234567890",
                SocketID2 = _hyd2.HydrantNumber,
                DistanceFrom1 = DISTANCE1.ToString(),
                DistanceFrom2 = DISTANCE2.ToString(),
                Note = "This is a note",
                SiteName = _echoshoreSite.Description,
            });

            MyAssert.Throws<ConstraintException>(() =>
                _target.Process(file));
        }

        [TestMethod]
        public void TestProcessThrowsWhenHydrant2LengthIsTooLong()
        {
            var file = SetupFileAndRecords(new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "2018-10-01T08:36:27Z",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z",
                POIStatusId = _fieldInvestigationrecommendedPointOfInterestStatus.Id.ToString(),
                POIStatus = _fieldInvestigationrecommendedPointOfInterestStatus.Description,
                Latitude = LATITUDE.ToString(),
                Longitude = LONGITUDE.ToString(),
                SocketID1 = _hyd1.HydrantNumber,
                SocketID2 = "HYD-123456789012345678",
                DistanceFrom1 = DISTANCE1.ToString(),
                DistanceFrom2 = DISTANCE2.ToString(),
                Note = "This is a note",
                SiteName = _echoshoreSite.Description,
            });

            MyAssert.Throws<ConstraintException>(() =>
                _target.Process(file));
        }

        [TestMethod]
        public void TestProcessThrowsWhenHydrant2IsBlank()
        {
            var file = SetupFileAndRecords(new LeakAlertFileRecord {
                PersistedCorrelatedNoiseId = "2",
                DatePCNCreated = "2018-10-01T08:36:27Z",
                FieldInvestigationRecommendedOn = "2022-10-01T08:36:27Z",
                POIStatusId = _fieldInvestigationrecommendedPointOfInterestStatus.Id.ToString(),
                POIStatus = _fieldInvestigationrecommendedPointOfInterestStatus.Description,
                Latitude = LATITUDE.ToString(),
                Longitude = LONGITUDE.ToString(),
                SocketID1 = _hyd1.HydrantNumber,
                SocketID2 = "",
                DistanceFrom1 = DISTANCE1.ToString(),
                DistanceFrom2 = DISTANCE2.ToString(),
                Note = "This is a note",
                SiteName = _echoshoreSite.Description,
            });

            MyAssert.Throws<KeyNotFoundException>(() =>
                _target.Process(file));
        }
    }
}
