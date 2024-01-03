using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class VehicleUtilizationControllerTest : MapCallMvcControllerTestBase<VehicleUtilizationController, Vehicle>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FleetManagementGeneral;
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Reports/VehicleUtilization/Search", role, RoleActions.Read);
                a.RequiresRole("~/Reports/VehicleUtilization/Index", role, RoleActions.Read);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }

        #endregion
    }
}
