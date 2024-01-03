using System;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCall.Common.MvcTest.Controllers
{
    [TestClass]
    public class ErrorControllerTest : ControllerTestBase<ErrorController>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            BaseInitialize();
        }

        #region Forbidden

        [TestMethod]
        public void Forbidden()
        {
            var result = _target.Forbidden() as ViewResult; // VERBOTEN!!!

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestForbiddenAllowsAnonymous()
        {
            MyAssert.MethodHasAttribute<AllowAnonymousAttribute>(_target, "Forbidden");
        }

        #endregion

        #region NotFound

        [TestMethod]
        public void TestNotFoundAllowsAnonymous()
        {
            MyAssert.MethodHasAttribute<AllowAnonymousAttribute>(_target, "NotFound");
        }

        [TestMethod]
        public void TestNotFoundDoesNotChangeModelIfModelIsAlreadySet()
        {
            var expected = new HandleErrorInfo(new Exception(),
                // ReSharper disable once Mvc.ControllerNotResolved, Mvc.ActionNotResolved
                "Controller", "Action");
            _target.ViewData.Model = expected;
            _target.NotFound();
            Assert.AreSame(expected, _target.ViewData.Model);
        }

        [TestMethod]
        public void TestNotFoundCreatesNewModelIfModelIsNull()
        {
            _target.RouteData.Values["controller"] = "Controller";
            _target.RouteData.Values["action"] = "Action";
            _target.ViewData.Model = null;
            var result = (HandleErrorInfo)_target.NotFound().Model;
            var exception = (HttpException)result.Exception;
            Assert.AreEqual(404, exception.GetHttpCode());
            Assert.AreEqual(ErrorController.DEFAULT_NOT_FOUND_MESSAGE, exception.Message);
            Assert.AreEqual("Controller", result.ControllerName);
            Assert.AreEqual("Action", result.ActionName);
        }

        #endregion
    }
}
