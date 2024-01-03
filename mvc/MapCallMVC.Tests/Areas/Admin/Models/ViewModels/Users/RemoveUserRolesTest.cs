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
    public class RemoveUserRolesTest : ViewModelTestBase<User, RemoveUserRoles>
    {
        #region Fields

        private RoleAction _readAction, _userAdminAction;
        private Module _module1, _module2;
        private OperatingCenter _opc1, _opc2;
        private User _currentUser;
        private Mock<IAuthenticationService<User>> _authServ;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _readAction = GetEntityFactory<RoleAction>().Create(new { Id = (int)RoleActions.Read });
            _userAdminAction = GetEntityFactory<RoleAction>().Create(new { Id = (int)RoleActions.UserAdministrator });
            _module1 = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BAPPTeamSharingGeneral});
            _module2 = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BPUGeneral});
            _opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            _opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
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
        public void TestMapToEntityOnlyRemovesRolesWithIdsThatMatchRolesToRemove()
        {
            var role1 = GetEntityFactory<Role>().Create(new {
                Module = _module1,
                Action = _userAdminAction,
                OperatingCenter = _opc1,
                User = _entity
            });
            var role2 = GetEntityFactory<Role>().Create(new {
                Module = _module1,
                Action = _userAdminAction,
                OperatingCenter = _opc1,
                User = _entity
            });
            var role3 = GetEntityFactory<Role>().Create(new {
                Module = _module1,
                Action = _userAdminAction,
                OperatingCenter = _opc1,
                User = _entity
            });

            _viewModel.RolesToRemove = new[] { role1.Id, role2.Id };

            _vmTester.MapToEntity();

            Assert.IsFalse(_entity.Roles.Contains(role1));
            Assert.IsFalse(_entity.Roles.Contains(role1));
            Assert.IsTrue(_entity.Roles.Contains(role3));
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsIfUserDoesNotHaveUserAdminAccessForModuleAndOperatingCenter()
        {
            var expectedRoleForCurrentUser = GetEntityFactory<Role>().Create(new {
                Module = _module1,
                Action = _userAdminAction,
                OperatingCenter = _opc1,
                User = _currentUser
            });
            var expectedRoleForTargetUser = GetEntityFactory<Role>().Create(new {
                Module = _module1,
                Action = _userAdminAction,
                OperatingCenter = _opc1,
                User = _entity
            });

            _currentUser.AggregateRoles.Clear();
            _viewModel.RolesToRemove = new[] { expectedRoleForTargetUser.Id }; 

            ValidationAssert.ModelStateHasError(x => x.RolesToRemove,
                $"You do not have the ability to administate the role module {(RoleModules)_module1.Id} in operating center {_opc1}.");

            _currentUser.Roles.Add(expectedRoleForCurrentUser);
            Session.Refresh(_currentUser); // Forces the AggregateRoles property to reload
            ValidationAssert.ModelStateIsValid(x => x.RolesToRemove);
        }
        
        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.RolesToRemove);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            var expectedRoleForCurrentUser = GetEntityFactory<Role>().Create(new {
                Module = _module1,
                Action = _userAdminAction,
                OperatingCenter = _opc1,
                User = _currentUser
            });
            var expectedRoleForTargetUser = GetEntityFactory<Role>().Create(new {
                Module = _module1,
                Action = _userAdminAction,
                OperatingCenter = _opc1,
                User = _entity
            });
            Session.Refresh(_currentUser); // Forces the AggregateRoles property to reload
            
            _viewModel.RolesToRemove = new[] { -1 };
            ValidationAssert.ModelStateHasError(x => x.RolesToRemove, "RolesToRemove's value does not match an existing object.");

            _viewModel.RolesToRemove = new[] { expectedRoleForTargetUser.Id };
            ValidationAssert.ModelStateIsValid(x => x.RolesToRemove);
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
