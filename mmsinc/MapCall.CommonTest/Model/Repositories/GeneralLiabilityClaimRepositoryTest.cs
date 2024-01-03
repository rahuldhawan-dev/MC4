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
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class GeneralLiabilityClaimRepositoryTest : MapCallMvcSecuredRepositoryTestBase<GeneralLiabilityClaim,
        GeneralLiabilityClaimRepositoryTest.TestGeneralLiabilityClaimRepository, User>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        protected override User CreateUser()
        {
            return GetEntityFactory<User>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestLinqDoesNotReturnGeneralLiabilityClaimsFormOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.OperationsHealthAndSafety});
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

            var validClaim = GetFactory<GeneralLiabilityClaimFactory>().Create(new {OperatingCenter = opCntr1});
            var invalidClaim = GetFactory<GeneralLiabilityClaimFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestGeneralLiabilityClaimRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(validClaim));
            Assert.IsFalse(result.Contains(invalidClaim));
        }

        [TestMethod]
        public void TestLinqReturnsAllTheClaimsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.OperationsHealthAndSafety});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var validClaim = GetFactory<GeneralLiabilityClaimFactory>().Create(new {OperatingCenter = opCntr1});
            var otherValidClaim = GetFactory<GeneralLiabilityClaimFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<TestGeneralLiabilityClaimRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(validClaim));
            Assert.IsTrue(result.Contains(otherValidClaim));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnIncidentsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.OperationsHealthAndSafety});
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

            var validClaim = GetFactory<GeneralLiabilityClaimFactory>().Create(new {OperatingCenter = opCntr1});
            var invalidClaim = GetFactory<GeneralLiabilityClaimFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestGeneralLiabilityClaimRepository>();

            var result = Repository.GimmeCriteria().List<GeneralLiabilityClaim>();

            Assert.IsTrue(result.Contains(validClaim));
            Assert.IsFalse(result.Contains(invalidClaim));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllClaimsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.OperationsHealthAndSafety});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var validClaim = GetFactory<GeneralLiabilityClaimFactory>().Create(new {OperatingCenter = opCntr1});
            var otherValidClaim = GetFactory<GeneralLiabilityClaimFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<TestGeneralLiabilityClaimRepository>();

            var result = Repository.GimmeCriteria().List<GeneralLiabilityClaim>();

            Assert.IsTrue(result.Contains(validClaim));
            Assert.IsTrue(result.Contains(otherValidClaim));
        }

        #endregion

        #region TestActionItemIsDeletedWithGeneralLiabilityClaim

        [TestMethod]
        public void TestActionItemIsDeletedWithGeneralLiabilityClaim()
        {
            this.TestActionItemIsDeletedWithThing("GeneralLiabilityClaims");
        }

        #endregion

        public class TestGeneralLiabilityClaimRepository : GeneralLiabilityClaimRepository
        {
            public ICriteria GimmeCriteria()
            {
                return Criteria;
            }

            public TestGeneralLiabilityClaimRepository(ISession session, IContainer container,
                IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session,
                container, authenticationService, roleRepo) { }
        }
    }
}
