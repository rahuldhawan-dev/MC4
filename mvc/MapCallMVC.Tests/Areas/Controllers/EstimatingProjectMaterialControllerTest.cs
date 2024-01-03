using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EstimatingProjectMaterialControllerTest : MapCallMvcControllerTestBase<EstimatingProjectMaterialController, EstimatingProjectMaterial>
    {
        #region Constants

        #endregion
        
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Request.CreateAndInitializeController<EstimatingProjectMaterialController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditEstimatingProjectMaterial)vm;
                return new { action = "Show", controller = "EstimatingProject", area = "ProjectManagement", id = model.EstimatingProject };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesEstimatingProjects;
                a.RequiresRole("~/ProjectManagement/EstimatingProjectMaterial/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProjectMaterial/Update/", module, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EstimatingProjectMaterial>().Create();
            var expected = 4;
            var edit = _viewModelFactory.BuildWithOverrides<EditEstimatingProjectMaterial, EstimatingProjectMaterial>(eq, new {Quantity = expected});
            var result = _target.Update(edit) as RedirectToRouteResult;

            Assert.AreEqual(expected, Session.Get<EstimatingProjectMaterial>(eq.Id).Quantity);
        }

        #endregion
    }
}