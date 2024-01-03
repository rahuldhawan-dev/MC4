using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels.PlanReviews;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Facilities.Controllers
{
    [TestClass]
    public class PlanReviewControllerTest : MapCallMvcControllerTestBase<PlanReviewController, PlanReview>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/Facilities/PlanReview/Search/", RoleModules.ProductionEquipment);
                a.RequiresRole("~/Facilities/PlanReview/Show/", RoleModules.ProductionEquipment);
                a.RequiresRole("~/Facilities/PlanReview/Index/", RoleModules.ProductionEquipment);
                a.RequiresRole("~/Facilities/PlanReview/New/", RoleModules.ProductionEquipment, RoleActions.Add);
                a.RequiresRole("~/Facilities/PlanReview/Create/", RoleModules.ProductionEquipment, RoleActions.Add);
                a.RequiresRole("~/Facilities/PlanReview/Edit/", RoleModules.ProductionEquipment, RoleActions.Edit);
                a.RequiresRole("~/Facilities/PlanReview/Update/", RoleModules.ProductionEquipment, RoleActions.Edit);
                a.RequiresRole("~/Facilities/PlanReview/Destroy/", RoleModules.ProductionEquipment, RoleActions.Delete);
            });
        }				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXlsExportsExcel()
        {
            var entity0 = GetEntityFactory<PlanReview>().Create(new {ReviewChangeNotes = "description 0"});
            var entity1 = GetEntityFactory<PlanReview>().Create(new {ReviewChangeNotes = "description 1"});
            var search = new SearchPlanReview();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ReviewChangeNotes, "ReviewChangeNotes");
                helper.AreEqual(entity1.ReviewChangeNotes, "ReviewChangeNotes", 1);
            }
        }

        #endregion

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // other tests needed to cover this
        }

        [TestMethod]
        public void TestNewReturnsViewWithCreateViewModelWhenParametersAreValid()
        {
            var emergencyResponsePlan = GetEntityFactory<EmergencyResponsePlan>().Create();

            var newViewModel = _viewModelFactory.BuildWithOverrides<NewPlanReview>(new {
                Plan = emergencyResponsePlan.Id
            });

            var actionResult = (ViewResult)_target.New(newViewModel);
            var viewModel = (CreatePlanReview)actionResult.Model;

            Assert.AreEqual(emergencyResponsePlan.Id, viewModel.Plan);
            MvcAssert.IsViewNamed(actionResult, "New");
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            const string reviewChangeNotes = "Please change the things as needed.";

            var entity = GetEntityFactory<PlanReview>().Create();

            _target.Update(_viewModelFactory.BuildWithOverrides<PlanReviewViewModel, PlanReview>(entity, new {
                ReviewChangeNotes = reviewChangeNotes
            }));

            var updatedEntity = Session.Get<PlanReview>(entity.Id);

            Assert.AreEqual(reviewChangeNotes, updatedEntity.ReviewChangeNotes);
        }

        #endregion
    }
}
