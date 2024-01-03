using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using MMSINC.ErrorHandlers;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.Core.MvcTest.ErrorHandlers
{
    [TestClass]
    public class NotFoundErrorHandlerAttributeTest
    {
        #region Fields

        private TestNotFoundErrorHandlerAttribute _target;

        private Mock<HttpContextBase> _httpContext;
        private Mock<HttpResponseBase> _response;
        private Mock<HttpRequestBase> _request;
        private Mock<Controller> _controller;
        private Mock<ControllerContext> _controllerContext;
        private HttpException _httpEx;

        private string _controllerName = "ControllerName";
        private string _actionName = "ActionName";
        private string _http404Desc = "NOOOOOOOO";

        #endregion

        #region Setup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new TestNotFoundErrorHandlerAttribute();
            _target.View = "This is a view name";
            InitializeMocks();
        }

        private void InitializeMocks(MockBehavior behavior = MockBehavior.Default)
        {
            _controller = new Mock<Controller>(behavior);
            _controllerContext = new Mock<ControllerContext>(behavior);
            _controller.Object.ControllerContext = _controllerContext.Object; // Not virtual, needs to be set foreal.
            _httpContext = new Mock<HttpContextBase>(behavior);
            _response = new Mock<HttpResponseBase>(behavior);
            _request = new Mock<HttpRequestBase>();
            _httpContext.Setup(x => x.Response).Returns(_response.Object);
            _httpContext.Setup(x => x.Request).Returns(_request.Object);
        }

        private void SetRouteData(ControllerContext context)
        {
            context.RouteData.Values["controller"] = _controllerName;
            context.RouteData.Values["action"] = _actionName;
        }

        // Have to do this since IsAjaxRequest is an extension method.
        private void SetIsAjaxRequest(bool wellIsIt)
        {
            var nvc = new NameValueCollection();
            if (wellIsIt)
            {
                nvc["X-Requested-With"] = "XMLHttpRequest";
            }

            _request.Setup(x => x.Headers).Returns(nvc);
        }

        #endregion

        #region Tests

        #region Properties

        [TestMethod]
        public void TestDefaultErrorMessageSetsAndGets()
        {
            _target.DefaultErrorMessage = "No";
            Assert.AreEqual("No", _target.DefaultErrorMessage);
        }

        [TestMethod]
        public void TestViewSetsAndGets()
        {
            _target.View = "Bilbo";
            Assert.AreEqual("Bilbo", _target.View);
        }

        [TestMethod]
        public void TestHandleAjaxRequestsSetsAndGets()
        {
            _target.HandleAjaxRequests = true;
            Assert.IsTrue(_target.HandleAjaxRequests);
        }

        [TestMethod]
        public void TestHandleAjaxRequestsDefaultsToFalse()
        {
            Assert.IsFalse(_target.HandleAjaxRequests);
        }

        #endregion

        #region OnException

        private ExceptionContext CreateExceptionContext()
        {
            _httpEx = new HttpException(404, "Hi");
            var ec = new ExceptionContext {
                Controller = _controller.Object,
                Exception = _httpEx,
                HttpContext = _httpContext.Object
            };
            SetRouteData(ec);
            return ec;
        }

        [TestMethod]
        public void TestOnExceptionThrowsForNullExceptionContext()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.OnException(null));
        }

        [TestMethod]
        public void TestOnExceptionDoesNothingWhenExceptionIsNotHttpException()
        {
            InitializeMocks(MockBehavior.Strict);
            var ec = CreateExceptionContext();
            ec.Exception = new Exception("Nope");
            _target.OnException(ec);
            Assert.IsInstanceOfType(ec.Result, typeof(EmptyResult));
            _response.VerifyAll();
        }

        [TestMethod]
        public void TestOnExceptionDoesNothingWhenHttpExceptionIsNot404()
        {
            InitializeMocks(MockBehavior.Strict);
            var ec = CreateExceptionContext();
            ec.Exception = new HttpException(214, "Nope");
            _target.OnException(ec);
            Assert.IsInstanceOfType(ec.Result, typeof(EmptyResult));
            _response.VerifyAll();
        }

        [TestMethod]
        public void TestOnExceptionDoesNotChangeContextException()
        {
            var ec = CreateExceptionContext();
            _target.OnException(ec);
            Assert.AreSame(_httpEx, ec.Exception,
                "The exception should not be changed as other exception filters might need it.");
        }

        [TestMethod]
        public void TestOnExceptionSetsResultToNewViewResult()
        {
            var ec = CreateExceptionContext();
            _target.OnException(ec);
            var result = (ViewResult)ec.Result;
            Assert.AreEqual(_target.View, result.ViewName);

            var model = (HandleErrorInfo)result.ViewData.Model;
            Assert.AreEqual(model.ActionName, _actionName);
            Assert.AreEqual(model.ControllerName, _controllerName);
            Assert.AreSame(_httpEx, model.Exception);
        }

        [TestMethod]
        public void TestOnExceptionSetsExpectedResponseSettings()
        {
            var ec = CreateExceptionContext();
            _target.OnException(ec);
            _response.Verify(x => x.Clear());
            _response.VerifySet(x => x.StatusCode = _httpEx.GetHttpCode(), "StatusCode needs to be set.");
            _response.VerifySet(x => x.TrySkipIisCustomErrors = true,
                "TrySkipIisCustomErrors needs to be true in IIS7+");
        }

        #endregion

        #region OnResultExecuting

        private ResultExecutingContext CreateResultExecutingContext()
        {
            _httpEx = new HttpException(404, "Hi");
            var rec = new ResultExecutingContext {
                Controller = _controller.Object,
                HttpContext = _httpContext.Object,
                Result = new HttpStatusCodeResult(404, _http404Desc)
            };

            SetRouteData(rec);
            return rec;
        }

        [TestMethod]
        public void TestOnResultExecutingThrowsForNullExceptionContext()
        {
            MyAssert.Throws<ArgumentNullException>(() => _target.OnResultExecuting(null));
        }

        private void TestOnResultExecutingDoesNothing(ActionResult result)
        {
            SetIsAjaxRequest(false);
            InitializeMocks(MockBehavior.Strict);
            var rec = CreateResultExecutingContext();
            rec.Result = result;
            _target.OnResultExecuting(rec);
            _response.VerifyAll();
        }

        [TestMethod]
        public void TestOnResultExecutingDoesNothingIfContextResultIsNull()
        {
            TestOnResultExecutingDoesNothing(null);
        }

        [TestMethod]
        public void TestOnResultExecutingDoesNothingIfContextResultIsNotHttpStatusCodeResult()
        {
            TestOnResultExecutingDoesNothing(new ViewResult());
        }

        [TestMethod]
        public void TestOnResultExecutingDoesNothingIfContextResultIsHttpStatusCodeResultButStatusCodeIsNot404()
        {
            TestOnResultExecutingDoesNothing(new HttpStatusCodeResult(200));
        }

        [TestMethod]
        public void TestOnResultExecutingDoesNothingIfRequestAcceptTypesIncludesApplicationJson()
        {
            SetIsAjaxRequest(false);
            InitializeMocks(MockBehavior.Strict);
            _request.Setup(x => x.AcceptTypes).Returns(new[] {"application/json"});
            var rec = CreateResultExecutingContext();
            rec.Result = new HttpStatusCodeResult(404); // A 404 would do something otherwise.
            _target.OnResultExecuting(rec);
            _response.VerifyAll();
            _request.Verify(x => x.AcceptTypes);
        }

        [TestMethod]
        public void TestOnResultExecutingSetsResultToNewViewResult()
        {
            SetIsAjaxRequest(false);
            _target.UseTestViewResult = true;
            var rec = CreateResultExecutingContext();
            _target.OnResultExecuting(rec);
            var result = (ViewResult)rec.Result;
            Assert.AreEqual(_target.View, result.ViewName);

            var model = (HandleErrorInfo)result.ViewData.Model;
            Assert.AreEqual(model.ActionName, _actionName);
            Assert.AreEqual(model.ControllerName, _controllerName);
            Assert.AreEqual(_http404Desc, model.Exception.Message);
        }

        [TestMethod]
        public void
            TestOnResultExecutingSetsDescriptionToDefaultErrorMessageIfDescriptionIsNotSetInHttpStatusCodeResult()
        {
            SetIsAjaxRequest(false);
            _target.DefaultErrorMessage = "DEFAAAULT!";
            _target.UseTestViewResult = true;
            var rec = CreateResultExecutingContext();
            rec.Result = new HttpStatusCodeResult(404);
            _target.OnResultExecuting(rec);
            var result = (ViewResult)rec.Result;
            Assert.AreEqual(_target.View, result.ViewName);

            var model = (HandleErrorInfo)result.ViewData.Model;
            Assert.AreEqual(model.ActionName, _actionName);
            Assert.AreEqual(model.ControllerName, _controllerName);
            Assert.AreEqual("DEFAAAULT!", model.Exception.Message);
        }

        [TestMethod]
        public void TestOnResultExecutingSetsExpectedResponseSettings()
        {
            var ec = CreateExceptionContext();
            _target.OnException(ec);
            _response.Verify(x => x.Clear());
            _response.VerifySet(x => x.StatusCode = _httpEx.GetHttpCode(), "StatusCode needs to be set.");
            _response.VerifySet(x => x.TrySkipIisCustomErrors = true,
                "TrySkipIisCustomErrors needs to be true in IIS7+");
        }

        [TestMethod]
        public void TestOnResultExecutingExecutesResult()
        {
            _target.UseTestViewResult = true;
            var rec = CreateResultExecutingContext();
            _target.OnResultExecuting(rec);
            Assert.IsTrue(_target.TestViewResultWasExecuted);
        }

        #endregion

        #endregion

        #region Test class

        private class TestNotFoundErrorHandlerAttribute : NotFoundErrorHandlerAttribute
        {
            private TestViewResult _testViewResult;

            public bool UseTestViewResult { get; set; }

            public bool TestViewResultWasExecuted
            {
                get { return (_testViewResult != null ? _testViewResult.ExecuteCalled : false); }
            }

            public override ViewResult CreateErrorResult(ControllerContext context, HttpException httpEx)
            {
                var result = base.CreateErrorResult(context, httpEx);
                if (!UseTestViewResult)
                {
                    return result;
                }

                // Need to pass in what's set originally for consistency.
                _testViewResult = new TestViewResult {
                    ViewName = result.ViewName,
                    ViewData = result.ViewData,
                    TempData = result.TempData
                };
                return _testViewResult;
            }

            private class TestViewResult : ViewResult
            {
                public bool ExecuteCalled { get; private set; }

                public override void ExecuteResult(ControllerContext context)
                {
                    ExecuteCalled = true;
                }
            }
        }

        #endregion
    }
}
