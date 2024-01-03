using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EstimateControllerTest : MapCallMvcControllerTestBase<EstimateController, EstimatingProject, EstimatingProjectRepository>
    {
        #region Fields

        private User _user;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesEstimatingProjects;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/ProjectManagement/Estimate/New/", module, RoleActions.Read);
                a.RequiresRole("~/ProjectManagement/Estimate/Create/", module, RoleActions.Read);
            });
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // Test override needed due to Id parameter.
            var result = _target.New(42);
            MvcAssert.IsViewNamed(result, "New");
            var model = (CreateEstimateForm)((ViewResult)result).Model;
            Assert.AreEqual(42, model.Id);
        }

        [TestMethod]
        public void TestCreateReturnsPdfViewWithExpectedModel()
        {
            InitializeControllerForRequest("~/ProjectManagement/Estimate/Create/1.pdf");
            var entity = GetEntityFactory<EstimatingProject>().Create();
            var createForm = new CreateEstimateForm(_container) {Id = entity.Id};
            
            var result = _target.Create(createForm);

            MvcAssert.IsViewNamed(result, "Pdf");
            var model = (EstimateForm)((PdfResult)result).Model;
            Assert.AreSame(entity, model.Project);
            Assert.AreSame(createForm, model.Throwaways);
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // Noop: This creates a pdf instead and does not generate a new record. Other tests cover this.
        }
        
        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // Noop: This creates a pdf instead and does not generate a new record. Other tests cover this.

        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("Action does not do validation check.");
        }

        #endregion
    }
}
