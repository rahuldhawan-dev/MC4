using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EstimatingProjectContractorLaborCostControllerTest : MapCallMvcControllerTestBase<EstimatingProjectContractorLaborCostController, EstimatingProjectContractorLaborCost>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Request.CreateAndInitializeController<EstimatingProjectContractorLaborCostController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditEstimatingProjectContractorLaborCost)vm;
                return new { action = "Show", controller = "EstimatingProject", area = "ProjectManagement", id = model.EstimatingProject };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.FieldServicesEstimatingProjects;
                a.RequiresRole("~/ProjectManagement/EstimatingProjectContractorLaborCost/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProjectContractorLaborCost/Update/", module, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EstimatingProjectContractorLaborCost>().Create();
            var expected = 2;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEstimatingProjectContractorLaborCost, EstimatingProjectContractorLaborCost>(eq, new {
                Quantity = expected
            }));

            Assert.AreEqual(expected, Session.Get<EstimatingProjectContractorLaborCost>(eq.Id).Quantity);
        }

        #endregion
    }
}