using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        GasMonitorRepositoryTest : MapCallMvcSecuredRepositoryTestBase<GasMonitor, TestGasMonitorRepository, User>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container.Inject<IDateTimeProvider>(new DateTimeProvider());
        }

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Tests

        #region BaseRepository

        [TestMethod]
        public void TestLinqDoesNotReturnGasMonitorsFromOtherOperatingCentersForUser()
        {
            var opcPrime = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.OperationsLockoutForms});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opcPrime});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opcPrime,
                User = user
            });

            Session.Save(user);

            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcPrime});
            var facility2 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcSecondary});
            var equipment = GetEntityFactory<Equipment>().Create(new {Facility = facility});
            var equipment2 = GetEntityFactory<Equipment>().Create(new {Facility = facility2});
            var validGasMonitor = GetEntityFactory<GasMonitor>().Create(new {Equipment = equipment});
            var notValidGasMonitor = GetEntityFactory<GasMonitor>().Create(new {Equipment = equipment2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestGasMonitorRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(validGasMonitor));
            Assert.IsFalse(result.Contains(notValidGasMonitor));
        }

        [TestMethod]
        public void TestLinqReturnsAllTheGasMonitorsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opcPrime = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.OperationsLockoutForms});
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

            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcPrime});
            var facility2 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcSecondary});
            var equipment = GetEntityFactory<Equipment>().Create(new {Facility = facility});
            var equipment2 = GetEntityFactory<Equipment>().Create(new {Facility = facility2});
            var validGasMonitor = GetEntityFactory<GasMonitor>().Create(new {Equipment = equipment});
            var notValidGasMonitor = GetEntityFactory<GasMonitor>().Create(new {Equipment = equipment2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestGasMonitorRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(validGasMonitor));
            Assert.IsTrue(result.Contains(notValidGasMonitor));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnGasMonitorsFromOtherOperatingCentersForUser()
        {
            var opcPrime = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.OperationsLockoutForms});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opcPrime});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opcPrime,
                User = user
            });

            Session.Save(user);

            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcPrime});
            var facility2 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcSecondary});
            var equipment = GetEntityFactory<Equipment>().Create(new {Facility = facility});
            var equipment2 = GetEntityFactory<Equipment>().Create(new {Facility = facility2});
            var validGasMonitor = GetEntityFactory<GasMonitor>().Create(new {Equipment = equipment});
            var notValidGasMonitor = GetEntityFactory<GasMonitor>().Create(new {Equipment = equipment2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestGasMonitorRepository>();

            var result = Repository.iCanHasCriteria().List<GasMonitor>();

            Assert.IsTrue(result.Contains(validGasMonitor));
            Assert.IsFalse(result.Contains(notValidGasMonitor));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllTheGasMonitorsIfUserHasMatchingRoleWithWildCardOperatingCenter()
        {
            var opcPrime = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.OperationsLockoutForms});
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

            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcPrime});
            var facility2 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcSecondary});
            var equipment = GetEntityFactory<Equipment>().Create(new {Facility = facility});
            var equipment2 = GetEntityFactory<Equipment>().Create(new {Facility = facility2});
            var validGasMonitor = GetEntityFactory<GasMonitor>().Create(new {Equipment = equipment});
            var notValidGasMonitor = GetEntityFactory<GasMonitor>().Create(new {Equipment = equipment2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestGasMonitorRepository>();

            var result = Repository.iCanHasCriteria().List<GasMonitor>();

            Assert.IsTrue(result.Contains(validGasMonitor));
            Assert.IsTrue(result.Contains(notValidGasMonitor));
        }

        #endregion

        #region RepositoryExtensions

        [TestMethod]
        public void TestGetGasMonitorsDueCalibrationDueIn7Days()
        {
            var calibrationFrequencyDays = 30;
            var opcPrime = GetFactory<UniqueOperatingCenterFactory>().Create();
            var facility = GetEntityFactory<Facility>().Create(new {OperatingCenter = opcPrime});
            var equipment = GetEntityFactory<Equipment>().Create(new {Facility = facility});
            var gasMonitor = GetFactory<GasMonitorFactory>().Create(new
                {CalibrationFrequencyDays = calibrationFrequencyDays, Equipment = equipment});
            var now = DateTime.Now;
            // get 7 days from now
            var sevenDaysFromNow = now.AddDays(7);
            // create a passing calibration that was performed at the appropriate time
            var calibration1 = GetEntityFactory<GasMonitorCalibration>().Create(new {
                GasMonitor = gasMonitor,
                CalibrationPassed = true,
                CalibrationDate = sevenDaysFromNow.AddDays(-calibrationFrequencyDays),
                CreatedBy = GetFactory<UserFactory>().Create()
            });
            // these rest of these are scenarios for gas monitors that should never return in the result
            var gasMonitorFutureCalibration = GetFactory<GasMonitorFactory>().Create(new
                {CalibrationFrequencyDays = calibrationFrequencyDays, Equipment = equipment});
            var calibration2 = GetEntityFactory<GasMonitorCalibration>().Create(new {
                GasMonitor = gasMonitorFutureCalibration,
                CalibrationPassed = true,
                CalibrationDate = sevenDaysFromNow.AddDays(-calibrationFrequencyDays + 1), // make this a day ahead
                CreatedBy = GetFactory<UserFactory>().Create()
            });
            var gasMonitorPastCalibration = GetFactory<GasMonitorFactory>().Create(new
                {CalibrationFrequencyDays = calibrationFrequencyDays, Equipment = equipment});
            var calibration3 = GetEntityFactory<GasMonitorCalibration>().Create(new {
                GasMonitor = gasMonitorPastCalibration,
                CalibrationPassed = true,
                CalibrationDate = sevenDaysFromNow.AddDays(-calibrationFrequencyDays - 1), // make this a day behind
                CreatedBy = GetFactory<UserFactory>().Create()
            });
            var gasMonitorNoCalibrations = GetFactory<GasMonitorFactory>().Create(new
                {CalibrationFrequencyDays = calibrationFrequencyDays, Equipment = equipment});
            var gasMonitorCalibrationNotPassingOnDate = GetFactory<GasMonitorFactory>().Create(new
                {CalibrationFrequencyDays = calibrationFrequencyDays, Equipment = equipment});
            ;
            var calibrationNoPassing = GetEntityFactory<GasMonitorCalibration>().Create(new {
                GasMonitor = gasMonitor,
                CalibrationPassed = false,
                CalibrationDate = sevenDaysFromNow.AddDays(-calibrationFrequencyDays),
                CreatedBy = GetFactory<UserFactory>().Create()
            });

            Session.Clear();
            Session.Flush();

            var dueCalibrationInSevenDays = _container.GetInstance<IRepository<GasMonitor>>()
                                                      .GetWithCalibrationDueSevenDaysFrom(now);

            Assert.AreEqual(1, Queryable.Count(dueCalibrationInSevenDays));
        }

        #endregion

        #endregion
    }

    public class TestGasMonitorRepository : GasMonitorRepository
    {
        public ICriteria iCanHasCriteria()
        {
            return Criteria;
        }

        public TestGasMonitorRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }
}
