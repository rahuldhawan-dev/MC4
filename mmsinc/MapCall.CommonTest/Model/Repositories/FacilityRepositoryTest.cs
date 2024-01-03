using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class FacilityRepositoryTest : MapCallMvcSecuredRepositoryTestBase<Facility, FacilityRepository, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionFacilities;

        #endregion

        #region Private Members

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
            e.For<ISensorMeasurementTypeRepository>().Use<SensorMeasurementTypeRepository>();
            e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Tests

        #region Linq

        [TestMethod]
        public void TestLinqDoesNotReturnFacilitiesFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });

            Session.Save(user);

            var facility1 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr1});
            var facility2 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<FacilityRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(facility1));
            Assert.IsFalse(result.Contains(facility2));
        }

        [TestMethod]
        public void TestLinqReturnsAllFacilitiesIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var facility1 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr1});
            var facility2 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<FacilityRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(facility1));
            Assert.IsTrue(result.Contains(facility2));
        }

        #endregion

        #region Criteria

        [TestMethod]
        public void TestCriteriaDoesNotReturnFacilitiesFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });

            Session.Save(user);

            var facility1 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr1});
            var facility2 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<FacilityRepository>();
            var model = new EmptySearchSet<Facility>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(facility1));
            Assert.IsFalse(result.Contains(facility2));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllFacilitiesIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var facility1 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr1});
            var facility2 = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<FacilityRepository>();
            var model = new EmptySearchSet<Facility>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(facility1));
            Assert.IsTrue(result.Contains(facility2));
        }

        #endregion

        [TestMethod]
        public void TestGetByTownIdReturnsFacilitiesWithTown()
        {
            var town = GetFactory<TownFactory>().Create();
            var bow = GetFactory<FacilityFactory>().Create(new {Town = town});
            var invalid = GetFactory<FacilityFactory>().Create();

            var result = Repository.GetByTownId(town.Id).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(bow));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestGetByOperatingCenterIdReturnsFacilitiesWithOperatingCenterId()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var bow = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opc});
            var invalid = GetFactory<FacilityFactory>().Create();

            var result = Repository.GetByOperatingCenterId(opc.Id).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(bow));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestGetMaxEquipmentNumberByEquipmentPurposeIdReturnsMaximumNumber()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var facility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opc});
            var equipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create();

            var result = Repository.GetNextEquipmentNumberForFacilityByEquipmentPurposeId(facility.Id, equipmentPurpose.Id);

            Assert.AreEqual(1, result);

            var equipment = GetFactory<EquipmentFactory>().Create(new {
                Facility = facility, EquipmentPurpose = equipmentPurpose, Number = 3
            });
            Session.Flush();
            Session.Clear();
            facility = _container.GetInstance<IFacilityRepository>().Find(facility.Id);

            result = Repository.GetNextEquipmentNumberForFacilityByEquipmentPurposeId(facility.Id, equipmentPurpose.Id);

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void TestGetMaxEquipmentNumberByEquipmentPurposeIdZeroReturnsMaximumNumber()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var facility = GetFactory<FacilityFactory>().Create(new {OperatingCenter = opc});
            var equipmentPurpose = GetFactory<EquipmentPurposeFactory>().Create();

            var result = Repository.GetNextEquipmentNumberForFacilityByEquipmentPurposeId(facility.Id, 0);

            Assert.AreEqual(1, result);

            var equipment = GetFactory<EquipmentFactory>().Create(new {
                Facility = facility,
                Number = 3
            });
            equipment.EquipmentPurpose = null;

            Session.Flush();
            Session.Clear();
            facility = _container.GetInstance<IFacilityRepository>().Find(facility.Id);

            result = Repository.GetNextEquipmentNumberForFacilityByEquipmentPurposeId(facility.Id, 0);

            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void TestGetReadingsReturnsListOfDatesAndTheirSummedScaledDataValuesForAllTheKilowattSensorsInAFacility()
        {
            var kiloWatt = GetFactory<KilowattSensorMeasurementTypeFactory>().Create();
            var facility = GetFactory<FacilityFactory>().Create();
            var equipmentOne = GetFactory<EquipmentFactory>().Create(new {Facility = facility});
            var equipmentTwo = GetFactory<EquipmentFactory>().Create(new {Facility = facility});

            var sensorOne = GetFactory<SensorFactory>().Create(new {Name = "Sensor One", MeasurementType = kiloWatt});
            var sensorTwo = GetFactory<SensorFactory>().Create(new {Name = "Sensor Two", MeasurementType = kiloWatt});
            // Make some readings
            var startDate = new DateTime(2014, 1, 1, 0, 0, 0);
            var endDate = startDate.AddDays(1);

            // Day one
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorOne, DateTimeStamp = startDate, ScaledData = 1001});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddHours(1), ScaledData = 2002});
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorTwo, DateTimeStamp = startDate, ScaledData = 5005});
            GetFactory<ReadingFactory>().Create(new {Sensor = sensorTwo, DateTimeStamp = startDate, ScaledData = 6006});

            // Day two
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(1), ScaledData = 3003});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(1), ScaledData = 4004});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorTwo, DateTimeStamp = startDate.AddDays(1), ScaledData = 7007});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorTwo, DateTimeStamp = startDate.AddDays(1), ScaledData = 8008});

            // Bad days
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(-1), ScaledData = 1001});
            GetFactory<ReadingFactory>().Create(new
                {Sensor = sensorOne, DateTimeStamp = startDate.AddDays(2), ScaledData = 1001});

            // Make sure sensor is associated with equipment.
            GetFactory<EquipmentSensorFactory>().Create(new {Equipment = equipmentOne, Sensor = sensorOne});
            GetFactory<EquipmentSensorFactory>().Create(new {Equipment = equipmentTwo, Sensor = sensorTwo});

            var result = Repository.GetReadings(facility.Id, ReadingGroupType.Daily, startDate, endDate);
            Assert.AreEqual(3503.5d, result[startDate]);
            Assert.AreEqual(5505.5d, result[startDate.AddDays(1)]);
            Assert.IsFalse(result.ContainsKey(startDate.AddDays(-1)));
            Assert.IsFalse(result.ContainsKey(startDate.AddDays(2)));
        }

        #endregion
    }
}
