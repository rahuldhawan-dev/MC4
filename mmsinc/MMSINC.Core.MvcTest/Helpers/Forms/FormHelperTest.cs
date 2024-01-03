using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Helpers;
using MMSINC.Testing;
using StructureMap;
using RouteCollectionExtensions = MMSINC.ClassExtensions.RouteCollectionExtensions;

// ReSharper disable Mvc.ActionNotResolved, Mvc.ControllerNotResolved, Mvc.AreaNotResolved

namespace MMSINC.Core.MvcTest.Helpers.Forms
{
    [TestClass]
    public class FormHelperTest
    {
        #region Fields

        private FormHelper<object> _target;
        private FakeMvcApplicationTester _application;
        private FakeMvcHttpHandler _request;
        private HtmlHelper<object> _htmlHelper;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _application = new FakeMvcApplicationTester(new Container());
            _request = _application.CreateRequestHandler();
            _htmlHelper = _request.CreateHtmlHelper<object>();
            _target = new FormHelper<object>(_htmlHelper);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _application.Dispose();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestBeginRouteReturnsCorrectAddRouteForm()
        {
            var parentModel = "Parent";
            var childModel = "Child";
            var expectedRouteName =
                RouteCollectionExtensions.GetManyToManyRouteName(ManyToManyRouteAction.Add, parentModel, childModel);
            _application.Routes.Clear();

            _application.Routes.AddManyToManyRoutes(parentModel, childModel, new string[] { }, true);

            var result = _target.BeginRouteForm(parentModel, childModel, new {id = 626, childId = 21},
                ManyToManyRouteAction.Add);
            Assert.AreEqual(expectedRouteName, result.RouteName);
            Assert.AreEqual("/Parent/626/Children/Add/21", result.GetUrl());
        }

        [TestMethod]
        public void TestBeginRouteReturnsCorrectDestroyRouteForm()
        {
            var parentModel = "Parent";
            var childModel = "Child";
            var expectedRouteName =
                RouteCollectionExtensions.GetManyToManyRouteName(ManyToManyRouteAction.Remove, parentModel, childModel);
            _application.Routes.Clear();

            _application.Routes.AddManyToManyRoutes(parentModel, childModel, new string[] { }, true);

            var result = _target.BeginRouteForm(parentModel, childModel, new {id = 626, childId = 21},
                ManyToManyRouteAction.Remove);
            Assert.AreEqual(expectedRouteName, result.RouteName);
            Assert.AreEqual("/Parent/626/Children/Remove/21", result.GetUrl());
        }

        [TestMethod]
        public void TestBeginRouteFormReturnsCorrectAddRouteFormForBug1780()
        {
            var parentModel = "Parent";
            var childModel = "Child";
            var expectedRouteName =
                RouteCollectionExtensions.GetManyToManyRouteName(ManyToManyRouteAction.Add, parentModel, childModel);
            _application.Routes.Clear();
            _application.Routes.AddManyToManyRoutes(parentModel, childModel, new string[] { }, true);

            var result = _target.BeginRouteForm(parentModel, childModel, new {id = 626}, ManyToManyRouteAction.Add);

            Assert.AreEqual(expectedRouteName, result.RouteName);
            Assert.AreEqual("/Parent/626/Children/Add", result.GetUrl());
        }

        [TestMethod]
        public void TestBeginFormDoesNotSetAjaxOptionsPropertyOnFormBuilder()
        {
            var form = _target.BeginForm("action", "controller");
            Assert.IsNull(form.Ajax);

            form = _target.BeginForm("action", "controller", "area");
            Assert.IsNull(form.Ajax);
        }

        [TestMethod]
        public void TestBeginRouteFormDoesNotSetAjaxOptionsPropertyOnFormBuilder()
        {
            var form = _target.BeginRouteForm("RouteName", new { });
            Assert.IsNull(form.Ajax);

            form = _target.BeginRouteForm("Parent", "Child", new { }, ManyToManyRouteAction.Add);
            Assert.IsNull(form.Ajax);
        }

        [TestMethod]
        public void TestBeginAjaxFormReturnsFormBuilderWithAjaxOptionsSet()
        {
            var form = _target.BeginAjaxForm("action", "controller");
            Assert.IsNotNull(form.Ajax);

            form = _target.BeginAjaxForm("action", "controller", "area");
            Assert.IsNotNull(form.Ajax);
        }

        [TestMethod]
        public void TestBeginAjaxRouteFormReturnsFormBuilderWithAjaxOptionsSet()
        {
            var form = _target.BeginAjaxRouteForm("RouteName", new { });
            Assert.IsNotNull(form.Ajax);

            form = _target.BeginAjaxRouteForm("Parent", "Child", new { }, ManyToManyRouteAction.Add);
            Assert.IsNotNull(form.Ajax);
        }

        #endregion
    }
}
