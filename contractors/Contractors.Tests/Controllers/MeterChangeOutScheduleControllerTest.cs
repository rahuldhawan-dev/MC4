using Contractors.Controllers;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class MeterChangeOutScheduleControllerTest : ContractorControllerTestBase<MeterChangeOutScheduleController, MeterChangeOut>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/MeterChangeOutSchedule/Index/");
            });
        }
    }
}