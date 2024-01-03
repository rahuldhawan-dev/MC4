using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class ServicesTDPendingServicesKPIControllerTest : MapCallMvcControllerTestBase<ServicesTDPendingServicesKPIController, Service, ServiceRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/ServicesTDPendingServicesKPI/Search", role);
                a.RequiresRole("~/Reports/ServicesTDPendingServicesKPI/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            Assert.Inconclusive("Test me");
        }
    }
}