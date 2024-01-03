using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class ProductionWorkOrderMeasurementPointValueControllerTest : MapCallMvcControllerTestBase<ProductionWorkOrderMeasurementPointValueController, ProductionWorkOrderMeasurementPointValue>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "ProductionWorkOrder", area = "Production", id = vm.ProductionWorkOrder };
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "ProductionWorkOrder", area = "Production", id = vm.ProductionWorkOrder };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/SAP/ProductionWorkOrderMeasurementPointValue/Create", role, RoleActions.Read);
                a.RequiresRole("~/SAP/ProductionWorkOrderMeasurementPointValue/Update", role, RoleActions.Read);
            });
        }
    }
}
