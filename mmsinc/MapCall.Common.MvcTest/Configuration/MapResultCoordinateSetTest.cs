using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.Common.MvcTest.Configuration
{
    [TestClass]
    public class MapResultCoordinateSetTest
    {
        #region Tests

        [TestMethod]
        public void TestEmptyConstructorSetsCoordinateRouteValuesPropertyToNull()
        {
            Assert.IsNull(new MapResultCoordinateSet(null).CoordinateRouteValues);
        }

        [TestMethod]
        public void TestLayerIdIsSerialized()
        {
            var target = new MapResultCoordinateSet(null);
            target.LayerId = "somelayer";
            var urlHelper = new UrlHelper(new RequestContext {
                RouteData = new RouteData()
            });
            var result = (IDictionary<string, object>)target.Serialize(urlHelper);
            Assert.AreEqual("somelayer", result["layerId"]);
        }

        [TestMethod]
        public void TestDrawLinesBetweenPointsIsSerialized()
        {
            var target = new MapResultCoordinateSet(null);
            var urlHelper = new UrlHelper(new RequestContext {
                RouteData = new RouteData()
            });
            var result = (IDictionary<string, object>)target.Serialize(urlHelper);
            Assert.AreEqual(false, result["drawLinesBetweenPoints"]);
        }

        [TestMethod]
        public void TestDrawLinesBetweenPointsIsFalseByDefault()
        {
            Assert.IsFalse(new MapResultCoordinateSet(null).DrawLinesBetweenPoints);
        }

        #endregion
    }
}
