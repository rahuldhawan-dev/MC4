using System.Collections.Generic;
using System.Linq;
using System.Net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallIntranet.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallIntranet.Tests
{
    [TestClass]
    public class NearMissSubCategoryControllerTest : MapCallIntranetControllerTestBase<NearMissSubCategoryController, NearMissSubCategory>
    {
        [TestMethod]
        public override void TestControllerAuthorization() {}

        [TestMethod]
        public void TestByCategoriesReturnsOnlyResultsWithAMatchingCategory()
        {
            var safetyNearMissCategory = GetFactory<ErgonomicsNearMissCategoryFactory>().Create();
            var envNearMissCategory = GetFactory<StormWaterNearMissCategoryFactory>().Create();

            var good = GetEntityFactory<NearMissSubCategory>().Create(new { Category = safetyNearMissCategory });
            var bad = GetEntityFactory<NearMissSubCategory>().Create(new { Category = envNearMissCategory });

            var result = _target.ByCategory(safetyNearMissCategory.Id);
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(good.Id, data.Single().Id);
        }
    }
}
