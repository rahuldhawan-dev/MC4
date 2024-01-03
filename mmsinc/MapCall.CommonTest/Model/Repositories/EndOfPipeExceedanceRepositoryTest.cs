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
using Moq;
using NHibernate;
using StructureMap;
using System.Linq;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class EndOfPipeExceedanceRepositoryTest : MapCallMvcSecuredRepositoryTestBase<EndOfPipeExceedance,
        TestEndOfPipeExceedanceRepository, User>
    {
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object));
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestLinqDoesNotReturnEndOfPipeExceedancesFromOtherOperatingCentersForUser()
        {
            var opcPrime = GetFactory<OperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<OperatingCenterFactory>().Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Environmental});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.EnvironmentalGeneral});
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

            var validEndOfPipeExceedance = GetFactory<EndOfPipeExceedanceFactory>().Create(new {OperatingCenter = opcPrime});
            var notValidEndOfPipeExceedance = GetFactory<EndOfPipeExceedanceFactory>().Create(new {OperatingCenter = opcSecondary});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestEndOfPipeExceedanceRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(validEndOfPipeExceedance));
            Assert.IsFalse(result.Contains(notValidEndOfPipeExceedance));
        }

        [TestMethod]
        public void TestLinqReturnsAllEndOfPipeExceedancesIfUserHasMatchingRoleWithWildCardOperatingCenter()
        {
            var opcPrime = GetFactory<OperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Environmental });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EnvironmentalGeneral });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false });
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new
            {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var validEndOfPipeExceedance = GetFactory<EndOfPipeExceedanceFactory>().Create(new {OperatingCenter = opcPrime});
            var notValidEndOfPipeExceedance = GetFactory<EndOfPipeExceedanceFactory>().Create(new {OperatingCenter = opcSecondary});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestEndOfPipeExceedanceRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(validEndOfPipeExceedance));
            Assert.IsTrue(result.Contains(notValidEndOfPipeExceedance));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnEndOfPipeExceedancesFromOtherOperatingCentersForUser()
        {
            var opcPrime = GetFactory<OperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Environmental });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EnvironmentalGeneral });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opcPrime });
            var role = GetFactory<RoleFactory>().Create(new
            {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opcPrime,
                User = user
            });

            Session.Save(user);

            var validEndOfPipeExceedance = GetFactory<EndOfPipeExceedanceFactory>().Create(new {OperatingCenter = opcPrime});
            var notValidEndOfPipeExceedance = GetFactory<EndOfPipeExceedanceFactory>().Create(new {OperatingCenter = opcSecondary});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestEndOfPipeExceedanceRepository>();

            var result = Repository.iCanHasCriteria().List<EndOfPipeExceedance>();

            Assert.IsTrue(result.Contains(validEndOfPipeExceedance));
            Assert.IsFalse(result.Contains(notValidEndOfPipeExceedance));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllTheEndOfPipeExceedancesIfUserHasMatchingRoleWithWildCardOperatingCenter()
        {
            var opcPrime = GetFactory<OperatingCenterFactory>().Create();
            var opcSecondary = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood" });
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Environmental });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EnvironmentalGeneral });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false });
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new
            {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var validEndOfPipeExceedance = GetFactory<EndOfPipeExceedanceFactory>().Create(new {OperatingCenter = opcPrime});
            var notValidEndOfPipeExceedance = GetFactory<EndOfPipeExceedanceFactory>().Create(new {OperatingCenter = opcSecondary});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestEndOfPipeExceedanceRepository>();

            var result = Repository.iCanHasCriteria().List<EndOfPipeExceedance>();

            Assert.IsTrue(result.Contains(validEndOfPipeExceedance));
            Assert.IsTrue(result.Contains(notValidEndOfPipeExceedance));
        }

        #endregion
    }

    public class TestEndOfPipeExceedanceRepository : EndOfPipeExceedanceRepository
    {
        public ICriteria iCanHasCriteria()
        {
            return Criteria;
        }

        public TestEndOfPipeExceedanceRepository(ISession session, IContainer container, IAuthenticationService<User> authenticationService, 
            IRepository<AggregateRole> roleRepo) : base(session, container, authenticationService, roleRepo) { }
    }
}

