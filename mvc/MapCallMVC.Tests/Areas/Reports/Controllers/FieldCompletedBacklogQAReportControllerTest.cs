using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class FieldCompletedBacklogQAReportControllerTest : MapCallMvcControllerTestBase<FieldCompletedBacklogQAReportController, WorkOrder, WorkOrderRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesWorkManagement;

            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/FieldCompletedBacklogQAReport/Index", role);
                a.RequiresRole("~/Reports/FieldCompletedBacklogQAReport/Search", role);
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
