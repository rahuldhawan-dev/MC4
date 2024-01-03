using System.Linq;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class SearchFacilityReadingsTest
    {
        [TestMethod]
        public void TestTotalReturnsSumTotalOfAllReadingCostTotals()
        {
            var target = new SearchFacilityReadings();
            var reading1 = new FacilityReadingCost {
                KwhCost = 1.00m,
                ReadingValue = 10
            };
            var reading2 = new FacilityReadingCost {
                KwhCost = 2.00m,
                ReadingValue = 13,
            };
            target.ReadingCosts = new[] {reading1, reading2};

            Assert.AreEqual(36m, target.Total);
        }

        [TestMethod]
        public void TestTotalReturnsNullIfReadingCostsIsNull()
        {
            var target = new SearchFacilityReadings();
            target.ReadingCosts = null;
            Assert.IsNull(target.Total);
        }

        [TestMethod]
        public void TestTotalReturnsZeroIfReadingCostsIsEmpty()
        {
            var target = new SearchFacilityReadings();
            target.ReadingCosts = Enumerable.Empty<FacilityReadingCost>();
            Assert.AreEqual(0m, target.Total);
        }
        

    }
}
