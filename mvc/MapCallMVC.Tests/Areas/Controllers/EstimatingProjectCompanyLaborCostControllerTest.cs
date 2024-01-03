using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EstimatingProjectCompanyLaborCostControllerTest : MapCallMvcControllerTestBase<EstimatingProjectCompanyLaborCostController, EstimatingProjectCompanyLaborCost>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Request.CreateAndInitializeController<EstimatingProjectCompanyLaborCostController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditEstimatingProjectCompanyLaborCost)vm;
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
                a.RequiresRole("~/ProjectManagement/EstimatingProjectCompanyLaborCost/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProjectCompanyLaborCost/Update/", module, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var ep = GetEntityFactory<EstimatingProject>().Create();
            var clc = GetEntityFactory<CompanyLaborCost>().Create();
            var eq = GetEntityFactory<EstimatingProjectCompanyLaborCost>().Create(new { Quantity = 1, EstimatingProject = ep, CompanyLaborCost = clc });
            var expected = 99;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEstimatingProjectCompanyLaborCost, EstimatingProjectCompanyLaborCost>(eq, new {
                Quantity = expected
            }));

            Assert.AreEqual(expected, Session.Get<EstimatingProjectCompanyLaborCost>(eq.Id).Quantity);
        }

        #endregion
    }
}