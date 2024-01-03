using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class MarkoutDamageBPUControllerTest : MapCallMvcControllerTestBase<MarkoutDamageBPUController, MarkoutDamage, MarkoutDamageRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/MarkoutDamageBPU/Search/", RoleModules.FieldServicesWorkManagement);
                a.RequiresRole("~/Reports/MarkoutDamageBPU/Index/", RoleModules.FieldServicesWorkManagement);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }
    }
}
