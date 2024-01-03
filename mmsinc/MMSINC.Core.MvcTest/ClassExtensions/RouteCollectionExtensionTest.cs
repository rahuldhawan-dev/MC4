using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using System.Web.Routing;

namespace MMSINC.Core.MvcTest.ClassExtensions
{
    [TestClass]
    public class RouteCollectionExtensionsTest
    {
        #region Private Members

        private RouteCollection _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new RouteCollection();
        }

        #endregion

        #region AddManyToManyRoutes

        [TestMethod]
        public void TestAddManyToManyRouteAddBothRoutesByDefault()
        {
            var parent = "Parent";
            var child = "Child";
            var namespaces = new[] {"MapCallMVC.Controllers", "MapCall.Common.Mvc.Controllers"};
            var routes = _target.AddManyToManyRoutes(parent, child, namespaces);

            foreach (var route in routes)
                Assert.IsTrue(_target.Contains(route));

            Assert.AreEqual("Parent/{id}/Children/Add/{childId}", ((Route)routes[0]).Url);
            Assert.AreEqual("Parent/{id}/Children/Remove/{childId}", ((Route)routes[1]).Url);
            Assert.AreEqual("Parent/{id}/Children", ((Route)routes[2]).Url);
        }

        [TestMethod]
        public void TestAddManyToManyRouteAddBothRoutesByDefaultWithArea()
        {
            var parent = "Parent";
            var child = "Child";
            var area = "Foo";
            var namespaces = new[] {"MapCallMVC.Controllers", "MapCall.Common.Mvc.Controllers"};
            var routes = _target.AddManyToManyRoutes(parent, child, namespaces, area: area);

            foreach (var route in routes)
                Assert.IsTrue(_target.Contains(route));

            Assert.AreEqual("Foo/Parent/{id}/Children/Add/{childId}", ((Route)routes[0]).Url);
            Assert.AreEqual("Foo/Parent/{id}/Children/Remove/{childId}", ((Route)routes[1]).Url);
            Assert.AreEqual("Foo/Parent/{id}/Children", ((Route)routes[2]).Url);
        }

        [TestMethod]
        public void TestAddManyToManyRouteOnlyAddsCreateRoute()
        {
            var parent = "Parent";
            var child = "Child";
            var namespaces = new[] {"MapCallMVC.Controllers", "MapCall.Common.Mvc.Controllers"};
            var routes = _target.AddManyToManyRoutes(parent, child, namespaces, true, false, index: false);

            Assert.AreEqual(1, routes.Count);
            Assert.AreEqual("Parent/{id}/Children/Add/{childId}", ((Route)routes[0]).Url);
        }

        [TestMethod]
        public void TestAddManyToManyRouteOnlyAddsCreateRouteWithArea()
        {
            var parent = "Parent";
            var child = "Child";
            var area = "Foo";
            var namespaces = new[] {"MapCallMVC.Controllers", "MapCall.Common.Mvc.Controllers"};
            var routes = _target.AddManyToManyRoutes(parent, child, namespaces, true, false, area, index: false);

            Assert.AreEqual(1, routes.Count);
            Assert.AreEqual("Foo/Parent/{id}/Children/Add/{childId}", ((Route)routes[0]).Url);
        }

        [TestMethod]
        public void TestAddManyToManyRouteOnlyAddsDestroyRoute()
        {
            var parent = "Parent";
            var child = "Child";
            var namespaces = new[] {"MapCallMVC.Controllers", "MapCall.Common.Mvc.Controllers"};
            var routes = _target.AddManyToManyRoutes(parent, child, namespaces, false, true, index: false);

            Assert.AreEqual(1, routes.Count);
            Assert.AreEqual("Parent/{id}/Children/Remove/{childId}", ((Route)routes[0]).Url);
        }

        [TestMethod]
        public void TestAddManyToManyRouteOnlyAddsDestroyRouteWithArea()
        {
            var parent = "Parent";
            var child = "Child";
            var area = "Foo";

            var namespaces = new[] {"MapCallMVC.Controllers", "MapCall.Common.Mvc.Controllers"};
            var routes = _target.AddManyToManyRoutes(parent, child, namespaces, false, true, area, index: false);

            Assert.AreEqual(1, routes.Count);
            Assert.AreEqual("Foo/Parent/{id}/Children/Remove/{childId}", ((Route)routes[0]).Url);
        }

        #endregion
    }
}
