using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Authentication
{
    [TestClass]
    public class AnonymousAuthorizerTest
    {
        #region Fields

        private AnonymousAuthorizer _target;
        private FakeMvcApplicationTester _appTester;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            var container = new Container();
            container.Inject(new Mock<IAuthenticationService>().Object);
            _appTester = new FakeMvcApplicationTester(container);
            _target = container.GetInstance<AnonymousAuthorizer>();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _appTester.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestAuthorizeSetsSkipAuthorizingToTrueIfActionAllowsAnonymous()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/Anonymous");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);

            Assert.IsFalse(args.SkipAuthorizingEntirely);
            _target.Authorize(args);
            Assert.IsTrue(args.SkipAuthorizingEntirely);
        }

        [TestMethod]
        public void TestAuthorizeSetsSkipAuthorizingToTrueIfControllerAllowsAnonymous()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAnonymous/Action");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);

            Assert.IsFalse(args.SkipAuthorizingEntirely);
            _target.Authorize(args);
            Assert.IsTrue(args.SkipAuthorizingEntirely);
        }

        [TestMethod]
        public void TestAuthorizeSetsContinueAuthorizingToFalseIfActionDoesNotAllowAnonymous()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/LoggedIn");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);

            Assert.IsFalse(args.SkipAuthorizingEntirely);
            _target.Authorize(args);
            Assert.IsFalse(args.SkipAuthorizingEntirely);
        }

        #endregion
    }
}
