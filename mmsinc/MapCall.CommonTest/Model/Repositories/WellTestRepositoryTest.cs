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
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class WellTestRepositoryTest : 
        MapCallMvcSecuredRepositoryTestBase<WellTest, TestWellTestRepository, User>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container.Inject<IDateTimeProvider>(new DateTimeProvider());
        }

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new { IsAdmin = true });
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestLinqDoesNotReturnWellTestsFromOtherOperatingCentersForUser()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Production });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.ProductionWorkManagement });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new {
                IsAdmin = false, 
                DefaultOperatingCenter = operatingCenter1
            });

            GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = operatingCenter1,
                User = user
            });

            Session.Save(user);

            var facility1 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter1 });
            var facility2 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter2 });

            var equipment1 = GetEntityFactory<Equipment>().Create(new { Facility = facility1 });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { Facility = facility2 });

            var wellTest1 = GetEntityFactory<WellTest>().Create(new { Equipment = equipment1 });
            var wellTest2 = GetEntityFactory<WellTest>().Create(new { Equipment = equipment2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestWellTestRepository>();

            var wellTests = Repository.GetAll().ToArray();

            CollectionAssert.Contains(wellTests, wellTest1);
            CollectionAssert.DoesNotContain(wellTests, wellTest2);
        }

        [TestMethod]
        public void TestLinqReturnsAllTheWellTestsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Production });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.ProductionWorkManagement });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false });
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var facility1 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter1 });
            var facility2 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter2 });

            var equipment1 = GetEntityFactory<Equipment>().Create(new { Facility = facility1 });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { Facility = facility2 });

            var wellTest1 = GetEntityFactory<WellTest>().Create(new { Equipment = equipment1 });
            var wellTest2 = GetEntityFactory<WellTest>().Create(new { Equipment = equipment2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestWellTestRepository>();

            var wellTests = Repository.GetAll().ToArray();

            CollectionAssert.Contains(wellTests, wellTest1);
            CollectionAssert.Contains(wellTests, wellTest2);
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnWellTestsFromOtherOperatingCentersForUser()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Production });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.ProductionWorkManagement });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { 
                IsAdmin = false, 
                DefaultOperatingCenter = operatingCenter1
            });

            GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = operatingCenter1,
                User = user
            });

            Session.Save(user);

            var facility1 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter1 });
            var facility2 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter2 });
            var equipment1 = GetEntityFactory<Equipment>().Create(new { Facility = facility1 });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { Facility = facility2 });

            var wellTest1 = GetEntityFactory<WellTest>().Create(new { Equipment = equipment1 });
            var wellTest2 = GetEntityFactory<WellTest>().Create(new { Equipment = equipment2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestWellTestRepository>();

            var wellTests = Repository.iCanHasCriteria().List();

            CollectionAssert.Contains(wellTests, wellTest1);
            CollectionAssert.DoesNotContain(wellTests, wellTest2);
        }

        [TestMethod]
        public void TestCriteriaReturnsAllTheWellTestsIfUserHasMatchingRoleWithWildCardOperatingCenter()
        {
            var operatingCenter1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Production });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.ProductionWorkManagement });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false });
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var facility1 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter1 });
            var facility2 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter2 });
            var equipment1 = GetEntityFactory<Equipment>().Create(new { Facility = facility1 });
            var equipment2 = GetEntityFactory<Equipment>().Create(new { Facility = facility2 });

            var wellTest1 = GetEntityFactory<WellTest>().Create(new { Equipment = equipment1 });
            var wellTest2 = GetEntityFactory<WellTest>().Create(new { Equipment = equipment2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestWellTestRepository>();

            var wellTests = Repository.iCanHasCriteria().List();

            CollectionAssert.Contains(wellTests, wellTest1);
            CollectionAssert.Contains(wellTests, wellTest2);
        }

        #endregion
    }

    public class TestWellTestRepository : WellTestRepository
    {
        public ICriteria iCanHasCriteria()
        {
            return Criteria;
        }

        public TestWellTestRepository(
            ISession session, 
            IContainer container,
            IAuthenticationService<User> authenticationService, 
            IRepository<AggregateRole> roleRepo
        ) : base(session, container, authenticationService, roleRepo) { }
    }
}
