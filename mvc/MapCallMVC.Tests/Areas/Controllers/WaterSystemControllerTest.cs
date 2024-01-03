using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Admin.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class WaterSystemControllerTest : MapCallMvcControllerTestBase<WaterSystemController, WaterSystem>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/Admin/WaterSystem/Search");
                a.RequiresSiteAdminUser("~/Admin/WaterSystem/Show");
                a.RequiresSiteAdminUser("~/Admin/WaterSystem/Edit");
                a.RequiresSiteAdminUser("~/Admin/WaterSystem/New");
                a.RequiresSiteAdminUser("~/Admin/WaterSystem/Create");
                a.RequiresSiteAdminUser("~/Admin/WaterSystem/Update");
                a.RequiresSiteAdminUser("~/Admin/WaterSystem/Index");
                a.RequiresLoggedInUserOnly("~/Admin/WaterSystem/ByOperatingCenterId/");
            });
        }
    }
}
