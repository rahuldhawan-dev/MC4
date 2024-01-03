using System.Globalization;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions.MapExtensions;
using MapCall.Common.Model.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.Common.MvcTest.ClassExtensions
{
    [TestClass]
    public class MapExtensionsTest
    {
        #region Fields

        private MvcApplicationTester<FakeMvcApplication> _app;
        private FakeCrudController _controller;
        private FakeMvcHttpHandler _request;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _app = new FakeMvcApplicationTester(new Container());
            _request = _app.CreateRequestHandler();
            _controller = _request.CreateAndInitializeController<FakeCrudController>();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _app.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSetMapViewCanOverwritePreviouslySetValues()
        {
            var first = new MapView();
            _controller.SetMapView(first);
            Assert.AreSame(first, _controller.GetMapView());

            var second = new MapView();
            _controller.SetMapView(second);
            Assert.AreSame(second, _controller.GetMapView());
        }

        [TestMethod]
        public void TestGetMapViewReturnsWhatSetMapViewSetsForASpecificController()
        {
            var expected = new MapView();
            _controller.SetMapView(expected);
            var differentController = _request.CreateAndInitializeController<FakeCrudController>();
            var differentExpected = new MapView();
            differentController.SetMapView(differentExpected);

            var result = _controller.GetMapView();
            var differentResult = differentController.GetMapView();

            Assert.AreSame(expected, result);
            Assert.AreSame(differentExpected, differentResult);
        }

        [TestMethod]
        public void TestGetMapViewForHtmlHelperReturnsSameValueAsGetMapViewForController()
        {
            var expected = new MapView();
            _controller.SetMapView(expected);
            var helper = _request.CreateHtmlHelper<object>(_controller);
            Assert.AreSame(helper.GetMapView(), _controller.GetMapView());
        }

        [TestMethod]
        public void
            TestGetMapViewWillGenerateANewMapViewIfOneIsNotRegisteredAndTheControllerIsDerivedFromControllerAndNotControllerBase()
        {
            var fakeHome = new FakeCrudController();
            _app.ControllerFactory.RegisterController("Home", fakeHome);

            var expectedRouteContext = new RouteContext(_controller.ControllerContext);
            _controller.ModelState.Add("SomeKey",
                new ModelState {Value = new ValueProviderResult("uh", "uh", CultureInfo.CurrentCulture)});
            _controller.SetMapView(null); // Ensure this is null.
            var result = _controller.GetMapView();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRouteContext.RouteControllerName, result.ControllerName);
            Assert.AreEqual(expectedRouteContext.ActionName, result.ActionName);
            Assert.IsTrue(result.Search.ContainsKey("SomeKey"));
            Assert.AreEqual("uh", result.Search["SomeKey"]);
        }

        [TestMethod]
        public void TestGenerateMapUrlGeneratesExpectedMapUrlFromMapView()
        {
            var mapView = new MapView();
            mapView.ControllerName = "SomeController";
            mapView.Search["SomeParameter"] = "12";

            var helper = _request.CreateHtmlHelper<object>(_controller);
            var result = helper.GenerateMapUrl(mapView);

            Assert.AreEqual("/Map?ControllerName=SomeController&Search%5BSomeParameter%5D=12", result);
        }

        [TestMethod]
        public void TestGenerateMapUrlGeneratesExpectedMapUrlWithAdditionalRouteDataWhenSupplied()
        {
            var mapView = new MapView();
            mapView.ControllerName = "SomeController";
            mapView.Search["SomeParameter"] = "12";

            var helper = _request.CreateHtmlHelper<object>(_controller);
            var result = helper.GenerateMapUrl(mapView, new {expected = "value"});

            Assert.AreEqual("/Map?ControllerName=SomeController&Search%5BSomeParameter%5D=12&expected=value", result);
        }

        [TestMethod]
        public void TestGenerateMapUrlGeneratesExpectedMapUrlWithAdditionalRouteDataWithAnArrayWhenSupplied()
        {
            var mapView = new MapView();
            mapView.ControllerName = "SomeController";
            mapView.Search["SomeParameter"] = "12";
            var opCntrs = new[] {10, 14};

            var helper = _request.CreateHtmlHelper<object>(_controller);
            var result = helper.GenerateMapUrl(mapView, new {expected = "value", operatingCenter = opCntrs});

            Assert.AreEqual(
                "/Map?ControllerName=SomeController&Search%5BSomeParameter%5D=12&expected=value&operatingCenter=10%2C14",
                result);
        }

        [TestMethod]
        public void TestGenerateMapUrlGeneratesExpectedMapUrlFromControllerViewDataIfMapViewArgumentIsNull()
        {
            var mapView = new MapView();
            mapView.ControllerName = "SomeController";
            mapView.Search["SomeParameter"] = "12";
            _controller.SetMapView(mapView);
            var helper = _request.CreateHtmlHelper<object>(_controller);
            var result = helper.GenerateMapUrl();

            Assert.AreEqual("/Map?ControllerName=SomeController&Search%5BSomeParameter%5D=12", result);
        }

        [TestMethod]
        public void TestGenerateMapUrlGeneratesAnAreaLessUrlWhenAreasAreInvolved()
        {
            _app.RegisterArea("SomeArea", (context) => {
                context.MapRoute(
                    name: "SomeArea_Default",
                    url: "SomeArea/{controller}/{action}",
                    defaults: new {area = "SomeArea", action = "Index"},
                    namespaces: new[] {"SomeAreaNamespace"});
            });
            var mapView = new MapView();
            mapView.ControllerName = "SomeController";
            mapView.Search["SomeParameter"] = "12";
            _controller.SetMapView(mapView);
            _request.RouteData.DataTokens["area"] = "SomeArea";
            var helper = _request.CreateHtmlHelper<object>(_controller);
            var result = helper.GenerateMapUrl();

            Assert.AreEqual("/Map?ControllerName=SomeController&Search%5BSomeParameter%5D=12&AreaName=SomeArea",
                result);
        }

        [TestMethod]
        public void TestMapLinkGeneratesAnAnchorTagForTheMapUrl()
        {
            var mapView = new MapView();
            mapView.ControllerName = "SomeController";
            mapView.Search["SomeParameter"] = "12";
            _controller.SetMapView(mapView);
            var helper = _request.CreateHtmlHelper<object>(_controller);
            var result = helper.MapLink("Map Link").ToString();
            Assert.AreEqual(
                "<a href=\"/Map?ControllerName=SomeController&amp;Search%5BSomeParameter%5D=12\">Map Link</a>", result);
        }

        [TestMethod]
        public void TestMapLinkGeneratesAnAnchorTagForTheMapUrlWithAdditionalRouteData()
        {
            var mapView = new MapView();
            mapView.ControllerName = "SomeController";
            mapView.Search["SomeParameter"] = "12";
            _controller.SetMapView(mapView);
            var helper = _request.CreateHtmlHelper<object>(_controller);
            var result = helper.MapLink("Map Link", null, null, new {expected = "value"}).ToString();
            Assert.AreEqual(
                "<a href=\"/Map?ControllerName=SomeController&amp;Search%5BSomeParameter%5D=12&amp;expected=value\">Map Link</a>",
                result);
        }

        [TestMethod]
        public void TestMapLinkButtonGeneratesALinkButtonForTheMapUrl()
        {
            var mapView = new MapView();
            mapView.ControllerName = "SomeController";
            mapView.Search["SomeParameter"] = "12";
            _controller.SetMapView(mapView);
            var helper = _request.CreateHtmlHelper<object>(_controller);
            var result = helper.MapLinkButton("Map Link").ToString();
            Assert.AreEqual(
                "<a class=\"link-button\" href=\"/Map?ControllerName=SomeController&amp;Search%5BSomeParameter%5D=12\"><span>Map Link</span></a>",
                result);
        }

        [TestMethod]
        public void TestMapLinkButtonGeneratesALinkButtonForTheMapUrlWithAdditionalRouteData()
        {
            var mapView = new MapView();
            mapView.ControllerName = "SomeController";
            mapView.Search["SomeParameter"] = "12";
            _controller.SetMapView(mapView);
            var helper = _request.CreateHtmlHelper<object>(_controller);
            var result = helper.MapLinkButton("Map Link", null, null, new {expected = "value"}).ToString();
            Assert.AreEqual(
                "<a class=\"link-button\" href=\"/Map?ControllerName=SomeController&amp;Search%5BSomeParameter%5D=12&amp;expected=value\"><span>Map Link</span></a>",
                result);
        }

        [TestMethod]
        public void TestMapLinkButtonGeneratesALinkButtonForTheMapUrlWithAdditionalRouteDataWithStringArrays()
        {
            var mapView = new MapView();
            mapView.ControllerName = "SomeController";
            mapView.Search["SomeParameter"] = "12";
            _controller.SetMapView(mapView);
            var helper = _request.CreateHtmlHelper<object>(_controller);
            var result = helper.MapLinkButton("Map Link", null, null, new { DefaultLayers = new[] { "Water Network", "Sewer Network", "Threat Alerts" }}).ToString();
            Assert.AreEqual(
                "<a class=\"link-button\" href=\"/Map?ControllerName=SomeController&amp;Search%5BSomeParameter%5D=12&amp;DefaultLayers%5B0%5D=Water%20Network&amp;DefaultLayers%5B1%5D=Sewer%20Network&amp;DefaultLayers%5B2%5D=Threat%20Alerts\"><span>Map Link</span></a>",
                result);
        }

        #endregion
    }
}
