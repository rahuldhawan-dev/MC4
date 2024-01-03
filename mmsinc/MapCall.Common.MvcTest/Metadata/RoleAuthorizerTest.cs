using System;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing;
using Moq;
using StructureMap;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Controllers;
using MapCall.Common.Model.Repositories.Users;
using MMSINC;
using MMSINC.Testing.ClassExtensions;

namespace MapCall.Common.MvcTest.Metadata
{
    [TestClass]
    public class RoleAuthorizerTest
    {
        #region Consts

        private const RoleModules DEFAULT_REQUIRED_CONTROLLER_MODULE = RoleModules.FieldServicesDataLookups,
                                  DEFAULT_ALTERNATE_REQUIRED_CONTROLLER_MODULE = RoleModules.FieldServicesAssets,
                                  DEFAULT_REQUIRED_ACTION_MODULE = RoleModules.FieldServicesImages,
                                  DEFAULT_ALTERNATE_REQUIRED_ACTION_MODULE = RoleModules.FieldServicesReports;

        private const string EXPECTED_ERROR_VIEW_NAME = "~/Errrrrrrr/oooor";

        #endregion

        #region Fields

        private RoleAuthorizer _target;
        private Mock<IAuthenticationService<User>> _authServ;
        private RoleService _roleServ;
        private User _user;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container(e => { e.For<IUserRepository>().Mock(); });
            _authServ = new Mock<IAuthenticationService<User>>();
            _container.Inject(_authServ.Object);
            _container.Inject<IAuthenticationService>(_authServ.Object);
            _roleServ = _container.GetInstance<RoleService>();
            _container.Inject<IRoleService>(_roleServ);
            _user = new User {
                Roles = new List<Role>()
            };
            InitAuthenticatedUser();
            _target = _container.GetInstance<RoleAuthorizer>();
            _target.ViewName = EXPECTED_ERROR_VIEW_NAME;
        }

        private void InitAuthenticatedUser()
        {
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
        }

        private AuthorizationContext CreateContext(Controller controller, string action)
        {
            var controllerContext = new ControllerContext {
                Controller = controller
            };
            var controllerDescriptor = new ReflectedControllerDescriptor(controller.GetType());
            var actionMethod = controller.GetType().GetMethod(action);
            var actionDescriptor = controllerDescriptor.GetCanonicalActions().Single(x => x.ActionName == action);

            var authContext = new AuthorizationContext(controllerContext, actionDescriptor);

            return authContext;
        }

        private AggregateRole CreateRole(RoleModules module, RoleActions action = RoleActions.Read)
        {
            var role = new AggregateRole {
                User = _user,
                Module = new Module(),
                Action = new RoleAction()
            };

            role.Module.SetPropertyValueByName("Id", (int)module);
            role.Action.SetPropertyValueByName("Id", (int)action);

            return role;
        }

        private void AssertAuthorizationPasses(AuthorizationContext context, bool callOnAuthorization = true)
        {
            if (callOnAuthorization)
            {
                _target.Authorize(new AuthorizationArgs(context));
            }

            Assert.IsNull(context.Result, "AuthorizationContext.Result should not be set when authorization succeeds.");
        }

        private void AssertAuthorizationFails(AuthorizationContext context, bool callOnAuthorization = true)
        {
            if (callOnAuthorization)
            {
                _target.Authorize(new AuthorizationArgs(context));
            }

            Assert.IsNotNull(context.Result, "AuthorizationContext.Result should not be set when authorization fails.");
            MvcAssert.IsViewNamed(context.Result, EXPECTED_ERROR_VIEW_NAME);
        }
        
        #endregion

        #region Tests

        [TestMethod]
        public void TestOnAuthorizeSetsResultToUnauthorizedViewIfUserIsNotAuthenticatedAndAControllerRoleIsRequired()
        {
            InitAuthenticatedUser();
            var expectedResult = new RedirectResult("blah");
            var context = CreateContext(new ControllerWithRequiredRolesOnSelf(), "ActionWithoutRole");
            context.Result = expectedResult;
            _target.Authorize(new AuthorizationArgs(context));
            MvcAssert.IsViewNamed(context.Result, EXPECTED_ERROR_VIEW_NAME);
        }

        [TestMethod]
        public void TestOnAuthorizeDoesNothingToUnauthorizedUsersIfNoRolesAreRequired()
        {
            InitAuthenticatedUser();
            AssertAuthorizationPasses(CreateContext(new ControllerWithNoRequiredRolesAnywhere(), "ActionWithoutRole"));
        }

        [TestMethod]
        public void TestOnAuthorizeSetsResultToUnauthorizedIfUserDoesNotHaveActionRole()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            AssertAuthorizationFails(CreateContext(new ControllerWithRequiredRolesOnActions(), "ActionWithRole"));
        }

        [TestMethod]
        public void TestOnAuthorizeSetsResultToUnauthorizedIfUserDoesNotHaveControllerRole()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            AssertAuthorizationFails(CreateContext(new ControllerWithRequiredRolesOnSelf(), "ActionWithoutRole"));
        }
        
        [TestMethod]
        public void TestOnAuthorizeReturnsNotAuthorizedWhenNotAuthorized()
        {
            _target.ReturnErrorsAsViews = false;
            
            InitAuthenticatedUser();
            _user.Roles.Clear();
            var context = CreateContext(new ControllerWithRequiredRolesOnSelf(), "ActionWithoutRole");
            
            _target.Authorize(new AuthorizationArgs(context));
            var result = (JsonHttpStatusCodeResult)context.Result;
            
            Assert.AreEqual(401, result.StatusCode);
            Assert.AreEqual(RoleAuthorizer.UNAUTHORIZED_ERROR, result.StatusDescription);
        }

        [TestMethod]
        public void TestOnAuthorizeAlsoChecksInheritedControllerLevelRoles()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            AssertAuthorizationFails(CreateContext(new ControllerWithInheritedClassLevelRoleRequirements(),
                "ActionWithoutRole"));
        }

        [TestMethod]
        public void TestOnAuthorizeAlsoChecksInheritedActionLevelRoles()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            AssertAuthorizationFails(CreateContext(new ControllerWithInheritedActionLevelRoleRequirements(),
                "SomeAction"));
        }

        [TestMethod]
        public void TestOnAuthorizeDoesNothingIfThereAreNoRequiredRolesOnControllerOrAction()
        {
            var context = CreateContext(new ControllerWithNoRequiredRolesAnywhere(), "ActionWithoutRole");
            AssertAuthorizationPasses(context);
        }

        [TestMethod]
        public void TestOnAuthorizeDoesNothingIfUserHasControllerRole()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            _user.AggregateRoles.Add(CreateRole(DEFAULT_REQUIRED_CONTROLLER_MODULE));
            var context = CreateContext(new ControllerWithRequiredRolesOnSelf(), "ActionWithoutRole");
            AssertAuthorizationPasses(context);
        }

        [TestMethod]
        public void TestOnAuthorizeDoesNothingIfUserHasControllerRoleForDynamicRoles()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            _user.AggregateRoles.Add(CreateRole(DEFAULT_REQUIRED_CONTROLLER_MODULE));
            AssertAuthorizationPasses(CreateContext(new ControllerWithDynamicRoles(), "ActionWithDataLookups"));
        }
        
        [TestMethod]
        public void TestOnAuthorizeFailsIfControllerHasDynamicRolesAndUserDoesNotHaveThoseRoles()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            AssertAuthorizationFails(CreateContext(new ControllerWithDynamicRoles(), "ActionWithCustomerGeneral"));
        }

        [TestMethod]
        public void TestOnAuthorizeFailsIfUserDoesNotHaveEveryRoleRequiredForController()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            var role = CreateRole(DEFAULT_REQUIRED_CONTROLLER_MODULE);
            var alternate = CreateRole(DEFAULT_ALTERNATE_REQUIRED_CONTROLLER_MODULE);
            var allTheRoles = new[] {role, alternate};

            // First ensure this test fails if the user has none of the roles required.
            AssertAuthorizationFails(
                CreateContext(new ControllerWithMultipleRequiredRolesOnSelf(), "ActionWithoutRole"));

            // Ensure it fails with each of the roles individually
            foreach (var r in allTheRoles)
            {
                _user.AggregateRoles.Clear();
                _user.AggregateRoles.Add(r);
                AssertAuthorizationFails(CreateContext(new ControllerWithMultipleRequiredRolesOnSelf(),
                    "ActionWithoutRole"));
            }

            // Finally ensure it passes if the user has ALL roles

            _user.AggregateRoles.Clear();
            _user.AggregateRoles.AddRange(allTheRoles);
            AssertAuthorizationPasses(CreateContext(new ControllerWithMultipleRequiredRolesOnSelf(),
                "ActionWithoutRole"));
        }

        [TestMethod]
        public void TestOnAuthorizeFailsIfUserDoesNotHaveEveryActionRole()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            var role = CreateRole(DEFAULT_REQUIRED_ACTION_MODULE);
            var alternate = CreateRole(DEFAULT_ALTERNATE_REQUIRED_ACTION_MODULE);
            var allTheRoles = new[] {role, alternate};

            // First ensure this test fails if the user has none of the roles required.
            AssertAuthorizationFails(CreateContext(new ControllerWithMultipleRequiredRolesOnActions(),
                "ActionWithMultipleRoles"));

            // Ensure it fails with each of the roles individually
            foreach (var r in allTheRoles)
            {
                _user.AggregateRoles.Clear();
                _user.AggregateRoles.Add(r);
                AssertAuthorizationFails(CreateContext(new ControllerWithMultipleRequiredRolesOnActions(),
                    "ActionWithMultipleRoles"));
            }

            // Finally ensure it passes if the user has ALL roles

            _user.AggregateRoles.Clear();
            _user.AggregateRoles.AddRange(allTheRoles);
            AssertAuthorizationPasses(CreateContext(new ControllerWithMultipleRequiredRolesOnActions(),
                "ActionWithMultipleRoles"));
        }

        [TestMethod]
        public void TestOnAuthorizeAllowsAccessForAnyUserThatIsASiteAdministratorAkaAlexJasonOrRossAndThatsIt()
        {
            InitAuthenticatedUser();
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            _user.AggregateRoles.Clear();

            AssertAuthorizationPasses(CreateContext(new ControllerWithMultipleRequiredRolesOnActions(),
                "ActionWithMultipleRoles"));
        }

        [TestMethod]
        public void TestOnAuthorizeAddsRequiredRolesToForbiddenRoleAccessModel()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            var context = CreateContext(new ControllerWithRequiredRolesOnActions(), "ActionWithRole");
            AssertAuthorizationFails(context);

            var viewResult = (ViewResult)context.Result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(ForbiddenRoleAccessModel));

            var model = (ForbiddenRoleAccessModel)viewResult.Model;
            Assert.AreEqual(1, model.RequiredRoles.Count);
            var requiredRole = model.RequiredRoles.Single();
            Assert.AreEqual(DEFAULT_REQUIRED_ACTION_MODULE, requiredRole.Module);
        }

        [TestMethod]
        public void TestOnAuthorizeReturnsPartialResultIfIsChildAction()
        {
            InitAuthenticatedUser();
            _user.AggregateRoles.Clear();
            var context = CreateContext(new ControllerWithRequiredRolesOnActions(), "ActionWithRole");
            context.RouteData.DataTokens.Add("ParentActionViewContext", "Oh");
            Assert.IsTrue(context.IsChildAction);
            AssertAuthorizationFails(context);
            MvcAssert.IsPartialView(context.Result);
        }

        #endregion

        #region Helper classes

        private class ControllerWithNoRequiredRolesAnywhere : Controller
        {
            public ActionResult ActionWithoutRole()
            {
                return null;
            }
        }

        private class ControllerWithRequiredRolesOnSelf : Controller
        {
            [RequiresRole(DEFAULT_REQUIRED_CONTROLLER_MODULE)]
            public ActionResult ActionWithoutRole()
            {
                return null;
            }
        }

        private class ControllerWithMultipleRequiredRolesOnSelf : Controller
        {
            [RequiresRole(DEFAULT_REQUIRED_CONTROLLER_MODULE)]
            [RequiresRole(DEFAULT_ALTERNATE_REQUIRED_CONTROLLER_MODULE)]
            public ActionResult ActionWithoutRole()
            {
                return null;
            }
        }

        private class ControllerWithRequiredRolesOnActions : Controller
        {
            [RequiresRole(DEFAULT_REQUIRED_ACTION_MODULE)]
            public ActionResult ActionWithRole()
            {
                return null;
            }
        }

        private class ControllerWithMultipleRequiredRolesOnActions : Controller
        {
            [RequiresRole(DEFAULT_REQUIRED_ACTION_MODULE)]
            [RequiresRole(DEFAULT_ALTERNATE_REQUIRED_ACTION_MODULE)]
            public ActionResult ActionWithMultipleRoles()
            {
                return null;
            }
        }

        private class ControllerWithCustomAttributes : Controller
        {
            [RequiresRole(DEFAULT_REQUIRED_ACTION_MODULE)]
            public ActionResult SomeAction()
            {
                return null;
            }
        }

        private class ControllerWithInheritedClassLevelRoleRequirements : ControllerWithRequiredRolesOnSelf { }

        private class ControllerWithInheritedActionLevelRoleRequirements : ControllerWithCustomAttributes { }

        private class ControllerWithDynamicRoles : Controller, IControllerOfRoles
        {
            public RoleModules GetDynamicRoleModuleForAction(string action)
            {
                if (action == nameof(ActionWithDataLookups))
                {
                    return RoleModules.FieldServicesDataLookups;
                }
                if (action == nameof(ActionWithCustomerGeneral))
                {
                    return RoleModules.CustomerGeneral;
                }

                throw new NotSupportedException(action);
            }

            [DynamicRequiresRole(RoleActions.Read)]
            public ActionResult ActionWithDataLookups()
            {
                return null;
            }

            [DynamicRequiresRole(RoleActions.Read)]
            public ActionResult ActionWithCustomerGeneral()
            {
                return null;
            }
        }
        #endregion
    }
}
