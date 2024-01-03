using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MMSINC.ErrorHandlers;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.Core.MvcTest.ErrorHandlers
{
    [TestClass]
    public class AjaxErrorHandlerAttributeTest
    {
        #region Fields

        private AjaxErrorHandlerAttribute _target;
        private ExceptionContext _exceptionContext;
        private Exception _exception;
        private Mock<HttpContextBase> _httpContext;
        private Mock<HttpRequestBase> _request;

        #endregion

        #region Setup

        [TestInitialize]
        public void InitializeTest()
        {
            _httpContext = new Mock<HttpContextBase>();
            _request = new Mock<HttpRequestBase>();
            _exception = new Exception("I'm your PRIIIIVATE exception! Exception for money!");
            _exceptionContext = new ExceptionContext {
                Exception = _exception,
                HttpContext = _httpContext.Object
            };
            _httpContext.Setup(x => x.Request).Returns(_request.Object);
            _target = new AjaxErrorHandlerAttribute();
            SetIsAjaxRequest(true);
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

        [TestMethod]
        public void TestShowExplicitErrorMessagesProperty()
        {
            Assert.IsFalse(_target.ShowExplicitErrorMessages);
            _target.ShowExplicitErrorMessages = true;
            Assert.IsTrue(_target.ShowExplicitErrorMessages);
        }

        [TestMethod]
        public void TestGenericErrorMessageProperty()
        {
            _target.GenericErrorMessage = "Whoops";
            Assert.AreEqual("Whoops", _target.GenericErrorMessage);
        }

        [TestMethod]
        public void TestGenericErrorMessagePropertyReturnsDefaultIfNull()
        {
            Assert.AreEqual(
                AjaxErrorHandlerAttribute.DEFAULT_GENERIC_ERROR_MESSAGE,
                _target.GenericErrorMessage);
        }

        [TestMethod]
        public void TestOnExceptionDoesNotHandleAnythingIfAjaxRequestIsFalse()
        {
            SetIsAjaxRequest(false);
            _target.OnException(_exceptionContext);
            Assert.IsFalse(_exceptionContext.ExceptionHandled);
            Assert.IsInstanceOfType(_exceptionContext.Result, typeof(EmptyResult),
                "This should be an EmptyResult if we don't handle it. In practice it might not depending on other filters, but in testing it should be empty.");
        }

        [TestMethod]
        public void TestOnExceptionHandlesThingsWhenRequestAcceptTypesContainsApplicationJson()
        {
            SetIsAjaxRequest(false);
            _request.Setup(x => x.AcceptTypes).Returns(new[] {"application/json"});
            _target.OnException(_exceptionContext);
            Assert.IsTrue(_exceptionContext.ExceptionHandled);
        }

        [TestMethod]
        public void TestOnExceptionDoesNotThrowWhenAcceptTypesIsNull()
        {
            SetIsAjaxRequest(false);
            _request.Setup(x => x.AcceptTypes).Returns((string[])null);
            MyAssert.DoesNotThrow(() => _target.OnException(_exceptionContext));
            _request.Verify(x => x.AcceptTypes);
        }

        [TestMethod]
        public void TestOnExceptionDoesNotSetExceptionHandledToTrueIfShowExplicitErrorMessageIsTrue()
        {
            _target.ShowExplicitErrorMessages = true;
            _target.OnException(_exceptionContext);
            Assert.IsFalse(_exceptionContext.ExceptionHandled);
        }

        [TestMethod]
        public void TestOnExceptionSetsExceptionHandledtoTrueIfShowExplicityErrorMessageIsFalse()
        {
            _target.ShowExplicitErrorMessages = false;
            _target.OnException(_exceptionContext);
            Assert.IsTrue(_exceptionContext.ExceptionHandled);
        }

        [TestMethod]
        public void TestOnExceptionSetsResultToInstanceOfJsonHttpStatusCodeResult()
        {
            _target.OnException(_exceptionContext);
            Assert.IsInstanceOfType(_exceptionContext.Result,
                typeof(JsonHttpStatusCodeResult));
        }

        [TestMethod]
        public void TestOnExceptionSetsStatusCodeOfResult()
        {
            _target.StatusCode = HttpStatusCode.Gone;
            _target.OnException(_exceptionContext);
            var result = (JsonHttpStatusCodeResult)_exceptionContext.Result;
            Assert.AreEqual((int)HttpStatusCode.Gone, result.StatusCode);
        }

        [TestMethod]
        public void TestOnExceptionSetsStatusDescriptionToExceptionMessageIfShowExplicitIsTrue()
        {
            _target.ShowExplicitErrorMessages = true;
            _target.OnException(_exceptionContext);
            var result = (JsonHttpStatusCodeResult)_exceptionContext.Result;
            Assert.AreEqual(_exception.Message, result.StatusDescription);
        }

        [TestMethod]
        public void TestOnExceptionSetsStatusDescriptionToGenericMessageIfShowExplicitIsFalse()
        {
            _target.ShowExplicitErrorMessages = false;
            _target.OnException(_exceptionContext);
            var result = (JsonHttpStatusCodeResult)_exceptionContext.Result;
            Assert.AreEqual(_target.GenericErrorMessage, result.StatusDescription);
        }

        [TestMethod]
        public void TestOnResultExecutedSeeminglyDoesNothing()
        {
            // It's a noop method. Code coverage.
            _target.OnResultExecuted(null);
        }

        #endregion
    }
}
