using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class WBSNumberControllerTest : MapCallMvcControllerTestBase<WBSNumberController, WBSNumber>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WBSNumber/ByOperatingCenterId");
            });
        }
    }
}