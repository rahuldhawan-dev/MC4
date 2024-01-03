using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class ServiceQualityAssuranceReportControllerTest : MapCallMvcControllerTestBase<ServiceQualityAssuranceReportController, WorkOrder, WorkOrderRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesAssets;

            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/ServiceQualityAssuranceReport/Index", role);
                a.RequiresRole("~/Reports/ServiceQualityAssuranceReport/Search", role);
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
