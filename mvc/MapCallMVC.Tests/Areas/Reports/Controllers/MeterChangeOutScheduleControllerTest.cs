using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Reports.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class MeterChangeOutScheduleControllerTest : MapCallMvcControllerTestBase<MeterChangeOutScheduleController, MeterChangeOut>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesMeterChangeOuts;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/MeterChangeOutSchedule/Index/", module, RoleActions.Read);
            });
        }
    }
}