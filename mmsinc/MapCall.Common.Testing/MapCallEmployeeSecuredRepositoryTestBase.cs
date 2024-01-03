using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.Common.Testing
{
    [TestClass]
    public class
        MapCallEmployeeSecuredRepositoryTestBase<TEntity, TRepository> : MapCallMvcSecuredRepositoryTestBase<TEntity,
            TRepository, User>
        where TEntity : class, IThingWithEmployee, new()
        where TRepository : MapCallEmployeeSecuredRepositoryBase<TEntity>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        #endregion

        [TestMethod]
        public void TestLinqDoesNotReturnDataForOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = Repository.Role});
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
            var employee1 = GetEntityFactory<Employee>().Create(new {OperatingCenter = opCntr1});
            var employee2 = GetEntityFactory<Employee>().Create(new {OperatingCenter = opCntr2});
            var valid = GetEntityFactory<TEntity>().Create(new {Employee = employee1});
            var invalid = GetEntityFactory<TEntity>().Create(new {Employee = employee2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<TRepository>();
            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestLinqReturnsDataForUserWithMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = Repository.Role});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });
            Session.Save(user);
            var employee1 = GetEntityFactory<Employee>().Create(new {OperatingCenter = opCntr1});
            var employee2 = GetEntityFactory<Employee>().Create(new {OperatingCenter = opCntr2});
            var valid = GetEntityFactory<TEntity>().Create(new {Employee = employee1});
            var invalid = GetEntityFactory<TEntity>().Create(new {Employee = employee2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<TRepository>();
            var result = Repository.GetAll().ToArray();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsTrue(result.Contains(invalid));
        }

        [TestMethod]
        public void TestCriteriaDoesNotReturnDataForOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = Repository.Role});
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
            var employee1 = GetEntityFactory<Employee>().Create(new {OperatingCenter = opCntr1});
            var employee2 = GetEntityFactory<Employee>().Create(new {OperatingCenter = opCntr2});
            var valid = GetEntityFactory<TEntity>().Create(new {Employee = employee1});
            var invalid = GetEntityFactory<TEntity>().Create(new {Employee = employee2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<TRepository>();
            var result = Repository.Criteria.List<TEntity>();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestCriteriaReturnsDataForUserWithMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opCntr2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            var module = GetFactory<ModuleFactory>().Create(new {Id = Repository.Role});
            var action = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            var user = GetFactory<UserFactory>().Create(new {IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });
            Session.Save(user);
            var employee1 = GetEntityFactory<Employee>().Create(new {OperatingCenter = opCntr1});
            var employee2 = GetEntityFactory<Employee>().Create(new {OperatingCenter = opCntr2});
            var valid = GetEntityFactory<TEntity>().Create(new {Employee = employee1});
            var invalid = GetEntityFactory<TEntity>().Create(new {Employee = employee2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<TRepository>();
            var result = Repository.Criteria.List<TEntity>();

            Assert.IsTrue(result.Contains(valid));
            Assert.IsTrue(result.Contains(invalid));
        }
    }
}
