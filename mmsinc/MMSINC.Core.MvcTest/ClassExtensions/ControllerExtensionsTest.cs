using System.Linq;
using System.Net;
using System.Web.Mvc;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.ClassExtensions
{
    [TestClass]
    public class ControllerExtensionsTest
    {
        #region Private Members

        private TestController _target;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _target = _container.GetInstance<TestController>();
        }

        [TestCleanup]
        public void TestCleanup() { }

        #endregion

        #region Excel

        [TestMethod]
        public void TestExcelReturnsExcelResultWithModelAddedAsANewSheet()
        {
            var model = new[] {new Model()};
            var result = _target.Excel(model);
            var resultSheet = (TypedSheet<Model>)result.Exporter.Sheets.Single();
            Assert.AreSame(model.Single(), resultSheet.Items.Single());
        }

        #endregion

        #region GetReflectedControllerDescriptor

        [TestMethod]
        public void TestGetReflectedControllerDescriptorReturnsReflectedControllerDescriptorForControllerType()
        {
            var result = _target.GetReflectedControllerDescriptor();
            Assert.AreEqual("Test", result.ControllerName);
            Assert.AreEqual(_target.GetType(), result.ControllerType);
        }

        #endregion

        #region HttpStatusCode

        [TestMethod]
        public void TestHttpStatusCodeReturnsExpectedResultForEnum()
        {
            var expected = HttpStatusCode.Created;

            var result = _target.HttpStatusCode(expected);

            Assert.AreEqual((int)expected, result.StatusCode);
        }

        #endregion

        #region SetRedirectUrl(string)

        [TestMethod]
        public void TestSetRedirectUrlSetsRedirectUrl()
        {
            var theUrl = "this is the url";

            Assert.AreSame(_target, _target.SetRedirectUrl(theUrl));

            Assert.AreEqual(theUrl, _target.TempData[ControllerExtensions.TempDataKeys.REDIRECT_URL]);
        }

        #endregion

        #region Forbidden()

        [TestMethod]
        public void TestForbiddenReturnsRedirectResultPoiningToTHE_FORBIDDEN_URL( /* dun dun dun */)
        {
            var result = _target.Forbidden();

            Assert.AreEqual(ControllerExtensions.Urls.FORBIDDEN, result.Url);
        }

        #endregion

        #region AddDropDownData

        #endregion

        #region RespondTo

        [TestMethod]
        public void TestRespondToReturnsResultFromResponseFormatter()
        {
            var pipeline = new FakeMvcHttpHandler(new Container());
            pipeline.RouteData.Values.Add(ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME,
                ResponseFormatter.KnownExtensions.JSON);
            var target = pipeline.CreateAndInitializeController<TestController>();
            var expected = new JsonResult();
            var result = target.RespondTo(x => x.Json(() => expected));
            Assert.AreSame(expected, result);
        }

        #endregion

        #region Helper Class

        private class Model
        {
            public string Property { get; set; }
        }

        #endregion
    }

    internal class TestController : MMSINC.Controllers.ControllerBase
    {
        public TestController(ControllerBaseArguments args) : base(args) { }
    }
}
