using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using System.Linq;

namespace MapCall.CommonTest.Model.ViewModels
{
    [TestClass]
    public class UserAdministrativeRoleAccessTest
    {
        #region Fields

        private User _user;
        private RoleAction _userAdminAction, _readAction;
        private OperatingCenter _opc1, _opc2, _opc3;
        private Module _module1, _module2;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _user = new User();
            _readAction = new RoleAction { Id = (int)RoleActions.Read };
            _userAdminAction = new RoleAction { Id = (int)RoleActions.UserAdministrator };
            _opc1 = new OperatingCenter { Id = 1 };
            _opc2 = new OperatingCenter { Id = 2 };
            _opc3 = new OperatingCenter { Id = 3 };
            _module1 = new Module { Id = 1 };    
            _module2 = new Module { Id = 2 };
        }

        private void InitUserForSiteAdmin()
        {
            _user.IsAdmin = true;
            _user.IsUserAdmin = false;
        }

        private void InitUserForUserAdmin()
        {
            _user.IsAdmin = false; 
            _user.IsUserAdmin = true;
        }

        private void AddRoleToUser(Role role)
        {
            _user.Roles.Add(role);
            _user.AggregateRoles.Add(new AggregateRole(role));
        }

        #endregion

        #region Tests

        #region Populating the OperatingCenterseByModule dictionary
        
        [TestMethod]
        public void TestOperatingCentersByModuleIsEmptyIfUserIsSiteAdminAndHasRoles()
        {
            InitUserForSiteAdmin();
            // Role itself doesn't need to be defined for this.
            AddRoleToUser(new Role());

            var target = new UserAdministrativeRoleAccess(_user);

            Assert.IsFalse(target.OperatingCentersByModule.Any());
        }

        [TestMethod]
        public void TestOperatingCentersByModuleIsEmptyIfUserIsNotASiteAdminOrAUserAdmin()
        {
            // Role itself doesn't need to be defined for this.
            AddRoleToUser(new Role());
            _user.IsAdmin = false;
            _user.IsUserAdmin = false;

            var target = new UserAdministrativeRoleAccess(_user);

            Assert.IsFalse(target.OperatingCentersByModule.Any());
        }

        [TestMethod]
        public void TestOperatingCentersByModuleIsPopulatedWithIfUserIsNotASiteAdminButIsAUserAdmin()
        {
            // Long test names are hard to read.
            // This tests that the OperatingCentersByModule prop is populated based *only* on roles
            // where the user has the UserAdministrator action. This is only if the user is a UserAdmin
            // and not if they are a site admin.
            InitUserForUserAdmin();
            AddRoleToUser(new Role { Action = _userAdminAction, OperatingCenter = _opc1, Module = _module1 });
            AddRoleToUser(new Role { Action = _readAction, OperatingCenter = _opc2, Module = _module2 });
            AddRoleToUser(new Role { Action = _readAction, OperatingCenter = _opc3, Module = _module1 });

            var target = new UserAdministrativeRoleAccess(_user);

            Assert.IsTrue(target.OperatingCentersByModule.ContainsKey(_module1), "module1 should be here because the user has UserAdmin access to it");
            Assert.IsFalse(target.OperatingCentersByModule.ContainsKey(_module2), "module2 should not be here because the user does not have UserAdmin access to it");
            Assert.IsTrue(target.OperatingCentersByModule[_module1].Contains(_opc1), "opc1 should be here because the user has UserAdmin access to a module for this opc");
            Assert.IsFalse(target.OperatingCentersByModule[_module1].Contains(_opc2), "opc2 should not be here because the user does not have UserAdmin access for this module in this opc");
            Assert.IsFalse(target.OperatingCentersByModule[_module1].Contains(_opc3), "opc3 should not be here because the user does not have UserAdmin access for this module in this opc");
        }

        [TestMethod]
        public void TestOperatingCentersByModuleWorksWithWildcardRoles()
        {
            InitUserForUserAdmin();
            AddRoleToUser(new Role { Action = _userAdminAction, OperatingCenter = null, Module = _module1 });

            var target = new UserAdministrativeRoleAccess(_user);

            Assert.IsTrue(target.OperatingCentersByModule.ContainsKey(_module1), "module1 should be here because the user has UserAdmin access to it");
            Assert.IsTrue(target.OperatingCentersByModule[_module1].Contains(null), "opc1 should be here because the user has UserAdmin access to a module for this opc");
        }

        #endregion

        #region CanAdministrate

        [TestMethod]
        public void TestCanAdministrateAlwaysReturnsTrueForSiteAdmins()
        {
            InitUserForSiteAdmin();

            var target = new UserAdministrativeRoleAccess(_user);

            // Arbitrary numbers! Hurrah!
            Assert.IsTrue(target.CanAdministrate(1242412, 12));
        }

        [TestMethod]
        public void TestCanAdministrateReturnsFalseForUsersThatAreNotSiteAdminsOrUserAdmins()
        {
            _user.IsAdmin = false;
            _user.IsUserAdmin = false;
            AddRoleToUser(new Role { Action = _userAdminAction, OperatingCenter = _opc1, Module = _module1 });

            var target = new UserAdministrativeRoleAccess(_user);
            
            Assert.IsFalse(target.CanAdministrate(_module1.Id, _opc1.Id), "User should not be able to administrate as they are not UserAdmins.");

            InitUserForUserAdmin();
            target = new UserAdministrativeRoleAccess(_user);

            Assert.IsTrue(target.CanAdministrate(_module1.Id, _opc1.Id), "User should be able to administrate since they are a UserAdmin.");
        }

        [TestMethod]
        public void TestCanAdministrateReturnsFalseIfUserDoesNotHaveMatchingModule()
        {
            InitUserForUserAdmin();
            AddRoleToUser(new Role { Action = _userAdminAction, OperatingCenter = _opc1, Module = _module1 });

            var target = new UserAdministrativeRoleAccess(_user);

            Assert.IsTrue(target.CanAdministrate(_module1.Id, _opc1.Id));
            Assert.IsFalse(target.CanAdministrate(_module2.Id, _opc1.Id));
        }
        
        [TestMethod]
        public void TestCanAdministrateReturnsFalseIfUserDoesNotHaveMatchingOperatingCenterForModule()
        {
            InitUserForUserAdmin();
            AddRoleToUser(new Role { Action = _userAdminAction, OperatingCenter = _opc1, Module = _module1 });

            var target = new UserAdministrativeRoleAccess(_user);

            Assert.IsTrue(target.CanAdministrate(_module1.Id, _opc1.Id));
            Assert.IsFalse(target.CanAdministrate(_module1.Id, _opc2.Id));
        }

        [TestMethod]
        public void TestCanAdministrateReturnsTrueIfUserHasWildcardOperatingCenterForModule()
        {
            InitUserForUserAdmin();
            AddRoleToUser(new Role { Action = _userAdminAction, OperatingCenter = null, Module = _module1 });

            var target = new UserAdministrativeRoleAccess(_user);

            Assert.IsTrue(target.CanAdministrate(_module1.Id, _opc1.Id));
        }

        [TestMethod]
        public void TestCanAdministrateReturnsTrueIfUserHasWildcardOperatingCenterAndTheyAreTryingToAdministrateAnotherWildcardRole()
        {
            // ie: Users should only be able to add/remove a wildcard role if they themselves also have a wildcard role for the same module.
            InitUserForUserAdmin();
            AddRoleToUser(new Role { Action = _userAdminAction, OperatingCenter = null, Module = _module1 });

            var target = new UserAdministrativeRoleAccess(_user);

            Assert.IsTrue(target.CanAdministrate(_module1.Id, null), "User should be able to administrate because they have a wildcard role themselves.");

            _user.AggregateRoles.Clear();
            target = new UserAdministrativeRoleAccess(_user);

            Assert.IsFalse(target.CanAdministrate(_module1.Id, null), "User should not be able to administrate because they do not have a wildcard role themselves.");
        }

        #endregion

        #endregion
    }
}
