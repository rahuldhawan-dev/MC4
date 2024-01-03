using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Admin.Models.ViewModels.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Admin.Models.ViewModels.Users
{
    [TestClass]
    public class RemoveUserRoleGroupsTest : ViewModelTestBase<User, RemoveUserRoleGroups>
    {
        #region Fields

        private RoleAction _readAction, _userAdminAction;
        private Module _moduleBapp, _moduleBpu;
        private OperatingCenter _opc1, _opc2;
        private User _currentUser;
        private RoleGroup _roleGroup;
        private RoleGroupRole _roleGroupRole1, _roleGroupRole2;
        private Mock<IAuthenticationService<User>> _authServ;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _readAction = GetEntityFactory<RoleAction>().Create(new { Id = (int)RoleActions.Read });
            _userAdminAction = GetEntityFactory<RoleAction>().Create(new { Id = (int)RoleActions.UserAdministrator });
            _moduleBapp = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BAPPTeamSharingGeneral });
            _moduleBpu = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BPUGeneral });
            _opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            _opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            _roleGroup = GetEntityFactory<RoleGroup>().Create(new {
                Name = "Role Group 1"
            });
            _roleGroupRole1 = GetEntityFactory<RoleGroupRole>().Create(new {
                Module = _moduleBapp,
                Action = _readAction,
                OperatingCenter = _opc1,
                RoleGroup = _roleGroup,
            });
            _roleGroupRole2 = GetEntityFactory<RoleGroupRole>().Create(new {
                Module = _moduleBpu,
                Action = _readAction,
                OperatingCenter = _opc2,
                RoleGroup = _roleGroup,
            });

            _currentUser = GetEntityFactory<User>().Create(new { IsUserAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_currentUser);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = new Mock<IAuthenticationService<User>>();
            e.For<IAuthenticationService<User>>().Use(_authServ.Object);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // noop, this all has to be tested manually.
        }

        [TestMethod]
        public void TestMapToEntityOnlyRemovesRolesWithIdsThatMatchRoleGroupsToRemove()
        {
            var roleGroup1 = GetEntityFactory<RoleGroup>().Create(new {
                Users = new List<User> { _entity }
            });

            var roleGroup2 = GetEntityFactory<RoleGroup>().Create(new {
                Users = new List<User> { _entity }
            });

            var roleGroup3 = GetEntityFactory<RoleGroup>().Create(new {
                Users = new List<User> { _entity }
            });

            _entity.RoleGroups.Add(roleGroup1);
            _entity.RoleGroups.Add(roleGroup2);
            _entity.RoleGroups.Add(roleGroup3);

            _viewModel.RoleGroupsToRemove = new[] { roleGroup1.Id, roleGroup2.Id };

            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.RoleGroups.Contains(roleGroup1));
            Assert.IsFalse(_entity.RoleGroups.Contains(roleGroup2));
            Assert.IsTrue(_entity.RoleGroups.Contains(roleGroup3));
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsIfCurrentUserDoesNotHaveUserAdminAccessForAllRolesInAllRoleGroupsBeingRemoved()
        {
            var otherRoleGroup = GetEntityFactory<RoleGroup>().Create(new { Name = "Other Role Group" });
            var otherRoleGroupRole = GetEntityFactory<RoleGroupRole>().Create(new {
                Module = _moduleBapp,
                Action = _readAction,
                OperatingCenter = _opc2,
                RoleGroup = otherRoleGroup,
            });
            
            var roleGroupThatStays = GetEntityFactory<RoleGroup>().Create();
            var roleGroupRoleThatStays = GetEntityFactory<RoleGroupRole>().Create(new {
                Module = _moduleBpu,
                Action = _readAction,
                OperatingCenter = _opc1,
                RoleGroup = roleGroupThatStays,
            });

            _entity.RoleGroups.Add(_roleGroup);
            _entity.RoleGroups.Add(otherRoleGroup);
            _entity.RoleGroups.Add(roleGroupThatStays);
            _viewModel.RoleGroupsToRemove = new[] { _roleGroup.Id, otherRoleGroup.Id }; 

            // A UserAdministrator role needs to be created for each role in the RoleGroup.
            // So creating only one should fail, but creating both should pass.
            GetEntityFactory<Role>().Create(new {
                Module = _roleGroupRole1.Module,
                Action = _userAdminAction,
                OperatingCenter = _roleGroupRole1.OperatingCenter,
                User = _currentUser
            });
            Session.Refresh(_currentUser);
            
            // Should see a validation error for *both* role groups.
            ValidationAssert.ModelStateHasNonPropertySpecificError("You do not have the ability to remove the \"Role Group 1\" role group as you do not have the user administrator access to all of the roles within the role group.");
            ValidationAssert.ModelStateHasNonPropertySpecificError("You do not have the ability to remove the \"Other Role Group\" role group as you do not have the user administrator access to all of the roles within the role group.");

            // Adding the missing role access for both role groups
            // should then allow the test to pass.
            GetEntityFactory<Role>().Create(new {
                Module = _roleGroupRole2.Module,
                Action = _userAdminAction,
                OperatingCenter = _roleGroupRole2.OperatingCenter,
                User = _currentUser
            });
            GetEntityFactory<Role>().Create(new {
                Module = otherRoleGroupRole.Module,
                Action = _userAdminAction,
                OperatingCenter = otherRoleGroupRole.OperatingCenter,
                User = _currentUser
            });
            Session.Refresh(_currentUser);

            ValidationAssert.ModelStateIsValid();
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.RoleGroupsToRemove);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            _currentUser.IsAdmin = true;
            _entity.RoleGroups.Add(_roleGroup);

            _viewModel.RoleGroupsToRemove = new[] { -1 };
            ValidationAssert.ModelStateHasError(x => x.RoleGroupsToRemove, "RoleGroupsToRemove's value does not match an existing object.");

            _viewModel.RoleGroupsToRemove = new[] { _roleGroup.Id };
            //also not working same issue...
            ValidationAssert.ModelStateIsValid(x => x.RoleGroupsToRemove);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop, model has no string length validators.
        }

        #endregion

        #endregion
    }
}
