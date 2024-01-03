using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MapCall.Common.ClassExtensions.RouteCollectionExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using rce = MapCall.Common.ClassExtensions.RouteCollectionExtensions.RouteCollectionExtensions;

namespace MapCall.Common.MvcTest.ClassExtensions
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

        #region Resources Route

        [TestMethod]
        public void TestAddResourcesRouteAddsResourcesRouteWithDefaultRouteName()
        {
            var route = _target.AddResourcesRoute();

            Assert.IsTrue(_target.Contains(route));
            Assert.AreEqual(route, _target[rce.ROUTE_NAME]);
        }

        [TestMethod]
        public void TestAddResourcesRouteAddsResourcesRouteWithSpecifiedName()
        {
            var name = "route name";
            var route = _target.AddResourcesRoute(name);

            Assert.IsTrue(_target.Contains(route));
            Assert.AreEqual(route, _target[name]);
        }

        [TestMethod]
        public void TestAddResourcesRouteThrowsExceptionWhenRouteWithDefaultNameAlreadyExists()
        {
            _target.AddResourcesRoute();

            MyAssert.Throws<ArgumentException>(() => _target.AddResourcesRoute());
        }

        [TestMethod]
        public void TestAddResourcesRouteThrowsExceptionWhenRouteWithSpecifiedNameAlreadyExists()
        {
            var name = "route name";
            _target.AddResourcesRoute(name);

            MyAssert.Throws<ArgumentException>(() => _target.AddResourcesRoute(name));
        }

        [TestMethod]
        public void TestResourcesRouteMustBeAddedBeforeDefaultRoute()
        {
            rce.DEFAULT_ROUTE_NAMES.Each(n => {
                _target = new RouteCollection();

                _target.MapRoute(n, rce.ROUTE_URL, new { });

                MyAssert.Throws<ConfigurationErrorsException>(
                    () => _target.AddResourcesRoute());

                // reset, try with specified name
                _target = new RouteCollection();

                _target.MapRoute(n, rce.ROUTE_URL, new { });

                MyAssert.Throws<ConfigurationErrorsException>(
                    () => _target.AddResourcesRoute("some route name"));
            });
        }

        #endregion
    }
}
