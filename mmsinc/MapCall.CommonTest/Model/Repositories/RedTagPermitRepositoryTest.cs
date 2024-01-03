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
    public class RedTagPermitRepositoryTest : 
        MapCallMvcSecuredRepositoryTestBase<RedTagPermit, TestRedTagPermitRepository, User>
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

        #region BaseRepository

        [TestMethod]
        public void TestLinqDoesNotReturnRedTagPermitsFromOtherOperatingCentersForUser()
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

            var redTagPermit1 = GetEntityFactory<RedTagPermit>().Create(new { Equipment = equipment1 });
            var redTagPermit2 = GetEntityFactory<RedTagPermit>().Create(new { Equipment = equipment2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestRedTagPermitRepository>();

            var redTagPermits = Repository.GetAll().ToArray();

            CollectionAssert.Contains(redTagPermits, redTagPermit1);
            CollectionAssert.DoesNotContain(redTagPermits, redTagPermit2);
        }

        [TestMethod]
        public void TestLinqReturnsAllTheRedTagPermitsIfUserHasMatchingRoleWithWildcardOperatingCenter()
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

            var redTagPermit1 = GetEntityFactory<RedTagPermit>().Create(new { Equipment = equipment1 });
            var redTagPermit2 = GetEntityFactory<RedTagPermit>().Create(new { Equipment = equipment2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestRedTagPermitRepository>();

            var redTagPermits = Repository.GetAll().ToArray();

            CollectionAssert.Contains(redTagPermits, redTagPermit1);
            CollectionAssert.Contains(redTagPermits, redTagPermit2);
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnRedTagPermitsFromOtherOperatingCentersForUser()
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

            var redTagPermit1 = GetEntityFactory<RedTagPermit>().Create(new { Equipment = equipment1 });
            var redTagPermit2 = GetEntityFactory<RedTagPermit>().Create(new { Equipment = equipment2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestRedTagPermitRepository>();

            var redTagPermits = Repository.iCanHasCriteria().List();

            CollectionAssert.Contains(redTagPermits, redTagPermit1);
            CollectionAssert.DoesNotContain(redTagPermits, redTagPermit2);
        }

        [TestMethod]
        public void TestCriteriaReturnsAllTheRedTagPermitsIfUserHasMatchingRoleWithWildCardOperatingCenter()
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

            var redTagPermit1 = GetEntityFactory<RedTagPermit>().Create(new { Equipment = equipment1 });
            var redTagPermit2 = GetEntityFactory<RedTagPermit>().Create(new { Equipment = equipment2 });

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestRedTagPermitRepository>();

            var redTagPermits = Repository.iCanHasCriteria().List();

            CollectionAssert.Contains(redTagPermits, redTagPermit1);
            CollectionAssert.Contains(redTagPermits, redTagPermit2);
        }

        #endregion

        #endregion
    }

    public class TestRedTagPermitRepository : RedTagPermitRepository
    {
        public ICriteria iCanHasCriteria()
        {
            return Criteria;
        }

        public TestRedTagPermitRepository(
            ISession session, 
            IContainer container,
            IAuthenticationService<User> authenticationService, 
            IRepository<AggregateRole> roleRepository) : base(session, container, authenticationService, roleRepository) { }
    }
}