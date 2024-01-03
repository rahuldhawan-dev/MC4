using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class ServicesCompletedByCategoryControllerTest : MapCallMvcControllerTestBase<ServicesCompletedByCategoryController, Service, ServiceRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/ServicesCompletedByCategory/Search", role);
                a.RequiresRole("~/Reports/ServicesCompletedByCategory/Index", role);
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