using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class WellTestTest : MapCallMvcInMemoryDatabaseTestBase<WellTest>
    {
        #region Tests

        [TestMethod]
        public void TestLogicalPropertiesCalculateAccurately()
        {
            var wellTestEntity = new WellTest {
                PumpingWaterLevel = 423.30m,
                StaticWaterLevel = 79.20m,
                PumpingRate = 945
            };

            // Pumping Water Level - Static Water Level
            const decimal expectedDrawDown = 344.1m;

            // Pumping Rate / DrawDown
            const decimal expectedSpecificCapacity = 2.7462946817785527462946817786m;

            Assert.AreEqual(wellTestEntity.DrawDown, expectedDrawDown);
            Assert.AreEqual(wellTestEntity.SpecificCapacity, expectedSpecificCapacity);
        }
        
        #endregion
    }
}
