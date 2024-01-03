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
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class EmergencyEquipmentRepositoryTest : MapCallMvcSecuredRepositoryTestBase<EmergencyEquipment,
        EmergencyEquipmentRepositoryTest.TestEmergencyEquipmentRepository, User>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            BaseTestInitialize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            BaseTestCleanup();
        }

        #endregion

        [TestMethod]
        public void TestLinqDoesNotReturnEmergencyEquipmentForOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.HumanResources});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ProductionFacilities});
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
            var valid = GetFactory<EmergencyEquipmentFactory>().Create(new {OperatingCenter = opCntr1});
            var invalid = GetFactory<EmergencyEquipmentFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestEmergencyEquipmentRepository>();
            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestLinqReturnsAllTheEmergencyEquipmentIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.HumanResources});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ProductionFacilities});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Session.Save(user);
            Session.Flush();

            var valid1 = GetFactory<EmergencyEquipmentFactory>().Create(new {OperatingCenter = opCntr1});
            var valid2 = GetFactory<EmergencyEquipmentFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestEmergencyEquipmentRepository>();
            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valid1));
            Assert.IsTrue(result.Contains(valid2));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnEmergencyEquipmentForOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.HumanResources});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ProductionFacilities});
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
            var valid = GetFactory<EmergencyEquipmentFactory>().Create(new {OperatingCenter = opCntr1});
            var invalid = GetFactory<EmergencyEquipmentFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestEmergencyEquipmentRepository>();
            var result = Repository.GimmeCriteriaOrGiveMeDeath().List<EmergencyEquipment>();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllTheEmergencyEquipmentIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.HumanResources});
            var module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.ProductionFacilities});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Session.Save(user);
            Session.Flush();

            var valid1 = GetFactory<EmergencyEquipmentFactory>().Create(new {OperatingCenter = opCntr1});
            var valid2 = GetFactory<EmergencyEquipmentFactory>().Create(new {OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object)
                                   .GetInstance<TestEmergencyEquipmentRepository>();
            var result = Repository.GimmeCriteriaOrGiveMeDeath().List<EmergencyEquipment>();

            Assert.IsTrue(result.Contains(valid1));
            Assert.IsTrue(result.Contains(valid2));
        }

        public class TestEmergencyEquipmentRepository : EmergencyEquipmentRepository
        {
            public ICriteria GimmeCriteriaOrGiveMeDeath()
            {
                return Criteria;
            }

            public TestEmergencyEquipmentRepository(ISession session, IContainer container,
                IAuthenticationService<User> authenticationService, IRepository<Role> roleRepo) : base(session,
                container, authenticationService, roleRepo) { }
        }
    }
}
