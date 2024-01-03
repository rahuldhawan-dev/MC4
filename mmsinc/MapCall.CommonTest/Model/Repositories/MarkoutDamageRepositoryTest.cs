using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        MarkoutDamageRepositoryTest : MapCallMvcSecuredRepositoryTestBase<MarkoutDamage, MarkoutDamageRepository, User>
    {
        #region Tests

        [TestMethod]
        public void TestLinqDoesNotReturnRecordsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.FieldServicesWorkManagement});
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

            var valid = GetFactory<MarkoutDamageFactory>().Create(new {OperatingCenter = opCntr1});
            var invalid = GetFactory<MarkoutDamageFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<MarkoutDamageRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestLinqReturnsAllTheRecordsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.FieldServicesWorkManagement});
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

            var valid = GetFactory<MarkoutDamageFactory>().Create(new {OperatingCenter = opCntr1});
            var otherValid = GetFactory<MarkoutDamageFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<MarkoutDamageRepository>();

            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsTrue(result.Contains(otherValid));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnRecordsFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.FieldServicesWorkManagement});
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

            var valid = GetFactory<MarkoutDamageFactory>().Create(new {OperatingCenter = opCntr1});
            var invalid = GetFactory<MarkoutDamageFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<MarkoutDamageRepository>();
            var model = new EmptySearchSet<MarkoutDamage>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllTheRecordsIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.FieldServices});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.FieldServicesWorkManagement});
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

            var valid = GetFactory<MarkoutDamageFactory>().Create(new {OperatingCenter = opCntr1});
            var otherValid = GetFactory<MarkoutDamageFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<MarkoutDamageRepository>();

            var model = new EmptySearchSet<MarkoutDamage>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(valid));
            Assert.IsTrue(result.Contains(otherValid));
        }

        #endregion
    }
}
