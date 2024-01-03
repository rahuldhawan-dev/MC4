using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EstimatingProjectOtherCostControllerTest : MapCallMvcControllerTestBase<EstimatingProjectOtherCostController, EstimatingProjectOtherCost>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Request.CreateAndInitializeController<EstimatingProjectOtherCostController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditEstimatingProjectOtherCost)vm;
                return new { action = "Show", controller = "EstimatingProject", area = "ProjectManagement", id = model.EstimatingProject };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesEstimatingProjects;
                a.RequiresRole("~/ProjectManagement/EstimatingProjectOtherCost/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProjectOtherCost/Update/", module, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var ep = GetEntityFactory<EstimatingProject>().Create();
            var eq = GetEntityFactory<EstimatingProjectOtherCost>().Create(new { Quantity = 1, Description = "Foo", Cost = 5m, EstimatingProject = ep }); 
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEstimatingProjectOtherCost, EstimatingProjectOtherCost>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<EstimatingProjectOtherCost>(eq.Id).Description);
        }

        #endregion
    }
}