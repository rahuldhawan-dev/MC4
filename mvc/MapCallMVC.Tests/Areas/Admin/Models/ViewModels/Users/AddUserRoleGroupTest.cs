using System.Collections.Generic;
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
    public class AddUserRoleGroupTest : ViewModelTestBase<User, AddUserRoleGroup>
    {
        #region Fields

        private RoleAction _readAction, _userAdminAction;
        private Module _module1, _module2;
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
            _module1 = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BAPPTeamSharingGeneral});
            _module2 = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BPUGeneral});
            _opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            _opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            _roleGroup = GetEntityFactory<RoleGroup>().Create();
            _roleGroupRole1 = GetEntityFactory<RoleGroupRole>().Create(new {
                Module = _module1,
                Action = _readAction,
                OperatingCenter = _opc1,
                RoleGroup = _roleGroup,
            });
            _roleGroupRole2 = GetEntityFactory<RoleGroupRole>().Create(new {
                Module = _module2,
                Action = _readAction,
                OperatingCenter = _opc2,
                RoleGroup = _roleGroup,
            });
            
            _currentUser = GetEntityFactory<User>().Create(new { IsUserAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_currentUser);
        }

        protected override AddUserRoleGroup CreateViewModel()
        {
            var vm = base.CreateViewModel();
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
        public void TestMapToEntityAddsRoleGroupToTheUser()
        {
            _viewModel.RoleGroup = _roleGroup.Id;

            _vmTester.MapToEntity();

            Assert.AreSame(_roleGroup, _entity.RoleGroups.Single());
        }
        
        [TestMethod]
        public void TestMapToEntityDoesNotAddDuplicateRoleGroupsToTheUser()
        {
            _viewModel.RoleGroup = _roleGroup.Id;

            Assert.IsTrue(_entity.RoleGroups.Count() == 0);

            _vmTester.MapToEntity();

            Assert.IsTrue(_entity.RoleGroups.Count() == 1);

            _vmTester.MapToEntity();
            
            Assert.IsTrue(_entity.RoleGroups.Count() == 1);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationPassesIfCurrentUserHasUserAdminAccessForAllModulesAndOperatingCentersInTheSelectedRoleGroup()
        {
            _viewModel.RoleGroup = _roleGroup.Id;
         
            // A UserAdministrator role needs to be created for each role in the RoleGroup.
            // So creating only one should fail, but creating both should pass.
            GetEntityFactory<Role>().Create(new {
                Module = _roleGroupRole1.Module,
                Action = _userAdminAction,
                OperatingCenter = _roleGroupRole1.OperatingCenter,
                User = _currentUser
            });
            Session.Refresh(_currentUser);

            Assert.IsTrue(_roleGroup.Roles.Any());
            
            ValidationAssert.ModelStateHasNonPropertySpecificError("You do not have the ability to add this user role group as you do not have the user administrator access to all of the roles within the role group.");

            GetEntityFactory<Role>().Create(new {
                Module = _roleGroupRole2.Module,
                Action = _userAdminAction,
                OperatingCenter = _roleGroupRole2.OperatingCenter,
                User = _currentUser
            });
            Session.Refresh(_currentUser);
            ValidationAssert.ModelStateIsValid();
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.RoleGroup);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop, done individually since these are array properties.
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
