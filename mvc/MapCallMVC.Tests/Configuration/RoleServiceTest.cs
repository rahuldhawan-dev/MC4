using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Configuration;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Configuration
{
    [TestClass]
    public class RoleServiceTest
    {
        #region Fields

        private RoleService _target;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private int opCenterCount;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container(e => { e.For<IUserRepository>().Mock(); });
            _authServ = new Mock<IAuthenticationService<User>>();
            _container.Inject(_authServ.Object);
            _user = new User {
                Roles = new List<Role>()
            };
            InitAuthenticatedUser();
            _target = _container.GetInstance<RoleService>();
        }

        private void InitAuthenticatedUser()
        {
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);
        }

        private void InitAnonymousUser()
        {
            _authServ.Setup(x => x.CurrentUser).Throws(new Exception("User isn't authenticated"));
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(false);
        }

        private AggregateRole CreateRole(RoleModules module, RoleActions action = RoleActions.Read, OperatingCenter opCenter = null)
        {
            // This is just here until factories get made.
            var role = new Role {
                User = _user,
                Module = new Module(),
                Action = new RoleAction(),
                OperatingCenter = opCenter
            };

            role.Module.SetPropertyValueByName("Id", (int)module);
            role.Action.SetPropertyValueByName("Id", (int)action);

            return new AggregateRole(role);
        }

        private OperatingCenter CreateOperatingCenter()
        {
            // This is also just here until factories get made.
            opCenterCount++;
            var opc = new OperatingCenter();
            opc.SetPropertyValueByName("OperatingCenterID", opCenterCount);
            return opc;
        }

        #endregion

        #region Tests

        #region CurrentUserRoles

        [TestMethod]
        public void TestCurrentUserRolesReturnsEmptyEnumerableIfCurrentUserIsNotAuthenticated()
        {
            InitAnonymousUser();
            var result = _target.CurrentUserRoles;
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestCurrentUserRolesReturnsCurrentUsersRolesIfCurrentUserIsAuthenticated()
        {
            var expectedRole = new AggregateRole();
            _user.AggregateRoles.Add(expectedRole);
            var result = _target.CurrentUserRoles;
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(expectedRole));
        }

        #endregion

        #region CanAccessRole

        [TestMethod]
        public void TestCanAccessRoleReturnsTrueForSiteAdminsIfTheyDontHaveARole()
        {
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            _user.Roles.Clear();
            Assert.IsTrue(_target.CanAccessRole(RoleModules.FieldServicesAssets, RoleActions.Add));
        }

        [TestMethod]
        public void TestCanAccessRoleReturnsFalseIfUserIsNotAuthenticated()
        {
            InitAnonymousUser();
            // Add a role that would otherwise be valid if the user was authenticated.
            var role = CreateRole(RoleModules.FieldServicesAssets, RoleActions.Add);
            _user.AggregateRoles.Add(role);
            Assert.IsFalse(_target.CanAccessRole(RoleModules.FieldServicesAssets, RoleActions.Add));
        }

        [TestMethod]
        public void TestCanAccessRoleReturnsTrueIfUserIsInRole()
        {
            var role = CreateRole(RoleModules.FieldServicesAssets, RoleActions.Add);
            _user.AggregateRoles.Add(role);
            Assert.IsTrue(_target.CanAccessRole(RoleModules.FieldServicesAssets, RoleActions.Add));
        }

        [TestMethod]
        public void TestCanAccessRoleRoleModuleParameterOverload()
        {
            var role = CreateRole(RoleModules.FieldServicesAssets, RoleActions.Read);
            _user.AggregateRoles.Add(role);

            Assert.IsTrue(_target.CanAccessRole(RoleModules.FieldServicesAssets), "Defaults to Read action, should have access.");
            Assert.IsFalse(_target.CanAccessRole(RoleModules.EventsEvents), "No role, so shouldn't have access");
        }

        [TestMethod]
        public void TestCanAccessRoleActionParameterOverload()
        {
            var role = CreateRole(RoleModules.FieldServicesAssets, RoleActions.Add);
            _user.AggregateRoles.Add(role);

            Assert.IsTrue(_target.CanAccessRole(RoleModules.FieldServicesAssets, RoleActions.Add));
            Assert.IsFalse(_target.CanAccessRole(RoleModules.FieldServicesAssets, RoleActions.Delete));
        }

        #region CanAccessRole based on different Actions

        private void TestCanAccessRoleActions(RoleActions requiredAction, RoleActions userAction, bool expectedResult)
        {
            var role = CreateRole(RoleModules.ContractorsGeneral, userAction);
            _user.AggregateRoles.Clear();
            _user.AggregateRoles.Add(role);
            Assert.AreEqual(expectedResult, _target.CanAccessRole(RoleModules.ContractorsGeneral, requiredAction));
        }

        [TestMethod]
        public void TestCanAccessRoleReturnsForReadAction()
        {
            TestCanAccessRoleActions(RoleActions.Read, RoleActions.Read, true);
            TestCanAccessRoleActions(RoleActions.Read, RoleActions.Edit, true);
            TestCanAccessRoleActions(RoleActions.Read, RoleActions.Add, true);
            TestCanAccessRoleActions(RoleActions.Read, RoleActions.Delete, true);
            TestCanAccessRoleActions(RoleActions.Read, RoleActions.UserAdministrator, true);
        }

        [TestMethod]
        public void TestCanAccessRoleReturnsForEditAction()
        {
            TestCanAccessRoleActions(RoleActions.Edit, RoleActions.Read, false);
            TestCanAccessRoleActions(RoleActions.Edit, RoleActions.Edit, true);
            TestCanAccessRoleActions(RoleActions.Edit, RoleActions.Add, false);
            TestCanAccessRoleActions(RoleActions.Edit, RoleActions.Delete, false);
            TestCanAccessRoleActions(RoleActions.Edit, RoleActions.UserAdministrator, true);
        }

        [TestMethod]
        public void TestCanAccessRoleReturnsForAddAction()
        {
            TestCanAccessRoleActions(RoleActions.Add, RoleActions.Read, false);
            TestCanAccessRoleActions(RoleActions.Add, RoleActions.Edit, false);
            TestCanAccessRoleActions(RoleActions.Add, RoleActions.Add, true);
            TestCanAccessRoleActions(RoleActions.Add, RoleActions.Delete, false);
            TestCanAccessRoleActions(RoleActions.Add, RoleActions.UserAdministrator, true);
        }

        [TestMethod]
        public void TestCanAccessRoleReturnsForDeleteAction()
        {
            TestCanAccessRoleActions(RoleActions.Delete, RoleActions.Read, false);
            TestCanAccessRoleActions(RoleActions.Delete, RoleActions.Edit, false);
            TestCanAccessRoleActions(RoleActions.Delete, RoleActions.Add, false);
            TestCanAccessRoleActions(RoleActions.Delete, RoleActions.Delete, true);
            TestCanAccessRoleActions(RoleActions.Delete, RoleActions.UserAdministrator, true);
        }

        [TestMethod]
        public void TestCanAccessRoleReturnsForUserAdministratorAction()
        {
            TestCanAccessRoleActions(RoleActions.UserAdministrator, RoleActions.Read, false);
            TestCanAccessRoleActions(RoleActions.UserAdministrator, RoleActions.Edit, false);
            TestCanAccessRoleActions(RoleActions.UserAdministrator, RoleActions.Add, false);
            TestCanAccessRoleActions(RoleActions.UserAdministrator, RoleActions.Delete, false);
            TestCanAccessRoleActions(RoleActions.UserAdministrator, RoleActions.UserAdministrator, true);
        }

        #endregion

        #region CanAccessRole based on OperatingCenter

        [TestMethod]
        public void TestCanAccessRoleReturnsTrueIfUserIsInRoleForSpecificOperatingCenter()
        {
            var opc = CreateOperatingCenter();
            var role = CreateRole(RoleModules.H2OGeneral, RoleActions.Read, opc);
            _user.AggregateRoles.Add(role);
            Assert.IsTrue(_target.CanAccessRole(RoleModules.H2OGeneral, RoleActions.Read, opc));
        }

        [TestMethod]
        public void TestCanAccessRoleReturnsTrueIfUserIsInRoleThatContainsWildcardOperatingCenter()
        {
            var otherOpc = CreateOperatingCenter();
            var role = CreateRole(RoleModules.H2OGeneral, RoleActions.Read, null);
            Assert.IsTrue(role.IsValidForAnyOperatingCenter);
            _user.AggregateRoles.Add(role);
            Assert.IsTrue(_target.CanAccessRole(RoleModules.H2OGeneral, RoleActions.Read, otherOpc));
        }

        [TestMethod]
        public void TestCanAccessRoleReturnsFalseIfUserIsInRoleButNotForThatOperatingCenter()
        {
            var opc = CreateOperatingCenter();
            var otherOpc = CreateOperatingCenter();
            var role = CreateRole(RoleModules.H2OGeneral, RoleActions.Read, opc);
            _user.AggregateRoles.Add(role);
            Assert.IsFalse(_target.CanAccessRole(RoleModules.H2OGeneral, RoleActions.Read, otherOpc));
        }

        [TestMethod]
        public void TestCanAccessRoleIgnoresOperatingCenterIfParameterIsNull()
        {
            var opc = CreateOperatingCenter();
            var role = CreateRole(RoleModules.H2OGeneral, RoleActions.Read, opc);
            _user.AggregateRoles.Add(role);
            // ReSharper disable RedundantArgumentDefaultValue
            Assert.IsTrue(_target.CanAccessRole(RoleModules.H2OGeneral, RoleActions.Read, null),
                "Passing null for OperatingCenter should result in the OperatingCenter being ignored when figuring out role authorization.");
            Assert.IsTrue(_target.CanAccessRole(RoleModules.H2OGeneral, RoleActions.Read, opc),
                "Passing in an OperatingCenter should result in OperatingCenter being used for figuring out role authorization.");
            // ReSharper restore RedundantArgumentDefaultValue
        }

        #endregion

        #endregion

        #endregion
    }
}
