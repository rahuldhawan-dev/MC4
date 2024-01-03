using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class EstimatingProjectPermitControllerTest : MapCallMvcControllerTestBase<EstimatingProjectPermitController, EstimatingProjectPermit>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Request.CreateAndInitializeController<EstimatingProjectPermitController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditEstimatingProjectPermit)vm;
                return new {
                    action = "Show", 
                    controller = "EstimatingProject", 
                    area = "ProjectManagement", 
                    id = model.EstimatingProject
                };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesEstimatingProjects;
                a.RequiresRole("~/ProjectManagement/EstimatingProjectPermit/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProjectPermit/Update/", module, RoleActions.Edit);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var ep = GetEntityFactory<EstimatingProject>().Create();
            var eq = GetEntityFactory<EstimatingProjectPermit>().Create(new { Quantity = 1, Cost = 5m, EstimatingProject = ep });
            var expected = 4;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEstimatingProjectPermit, EstimatingProjectPermit>(eq, new {
                Quantity = expected
            }));

            Assert.AreEqual(expected, Session.Get<EstimatingProjectPermit>(eq.Id).Quantity);
        }

        #endregion
    }
}