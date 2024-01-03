using Contractors.Controllers;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class SmallMeterLocationControllerTest : ContractorControllerTestBase<SmallMeterLocationController, SmallMeterLocation>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.AllowsAnonymousAccess("~/SmallMeterLocation/ByMeterSupplementalLocationId");
            });
        }
    }
}