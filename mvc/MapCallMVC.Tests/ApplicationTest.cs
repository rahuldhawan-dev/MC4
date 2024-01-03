using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests 
{
    [TestClass]
    public class ApplicationTest
    {
        #region Fields

        // ReSharper disable RedundantNameQualifier
        private MapCallMvcApplicationTester _tester;
        private FakeMvcHttpHandler _request;

        private IContainer _container;
        // ReSharper restore RedundantNameQualifier

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = MapCallMvcApplicationTester.InitializeDummyObjectFactory();
            _container.Inject<IViewModelFactory>(_container.GetInstance<ViewModelFactory>());
            _tester = _container.With(true).GetInstance<MapCallMvcApplicationTester>();
            _request = _tester.CreateRequestHandler();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _container.Reset();
            _tester.Dispose();
        }

        #endregion

        #region Private Methods
        
        private RouteData GetRouteData(string url)
        {
            _request.Request.Setup(x => x.AppRelativeCurrentExecutionFilePath).Returns(url);
            return _request.RouteCollection.GetRouteData(_request.HttpContext.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSecureFormValueProviderFactoryIsInsertedToFirstPositionOfValueProviderFactories()
        {
            var vpfc = new ValueProviderFactoryCollection();
            var someFactory = new Mock<ValueProviderFactory>();
            vpfc.Insert(0, someFactory.Object);

            _tester.ApplicationInstance.RegisterValueProviderFactories(vpfc, _container);

            var result = vpfc[0];
            Assert.IsInstanceOfType(result, typeof(SecureFormValueProviderFactory<SecureFormToken, SecureFormDynamicValue>));
        }

        #region Routing

        [TestMethod]
        public void TestCanGenerateRouteToIndexWithExtensionInPath()
        {
            var result = _request.UrlHelper.Action("Index", "SomeController", new { ext = "blah" });
            Assert.AreEqual("/SomeController/Index.blah", result);
        }

        [TestMethod]
        public void TestCanGenerateRouteToIndexWithoutExtensionInPath()
        {
            var result = _request.UrlHelper.Action("Index", "SomeController");
            Assert.AreEqual("/SomeController", result);
        }

        [TestMethod]
        public void TestCanGenerateRouteToNonIndexRoutesAndDoesNotAddExtensionToUrlWhenThereIsNoExtensionParameter()
        {
            var result = _request.UrlHelper.Action("Show", "SomeController", new { id = 4 });
            Assert.AreEqual("/SomeController/Show/4", result);
        }

        [TestMethod]
        public void TestCanGenerateRouteToNonIndexRoutesWithExtensions()
        {
            var result = _request.UrlHelper.Action("Show", "SomeController", new { ext = "xls", id = 4 });
            Assert.AreEqual("/SomeController/Show/4.xls", result);
        }

        [TestMethod]
        public void TestCanGenerateRoutesForDefaultChildActionLikeThings()
        {
            var result = _request.UrlHelper.RouteUrl("DefaultShowChild",
                new {action = "Readings", Controller = "Equipment", id = 42});
            Assert.AreEqual("/Equipment/Show/42/Readings", result);
        }

        [TestMethod]
        public void TestCanRouteToIndexWithExtensionInPath()
        {
            var result = GetRouteData("~/SomeController/Index.xls");
            Assert.AreEqual("SomeController", result.GetRequiredString("controller"));
            Assert.AreEqual("Index", result.GetRequiredString("action"));
            Assert.AreEqual(ResponseFormatter.KnownExtensions.EXCEL_2003,
                result.GetRequiredString(ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME));
        }

        [TestMethod]
        public void TestCanRouteToIndexWithoutExtensionInPath()
        {
            var result = GetRouteData("~/SomeController/");
            Assert.AreEqual("SomeController", result.GetRequiredString("controller"));
            Assert.AreEqual("Index", result.GetRequiredString("action"));

            result = GetRouteData("~/SomeController/Index");
            Assert.AreEqual("SomeController", result.GetRequiredString("controller"));
            Assert.AreEqual("Index", result.GetRequiredString("action"));
        }

        [TestMethod]
        public void TestCanRouteToNonIndexActionsWithExtensionInPath()
        {
            var result = GetRouteData("~/SomeController/Show/4.xls");
            Assert.AreEqual("SomeController", result.GetRequiredString("controller"));
            Assert.AreEqual("Show", result.GetRequiredString("action"));
            Assert.AreEqual("xls", result.GetRequiredString(ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME));
            Assert.AreEqual("4", result.GetRequiredString("id"));
        }

        [TestMethod]
        public void TestCanRouteToNonIndexRoutesAndDoesNotAddExtensionToUrlWhenThereIsNoExtensionParameter()
        {
            var result = GetRouteData("~/SomeController/Show/4");
            Assert.AreEqual("SomeController", result.GetRequiredString("controller"));
            Assert.AreEqual("Show", result.GetRequiredString("action"));
            Assert.AreEqual("4", result.GetRequiredString("id"));
            Assert.IsNull(result.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME]);
        }

        [TestMethod]
        public void TestRouteContextsWorkWithUrlsThatAreIntheDefaultChildActionFormatThatAlsoIncludeAdditionalQueryStringData()
        {
            //var url =
            //    _request.UrlHelper.RouteUrl(
            //        new {action = "Show", Controller = "Equipment", id = 42, MoreData = "MoreData"});
            //var rc = new RouteContext(_request.RequestContext, url);

            // var neato = _tester.CreateRequestHandler("~/Equipment/Show/42/Readings?MoreData=MoreData");
            // var rc = neato.RouteContext;
            var url = _request.UrlHelper.RouteUrl("DefaultShowChild", new { action = "Readings", Controller = "Equipment", id = 42, MoreData = "MoreData" });
            var rc = new RouteContext(_request.RequestContext, url);
        }
        


        #endregion

        #endregion
    }
}
