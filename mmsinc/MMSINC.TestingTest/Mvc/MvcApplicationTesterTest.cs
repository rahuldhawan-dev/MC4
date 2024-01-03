using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MMSINC.TestingTest.Mvc
{
    [TestClass]
    public class MvcApplicationTesterTest
    {
        #region Fields

        private MvcApplicationTester<FakeMvcApplication> _target;
        private FakeCrudController _fakeHomeController;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _fakeHomeController = new FakeCrudController();
            _target = new MvcApplicationTester<FakeMvcApplication>(_container);
            _target.ControllerFactory.RegisterController("Home", _fakeHomeController);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _target.Dispose();
        }

        #endregion

        #region Constructor/Initialization

        [TestMethod]
        public void TestConstructorReplacesRouteTableRoutesWithFake()
        {
            // Properly dispose the initial instance so we're reset.
            _target.Dispose();

            var original = RouteTable.Routes;
            _target = new MvcApplicationTester<FakeMvcApplication>(_container);
            Assert.AreNotSame(original, _target.Routes);
            Assert.AreSame(_target.Routes, RouteTable.Routes);
        }

        [TestMethod]
        public void TestConstructorReplacesGlobalFiltersWithFake()
        {
            // Properly dispose the initial instance so we're reset.
            _target.Dispose();

            var original = GlobalFilters.Filters;
            _target = new MvcApplicationTester<FakeMvcApplication>(_container);
            Assert.AreNotSame(original, _target.Filters.GlobalFilters);
            Assert.AreSame(_target.Filters.GlobalFilters, GlobalFilters.Filters);
        }

        #endregion

        #region CreateRequestHandler

        [TestMethod]
        public void TestCreateRequestHandlerThrowsIfRequestPathIsNotVirtualPath()
        {
            MyAssert.Throws(() => _target.CreateRequestHandler("/Not/Virtual"));
        }

        [TestMethod]
        public void TestCreateRequestHandlerSetsHttpMethodOnMockRequest()
        {
            var request = _target.CreateRequestHandler("~/Home/Index", httpMethod: "GET");
            Assert.AreEqual("GET", request.Request.Object.HttpMethod);

            request = _target.CreateRequestHandler("~/Home/Index", httpMethod: "POST");
            Assert.AreEqual("POST", request.Request.Object.HttpMethod);
        }

        [TestMethod]
        public void TestCreateRequestHandlerWithNoParametersReturnsNewHandlerInstanceWithSameRouteCollectionAsSelf()
        {
            var request = _target.CreateRequestHandler();
            Assert.AreSame(_target.Routes, request.RouteCollection);
        }

        [TestMethod]
        public void TestCreateRequestHandlerReturnsNewHandlerInstanceWithSameRouteCollectionAsSelf()
        {
            var request = _target.CreateRequestHandler("~/");
            Assert.AreSame(_target.Routes, request.RouteCollection);
        }

        #endregion

        #region Process

        [TestMethod]
        public void TestProcessingFakeMvcHandlerDoesNotThrowException()
        {
            // Need to get ActionInvoker working.
            var request = _target.CreateRequestHandler("~/Home/Index", httpMethod: "GET");
            request.Process();
        }

        #endregion

        #region Helper Classes

        private class ControllerThatReturnsAView : Controller
        {
            [HttpGet]
            public ActionResult Show()
            {
                // ReSharper disable once Mvc.ViewNotResolved
                return View();
            }
        }

        #endregion
    }
}
