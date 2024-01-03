using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;
using StructureMap;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.Common.MvcTest.Metadata
{
    [TestClass]
    public class RequiresUserAdminAuthorizerTest
    {
        #region Fields

        private RequiresUserAdminAuthorizer _target;
        private FakeMvcApplicationTester _appTester;
        private Mock<IAuthenticationService<User>> _authServ;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _appTester = new FakeMvcApplicationTester(_container);
            _appTester.ControllerFactory.RegisterController(new ThingyController());
            _authServ = new Mock<IAuthenticationService<User>>();
            _container.Inject(_authServ.Object);
            _container.Inject((IAuthenticationService)_authServ.Object);

            _target = _container.GetInstance<RequiresUserAdminAuthorizer>();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _appTester.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestAuthorizeSetsContinueAuthorizingToTrueIfNoRequiresAdminAttributeIsFound()
        {
            // This test is kinda tough because we're basically testing that nothing happens.
            // We're not setting any values or modifying anything if the user is valid.
            var req = _appTester.CreateRequestHandler("~/Thingy/LoggedIn");
            var controller = req.CreateAndInitializeController<ThingyController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);

            _target.Authorize(args);
            Assert.IsFalse(args.SkipAuthorizingEntirely);
            Assert.IsNull(authContext.Result);
        }

        [TestMethod]
        public void TestAuthorizeSetsResultToRedirectToForbiddenPageIfUserIsNotUserAdminOrSiteAdmin()
        {
            var req = _appTester.CreateRequestHandler("~/Thingy/UserAdminOnly");
            var controller = req.CreateAndInitializeController<ThingyController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);
            _authServ.Setup(x => x.CurrentUser.IsUserAdmin).Returns(false);
            _authServ.Setup(x => x.CurrentUser.IsAdmin).Returns(false);

            _target.Authorize(args);
            Assert.IsFalse(args.SkipAuthorizingEntirely);
            MvcAssert.RedirectsToUrl(authContext.Result, "~/Error/Forbidden");
        }

        [TestMethod]
        public void TestAuthorizeDoesNotDoAnythingIfRequiresUserAdminAndUserIsUserAdmin()
        {
            // This test is kinda tough because we're basically testing that nothing happens.
            // We're not setting any values or modifying anything if the user is valid.
            var req = _appTester.CreateRequestHandler("~/Thingy/LoggedIn");
            var controller = req.CreateAndInitializeController<ThingyController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);
            _authServ.Setup(x => x.CurrentUser.IsUserAdmin).Returns(true);
            _authServ.Setup(x => x.CurrentUser.IsAdmin).Returns(false);

            _target.Authorize(args);
            Assert.IsFalse(args.SkipAuthorizingEntirely);
            Assert.IsNull(authContext.Result);
        }
        
        [TestMethod]
        public void TestAuthorizeDoesNotDoAnythingIfRequiresUserAdminAndUserIsSiteAdmin()
        {
            // This test is kinda tough because we're basically testing that nothing happens.
            // We're not setting any values or modifying anything if the user is valid.
            var req = _appTester.CreateRequestHandler("~/Thingy/LoggedIn");
            var controller = req.CreateAndInitializeController<ThingyController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);
            _authServ.Setup(x => x.CurrentUser.IsUserAdmin).Returns(false);
            _authServ.Setup(x => x.CurrentUser.IsAdmin).Returns(true);

            _target.Authorize(args);
            Assert.IsFalse(args.SkipAuthorizingEntirely);
            Assert.IsNull(authContext.Result);
        }

        #endregion

        #region Helpers

        public class ThingyController : Controller
        {
            // No attribute needed
            public virtual ActionResult LoggedIn() => null;

            [RequiresUserAdmin]
            public virtual ActionResult UserAdminOnly() => null;
        }

        #endregion
    }
}
