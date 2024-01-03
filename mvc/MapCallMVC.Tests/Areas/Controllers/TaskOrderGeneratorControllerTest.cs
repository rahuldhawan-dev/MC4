using System;
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
    public class TaskOrderGeneratorControllerTest : MapCallMvcControllerTestBase<TaskOrderGeneratorController, EstimatingProject, EstimatingProjectRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (TaskOrderGeneratorForm)vm;
                // None of these values matter. 
                // They're just required fields for a pdf. They don't get saved anyplace.
                model.EffectiveAgreementDate = DateTime.Now;
                model.BeginDate = DateTime.Now;
                model.SubstantialCompletionDate = DateTime.Now;
                model.EndDate = DateTime.Now;
                model.ContractorAgreementDate = DateTime.Now;
                model.AsDetailedIn = "It got detailed!";
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesEstimatingProjects;
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/TaskOrderGenerator/Show", role);
                a.RequiresRole("~/ProjectManagement/TaskOrderGenerator/New", role);
                a.RequiresRole("~/ProjectManagement/TaskOrderGenerator/Create", role, RoleActions.Read);
            });
        }

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // override needed because of TempData and view model
            _target.TempData[TaskOrderGeneratorController.MODEL_TEMP_DATA_KEY] = new TaskOrderGeneratorForm(_container);

            var entity = GetEntityFactory<EstimatingProject>().Create();

            var result = _target.Show(entity.Id) as ViewResult;
            var model = result.Model;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof(TaskOrderGeneratorForm));
        }

        [TestMethod]
        public void TestShow404sIfTaskOrderGeneratorFormTempDataDoesNotExist()
        {
            _target.TempData[TaskOrderGeneratorController.MODEL_TEMP_DATA_KEY] = null;

            Assert.IsNotNull(_target.Show(666) as HttpNotFoundResult);
        }

        [TestMethod]
        public void TestShow404sIfTaskOrderGeneratorDoesNotExist()
        {
            _target.TempData[TaskOrderGeneratorController.MODEL_TEMP_DATA_KEY] = new TaskOrderGeneratorForm(_container);

            Assert.IsNotNull(_target.Show(666) as HttpNotFoundResult);
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override because base test doesn't set the Id on the view model it creates(because it
            // doesn't make sense to normally).
            var model = _viewModelFactory.Build<TaskOrderGeneratorForm, EstimatingProject>(
                GetEntityFactory<EstimatingProject>().Create());
            
            var result = _target.New(model) as ViewResult;
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(TaskOrderGeneratorForm));
            MvcAssert.IsViewNamed(result, "New");
        }

        [TestMethod]
        public void TestNewReturnsNotFoundIfEstimatingProjectDoesNotExist()
        {
            MvcAssert.IsNotFound(_target.New(new TaskOrderGeneratorForm(_container)));
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("Why doesn't this do a validation check?");
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            Assert.Inconclusive("Test that this is storing in TempData");
        }

        #endregion
    }
}