using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class StandardOperatingProcedureQuestionControllerTest : MapCallMvcControllerTestBase<StandardOperatingProcedureQuestionController, StandardOperatingProcedureQuestion>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/StandardOperatingProcedureQuestion/Activate/", StandardOperatingProcedureController.ROLE, RoleActions.UserAdministrator);
                a.RequiresRole("~/StandardOperatingProcedureQuestion/Deactivate/", StandardOperatingProcedureController.ROLE, RoleActions.UserAdministrator);
            });
        }

        [TestMethod]
        public void TestActivateReturns404IfQuestionDoesNotExist()
        {
            var result = _target.Activate(1);
            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void TestActivateActivatesQuestion()
        {
            var question = GetFactory<StandardOperatingProcedureQuestionFactory>().Create(new {
                IsActive = false
            });

            Assert.IsFalse(question.IsActive);
            var result = _target.Activate(question.Id);
            Assert.IsTrue(question.IsActive);
            MvcAssert.RedirectsToRoute(result, "StandardOperatingProcedure", "Show", new{ id = question.StandardOperatingProcedure.Id });
        }

        [TestMethod]
        public void TestDeactivateREturns404IfQuestionDoesNotExist()
        {
            var result = _target.Deactivate(1);
            MvcAssert.IsNotFound(result);
        }

        [TestMethod]
        public void TestDeactivateDeactivatesQuestion()
        {
            var question = GetFactory<StandardOperatingProcedureQuestionFactory>().Create(new
            {
                IsActive = true
            });

            Assert.IsTrue(question.IsActive);
            var result = _target.Deactivate(question.Id);
            Assert.IsFalse(question.IsActive);
            MvcAssert.RedirectsToRoute(result, "StandardOperatingProcedure", "Show", new { id = question.StandardOperatingProcedure.Id });
        }
        
        #endregion
    }
}
