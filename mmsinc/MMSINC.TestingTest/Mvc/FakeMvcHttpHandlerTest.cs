using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using System.Web.Mvc;
using StructureMap;

namespace MMSINC.TestingTest.Mvc
{
    [TestClass]
    public class FakeMvcHttpHandlerTest
    {
        #region Fields

        private FakeMvcHttpHandler _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new FakeMvcHttpHandler(new Container());
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestHttpContextResponseIsLinkedToResponse()
        {
            Assert.AreSame(_target.Response.Object, _target.HttpContext.Object.Response);
        }

        [TestMethod]
        public void TestHttpContextRequestIsLinkedToRequest()
        {
            Assert.AreSame(_target.Request.Object, _target.HttpContext.Object.Request);
        }

        [TestMethod]
        public void TestHttpContextItemsIsLinkedToHttpContextItems()
        {
            Assert.AreSame(_target.HttpContextItems, _target.HttpContext.Object.Items);
        }

        [TestMethod]
        public void TestCreateAndInitializeControllerSetsTempData()
        {
            var controller = _target.CreateAndInitializeController<FakeController>();
            Assert.AreSame(_target.TempData, controller.TempData);
        }

        [TestMethod]
        public void TestCreateAndInitializeControllerSetsControllerContextFromRequestContext()
        {
            var controller = _target.CreateAndInitializeController<FakeController>();
            Assert.AreSame(_target.RequestContext, controller.ControllerContext.RequestContext);
            Assert.AreSame(_target.HttpContext.Object, controller.ControllerContext.HttpContext,
                "ControllerContext gets the HttpContext from the RequestContext.");
        }

        [TestMethod]
        public void TestRequestHeadersReturnsRequestHeaders()
        {
            Assert.AreSame(_target.RequestHeaders, _target.Request.Object.Headers);
        }

        [TestMethod]
        public void TestRequestFormReturnsRequestForm()
        {
            Assert.AreSame(_target.RequestForm, _target.Request.Object.Form);
        }

        [TestMethod]
        public void TestCreateHtmlHelperClonesViewDataDictionaryCauseThatsWhatMVCDoes()
        {
            var model = new object();
            var vdd = new ViewDataDictionary(model);
            vdd["SomeKey"] = "SomeValue";
            var helper = _target.CreateHtmlHelper(vdd, model);
            Assert.AreSame(vdd, helper.ViewContext.ViewData,
                "This should be the same as we create the ViewContext and set its ViewData property.");
            Assert.AreSame(vdd, helper.ViewDataContainer.ViewData);
            Assert.AreNotSame(vdd, helper.ViewContext,
                "These shouldn't be the same because HtmlHelper will clone the ViewContext's ViewData");
            Assert.AreEqual("SomeValue", helper.ViewData["SomeKey"],
                "The HtmlHelper should have a clone of the VDD we passed in.");
        }

        [TestMethod]
        public void TestGetRouteDataOnRouteCollectionShouldReturnCorrectDefaultValuesWhenARequestPathIsSet()
        {
            _target = new FakeMvcHttpHandler(new Container(), requestPath: "~/SomeController/SomeAction");
            // _target.RouteCollection.RouteExistingFiles = true;
            Assert.IsNotNull(_target.Request.Object.AppRelativeCurrentExecutionFilePath, "This should be set");
            var routeData = _target.RouteCollection.GetRouteData(_target.HttpContext.Object);
            Assert.IsNotNull(routeData);
            Assert.AreEqual("SomeController", routeData.GetRequiredString("controller"));
            Assert.AreEqual("SomeAction", routeData.GetRequiredString("action"));
        }

        #endregion
    }
}
