using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Filters;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCall.Common.MvcTest.Metadata
{
    [TestClass, DoNotParallelize]
    public class UserStorageFilterTest
    {
        #region Fields

        private UserStorageFilter _target;
        private FakeMvcApplicationTester _tester;
        private FakeMvcHttpHandler _request;
        private Mock<IAuthenticationService> _authServ;
        private Mock<IAdministratedUser> _user;
        private Mock<ActionExecutingContext> _actionExecutingContext;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _authServ = new Mock<IAuthenticationService>();
            _user = new Mock<IAdministratedUser>();
            _user.Setup(x => x.UniqueName).Returns("some user");
            _user.Setup(x => x.Id).Returns(42);
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
            _container.Inject(_authServ.Object);

            var controller = new CrudController();
            _tester = new FakeMvcApplicationTester(_container);
            _tester.ControllerFactory.RegisterController(controller);

            _request = _tester.CreateRequestHandler("~/Crud/Search/");
            _target = _container.GetInstance<UserStorageFilter>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _tester.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void
            TestOnActionExecutingAddsCookieAndSetsEnabledToFalseIfCurrentUserIdentifierIsNullOrNotAuthenticated()
        {
            var controllerContext = _request.CreateControllerContext(new CrudController());
            var rc = new RouteContext(_request.RequestContext);
            var filterContext =
                new ActionExecutingContext(controllerContext, rc.ActionDescriptor, new RouteValueDictionary());

            _target.OnActionExecuting(filterContext);

            Assert.AreEqual(1, _request.ResponseCookies.Count);
            Assert.AreEqual("false",
                _request.ResponseCookies[UserStorageFilter.CookieKeys.NAME][UserStorageFilter.CookieKeys.ENABLED]);
        }

        [TestMethod]
        public void TestOnActionExecutingAddsCookieAndSetsEnabledToFalseIfCurrentUserIsNotAuthenticated()
        {
            _authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(false);
            var controllerContext = _request.CreateControllerContext(new CrudController());
            var rc = new RouteContext(_request.RequestContext);
            var filterContext =
                new ActionExecutingContext(controllerContext, rc.ActionDescriptor, new RouteValueDictionary());

            _target.OnActionExecuting(filterContext);

            Assert.AreEqual(1, _request.ResponseCookies.Count);
            Assert.AreEqual("false",
                _request.ResponseCookies[UserStorageFilter.CookieKeys.NAME][UserStorageFilter.CookieKeys.ENABLED]);
        }

        [TestMethod]
        public void TestOnActionExecutingSetsCookieIdentifier()
        {
            var first = "first";
            _authServ.Setup(x => x.CurrentUserIdentifier).Returns(first);
            var controllerContext = _request.CreateControllerContext(new CrudController());
            var rc = new RouteContext(_request.RequestContext);
            var filterContext =
                new ActionExecutingContext(controllerContext, rc.ActionDescriptor, new RouteValueDictionary());

            _target.OnActionExecuting(filterContext);

            Assert.AreEqual(1, _request.ResponseCookies.Count);
            Assert.AreEqual(first,
                _request.ResponseCookies[UserStorageFilter.CookieKeys.NAME][UserStorageFilter.CookieKeys.KEY]);
            Assert.AreEqual("true",
                _request.ResponseCookies[UserStorageFilter.CookieKeys.NAME][UserStorageFilter.CookieKeys.ENABLED]);
        }

        [TestMethod]
        public void TestReplacesCookieIfIdentifiedAndKeyDontMatch()
        {
            var second = "second";
            _authServ.Setup(x => x.CurrentUserIdentifier).Returns(second);
            var controllerContext = _request.CreateControllerContext(new CrudController());
            var rc = new RouteContext(_request.RequestContext);
            var filterContext =
                new ActionExecutingContext(controllerContext, rc.ActionDescriptor, new RouteValueDictionary());
            var cookie = new HttpCookie(UserStorageFilter.CookieKeys.NAME);
            cookie.Values.Add(UserStorageFilter.CookieKeys.KEY, "first");
            cookie.Values.Add(UserStorageFilter.CookieKeys.ENABLED, "true");
            _request.RequestCookies.Add(cookie);

            Assert.AreEqual("first",
                _request.RequestCookies[UserStorageFilter.CookieKeys.NAME][UserStorageFilter.CookieKeys.KEY]);

            _target.OnActionExecuting(filterContext);

            Assert.AreNotEqual("first",
                _request.ResponseCookies[UserStorageFilter.CookieKeys.NAME][UserStorageFilter.CookieKeys.KEY]);
            Assert.AreEqual(second,
                _request.ResponseCookies[UserStorageFilter.CookieKeys.NAME][UserStorageFilter.CookieKeys.KEY]);
        }

        [TestMethod, DoNotParallelize]
        public void TestThatTheCookieIsEnabledIfThereIsAnIdentifierAndEnabled()
        {
            _authServ.Setup(x => x.CurrentUserIdentifier).Returns("AnIdentifier");
            var controllerContext = _request.CreateControllerContext(new CrudController());
            var rc = new RouteContext(_request.RequestContext);
            var filterContext =
                new ActionExecutingContext(controllerContext, rc.ActionDescriptor, new RouteValueDictionary());
            var cookie = new HttpCookie(UserStorageFilter.CookieKeys.NAME);
            cookie.Values.Add(UserStorageFilter.CookieKeys.KEY, "first");
            cookie.Values.Add(UserStorageFilter.CookieKeys.ENABLED, "true");
            _request.RequestCookies.Add(cookie);

            _target.OnActionExecuting(filterContext);

            Assert.AreEqual("true",
                _request.ResponseCookies[UserStorageFilter.CookieKeys.NAME][UserStorageFilter.CookieKeys.ENABLED]);

            MMSINC.MvcApplication.IsInTestMode = true;

            // Clear these because otherwise an error will get thrown since we'd end
            // up adding a duplicate cookie.
            _request.ResponseCookies.Clear();

            _target.OnActionExecuting(filterContext);

            try
            {
                Assert.AreEqual("false",
                    _request.ResponseCookies[UserStorageFilter.CookieKeys.NAME][UserStorageFilter.CookieKeys.ENABLED]);
            }
            finally
            {
                MMSINC.MvcApplication.IsInTestMode = false;
            }
        }

        #endregion

        #region Test Classes

        private class CrudController : Controller
        {
            public ActionResult Search()
            {
                return null;
            }
        }

        #endregion
    }
}
