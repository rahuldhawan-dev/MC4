using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ServiceLineProtectionInvestigationRepositoryTest : MapCallMvcSecuredRepositoryTestBase<
        ServiceLineProtectionInvestigation, ServiceLineProtectionInvestigationRepository, User>
    {
        #region Constants

        public const RoleModules ROLE_MODULE = RoleModules.ServiceLineProtection;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {IsAdmin = true});
        }

        #endregion

        #region Linq

        [TestMethod]
        public void TestLinqDoesNotReturnServiceLineProtectionInvestigationsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
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

            var slpi1 = GetFactory<ServiceLineProtectionInvestigationFactory>().Create(new {OperatingCenter = opCntr1});
            var slpi2 = GetFactory<ServiceLineProtectionInvestigationFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<ServiceLineProtectionInvestigationRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(slpi1));
            Assert.IsFalse(result.Contains(slpi2));
        }

        [TestMethod]
        public void
            TestLinqReturnsAllServiceLineProtectionInvestigationsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
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

            var slpi1 = GetFactory<ServiceLineProtectionInvestigationFactory>().Create(new {OperatingCenter = opCntr1});
            var slpi2 = GetFactory<ServiceLineProtectionInvestigationFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<ServiceLineProtectionInvestigationRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(slpi1));
            Assert.IsTrue(result.Contains(slpi2));
        }

        #endregion

        #region Criteria

        [TestMethod]
        public void TestCriteriaDoesNotReturnServiceLineProtectionInvestigationsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
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

            var slpi1 = GetFactory<ServiceLineProtectionInvestigationFactory>().Create(new {OperatingCenter = opCntr1});
            var slpi2 = GetFactory<ServiceLineProtectionInvestigationFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<ServiceLineProtectionInvestigationRepository>();
            var model = new EmptySearchSet<ServiceLineProtectionInvestigation>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(slpi1));
            Assert.IsFalse(result.Contains(slpi2));
        }

        [TestMethod]
        public void
            TestCriteriaReturnsAllServiceLineProtectionInvestigationsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = ROLE_MODULE});
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

            var slpi1 = GetFactory<ServiceLineProtectionInvestigationFactory>().Create(new {OperatingCenter = opCntr1});
            var slpi2 = GetFactory<ServiceLineProtectionInvestigationFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.GetInstance<ServiceLineProtectionInvestigationRepository>();
            var model = new EmptySearchSet<ServiceLineProtectionInvestigation>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(slpi1));
            Assert.IsTrue(result.Contains(slpi2));
        }

        #endregion
    }
}
