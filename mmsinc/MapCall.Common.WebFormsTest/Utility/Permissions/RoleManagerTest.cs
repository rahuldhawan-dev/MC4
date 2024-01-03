using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Permissions;
using MapCall.Common.Utility.Permissions.Roles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Permissions;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;

namespace MapCall.Common.WebFormsTest.Utility.Permissions
{
    [TestClass]
    public class RoleManagerTest
    {
        #region Fields

        private Mock<IHttpContext> _context;
        private Mock<IRoleManager> _mockRoleManager;
        private Mock<ILookupCache> _lookupCache;
        private Mock<IUser> _user;
        private User _dbUser;
        private Mock<IIdentity> _ident;
        private Mock<IRoleCacheManager> _cacheManager;
        private Collection<IRole> _userRoles;
        private Mock<IDbConnection> _connection;
        private Mock<IRepository<User>> _userRepo;
        private IContainer _container;

        #endregion

        #region Initialcleanerizers

        [TestInitialize]
        public void InitializeRoleManagerTest()
        {
            _context = new Mock<IHttpContext>();
            _mockRoleManager = new Mock<IRoleManager>();
            _userRepo = new Mock<IRepository<User>>();

            _dbUser = new User {UserName = "foo"};
            _userRepo.Setup(x => x.Where(It.Is<Expression<Func<User, bool>>>(fn => fn.Compile().Invoke(_dbUser))))
                     .Returns(new[] {_dbUser}.AsQueryable());

            _container = new Container(e => {
                e.For<IHttpContext>().Use(_context.Object);
                e.For<IRepository<User>>().Use(_userRepo.Object);
            });
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        private TestRoleManager InitializeBuilder()
        {
            _user = new Mock<IUser>();
            _user.Setup(x => x.Name).Returns("someuser");
            _userRepo.Setup(x => x.Where(
                          It.Is<Expression<Func<User, bool>>>(fn =>
                              fn.Compile().Invoke(new User {UserName = "someuser"}))))
                     .Returns(new[] {_dbUser}.AsQueryable());

            _ident = new Mock<IIdentity>();
            _cacheManager = new Mock<IRoleCacheManager>();
            _userRoles = new Collection<IRole>();
            _connection = new Mock<IDbConnection>();

            _user.SetupGet(x => x.Identity).Returns(_ident.Object);
            _cacheManager.Setup(x => x.GetAllRolesForUser(_user.Object))
                         .Returns(_userRoles);

            var t = _container.GetInstance<TestRoleManager>();
            InitializeMockLookupCache();
            t.CacheManager = _cacheManager.Object;
            t.Lookup = _lookupCache.Object;
            t.TestDbConnection = _connection.Object;
            return t;
        }

        private void InitializeMockLookupCache()
        {
            _lookupCache = new Mock<ILookupCache>();

            var actions = new List<IRoleAction> {
                new RoleAction {ActionId = 1, Name = "User Administrator"},
                new RoleAction {ActionId = 2, Name = "Read"}
            };

            var apps = new List<IRoleApplication> {
                new RoleApplication {ApplicationId = 1, Name = "Application 1"},
                new RoleApplication {ApplicationId = 2, Name = "Application 2"}
            };

            var mods = new List<IRoleModule> {
                new RoleModule {ModuleId = 1, ApplicationId = 1, Name = "Module 1 App 1"},
                new RoleModule {ModuleId = 2, ApplicationId = 2, Name = "Module 2 App 2"},
                new RoleModule {ModuleId = 3, ApplicationId = 1, Name = "Duplicate Module Name Different App"},
                new RoleModule {ModuleId = 4, ApplicationId = 2, Name = "Duplicate Module Name Different App"},
            };

            var opc = new List<IOperatingCenter> {
                new OperatingCenter {
                    OperatingCenterId = TestRoleManager.OPERATING_CENTER_ID_ALL,
                    OperatingCenterCode = TestRoleManager.OPERATING_CENTER_CODE_ALL
                },
                new OperatingCenter {OperatingCenterId = 1, OperatingCenterCode = "OPC 1"},
                new OperatingCenter {OperatingCenterId = 2, OperatingCenterCode = "OPC 2"},
                new OperatingCenter {OperatingCenterId = 3, OperatingCenterCode = "OPC 3"},
            };

            _lookupCache.SetupGet(x => x.Actions)
                        .Returns(actions.ToDictionary(x => x.ActionId));
            _lookupCache.SetupGet(x => x.Applications)
                        .Returns(apps.ToDictionary(x => x.ApplicationId));
            _lookupCache.SetupGet(x => x.ApplicationsByName)
                        .Returns(apps.ToDictionary(x => x.Name));
            _lookupCache.SetupGet(x => x.Modules)
                        .Returns(mods.ToDictionary(x => x.ModuleId));
            _lookupCache.SetupGet(x => x.OperatingCenters)
                        .Returns(opc.ToDictionary(x => x.OperatingCenterId));
            _lookupCache.SetupGet(x => x.OperatingCentersByName)
                        .Returns(opc.ToDictionary(x => x.OperatingCenterCode));
        }

        #endregion

        #region Tests

        #region Constant tests

        [TestMethod]
        public void TestOperatingCenterIdAllConstIsEqualToNegativeOne()
        {
            const int expected = -1;
            Assert.AreEqual(expected, TestRoleManager.OPERATING_CENTER_ID_ALL,
                "The RoleManager.OPERATING_CENTER_ID_ALL constant must be equal to -1.");
        }

        #endregion

        #region CacheManager property

        [TestMethod]
        public void TestCacheManagerPropertyDoesNotReturnNull()
        {
            var target = InitializeBuilder();
            target.CacheManager = null; // kill the mock.
            var oh = target.CacheManager;
            Assert.IsNotNull(oh);
        }

        #endregion

        #region Current property

        [TestMethod]
        public void TestRoleManagerCurrentReturnsItemInContextItems()
        {
            var dict = new Dictionary<object, object>();
            dict[RoleManager.ROLE_MANAGE_CONTEXT_ITEMS_KEY] = _mockRoleManager.Object;
            _context.SetupGet(x => x.Items).Returns(dict);
            var current = TestRoleManager.Current;

            Assert.AreSame(_mockRoleManager.Object, current);
        }

        [TestMethod]
        public void TestRoleManagerCurrentCreatesNewInstanceWhenContextItemsDoesNotContainARoleManager()
        {
            _container.Inject(new Mock<IRoleManager>().Object);
            var dict = new Dictionary<object, object>();
            _context.SetupGet(x => x.Items).Returns(dict);
            var current = _container.GetInstance<IRoleManager>();

            Assert.IsNotNull(current);
        }

        #endregion

        #region SharedLookup property

        [TestMethod]
        public void TestAddAllOperatingCenterToLookupCacheAddsTheAllOperatingCenterToLookupCache()
        {
            var target = InitializeBuilder();

            // Remove this entry since it gets added in the initializer.
            target.Lookup.OperatingCenters.Remove(TestRoleManager.OPERATING_CENTER_ID_ALL);
            target.Lookup.OperatingCentersByName.Remove(TestRoleManager.OPERATING_CENTER_CODE_ALL);

            TestRoleManager.AddAllOperatingCenterToLookupCache(target.Lookup);

            Assert.IsTrue(
                target.Lookup.OperatingCenters.ContainsKey(
                    TestRoleManager.OPERATING_CENTER_ID_ALL));

            var result =
                target.Lookup.OperatingCenters[
                    TestRoleManager.OPERATING_CENTER_ID_ALL];

            Assert.AreEqual(TestRoleManager.OPERATING_CENTER_ID_ALL, result.OperatingCenterId);
            Assert.AreEqual(TestRoleManager.OPERATING_CENTER_CODE_ALL, result.OperatingCenterCode);
        }

        [TestMethod]
        public void TestAddAllOperatingCenterToLookupCacheAddsTheAllOperatingCenterToLookupCacheOperatingCentersByName()
        {
            var target = InitializeBuilder();

            // Remove this entry since it gets added in the initializer.
            target.Lookup.OperatingCenters.Remove(TestRoleManager.OPERATING_CENTER_ID_ALL);
            target.Lookup.OperatingCentersByName.Remove(TestRoleManager.OPERATING_CENTER_CODE_ALL);

            TestRoleManager.AddAllOperatingCenterToLookupCache(target.Lookup);

            Assert.IsTrue(
                target.Lookup.OperatingCentersByName.ContainsKey(
                    TestRoleManager.OPERATING_CENTER_CODE_ALL));

            var result =
                target.Lookup.OperatingCentersByName[TestRoleManager.OPERATING_CENTER_CODE_ALL];

            Assert.AreEqual(TestRoleManager.OPERATING_CENTER_ID_ALL, result.OperatingCenterId);
            Assert.AreEqual(TestRoleManager.OPERATING_CENTER_CODE_ALL, result.OperatingCenterCode);
        }

        #endregion

        #region Lookup property

        [TestMethod]
        public void TestLookUpPropertyIsNotNull()
        {
            var target = InitializeBuilder();
            Assert.IsNotNull(target.Lookup);
        }

        #endregion

        #region CreateRoleCache method

        [TestMethod]
        public void TestCreateRoleCacheSucceeeds()
        {
            var target = InitializeBuilder();

            var cmd = new Mock<IDbCommand>();
            var parms = new Mock<IDataParameterCollection>();
            var reader = new Mock<IDataReader>();
            var localParms = new List<IDataParameter>();

            _user.Setup(x => x.Name).Returns("Joey Joe Joe");
            _connection.Setup(x => x.CreateCommand()).Returns(cmd.Object);
            cmd.Setup(x => x.Parameters).Returns(parms.Object);
            cmd.Setup(x => x.CreateParameter()).Returns(() => {
                var p = new SqlParameter();
                localParms.Add(p);
                return p;
            });
            cmd.Setup(x => x.ExecuteReader()).Returns(reader.Object);

            var result = target.TestCreateRoleCache(_user.Object);
            var resultParms = localParms.ToDictionary(x => x.ParameterName);
            Assert.IsTrue(resultParms.ContainsKey(RoleManager.CreateRoleCacheParams.USERNAME));
            Assert.AreEqual("Joey Joe Joe", resultParms[RoleManager.CreateRoleCacheParams.USERNAME].Value);
            Assert.IsNotNull(result);

            _connection.Verify(x => x.Open());
            _connection.Verify(x => x.Dispose());
            cmd.Verify(x => x.Dispose());
        }

        #endregion

        #region GetAllRolesForUser method

        [TestMethod]
        public void TestGetAllRolesForUserThrowsForNullUser()
        {
            var target = InitializeBuilder();
            MyAssert.Throws<ArgumentNullException>(
                () => target.GetAllRolesForUser(null));
        }

        [TestMethod]
        public void TestGetAllRolesForUserReturnsSameRolesFromRoleCacheManager()
        {
            var target = InitializeBuilder();
            var result = target.GetAllRolesForUser(_user.Object);
            Assert.IsNotNull(result);
            Assert.AreSame(_userRoles, result);
        }

        #endregion

        #region GetMatchingPermissionRoles method

        [TestMethod]
        public void TestGetMatchingPermissionRolesThrowsForNullParameters()
        {
            var target = InitializeBuilder();
            MyAssert.Throws(() => target.TestGetMatchingPermissionRoles(null));
        }

        [TestMethod]
        public void TestGetMatchingPermissionRolesReturnsCorrectMatches()
        {
            var target = InitializeBuilder();

            var module = new Mock<IModulePermissions>();
            module.Setup(x => x.Application).Returns("Application 1");
            module.Setup(x => x.Module).Returns("Module 1 App 1");

            var perm = new Mock<IPermissionsObject>();
            perm.Setup(x => x.User).Returns(_user.Object);
            perm.Setup(x => x.SpecificPermissions).Returns(module.Object);
            perm.Setup(x => x.Action).Returns(ModuleAction.Add);

            var expectedRole = new Role {
                ActionId = (int)ModuleAction.Add,
                ApplicationId = 1,
                Module = "Module 1 App 1"
            };
            var unexpectedAppRole = new Role {
                ActionId = (int)ModuleAction.Add,
                ApplicationId = 2,
                Module = "Module 1 App 1"
            };
            var unexpectedModuleRole = new Role {
                ActionId = (int)ModuleAction.Add,
                ApplicationId = 2,
                Module = "Some Other Module"
            };
            var unexpectedActionRole = new Role {
                ActionId = (int)ModuleAction.Read,
                ApplicationId = 2,
                Module = "Module 1 App 1"
            };
            _userRoles.Add(expectedRole);
            _userRoles.Add(unexpectedAppRole);
            _userRoles.Add(unexpectedModuleRole);
            _userRoles.Add(unexpectedActionRole);

            var result = target.TestGetMatchingPermissionRoles(perm.Object);

            Assert.IsTrue(result.Contains(expectedRole));
            Assert.IsFalse(result.Contains(unexpectedAppRole));
            Assert.IsFalse(result.Contains(unexpectedModuleRole));
            Assert.IsFalse(result.Contains(unexpectedActionRole));
        }

        [TestMethod]
        public void TestGetMatchingPermissionRolesReturnsEmptyCollectionIfNoMatchesFound()
        {
            var target = InitializeBuilder();

            var module = new Mock<IModulePermissions>();
            module.Setup(x => x.Application).Returns("Application 1");
            module.Setup(x => x.Module).Returns("Module 1 App 1");

            var perm = new Mock<IPermissionsObject>();
            perm.Setup(x => x.User).Returns(_user.Object);
            perm.Setup(x => x.SpecificPermissions).Returns(module.Object);
            perm.Setup(x => x.Action).Returns(ModuleAction.Add);

            var result = target.TestGetMatchingPermissionRoles(perm.Object);

            Assert.IsFalse(result.Any());
        }

        #endregion

        #region HydrateRoleStringProperties method

        [TestMethod]
        public void TestHydrateRoleStringPropertiesDoesItCorrectly()
        {
            var target = InitializeBuilder();
            var role = new Role {
                ApplicationId = 1,
                ActionId = 1,
                ModuleId = 1,
                OperatingCenterId = 1
            };

            target.TestHydrateRoleStringProperties(role);

            Assert.AreEqual("Application 1", role.Application);
            Assert.AreEqual("User Administrator", role.Action);
            Assert.AreEqual("Module 1 App 1", role.Module);
            Assert.AreEqual("OPC 1", role.OperatingCenter);
        }

        #endregion

        #region ReadRoles method

        // This method is gigantic, so deal!
        private void TestReadRoles(
            int expectedUserId,
            int? expectedOpCenterId,
            int expectedAppId,
            int expectedModuleId,
            int expectedActionId)
        {
            const int expectedUserIdOrd = 1;
            const int expectedOpCenterIdOrd = 2;
            const int expectedAppIdOrd = 3;
            const int expectedModuleIdOrd = 4;
            const int expectedActionIdOrd = 5;

            var reader = new Mock<IDataReader>();
            reader.Setup(x => x.GetOrdinal("UserID")).Returns(expectedUserIdOrd);
            reader.Setup(x => x.GetOrdinal("OperatingCenterID")).Returns(expectedOpCenterIdOrd);
            reader.Setup(x => x.GetOrdinal("ApplicationID")).Returns(expectedAppIdOrd);
            reader.Setup(x => x.GetOrdinal("ModuleID")).Returns(expectedModuleIdOrd);
            reader.Setup(x => x.GetOrdinal("ActionID")).Returns(expectedActionIdOrd);

            reader.Setup(x => x.GetInt32(expectedUserIdOrd)).Returns(expectedUserId);
            reader.Setup(x => x.GetInt32(expectedAppIdOrd)).Returns(expectedAppId);
            reader.Setup(x => x.GetInt32(expectedModuleIdOrd)).Returns(expectedModuleId);
            reader.Setup(x => x.GetInt32(expectedActionIdOrd)).Returns(expectedActionId);

            reader.Setup(x => x.IsDBNull(expectedOpCenterIdOrd)).Returns(
                () => !expectedOpCenterId.HasValue);

            if (expectedOpCenterId.HasValue)
            {
                reader.Setup(x => x.GetInt32(expectedOpCenterIdOrd)).Returns(
                    expectedOpCenterId.Value);
            }

            var readTarget = true;
            reader.Setup(x => x.Read())
                  .Returns(() => readTarget)
                  .Callback(() => readTarget = false);

            var target = InitializeBuilder();
            var result = target.TestReadRoles(reader.Object);

            Assert.AreEqual(1, result.Count());

            var role = result.Single();

            Assert.AreEqual(expectedUserId, role.UserId);
            Assert.AreEqual(expectedAppId, role.ApplicationId);
            Assert.AreEqual(expectedModuleId, role.ModuleId);
            Assert.AreEqual(expectedActionId, role.ActionId);
            if (expectedOpCenterId.HasValue)
            {
                Assert.AreEqual(expectedOpCenterId, role.OperatingCenterId);
            }
            else
            {
                Assert.AreEqual(TestRoleManager.OPERATING_CENTER_ID_ALL,
                    role.OperatingCenterId);
            }

            reader.Verify(x => x.GetOrdinal("UserID"));
            reader.Verify(x => x.GetOrdinal("OperatingCenterID"));
            reader.Verify(x => x.GetOrdinal("ApplicationID"));
            reader.Verify(x => x.GetOrdinal("ModuleID"));
            reader.Verify(x => x.GetOrdinal("ActionID"));
            reader.Verify(x => x.GetInt32(expectedUserIdOrd));
            reader.Verify(x => x.GetInt32(expectedAppIdOrd));
            reader.Verify(x => x.GetInt32(expectedModuleIdOrd));
            reader.Verify(x => x.GetInt32(expectedActionIdOrd));
            reader.Verify(x => x.IsDBNull(expectedOpCenterIdOrd));
            if (expectedOpCenterId.HasValue)
            {
                reader.Verify(x => x.GetInt32(expectedOpCenterIdOrd));
            }
        }

        [TestMethod]
        public void TestReadRolesDoesntFail()
        {
            TestReadRoles(42, 1, 1, 1, 1);
        }

        [TestMethod]
        public void TestReadRolesReplacedNullOperatingCenterIdWithConst()
        {
            TestReadRoles(653, null, 1, 1, 1);
        }

        #endregion

        #region UserCanAdministrateRole method

        [TestMethod]
        public void TestUserCanAdministrateRoleThrowsForNullRoleParameters()
        {
            var target = InitializeBuilder();
            MyAssert.Throws<ArgumentNullException>(() => target.UserCanAdministrateRole(null, null));
            MyAssert.Throws<ArgumentNullException>(() => target.UserCanAdministrateRole(_user.Object, null));
            MyAssert.Throws<ArgumentNullException>(() => target.UserCanAdministrateRole(null, new Role()));
        }

        [TestMethod]
        public void TestUserCanAdministrateRoleReturnsTrueIfUserIsAdminIsTrue()
        {
            var target = InitializeBuilder();

            _dbUser.IsAdmin = false;
            Assert.IsFalse(target.UserCanAdministrateRole(_user.Object, new Role()));

            _dbUser.IsAdmin = true;
            Assert.IsTrue(target.UserCanAdministrateRole(_user.Object, new Role()));
        }

        [TestMethod]
        public void TestUserCanAdministrateRoleReturnsTrueWhenRoleActionIsUserAdmin()
        {
            var target = InitializeBuilder();
            var adminRole = new Role {
                ActionId = (int)ModuleAction.Administrate,
                ApplicationId = 1,
                ModuleId = 1,
                OperatingCenterId = 1
            };
            _userRoles.Add(adminRole);

            var result = target.UserCanAdministrateRole(_user.Object, adminRole);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestUserCanAdministrateRoleReturnsFalseWhenRoleActionIsNotUseAdminButEverythingElseTheSame()
        {
            var target = InitializeBuilder();
            var adminRole = new Role {
                ActionId = (int)ModuleAction.Read,
                ApplicationId = 1,
                ModuleId = 1,
                OperatingCenterId = 1
            };
            _userRoles.Add(adminRole);

            var result = target.UserCanAdministrateRole(_user.Object, adminRole);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestUserCanAdministrateRoleReturnsFalseWhenNoRolesExistForUser()
        {
            var target = InitializeBuilder();
            var adminRole = new Role {
                ActionId = (int)ModuleAction.Read,
                ApplicationId = 1,
                ModuleId = 1,
                OperatingCenterId = 1
            };

            var result = target.UserCanAdministrateRole(_user.Object, adminRole);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestUserCanAdministrateRoleReturnsFalseWhenUseRoleInDifferentOperatingCenter()
        {
            var adminRole = new Role {
                ActionId = (int)ModuleAction.Administrate,
                ApplicationId = 1,
                ModuleId = 1,
                OperatingCenterId = 1
            };

            var checkRole = new Role {
                ActionId = (int)ModuleAction.Administrate,
                ApplicationId = 1,
                ModuleId = 1,
                OperatingCenterId = 2
            };

            var target = InitializeBuilder();
            _userRoles.Add(adminRole);

            var result = target.UserCanAdministrateRole(_user.Object, checkRole);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestUserCanAdministrateReturnsTrueWhenUserCanAdministrateInAllOperatingCenter()
        {
            var adminRole = new Role {
                ActionId = (int)ModuleAction.Administrate,
                ApplicationId = 1,
                ModuleId = 1,
                OperatingCenterId = TestRoleManager.OPERATING_CENTER_ID_ALL
            };

            var checkRole = new Role {
                ActionId = (int)ModuleAction.Administrate,
                ApplicationId = 1,
                ModuleId = 1,
                OperatingCenterId = 2
            };

            var target = InitializeBuilder();
            _userRoles.Add(adminRole);

            var result = target.UserCanAdministrateRole(_user.Object, checkRole);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestUserCanAdministrateReturnsFalseWhenUserIsInDifferentModule()
        {
            var adminRole = new Role {
                ActionId = (int)ModuleAction.Administrate,
                ApplicationId = 1,
                ModuleId = 1,
                OperatingCenterId = 1
            };

            var checkRole = new Role {
                ActionId = (int)ModuleAction.Read,
                ApplicationId = 1,
                ModuleId = 2,
                OperatingCenterId = 1
            };

            var target = InitializeBuilder();
            _userRoles.Add(adminRole);

            var result = target.UserCanAdministrateRole(_user.Object, checkRole);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestUserCanAdministrateReturnsFalseWhenUserIsInDifferentApplication()
        {
            var adminRole = new Role {
                ActionId = (int)ModuleAction.Administrate,
                ApplicationId = 1,
                ModuleId = 1,
                OperatingCenterId = 1
            };

            var checkRole = new Role {
                ActionId = (int)ModuleAction.Read,
                ApplicationId = 2,
                ModuleId = 1,
                OperatingCenterId = 1
            };

            var target = InitializeBuilder();
            _userRoles.Add(adminRole);

            var result = target.UserCanAdministrateRole(_user.Object, checkRole);
            Assert.IsFalse(result);
        }

        #endregion

        #region UserIsInRole method

        [TestMethod]
        public void TestUserIsInRoleReturnsTrueWhenUserHasAtleastOneMatchingRoleInAnyOperatingCenter()
        {
            var target = InitializeBuilder();

            var module = new Mock<IModulePermissions>();
            module.Setup(x => x.Application).Returns("Application 1");
            module.Setup(x => x.Module).Returns("Module 1 App 1");

            var perm = new Mock<IPermissionsObject>();
            perm.Setup(x => x.User).Returns(_user.Object);
            perm.Setup(x => x.SpecificPermissions).Returns(module.Object);
            perm.Setup(x => x.Action).Returns(ModuleAction.Add);

            var expectedRole = new Role {
                ActionId = (int)ModuleAction.Add,
                ApplicationId = 1,
                Module = "Module 1 App 1",
                OperatingCenterId = 1,
            };
            var unexpectedAppRole = new Role {
                ActionId = (int)ModuleAction.Add,
                ApplicationId = 1,
                Module = "Module 1 App 1",
                OperatingCenterId = 2,
            };

            _userRoles.Add(expectedRole);
            _userRoles.Add(unexpectedAppRole);

            var result = target.UserIsInRole(perm.Object);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestUserIsInRoleReturnsFalseIfUserIsNotInMatchingRole()
        {
            var target = InitializeBuilder();

            var module = new Mock<IModulePermissions>();
            module.Setup(x => x.Application).Returns("Application 1");
            module.Setup(x => x.Module).Returns("Module 1 App 1");

            var perm = new Mock<IPermissionsObject>();
            perm.Setup(x => x.User).Returns(_user.Object);
            perm.Setup(x => x.SpecificPermissions).Returns(module.Object);
            perm.Setup(x => x.Action).Returns(ModuleAction.Add);

            var expectedRole = new Role {
                ActionId = (int)ModuleAction.Add,
                ApplicationId = 2,
                Module = "Module 2 App 2",
                OperatingCenterId = 1,
            };
            var unexpectedAppRole = new Role {
                ActionId = (int)ModuleAction.Add,
                ApplicationId = 2,
                Module = "Module 2 App 2",
                OperatingCenterId = 2,
            };

            _userRoles.Add(expectedRole);
            _userRoles.Add(unexpectedAppRole);

            var result = target.UserIsInRole(perm.Object);

            Assert.IsFalse(result);
        }

        #endregion

        #region UserIsInRoleWithOperatingCenter method

        [TestMethod]
        public void TestUserIsInRoleWithOperatingCenterReturnsTrueWhenUserHasMatchingRole()
        {
            var target = InitializeBuilder();

            var module = new Mock<IModulePermissions>();
            module.Setup(x => x.Application).Returns("Application 1");
            module.Setup(x => x.Module).Returns("Module 1 App 1");

            var perm = new Mock<IPermissionsObject>();
            perm.Setup(x => x.User).Returns(_user.Object);
            perm.Setup(x => x.SpecificPermissions).Returns(module.Object);
            perm.Setup(x => x.Action).Returns(ModuleAction.Add);

            var expectedRole = new Role {
                ActionId = (int)ModuleAction.Add,
                ApplicationId = 1,
                Module = "Module 1 App 1",
                OperatingCenterId = 1
            };
            var unexpectedAppRole = new Role {
                ActionId = (int)ModuleAction.Add,
                ApplicationId = 1,
                Module = "Module 1 App 1",
                OperatingCenterId = 2
            };
            _userRoles.Add(expectedRole);
            _userRoles.Add(unexpectedAppRole);

            var result = target.UserIsInRoleWithOperatingCenter(perm.Object, "OPC 1");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestUserIsInRoleWithOperatingCenterReturnsFalseWhenUserDoesNotHaveMatchingRole()
        {
            var target = InitializeBuilder();

            var module = new Mock<IModulePermissions>();
            module.Setup(x => x.Application).Returns("Application 1");
            module.Setup(x => x.Module).Returns("Module 1 App 1");

            var perm = new Mock<IPermissionsObject>();
            perm.Setup(x => x.User).Returns(_user.Object);
            perm.Setup(x => x.SpecificPermissions).Returns(module.Object);
            perm.Setup(x => x.Action).Returns(ModuleAction.Add);

            var unexpectedAppRole = new Role {
                ActionId = (int)ModuleAction.Add,
                ApplicationId = 2,
                Module = "Module 2 App 2",
                OperatingCenterId = 2
            };
            _userRoles.Add(unexpectedAppRole);

            var result = target.UserIsInRoleWithOperatingCenter(perm.Object, "OPC 1");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestUserIsInRoleWithOperatingCenterReturnsFalseWhenUserDoesNotHaveAnyRoles()
        {
            var target = InitializeBuilder();

            var module = new Mock<IModulePermissions>();
            module.Setup(x => x.Application).Returns("Application 1");
            module.Setup(x => x.Module).Returns("Module 1 App 1");

            var perm = new Mock<IPermissionsObject>();
            perm.Setup(x => x.User).Returns(_user.Object);
            perm.Setup(x => x.SpecificPermissions).Returns(module.Object);
            perm.Setup(x => x.Action).Returns(ModuleAction.Add);

            var result = target.UserIsInRoleWithOperatingCenter(perm.Object, "OPC 1");

            Assert.IsFalse(result);
            Assert.IsFalse(_userRoles.Any());
        }

        [TestMethod]
        public void
            TestUserIsInRoleWithOperatingCenterReinitializesLookupWhenNotOperatingCentersByNameContainsContainsOperatingCenterCode()
        {
            var target = InitializeBuilder();

            var module = new Mock<IModulePermissions>();
            module.Setup(x => x.Application).Returns("Application 1");
            module.Setup(x => x.Module).Returns("Module 1 App 1");

            var perm = new Mock<IPermissionsObject>();
            perm.Setup(x => x.User).Returns(_user.Object);
            perm.Setup(x => x.SpecificPermissions).Returns(module.Object);
            perm.Setup(x => x.Action).Returns(ModuleAction.Add);

            target.UserIsInRoleWithOperatingCenter(perm.Object, "THIS IS NOT A REAL OPERATING CENTER CODE");

            _lookupCache.Verify(x => x.Reinitialize());
        }

        #endregion

        #endregion
    }

    public class TestRoleManager : RoleManager
    {
        #region Properties

        public IDbConnection TestDbConnection { get; set; }

        #endregion

        #region Methods

        public IEnumerable<IRole> TestReadRoles(IDataReader reader)
        {
            return ReadRoles(reader);
        }

        public IEnumerable<IRole> TestCreateRoleCache(IUser user)
        {
            return CreateRoleCache(user);
        }

        public IEnumerable<IRole> TestGetMatchingPermissionRoles(IPermissionsObject obj)
        {
            return GetMatchingPermissionRoles(obj);
        }

        public void TestHydrateRoleStringProperties(Role r)
        {
            HydrateRoleStringProperties(r);
        }

        protected override IDbConnection GetConnection()
        {
            return TestDbConnection;
        }

        #endregion

        public TestRoleManager(IContainer container) : base(container) { }
    }
}
