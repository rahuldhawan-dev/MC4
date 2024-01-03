using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using Moq;
using StructureMap;
using System.Web.Mvc;

namespace MMSINC.Core.MvcTest.Authentication
{
    [TestClass]
    public class MvcAuthorizationFilterTest
    {
        #region Fields

        private MvcAuthorizationFilter _target;
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

            _target = _container.GetInstance<MvcAuthorizationFilter>();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _appTester.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void
            TestConstructorCreatesDefaultListOfAuthorizersInTheOrderOfAllowAnonymousThenAuthenticationThenAdminOnly()
        {
            var target = _container.GetInstance<MvcAuthorizationFilter>();
            Assert.AreEqual(3, target.Authorizors.Count);

            Assert.IsInstanceOfType(target.Authorizors[0], typeof(AnonymousAuthorizer));
            Assert.IsInstanceOfType(target.Authorizors[1], typeof(FormsAuthenticationAuthorizer));
            Assert.IsInstanceOfType(target.Authorizors[2], typeof(AdminAuthorizer));
        }

        [TestMethod]
        public void TestAuthorizeSkipsAuthorizationIfActionAllowsAnonymous()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/Anonymous");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var context = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);

            Assert.IsFalse(_target.LastAuthorizationPassed);
            _target.OnAuthorization(context);
            Assert.IsTrue(_target.LastAuthorizationPassed);
        }

        [TestMethod]
        public void TestAuthorizeFailsIfRequiresLoginButUserIsNotLoggedIn()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/LoggedIn");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var context = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);

            Assert.IsFalse(_target.LastAuthorizationPassed);
            _target.OnAuthorization(context);
            Assert.IsFalse(_target.LastAuthorizationPassed);
        }

        [TestMethod]
        public void TestAuthorizePassesIfRequiresLoginAndUserIsLoggedIn()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/LoggedIn");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var context = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);

            Assert.IsFalse(_target.LastAuthorizationPassed);
            _target.OnAuthorization(context);
            Assert.IsTrue(_target.LastAuthorizationPassed);
        }

        [TestMethod]
        public void TestAuthorizeFailsIfRequiresAdminAndUserIsNotAdmin()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/AdminOnly");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var context = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);

            Assert.IsFalse(_target.LastAuthorizationPassed);
            _target.OnAuthorization(context);
            Assert.IsFalse(_target.LastAuthorizationPassed);
        }

        [TestMethod]
        public void TestAuthorizePassesIfRequiresAdminAndUserIsAdmin()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/AdminOnly");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var context = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            Assert.IsFalse(_target.LastAuthorizationPassed);
            _target.OnAuthorization(context);
            Assert.IsTrue(_target.LastAuthorizationPassed);
        }

        [TestMethod]
        public void TestAuthorizeDisablesServerCachingIfNotAnonymous()
        {
            var req = _appTester.CreateRequestHandler("~/FakeAuthentication/LoggedIn");
            var controller = req.CreateAndInitializeController<FakeAuthenticationController>();
            var context = new AuthorizationContext(controller.ControllerContext, req.RouteContext.ActionDescriptor);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
            _target.OnAuthorization(context);
            req.ResponseCache.Verify(x => x.SetNoServerCaching());
        }

        #endregion
    }
}
