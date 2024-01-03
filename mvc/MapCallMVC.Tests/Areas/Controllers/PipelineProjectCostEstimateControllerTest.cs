using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class PipelineProjectCostEstimateControllerTest : MapCallMvcControllerTestBase<PipelineProjectCostEstimateController, EstimatingProject, EstimatingProjectRepository >
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
            var role = RoleModules.FieldServicesEstimatingProjects;
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/PipelineProjectCostEstimate/Show", role);
                a.RequiresRole("~/ProjectManagement/PipelineProjectCostEstimate/New", role);
                a.RequiresRole("~/ProjectManagement/PipelineProjectCostEstimate/Create", role);
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // noop. TempData stuff dealt with in other tests.
        }

        [TestMethod]
        public void TestShowShowsThePipelineProjectCostEstimateIfItExists()
        {
            _target.TempData[PipelineProjectCostEstimateController.MODEL_TEMP_DATA_KEY] = new PipelineProjectCostEstimate(_container);

            var entity = GetEntityFactory<EstimatingProject>().Create();

            var result = _target.Show(entity.Id) as ViewResult;
            var model = result.Model;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof(PipelineProjectCostEstimate));
        }

        [TestMethod]
        public void TestShow404sIfPipelineProjectCostEstimateDoesNotExist()
        {
            _target.TempData[PipelineProjectCostEstimateController.MODEL_TEMP_DATA_KEY] = new PipelineProjectCostEstimate(_container);

            Assert.IsNotNull(_target.Show(666) as HttpNotFoundResult);
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // Test override needed because doesn't use ActionHelper and also returns a 404

            var model = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(GetEntityFactory<EstimatingProject>().Create());

            var result = _target.New(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(PipelineProjectCostEstimate));
            MvcAssert.IsViewNamed(result, "New");
        }

        [TestMethod]
        public void TestNewReturnsNotFoundIfEstimatingProjectDoesNotExist()
        {
            Assert.IsNotNull(_target.New(new PipelineProjectCostEstimate(_container)) as HttpNotFoundResult);
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // noop: The controller doesn't create anything in the database.
            Assert.Inconclusive("Test that the model is stored in Tempdata");
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // noop: The controller doesn't create anything in the database.
            Assert.Inconclusive("Fix this test. It doesn't work with the automated tests.");
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // noop: The controller doesn't create anything in the database.
            Assert.Inconclusive("Fix this test. It doesn't work with the automated tests.");
        }
        
        #endregion
    }
}
