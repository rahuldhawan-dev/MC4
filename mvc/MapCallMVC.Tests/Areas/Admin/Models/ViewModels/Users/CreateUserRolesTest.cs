using System.Linq;
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
    public class CreateUserRolesTest : ViewModelTestBase<User, CreateUserRoles>
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

        protected override CreateUserRoles CreateViewModel()
        {
            var vm = base.CreateViewModel();
            vm.Modules = new int[] { };
            vm.Actions = new int[] { };
            vm.OperatingCenters = new int[] { };
            return vm;
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
        public void TestMapToEntityCreatesRoleForEachOperatingCenterModuleAndAction()
        {
            // Sanity to make sure we're not pulling in unused actions/modules/operating centers.
            var unusedAction = GetEntityFactory<RoleAction>().Create();
            var unusedModule = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BusinessPerformanceGeneral});
            var unusedOpc = GetFactory<UniqueOperatingCenterFactory>().Create();

            _viewModel.IsForAllOperatingCenters = false;
            _viewModel.OperatingCenters = new[] { _opc1.Id, _opc2.Id };
            _viewModel.Modules = new[] { _module1.Id, _module2.Id };
            _viewModel.Actions = new[] { _readAction.Id, _userAdminAction.Id };

            _vmTester.MapToEntity();

            Assert.AreEqual(8, _entity.Roles.Count);
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _readAction && x.Module == _module1 && x.OperatingCenter == _opc1));
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _userAdminAction && x.Module == _module1 && x.OperatingCenter == _opc1));
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _readAction && x.Module == _module2 && x.OperatingCenter == _opc1));
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _userAdminAction && x.Module == _module2 && x.OperatingCenter == _opc1));
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _readAction && x.Module == _module1 && x.OperatingCenter == _opc2));
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _userAdminAction && x.Module == _module1 && x.OperatingCenter == _opc2));
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _readAction && x.Module == _module2 && x.OperatingCenter == _opc2));
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _userAdminAction && x.Module == _module2 && x.OperatingCenter == _opc2));
        }
        
        [TestMethod]
        public void TestMapToEntitySetsOperatingCenterToNullOnAllNewRolesIfIsForAllOperatingCentersIsTrue()
        {
            _viewModel.Id = _entity.Id;
            _viewModel.IsForAllOperatingCenters = true;
            _viewModel.Modules = new[] { _module1.Id };
            _viewModel.Actions = new[] { _readAction.Id };

            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.Roles.Count);
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _readAction && x.Module == _module1 && x.OperatingCenter == null));
        }

        [TestMethod]
        public void TestMapToEntityDoesNotCreateAdditionalRolesForSpecificOperatingCentersIfIsForAllOperatingCentersIsTrue()
        {
            _viewModel.IsForAllOperatingCenters = true;
            _viewModel.OperatingCenters = new[] { _opc1.Id, _opc2.Id };
            _viewModel.Modules = new[] { _module1.Id, _module2.Id };
            _viewModel.Actions = new[] { _readAction.Id, _userAdminAction.Id };

            _vmTester.MapToEntity();

            Assert.AreEqual(4, _entity.Roles.Count, "There would be 12 if this was implemented incorrectly.");
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _readAction && x.Module == _module1 && x.OperatingCenter == null));
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _userAdminAction && x.Module == _module1 && x.OperatingCenter == null));
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _readAction && x.Module == _module2 && x.OperatingCenter == null));
            Assert.IsTrue(_entity.Roles.Any(x => x.Action == _userAdminAction && x.Module == _module2 && x.OperatingCenter == null));
        }

        [TestMethod]
        public void TestMapToEntityDoesNotAddDuplicateRolesForPreexistingRoles()
        {
            var expectedRole = new Role {
                Module = _module1,
                Action = _readAction,
                OperatingCenter = _opc1
            };
            _entity.Roles.Add(expectedRole);
            _viewModel.IsForAllOperatingCenters = false;
            _viewModel.OperatingCenters = new[] { _opc1.Id };
            _viewModel.Modules = new[] { _module1.Id };
            _viewModel.Actions = new[] { _readAction.Id };

            _vmTester.MapToEntity();

            Assert.AreEqual(1, _entity.Roles.Count);
            Assert.AreSame(expectedRole, _entity.Roles.Single());
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsIfUserDoesNotHaveUserAdminAccessForModuleAndOperatingCenter()
        { 
            var expectedRole = new AggregateRole {
                Module = _module1,
                Action = _userAdminAction,
                OperatingCenter = _opc1
            };

            _currentUser.AggregateRoles.Clear();

            _viewModel.IsForAllOperatingCenters = false;
            _viewModel.OperatingCenters = new[] { _opc1.Id };
            _viewModel.Modules = new[] { _module1.Id };
            _viewModel.Actions = new[] { _readAction.Id };

            ValidationAssert.ModelStateHasError(x => x.Modules,
                $"You do not have the ability to administate the role module {(RoleModules)_module1.Id} in operating center {_opc1}.");

            _currentUser.AggregateRoles.Add(expectedRole);
            ValidationAssert.ModelStateIsValid(x => x.Modules);
        }

        [TestMethod]
        public void TestValidationFailsIfCurrentUserTriesToAddWildcardRoleToModuleTheyDoNotHaveWildcardRoleThemselves()
        {
            var expectedRole = new AggregateRole {
                Module = _module1,
                Action = _userAdminAction,
                OperatingCenter = null
            };

            _currentUser.AggregateRoles.Clear();

            _viewModel.IsForAllOperatingCenters = true;
            _viewModel.OperatingCenters = null;
            _viewModel.Modules = new[] { _module1.Id };
            _viewModel.Actions = new[] { _readAction.Id };

            ValidationAssert.ModelStateHasError(x => x.Modules,
                $"You do not have the ability to administate the role module {(RoleModules)_module1.Id} in operating center ALL.");

            _currentUser.AggregateRoles.Add(expectedRole);
            ValidationAssert.ModelStateIsValid(x => x.Modules);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Actions);
            ValidationAssert.PropertyIsRequired(x => x.Modules);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop, done individually since these are array properties.
        }
        
        [TestMethod]
        public void TestActionsEntityMustExist()
        {
            _viewModel.Actions = new[] { -1 };
            ValidationAssert.ModelStateHasError(x => x.Actions, "Actions's value does not match an existing object.");

            _viewModel.Actions = new[] { _readAction.Id };
            ValidationAssert.ModelStateIsValid(x => x.Actions);
        }

        [TestMethod]
        public void TestModulesEntityMustExist()
        {
            _viewModel.Modules = new[] { -1 };
            ValidationAssert.ModelStateHasError(x => x.Modules, "Modules's value does not match an existing object.");

            _viewModel.Modules = new[] { _module1.Id };
            ValidationAssert.ModelStateIsValid(x => x.Modules);
        }

        [TestMethod]
        public void TestOperatingCentersEntityMustExist()
        {
            _viewModel.OperatingCenters = new[] { -1 };
            ValidationAssert.ModelStateHasError(x => x.OperatingCenters, "OperatingCenters's value does not match an existing object.");

            _viewModel.OperatingCenters = new[] { _opc1.Id };
            ValidationAssert.ModelStateIsValid(x => x.OperatingCenters);
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
