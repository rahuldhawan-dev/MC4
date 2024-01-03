using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallIntranet.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallIntranet.Tests
{
    [TestClass]
    public class NearMissCategoryControllerTest : MapCallIntranetControllerTestBase<NearMissCategoryController, NearMissCategory>
    {
        [TestMethod]
        public override void TestControllerAuthorization() {}

        [TestMethod]
        public void TestGetByTypeId()
        {
            var envType = GetFactory<EnvironmentalNearMissTypeFactory>().Create();
            var safetyType = GetFactory<SafetyNearMissTypeFactory>().Create();
            var safetyNearMissCategory = GetFactory<ErgonomicsNearMissCategoryFactory>().Create();
            var envNearMissCategory = GetFactory<StormWaterNearMissCategoryFactory>().Create();
            Session.Flush();

            var result = _target.GetByTypeId(safetyType.Id);
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(safetyNearMissCategory.Id, data.Single().Id);
        }
    }
}
