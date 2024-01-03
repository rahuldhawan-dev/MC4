using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class NearMissCategoryControllerTest : MapCallMvcControllerTestBase<NearMissCategoryController, NearMissCategory>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/HealthAndSafety/NearMissCategory/ByType/");
            });
        }

        [TestMethod]
        public void TestByTypeReturnsOnlyResultsWithAMatchingType()
        {
            var envType = GetFactory<EnvironmentalNearMissTypeFactory>().Create();
            var safetyType = GetFactory<SafetyNearMissTypeFactory>().Create();
            var safetyNearMissCategory = GetFactory<ErgonomicsNearMissCategoryFactory>().Create();
            var envNearMissCategory = GetFactory<StormWaterNearMissCategoryFactory>().Create();
            Session.Flush();
            var result = _target.ByType(safetyType.Id);
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(safetyNearMissCategory.Id, data.Single().Id);
        }
    }
}
