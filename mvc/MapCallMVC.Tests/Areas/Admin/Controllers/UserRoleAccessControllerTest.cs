using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Admin.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCallMVC.Areas.Admin.Models.ViewModels.Users;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Admin.Controllers
{
    [TestClass]
    public class UserRoleAccessControllerTest : MapCallMvcControllerTestBase<UserRoleAccessController, User>
    {
        #region Fields

        private User _targetUser;
        private RoleAction _userAdminAction, _readAction;
        private OperatingCenter _opc1, _opc2;
        private Module _module1, _module2;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _targetUser = GetEntityFactory<User>().Create();
            _readAction = GetEntityFactory<RoleAction>().Create(new { Id = (int)RoleActions.Read });
            _userAdminAction = GetEntityFactory<RoleAction>().Create(new { Id = (int)RoleActions.UserAdministrator });
            _opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            _opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var app = GetEntityFactory<Application>().Create(new { Id = RoleApplications.Operations });
            _module1 = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BAPPTeamSharingGeneral, Application = app });    
            _module2 = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.ContractorsGeneral, Application = app });

            // I have absolutely no idea why, but the modules are not being saved/flushed properly when
            // returned from the factory. I haven't seen this problem anywhere else that uses the same module factory.
            Session.Save(_module1);
            Session.Save(_module2);
            Session.Flush();
        }

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new {
                IsUserAdmin = true
            });
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.NewReturnsPartialView = true;
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "User", area = string.Empty, id = vm.Id };
            options.CreateRedirectsToRouteOnErrorArgs = (vm) => new { action = "Show", controller = "User", area = string.Empty, id = vm.Id };
            options.CreateValidEntity = () => _targetUser;
            options.InitializeCreateViewModel = (vm) => {
                _currentUser.IsAdmin = true;
                var model = (CreateUserRoles)vm;
                model.Id = _targetUser.Id;
                model.Modules = new[] { _module1.Id };
                model.Actions = new[] { _readAction.Id };
                model.OperatingCenters = new[] { _opc1.Id };
            };
            options.DestroyRedirectsToRouteOnErrorArgs = (vm) => new { action = "Show", controller = "User", area = string.Empty, id = vm.Id };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresUserAdminUser("~/Admin/UserRoleAccess/New/");
                a.RequiresUserAdminUser("~/Admin/UserRoleAccess/Create/");
                a.RequiresUserAdminUser("~/Admin/UserRoleAccess/Destroy/");
                a.RequiresSiteAdminUser("~/Admin/UserRoleAccess/NewForRoleGroup/");
            });
        }

        #region New

        [TestMethod]
        public void TestNewDisplaysAllOperatingCentersAndModulesForSiteAdmins()
        {
            _currentUser.IsAdmin = true;
            Session.Save(_currentUser);

            _target.New(_targetUser.Id);

            var opCenters = (IEnumerable<OperatingCenterDisplayItem>)_target.ViewData["OperatingCenters"];
            Assert.IsNull(opCenters, "There shouldn't be anything in ViewData for OperatingCenters as this will be autopopulated by ControlHelper in the view.");

            var modules = (IEnumerable<ModuleDisplayItem>)_target.ViewData["Modules"];
            Assert.IsNull(modules, "There shouldn't be anything in ViewData for Modules as this will be autopopulated by ControlHelper in the view.");
        }

        [TestMethod]
        public void TestNewDisplaysOnlyOperatingCentersAndModulesThatCurrentUserAdminHasAUserAdminRoleFor()
        {
            var goodRole = GetEntityFactory<Role>().Create(new { User = _currentUser, Action = _userAdminAction, Module = _module1, OperatingCenter = _opc1 });
            var badRole = GetEntityFactory<Role>().Create(new { User = _currentUser, Action = _readAction, Module = _module2, OperatingCenter = _opc2 });
            Session.Refresh(_currentUser);
            _target.New(_targetUser.Id);

            var opCenters = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenters"];
            Assert.IsTrue(opCenters.Any(x => x.Value == _opc1.Id.ToString()));
            Assert.IsFalse(opCenters.Any(x => x.Value == _opc2.Id.ToString()));

            var modules = (IEnumerable<SelectListItem>)_target.ViewData["Modules"];
            Assert.IsTrue(modules.Any(x => x.Value == _module1.Id.ToString()));
            Assert.IsFalse(modules.Any(x => x.Value == _module2.Id.ToString()));
        }
        
        [TestMethod]
        public void TestNewDisplaysAllOperatingCentersWhenCurrentUserAdminHasAUserAdminRoleWithALLOperatingCenters()
        {
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new { User = _currentUser, Action = _userAdminAction, Module = _module1 });
            Session.Save(role);
            _currentUser.AggregateRoles.Add(new AggregateRole(role));
            _target.New(_targetUser.Id);

            var opCenters = (IEnumerable<OperatingCenterDisplayItem>)_target.ViewData["OperatingCenters"];
            Assert.IsNull(opCenters, "There shouldn't be anything in ViewData for OperatingCenters as this will be autopopulated by ControlHelper in the view.");
        }

        [TestMethod]
        public void TestNewSetsUserCanAdministrateAllOperatingCentersIfUserIsSiteAdmin()
        {
            _currentUser.IsAdmin = true;

            var resultModel = (CreateUserRoles)((ViewResultBase)_target.New(_targetUser.Id)).Model;
            Assert.IsTrue(resultModel.UserCanAdministrateAllOperatingCenters);
        }

        [TestMethod]
        public void TestNewSetsUserCanAdministrateAllOperatingCentersIfUserAdministratorCanAdministrateAllOperatingCenters()
        {
            var resultModel = (CreateUserRoles)((ViewResultBase)_target.New(_targetUser.Id)).Model;
            Assert.IsFalse(resultModel.UserCanAdministrateAllOperatingCenters);

            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new { User = _currentUser, Action = _userAdminAction, Module = _module1 });
            Session.Save(role);
            _currentUser.AggregateRoles.Add(new AggregateRole(role));

            _target.New(_targetUser.Id);

            resultModel = (CreateUserRoles)((ViewResultBase)_target.New(_targetUser.Id)).Model;
            Assert.IsTrue(resultModel.UserCanAdministrateAllOperatingCenters);
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // Test override needed because of the param on New.

            var result = _target.New(_targetUser.Id);
            MvcAssert.IsPartialView(result);
            MvcAssert.IsViewNamed(result, "_New");
            Assert.IsInstanceOfType(((ViewResultBase)result).Model, typeof(CreateUserRoles));
        }

        #endregion

        #region NewForRoleGroup

        [TestMethod]
        public void TestNewForRoleGroupReturnsNewViewWithNewViewModel()
        {
            var result = _target.NewForRoleGroup();
            MvcAssert.IsPartialView(result);
            MvcAssert.IsViewNamed(result, "_New");
            Assert.IsInstanceOfType(((ViewResultBase)result).Model, typeof(CreateUserRoles));
        }
        
        [TestMethod]
        public void TestNewForRoleGroupDisplaysAllOperatingCentersAndModulesForSiteAdmins()
        {
            _currentUser.IsAdmin = true;
            Session.Save(_currentUser);

            _target.NewForRoleGroup();

            var opCenters = (IEnumerable<OperatingCenterDisplayItem>)_target.ViewData["OperatingCenters"];
            Assert.IsNull(opCenters, "There shouldn't be anything in ViewData for OperatingCenters as this will be autopopulated by ControlHelper in the view.");

            var modules = (IEnumerable<ModuleDisplayItem>)_target.ViewData["Modules"];
            Assert.IsNull(modules, "There shouldn't be anything in ViewData for Modules as this will be autopopulated by ControlHelper in the view.");
        }

        [TestMethod]
        public void TestNewForRoleGroupSetsUserCanAdministrateAllOperatingCentersToTrue()
        {
            var result = _target.NewForRoleGroup();
            var model = (CreateUserRoles)((PartialViewResult)result).Model;
            Assert.IsTrue(model.UserCanAdministrateAllOperatingCenters);
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // Test override needed because there's no way for the base test thinks
            // we'd be creating a new User object.
            var currentUserRole = GetEntityFactory<Role>().Create(new { User = _currentUser, Action = _userAdminAction, Module = _module1, OperatingCenter = _opc1 });
            Session.Refresh(_currentUser);
            // _currentUser.AggregateRoles.Add(new AggregateRole(currentUserRole));
            var model = _viewModelFactory.Build<CreateUserRoles>();
            model.Id = _targetUser.Id;
            model.Modules = new[] { _module1.Id };
            model.OperatingCenters = new[] { _opc1.Id };
            model.Actions = new[] { _userAdminAction.Id };

            _target.Create(model);

            Assert.IsTrue(_targetUser.Roles.Any(x => x.Action == _userAdminAction && x.Module == _module1 && x.OperatingCenter == _opc1 && x.User == _targetUser));
        }

        #endregion

        #region Destroy

        [TestMethod]
        public override void TestDestroyActuallyDeletesTheRecordAndOnlyTheRecord()
        {
            // Test override needed because we aren't deleting the User, but removing
            // roles from the User object.

            var currentUserRole = GetEntityFactory<Role>().Create(new { User = _currentUser, Action = _userAdminAction, Module = _module1, OperatingCenter = _opc1 });
            var targetUserRole = GetEntityFactory<Role>().Create(new { User = _targetUser, Action = _userAdminAction, Module = _module1, OperatingCenter = _opc1 });
            var otherTargetUserRole = GetEntityFactory<Role>().Create(new { User = _targetUser, Action = _userAdminAction, Module = _module1, OperatingCenter = _opc2 });
            Session.Refresh(_currentUser);
            var model = _viewModelFactory.Build<RemoveUserRoles>();
            model.Id = targetUserRole.Id;
            model.RolesToRemove = new[] { targetUserRole.Id };

            // Something about this test is weird and it requires the User/Role to be evicted first.
            // Otherwise, even though the code is removing the Role from the User, that change never
            // gets persisted to the database. The delete statement never runs.
            Session.Evict(_targetUser);
            Session.Evict(targetUserRole);
            _targetUser = Session.Query<User>().Single(x => x.Id == _targetUser.Id);

            var result = _target.Destroy(model);

            Assert.IsTrue(_target.ModelState.IsValid, "Sanity.");
            Assert.IsNull(Session.Query<Role>().SingleOrDefault(x => x.Id == targetUserRole.Id));
            Assert.IsNotNull(Session.Query<Role>().SingleOrDefault(x => x.Id == otherTargetUserRole.Id));
        }

        [TestMethod]
        public override void TestDestroyRedirectsToSearchPageWhenRecordIsSuccessfullyDestroyed()
        {
            // Test needs to be overridden because it uses a model rather an int
            // also it redirects to the User/Show page rather than a Search page.

            var currentUserRole = GetEntityFactory<Role>().Create(new { User = _currentUser, Action = _userAdminAction, Module = _module1, OperatingCenter = _opc1 });
            var targetUserRole = GetEntityFactory<Role>().Create(new { User = _targetUser, Action = _userAdminAction, Module = _module1, OperatingCenter = _opc1 });
            var otherTargetUserRole = GetEntityFactory<Role>().Create(new { User = _targetUser, Action = _userAdminAction, Module = _module1, OperatingCenter = _opc2 });
            var model = _viewModelFactory.Build<RemoveUserRoles>();
            model.Id = targetUserRole.Id;
            model.RolesToRemove = new[] { targetUserRole.Id };

            var result = _target.Destroy(model);
            MvcAssert.RedirectsToRoute(result, new { action = "Show", controller = "User", id = _targetUser.Id, area = string.Empty });
        }

        #endregion

        #endregion
    }
}
