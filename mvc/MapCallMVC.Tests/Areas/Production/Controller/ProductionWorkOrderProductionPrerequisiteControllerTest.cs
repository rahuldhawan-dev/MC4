using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class ProductionWorkOrderProductionPrerequisiteControllerTest : MapCallMvcControllerTestBase<ProductionWorkOrderProductionPrerequisiteController, ProductionWorkOrderProductionPrerequisite>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditProductionWorkOrderProductionPrerequisite)vm;
                return new { action = "Show", controller = "ProductionWorkOrder", area = "Production", id = model.ProductionWorkOrder.Id };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/SAP/ProductionWorkOrderProductionPrerequisite/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/SAP/ProductionWorkOrderProductionPrerequisite/Update", role, RoleActions.Edit);
            });
        }
    }
}
