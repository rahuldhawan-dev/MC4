using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Metadata;
using MMSINC.Testing;
using Moq;
using StructureMap;
using System.Web.Mvc;

namespace MMSINC.Core.MvcTest.Authentication
{
    [TestClass]
    public class AuthenticationAuthorizerTest
    {
        #region Fields

        private TestAuthenticationAuthorizer _target;
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
            _target = _container.GetInstance<TestAuthenticationAuthorizer>();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _appTester.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestAuthorizeDoesNotSetResultIfUserIsLoggedIn()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/LoggedIn");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);

            _target.Authorize(args);
            Assert.IsNull(authContext.Result);
        }

        [TestMethod]
        public void TestAuthorizeSetsResultToRedirectIfUserIsNotLoggedIn()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/LoggedIn");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(false);

            _target.Authorize(args);

            MvcAssert.RedirectsToUrl(authContext.Result, "~/Authentication/LogOff");
        }

        [TestMethod]
        public void TestAuthorizeExecutesTokenAttributeIfOneExistsForTheAction()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/LoggedIn");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var authContext = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            var args = new AuthorizationArgs(authContext);

            var token = new TestTokenValidationAttribute();
            _target.TestTokenAttribute = token;

            _target.Authorize(args);
            Assert.IsTrue(token.WasExecuted);
        }

        #endregion

        #region Test classes

        private class TestAuthenticationAuthorizer : FormsAuthenticationAuthorizer
        {
            public TestTokenValidationAttribute TestTokenAttribute { get; set; }

            protected override TokenValidationAttribute GetTokenValidationAttribute(AuthorizationArgs authArgs)
            {
                return TestTokenAttribute;
            }

            public TestAuthenticationAuthorizer(IContainer container) : base(container) { }
        }

        private class TestTokenValidationAttribute : TokenValidationAttribute
        {
            public bool WasExecuted { get; set; }

            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                WasExecuted = true;
            }
        }

        #endregion
    }
}
