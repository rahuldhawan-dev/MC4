using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class StandardOperatingProcedureReviewControllerTest : MapCallMvcControllerTestBase<StandardOperatingProcedureReviewController, StandardOperatingProcedureReview>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateStandardOperatingProcedureReview)vm;
                model.Questions = new List<CreateStandardOperatingProcedureReview.CSOPRReviewAnswer>();
            };
        }

        protected override MapCall.Common.Model.Entities.Users.User CreateUser()
        {
            var user = base.CreateUser();
            user.IsAdmin = true;
            return user;
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ManagementGeneral;
            Assert.AreEqual(module, StandardOperatingProcedureController.ROLE, "The role for this controller must be the same as the SOPController's.");
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/StandardOperatingProcedureReview/New/", module, RoleActions.Add);
                a.RequiresRole("~/StandardOperatingProcedureReview/Create/", module, RoleActions.Add);
                a.RequiresRole("~/StandardOperatingProcedureReview/Show/", module, RoleActions.Read);
                a.RequiresRole("~/StandardOperatingProcedureReview/Index/", module, RoleActions.Read);
                a.RequiresRole("~/StandardOperatingProcedureReview/Search/", module, RoleActions.Read);
                a.RequiresRole("~/StandardOperatingProcedureReview/Edit/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/StandardOperatingProcedureReview/Update/", module, RoleActions.UserAdministrator);
            });
        }

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to New parameter
            var sopId = GetFactory<StandardOperatingProcedureFactory>().Create().Id;
            var result = _target.New(sopId);
            MvcAssert.IsViewNamed(result, "New");
            var model = (CreateStandardOperatingProcedureReview)((ViewResultBase)result).Model;
            Assert.AreEqual(sopId, model.StandardOperatingProcedure);
        }

        #endregion

        #region Search

        [TestMethod]
        public override void TestSearchReturnsSearchViewWithModel()
        {
            // Override test because Search action uses different model
            // type than Index action.

            var result = (ViewResult)_target.Search();

            Assert.IsInstanceOfType(result.Model, typeof(SOPWrapModel));
            MvcAssert.IsViewNamed(result, "Search");
        }

        #endregion

        #endregion
    }
}
