using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests
{
    [TestClass]
    public class MvcApplicationTest
    {
        #region Private Members

        private MvcApplication _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new MvcApplication();
        }

        #endregion

        #region Private Methods

        // TODO: this will be needed for testing outgoing routes
        private UrlHelper GetUrlHelper(string appPath = "/", RouteCollection routes = null)
        {
            if (routes == null)
            {
                routes = new RouteCollection();
                _target.RegisterRoutes(routes);
            }

            var httpContext = new StubHttpContextForRouting(appPath);
            var routeData = new RouteData();
            routeData.Values.Add("controller", "defaultcontroller");
            routeData.Values.Add("action", "defaultaction");
            var requestContext = new RequestContext(httpContext, routeData);
            return new UrlHelper(requestContext, routes);
        }

        #endregion

        #region Tests

        #region Incoming Routes

        [TestMethod]
        public void TestRootRoute()
        {
            _target.TestRoute("~/", new {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            });
        }

        [TestMethod]
        public void TestRouteWithControllerNoActionNoId()
        {
            _target.TestRoute("~/controller1", new {
                controller = "controller1",
                action = "Index",
                id = UrlParameter.Optional
            });
        }

        [TestMethod]
        public void TestRouteWithControllerAndActionNoId()
        {
            _target.TestRoute("~/controller1/action2", new {
                controller = "controller1",
                action = "action2",
                id = UrlParameter.Optional
            });
        }

        [TestMethod]
        public void TestRouteWithControllerActionAndId()
        {
            _target.TestRoute("~/controller1/action2/id3", new {
                controller = "controller1",
                action = "action2",
                id = "id3"
            });
        }

        [TestMethod]
        public void TestRouteWithTooManySegments()
        {
            _target.TestRouteInvalid("~/too/many/segments/man");
        }

        [TestMethod]
        public void TestRouteForAxdResource()
        {
            _target.TestRouteIgnored("~/foo.axd/bar/baz/biff");
        }

        [TestMethod]
        public void TestRouteForEmbeddedMapCallCommonJSResource()
        {
            _target.TestRoute("~/Content/JS/someScript.js", new {
                controller = "Resources", action = "JS", file = "someScript.js"
            });
        }

        [TestMethod]
        public void TestRouteForEmbeddedMapCallCommonCSSResource()
        {
            _target.TestRoute("~/Content/CSS/someStylesheet.css", new {
                controller = "Resources", action = "CSS", file = "someStylesheet.css"
            });
        }

        #endregion

        #endregion
    }
}
