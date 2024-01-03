using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MMSINC.Core.MvcTest.Results
{
    [TestClass]
    public class ResponseFormatterTest
    {
        #region Fields

        private ResponseFormatter _target;
        private FakeMvcHttpHandler _handler;
        private ViewResultBase _viewResult;
        private JsonResult _jsonResult;
        private ExcelResult _excelResult;
        private EmptyResult _emptyResult;
        private EmptyResult _fragResult;
        private EmptyResult _pdfResult;
        private EmptyResult _calResult;
        private ControllerContext _controllerContext;

        #endregion

        #region Init

        [TestInitialize]
        public void InitializeTest()
        {
            _handler = new FakeMvcHttpHandler(new Container());
            _controllerContext = _handler.CreateAndInitializeController<FakeController>().ControllerContext;
            _viewResult = new Mock<ViewResultBase>().Object;
            _jsonResult = new JsonResult();
            _excelResult = new ExcelResult();
            _emptyResult = new EmptyResult();
            _pdfResult = new EmptyResult();
            _calResult = new EmptyResult();
            _target = Initialize((formatter) => {
                formatter.View(() => _viewResult);
                formatter.Excel(() => _excelResult);
                formatter.Json(() => _jsonResult);
                formatter.Fragment(() => _fragResult);
                formatter.Pdf(() => _pdfResult);
                formatter.Calendar(() => _calResult);
            });
        }

        private ResponseFormatter Initialize(Action<ResponseFormatter> initializer)
        {
            return new ResponseFormatter(initializer);
        }

        private void SetExtension(string ext)
        {
            _handler.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ext;
        }

        private void TestExtension(string ext, ActionResult expectedResult, bool includeCaseInsensitivtyCheck = true)
        {
            // This test ensures that the extension is case-insensitive.
            var extensions = new List<string> {ext};
            if (includeCaseInsensitivtyCheck)
            {
                extensions.Add(ext.ToUpperInvariant());
                extensions.Add(ext.ToLowerInvariant());
            }

            foreach (var e in extensions)
            {
                SetExtension(e);
                Assert.AreSame(expectedResult, _target.GetActionResult(_controllerContext));
            }
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorThrowsExceptionForNullInitializerParameter()
        {
            MyAssert.Throws<ArgumentNullException>(() => new ResponseFormatter(null));
        }

        #region GetActionResult

        [TestMethod]
        public void TestGetActionResultReturnsViewResultWhenNoExtensionIsPresent()
        {
            _handler.RouteData.Values.Remove(ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME);
            Assert.AreSame(_viewResult, _target.GetActionResult(_controllerContext));

            TestExtension(null, _viewResult, includeCaseInsensitivtyCheck: false);
            TestExtension(string.Empty, _viewResult);
            TestExtension("   ", _viewResult);
        }

        [TestMethod]
        public void TestGetActionResultReturnsExcelResultWhenRouteExtensionIsExcel()
        {
            TestExtension(ResponseFormatter.KnownExtensions.EXCEL_2003, _excelResult);
        }

        [TestMethod]
        public void TestGetActionResultReturnsJsonResultWhenRouteExtensionIsJson()
        {
            TestExtension(ResponseFormatter.KnownExtensions.JSON, _jsonResult);
        }

        [TestMethod]
        public void TestGetActionResultReturnsFragmentResultWhenRouteExtensionIsFragment()
        {
            TestExtension(ResponseFormatter.KnownExtensions.FRAGMENT, _fragResult);
        }

        [TestMethod]
        public void TestGetActionResultReturnsFragmentResultWhenRouteExtensionIsPdf()
        {
            TestExtension(ResponseFormatter.KnownExtensions.PDF, _pdfResult);
        }

        [TestMethod]
        public void TestGetActionResultReturnsCalendarResultWhenRouteExtensionIsCalendar()
        {
            TestExtension(ResponseFormatter.KnownExtensions.CALENDAR, _calResult);
        }

        [TestMethod]
        public void TestGetActionResultReturnsHttpNotFoundResultIfRouteExtensionIsNotRegistered()
        {
            SetExtension("I most certainly am not an extension");
            MvcAssert.IsNotFound(_target.GetActionResult(_controllerContext));
        }

        [TestMethod]
        public void TestViewCanReturnNonViewResults()
        {
            _target = new ResponseFormatter(f => { f.View(() => _emptyResult); });
            SetExtension(null);

            Assert.AreSame(_emptyResult, _target.GetActionResult(_controllerContext));
        }

        [TestMethod]
        public void TestJsonCanReturnNonJsonResults()
        {
            _target = new ResponseFormatter(f => { f.Json(() => _emptyResult); });
            SetExtension(ResponseFormatter.KnownExtensions.JSON);

            Assert.AreSame(_emptyResult, _target.GetActionResult(_controllerContext));
        }

        [TestMethod]
        public void TestExcelCanReturnNonExcelResults()
        {
            _target = new ResponseFormatter(f => { f.Excel(() => _emptyResult); });
            SetExtension(ResponseFormatter.KnownExtensions.EXCEL_2003);

            Assert.AreSame(_emptyResult, _target.GetActionResult(_controllerContext));
        }

        [TestMethod]
        public void TestFragmentCanReturnNonViewResults()
        {
            _target = new ResponseFormatter(f => { f.Fragment(() => _jsonResult); });
            SetExtension(ResponseFormatter.KnownExtensions.FRAGMENT);

            Assert.AreSame(_jsonResult, _target.GetActionResult(_controllerContext));
        }

        #endregion

        #endregion
    }
}
