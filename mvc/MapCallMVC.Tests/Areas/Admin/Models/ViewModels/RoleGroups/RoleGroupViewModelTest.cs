using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Admin.Models.ViewModels.RoleGroups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Admin.Models.ViewModels.RoleGroups
{
    [TestClass]
    public class RoleGroupViewModelTest : ViewModelTestBase<RoleGroup, RoleGroupViewModel>
    {
        #region Fields

        private RoleAction _readAction, _userAdminAction;
        private Module _module1, _module2;
        private OperatingCenter _opc1, _opc2;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void BaseTestInitialize()
        {
            _readAction = GetEntityFactory<RoleAction>().Create(new { Id = (int)RoleActions.Read });
            _userAdminAction = GetEntityFactory<RoleAction>().Create(new { Id = (int)RoleActions.UserAdministrator });
            _module1 = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BAPPTeamSharingGeneral});
            _module2 = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BPUGeneral});
            _opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            _opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestExistingRolesPropertyReturnsRolesFromRoleGroupWhenIdIsGreaterThanZero()
        {
            var expectedRTR = new RoleGroupRole();
            _entity.Roles.Add(expectedRTR);

            Assert.IsTrue(_viewModel.ExistingRoles.Contains(expectedRTR));
        }

        [TestMethod]
        public void TestExistingRolesPropertyReturnsEmptyEnumerableWhenIdIsZero()
        {
            _viewModel.Id = 0;
            Assert.AreSame(Enumerable.Empty<RoleGroupRole>(), _viewModel.ExistingRoles);
        }

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Name);
        }

        [TestMethod]
        public void TestMapToEntityAddsNewRoleGroupRolesToEntity()
        {
            var groupRoleVm = _viewModelFactory.Build<CreateRoleGroupRole>();
            groupRoleVm.OperatingCenter = _opc1.Id;
            groupRoleVm.Module = _module1.Id;
            groupRoleVm.Action = _readAction.Id;
            _viewModel.NewRoles = new List<CreateRoleGroupRole>();
            _viewModel.NewRoles.Add(groupRoleVm);

            _vmTester.MapToEntity();

            var entityRole = _entity.Roles.Single();
            Assert.AreSame(_opc1, entityRole.OperatingCenter);
            Assert.AreSame(_module1, entityRole.Module);
            Assert.AreSame(_readAction, entityRole.Action);
        }
        
        [TestMethod]
        public void TestMapToEntityRemovesRoleGroup()
        {
            var groupRoleToRemove = GetEntityFactory<RoleGroupRole>().Create(new { RoleGroup = _entity, OperatingCenter = _opc1, Module = _module1, Action = _readAction });
            var groupRoleToKeep = GetEntityFactory<RoleGroupRole>().Create(new { RoleGroup = _entity, OperatingCenter = _opc2, Module = _module1, Action = _readAction });
            _viewModel.RolesToRemove = new[] { groupRoleToRemove.Id };

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.Roles.Contains(groupRoleToKeep));
            Assert.IsFalse(_entity.Roles.Contains(groupRoleToRemove));
        }

        [TestMethod]
        public void TestMapToEntityDoesNotErrorIfUserDoesNotHaveAMatchingRoleWhenRemovingRoles()
        {
            // So basically, this tests that the user's roles weren't modified since the role
            // being removed is a role they did not have in the first place.
            var groupRole = GetEntityFactory<RoleGroupRole>().Create(new { RoleGroup = _entity, OperatingCenter = _opc1, Module = _module1, Action = _readAction });
            var user = GetEntityFactory<User>().Create();
            var userRoleToKeep = GetEntityFactory<Role>().Create(new { User = user, OperatingCenter = _opc2, Module = _module2, Action = _readAction });
            _entity.Users.Add(user);
            _viewModel.RolesToRemove = new[] { groupRole.Id }; 

            _vmTester.MapToEntity();

            Assert.AreEqual(1, user.Roles.Count);
            Assert.AreSame(userRoleToKeep, user.Roles.Single());
        }

        [TestMethod]
        public void TestMapToEntityDoesNotRemoveARoleIfTheUserHasAnotherGroupAssignedToThemWithAnIdenticalRole()
        {
            var otherGroup = GetEntityFactory<RoleGroup>().Create();
            var otherGroupRole = GetEntityFactory<RoleGroupRole>().Create(new { RoleGroup = otherGroup, OperatingCenter = _opc1, Module = _module1, Action = _readAction });
            var groupRoleToRemove = GetEntityFactory<RoleGroupRole>().Create(new { RoleGroup = _entity, OperatingCenter = _opc1, Module = _module1, Action = _readAction });
            var user = GetEntityFactory<User>().Create();
            var userRoleToKeep = GetEntityFactory<Role>().Create(new { User = user, OperatingCenter = _opc1, Module = _module1, Action = _readAction });
            _entity.Users.Add(user);
            user.RoleGroups.Add(_entity);
            otherGroup.Users.Add(user);
            user.RoleGroups.Add(otherGroup);
            _viewModel.RolesToRemove = new[] { groupRoleToRemove.Id };

            _vmTester.MapToEntity();

            Assert.AreEqual(1, user.Roles.Count);
            Assert.AreSame(userRoleToKeep, user.Roles.Single());
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsIfNameIsNotUnique()
        {
            var otherGroup = GetEntityFactory<RoleGroup>().Create();
            _viewModel.Name = otherGroup.Name;

            ValidationAssert.ModelStateHasError(x => x.Name, "This group name is already in used.");

            _viewModel.Name = "Some other name";
            ValidationAssert.ModelStateIsValid(x => x.Name);
        }

        [TestMethod]
        public void TestValidationPassesIfTheNameOnlyExistsForTheCurrentRecordBeingEdited()
        {
            Assert.AreNotEqual(0, _viewModel.Id, "Sanity. Ensures we're not suddenly testing a view model that references an unsaved entity.");
            _viewModel.Name = _entity.Name;
            ValidationAssert.ModelStateIsValid(x => x.Name);
        }

        [TestMethod]
        public void TestValidationFailsIfRolesToRemoveContainsIdOfRoleGroupRoleThatBelongsToADifferentRoleGroup()
        {
            var roleGroupRole = GetEntityFactory<RoleGroupRole>().Create(new { RoleGroup = _entity });
            var badRoleGroupRole = GetEntityFactory<RoleGroupRole>().Create();
            _viewModel.RolesToRemove = new[] { roleGroupRole.Id, badRoleGroupRole.Id };

            ValidationAssert.ModelStateHasError(x => x.RolesToRemove, "Unable to remove one or more roles because they do not belong to this role group.");

            _viewModel.RolesToRemove = new[] { roleGroupRole.Id };
            ValidationAssert.ModelStateIsValid(x => x.RolesToRemove);
        }
        
        [TestMethod]
        public void TestValidationFailsIfDuplicateNewRolesAreFoundInTheViewModel()
        {
            var role1 = _viewModelFactory.Build<CreateRoleGroupRole>();
            role1.Action = 1;
            role1.Module = 1;
            role1.OperatingCenter = 1;
            var role2 = _viewModelFactory.Build<CreateRoleGroupRole>();
            role2.Action = 1;
            role2.Module = 1;
            role2.OperatingCenter = 1;
            var role3 = _viewModelFactory.Build<CreateRoleGroupRole>();
            role3.Action = 1;
            role3.Module = 1;
            role3.OperatingCenter = null;

            _viewModel.NewRoles = new List<CreateRoleGroupRole>();
            _viewModel.NewRoles.Add(role1);
            _viewModel.NewRoles.Add(role2);

            // Should fail because there's two duplicates.
            ValidationAssert.ModelStateHasError(x => x.NewRoles, "Duplicate new roles were found.");

            // Should pass because theere's only one role.
            _viewModel.NewRoles.Remove(role2);
            ValidationAssert.ModelStateIsValid(x => x.NewRoles);

            // Should pass because there's two distinctly different roles.
            _viewModel.NewRoles.Add(role3);
            ValidationAssert.ModelStateIsValid(x => x.NewRoles);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Name);
        }
        
        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // There's only one actual entity-type property to test.
            var roleGroupRole = GetEntityFactory<RoleGroupRole>().Create(new { RoleGroup = _entity });
            _viewModel.RolesToRemove = new[] { 0 };
            ValidationAssert.ModelStateHasError(x => x.RolesToRemove, "RolesToRemove's value does not match an existing object.");

            _viewModel.RolesToRemove = new[] { roleGroupRole.Id };
            ValidationAssert.ModelStateIsValid(x => x.RolesToRemove);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.Name, RoleGroup.StringLengths.NAME);
        }

        #endregion

        #endregion
    }
}
