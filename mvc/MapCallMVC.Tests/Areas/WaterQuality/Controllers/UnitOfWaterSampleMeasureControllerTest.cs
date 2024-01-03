using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.WaterQuality.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class UnitOfWaterSampleMeasureControllerTest : MapCallMvcControllerTestBase<UnitOfWaterSampleMeasureController, UnitOfWaterSampleMeasure>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/WaterQuality/UnitOfWaterSampleMeasure/ForSampleIdMatrix");
            });
        }
    }
}
