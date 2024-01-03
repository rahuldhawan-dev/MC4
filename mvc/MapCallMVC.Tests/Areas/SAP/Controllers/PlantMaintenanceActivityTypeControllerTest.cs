using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.SAP.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.SAP.Controllers
{
    [TestClass]
    public class PlantMaintenanceActivityTypeControllerTest : MapCallMvcControllerTestBase<PlantMaintenanceActivityTypeController, PlantMaintenanceActivityType>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/SAP/PlantMaintenanceActivityType/ByOrderTypeId");
                a.RequiresLoggedInUserOnly("~/SAP/PlantMaintenanceActivityType/ByOrderTypeIds");
            });
        }
    }
}
