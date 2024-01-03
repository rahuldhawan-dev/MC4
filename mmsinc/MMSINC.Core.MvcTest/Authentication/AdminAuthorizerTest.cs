using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;
using StructureMap;
using System.Web.Mvc;

namespace MMSINC.Core.MvcTest.Authentication
{
    [TestClass]
    public class AdminAuthorizerTest
    {
        #region Fields

        private AdminAuthorizer _target;
        private FakeMvcApplicationTester _appTester;
        private Mock<IAuthenticationService> _authServ;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _appTester = new FakeMvcApplicationTester(_container);
            _authServ = new Mock<IAuthenticationService>();
            _container.Inject(_authServ.Object);

            _target = _container.GetInstance<AdminAuthorizer>();
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
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/LoggedIn");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);

            _target.Authorize(args);
            Assert.IsFalse(args.SkipAuthorizingEntirely);
            Assert.IsNull(authContext.Result);
        }

        [TestMethod]
        public void TestAuthorizeSetsResultToRedirectToForbiddenPageIfUserIsNotAdmin()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/AdminOnly");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);

            _target.Authorize(args);
            Assert.IsFalse(args.SkipAuthorizingEntirely);
            MvcAssert.RedirectsToUrl(authContext.Result, "~/Error/Forbidden");
        }

        #endregion
    }
}
