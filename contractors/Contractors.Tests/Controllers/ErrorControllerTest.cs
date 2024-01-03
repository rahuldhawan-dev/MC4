using System;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Controllers;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class ErrorControllerTest : ControllerTestBase<ErrorController, IRepository<ContractorUser>>
    {
        [TestInitialize]
        public void ErrorControllerTestInitialize()
        {
            BaseInitialize();
        }

        [TestMethod]
        public void TestWhyDoIUseThisBaseClass()
        {
            Assert.Inconclusive("TODO: I should be using the regular ControllerBaseTest shouldn't I?");
        }

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

        #region NotFound

        [TestMethod]
        public void TestNotFoundDoesNotChangeModelIfModelIsAlreadySet()
        {
            var expected = new HandleErrorInfo(new Exception(),
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
