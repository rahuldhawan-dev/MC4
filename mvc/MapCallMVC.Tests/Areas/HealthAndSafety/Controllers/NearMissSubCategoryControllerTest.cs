using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class NearMissSubCategoryControllerTest : MapCallMvcControllerTestBase<NearMissSubCategoryController, NearMissSubCategory>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/HealthAndSafety/NearMissSubCategory/ByCategory/");
            });
        }

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
