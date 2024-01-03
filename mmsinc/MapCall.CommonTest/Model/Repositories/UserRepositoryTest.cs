using System;
using System.Linq;
using System.Runtime.InteropServices;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Model.ViewModels.Users;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class UserRepositoryTest : InMemoryDatabaseTest<User, UserRepository>
    {
        #region Fields

        private User _goodUser;
        private User _badUser;
        private OperatingCenter _goodOpCenter;
        private Role _goodRole;
        private Role _badRole;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _goodUser = GetFactory<UserFactory>().Create();
            _badUser = GetFactory<UserFactory>().Create();
            _goodOpCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            _goodRole = GetFactory<RoleFactory>().Create(new {User = _goodUser});
            _badRole = GetFactory<RoleFactory>().Create(new {User = _badUser});
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<DateTimeProvider>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestTryGetUserByUserName()
        {
            Assert.AreSame(_goodUser, Repository.TryGetUserByUserName(_goodUser.UserName));
            Assert.IsNull(Repository.TryGetUserByUserName("blargh"),
                "TryGetUserByUserName should not throw an error when the user isn't found.");
        }

        [TestMethod]
        public void TestSearchUsersCanSearchByEmployeeId()
        {
            var employee = GetFactory<EmployeeFactory>().Create(new { EmployeeId = "545454" });
            var userWithEmployee = GetFactory<UserFactory>().Create(new { Employee = employee });
            var userWithoutEmployee = GetFactory<UserFactory>().Create();

            var search = new TestSearchUser();

            search.EmployeeId = "545454";
            var result = Repository.SearchUsers(search).ToList();
            Assert.IsFalse(result.Contains(userWithoutEmployee));
            Assert.IsTrue(result.Contains(userWithEmployee));

            search.EmployeeId = null;
            result = Repository.SearchUsers(search).ToList();
            Assert.IsTrue(result.Contains(userWithoutEmployee));
            Assert.IsTrue(result.Contains(userWithEmployee));
        }

        [TestMethod]
        public void TestGetUsersWithRoleReturnsMatchesForModules()
        {
            var goodModule = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.EventsEvents});
            _goodRole.Module = goodModule;
            Session.Save(_goodRole);
            Session.Flush();
            var result = Repository.GetUsersWithRole(_goodRole.Module.Id, null, null, null);
            Assert.AreSame(_goodUser, result.Single());
        }

        [TestMethod]
        public void TestGetUsersWithRoleReturnsMatchesForModulesRegardlessOfActionIfActionIsNull()
        {
            var goodModule = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.EventsEvents});
            _goodRole.Module = goodModule;
            _goodRole.Action = GetFactory<AddActionFactory>().Create();
            Session.Save(_goodRole);
            _badRole.Module = goodModule;
            _badRole.Action = GetFactory<DeleteActionFactory>().Create();
            Session.Save(_badRole);
            Session.Flush();
            var result = Repository.GetUsersWithRole(_goodRole.Module.Id, null, null, null);
            Assert.IsTrue(result.Contains(_goodUser));
            Assert.IsTrue(result.Contains(_badUser));
        }

        [TestMethod]
        public void TestGetUsersWithRoleReturnsMatchesForModulesRegardlessOfActionIfActionIsRead()
        {
            var goodModule = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.EventsEvents});
            _goodRole.Module = goodModule;
            _goodRole.Action = GetFactory<AddActionFactory>().Create();
            Session.Save(_goodRole);
            _badRole.Module = goodModule;
            _badRole.Action = GetFactory<DeleteActionFactory>().Create();
            Session.Save(_badRole);
            Session.Flush();
            var result = Repository.GetUsersWithRole(_goodRole.Module.Id, null, (int)RoleActions.Read, null);
            Assert.IsTrue(result.Contains(_goodUser));
            Assert.IsTrue(result.Contains(_badUser));
        }

        [TestMethod]
        public void TestGetUsersWithRoleReturnsMatchesForModuleRegardlessOfOperatingCenterIfOperatingCenterIsNull()
        {
            var goodModule = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.EventsEvents});
            _goodRole.Module = goodModule;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            _goodRole.OperatingCenter = opc;
            Session.Save(_goodRole);
            Session.Flush();
            var result = Repository.GetUsersWithRole(_goodRole.Module.Id, null, null, null);
            Assert.AreSame(_goodUser, result.Single());
        }

        [TestMethod]
        public void
            TestGetUserWithRoleReturnsMatchesForModuleRegardlessOfOperatingCenterIfOperatingCenterIsSetButUserHasWildCardOperatingCenter()
        {
            var goodModule = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.EventsEvents});
            _goodRole.Module = goodModule;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            _goodRole.OperatingCenter = null;
            Session.Save(_goodRole);
            Session.Flush();
            var result = Repository.GetUsersWithRole(_goodRole.Module.Id, opc.Id, null, null);
            Assert.AreSame(_goodUser, result.Single());
        }

        [TestMethod]
        public void TestGetUserWithRoleReturnsMatchesInOrderByUserName()
        {
            _goodUser.UserName = "B";
            _badUser.UserName = "A";
            Session.Save(_goodUser);
            Session.Save(_badUser);
            Session.Flush();
            var result = Repository.GetUsersWithRole(_goodRole.Module.Id, null, null, null).ToArray();
            Assert.AreSame(_badUser, result[0]);
            Assert.AreSame(_goodUser, result[1]);
        }

        [TestMethod]
        public void TestGetUsersFilterByWithAndWithOutOperatingCenterReturnsMatchesForModules()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodModule = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EventsEvents });
            _goodRole.Module = goodModule;
            _goodRole.OperatingCenter = opc;
            Session.Save(_goodRole);
            Session.Flush();
            var result = Repository.GetUsersFilterByWithAndWithOutOperatingCenter(_goodRole.Module.Id, opc.Id, null, null);
            Assert.AreSame(_goodUser, result.Single());
        }

        [TestMethod]
        public void TestGetUsersFilterByWithAndWithOutOperatingCenterReturnsMatchesForModulesRegardlessOfActionIfActionIsNull()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodModule = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EventsEvents });
            _goodRole.OperatingCenter = opc;
            _badRole.OperatingCenter = null;
            _goodRole.Module = goodModule;
            _goodRole.Action = GetFactory<AddActionFactory>().Create();
            Session.Save(_goodRole);
            _badRole.Module = goodModule;
            _badRole.Action = GetFactory<DeleteActionFactory>().Create();
            Session.Save(_badRole);
            Session.Flush();
            var result = Repository.GetUsersFilterByWithAndWithOutOperatingCenter(_goodRole.Module.Id, opc.Id, null, null);
            Assert.IsTrue(result.Contains(_goodUser));
            Assert.IsTrue(result.Contains(_badUser));
        }

        [TestMethod]
        public void TestGetUsersFilterByWithAndWithOutOperatingCenterReturnsMatchesForModulesRegardlessOfActionIfActionIsRead()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var badModule = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EventsEvents });
            var goodModule = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EventsEvents });
            _goodRole.OperatingCenter = opc;
            _badRole.OperatingCenter = null;
            _goodRole.Module = goodModule;
            _goodRole.Action = GetFactory<AddActionFactory>().Create();
            Session.Save(_goodRole);
            _badRole.Module = badModule;
            _badRole.Action = GetFactory<DeleteActionFactory>().Create();
            Session.Save(_badRole);
            Session.Flush();
            var result = Repository.GetUsersFilterByWithAndWithOutOperatingCenter(_goodRole.Module.Id, opc.Id, (int)RoleActions.Read, null);
            Assert.IsTrue(result.Contains(_goodUser));
            Assert.IsTrue(result.Contains(_badUser));
        }

        [TestMethod]
        public void TestGetUsersFilterByWithAndWithOutOperatingCenterReturnsMatchesForModuleRegardlessOfOperatingCenterIfOperatingCenterIsNull()
        {
            var goodModule = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EventsEvents });
            _goodRole.Module = goodModule;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            _goodRole.OperatingCenter = opc;
            Session.Save(_goodRole);
            Session.Flush();
            var result = Repository.GetUsersFilterByWithAndWithOutOperatingCenter(_goodRole.Module.Id, opc.Id, null, null);
            Assert.AreSame(_goodUser, result.Single());
        }

        [TestMethod]
        public void
            TestGetUsersFilterByWithAndWithOutOperatingCenterReturnsMatchesForModuleRegardlessOfOperatingCenterIfOperatingCenterIsSetButUserHasWildCardOperatingCenter()
        {
            var goodModule = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EventsEvents });
            _goodRole.Module = goodModule;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            _goodRole.OperatingCenter = null;
            Session.Save(_goodRole);
            Session.Flush();
            var result = Repository.GetUsersFilterByWithAndWithOutOperatingCenter(_goodRole.Module.Id, opc.Id, null, null);
            Assert.AreSame(_goodUser, result.Single());
        }

        [TestMethod]
        public void TestGetUsersFilterByWithAndWithOutOperatingCenterReturnsMatchesInOrderByUserName()
        {
            var goodModule = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EventsEvents });
            _goodRole.Module = goodModule;
            var badModule = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EventsEvents });
            _badRole.Module = badModule;
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            _goodRole.OperatingCenter = opc;
            _badRole.OperatingCenter = null;
            _goodUser.UserName = "B";
            _badUser.UserName = "A";
            Session.Save(_goodUser);
            Session.Save(_badUser);
            Session.Flush();
            var result = Repository.GetUsersFilterByWithAndWithOutOperatingCenter(_goodRole.Module.Id, opc.Id, null, null).ToArray();
            Assert.AreSame(_goodUser, result[0]);
            Assert.AreSame(_badUser, result[1]);
        }

        [TestMethod]
        public void TestGetUsersByOperatingCenterIdReturnsUsersByOperatingCenterId()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var otherOperatingCenter = GetFactory<OperatingCenterFactory>()
               .Create(new {OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var validUser = GetEntityFactory<User>().Create(new
                {DefaultOperatingCenter = operatingCenter, UserName = "Kirwan", UserType = typeof(UserTypeFactory)});
            var invalidUser = GetEntityFactory<User>().Create(new {
                DefaultOperatingCenter = otherOperatingCenter, UserName = "Keane", UserType = typeof(UserTypeFactory)
            });

            var result = Repository.GetUsersByOperatingCenterId(operatingCenter.Id);

            //Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(validUser));
            Assert.IsFalse(result.Contains(invalidUser));
        }

        [TestMethod]
        public void TestSearchUserTrackingReturnsWhenSearchingLastLoggedInAt()
        {
            var goodDateToYouSir = DateTime.Now;
            _goodUser.LastLoggedInAt = goodDateToYouSir;
            _badUser.LastLoggedInAt = goodDateToYouSir.AddDays(-2);
            Repository.Save(new[] {_goodUser, _badUser});

            var model = new TestSearchUserTracking();
            model.LastLoggedInAt = new DateRange {
                Operator = RangeOperator.Between,
                Start = goodDateToYouSir.Date,
                End = goodDateToYouSir.Date.AddDays(1)
            };

            var result = Repository.SearchUserTracking(model);
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod]
        public void TestSearchUserTrackingCanReturnUsersWhoHaveNotLoggedInDuringASpecificTimePeriod()
        {
            var goodLog /* sit, log, sit */ = GetFactory<AuthenticationLogFactory>().Create(new {
                User = _goodUser,
                LoggedInAt = new DateTime(2015, 9, 15)
            });
            var badLog = GetFactory<AuthenticationLogFactory>().Create(new {
                User = _badUser,
                LoggedInAt = new DateTime(2015, 9, 17)
            });

            var model = new TestSearchUserTracking();
            model.NotLoggedInAt = new DateRange {
                Operator = RangeOperator.Between,
                Start = new DateTime(2015, 9, 15),
                End = new DateTime(2015, 9, 16)
            };

            var result = Repository.SearchUserTracking(model);
            Assert.IsFalse(result.Contains(_goodUser));
            Assert.IsTrue(result.Contains(_badUser));
        }

        #endregion

        #region Test classes

        private class TestSearchUserTracking : SearchSet<User>, ISearchUserTracking
        {
            public int? DefaultOperatingCenter { get; set; }
            public int? User { get; set; }
            public DateRange LastLoggedInAt { get; set; }
            public DateRange NotLoggedInAt { get; set; }
        }

        private class TestSearchUser : SearchSet<User>, ISearchUser
        {
            public string EmployeeId { get; set; }
        }

        #endregion
    }
}
