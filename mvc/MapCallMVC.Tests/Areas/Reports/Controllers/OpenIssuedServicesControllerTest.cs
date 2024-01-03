using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class OpenIssuedServicesControllerTest : MapCallMvcControllerTestBase<OpenIssuedServicesController, Service, ServiceRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Reports/OpenIssuedServices/Index");
                a.RequiresLoggedInUserOnly("~/Reports/OpenIssuedServices/Search");
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            Assert.Inconclusive("Test me");
        }
    }
}
