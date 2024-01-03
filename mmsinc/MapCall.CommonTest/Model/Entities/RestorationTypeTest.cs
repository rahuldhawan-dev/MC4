using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class RestorationTypeTest
    {
        [TestMethod]
        public void TestMeasurementTypeReturnsSquareFtIfDescriptionDoesNotContainTheWordCurb()
        {
            var target = new RestorationType {
                Description = "it's me maaaarkiplier"
            };

            Assert.AreEqual(RestorationMeasurementTypes.SquareFt, target.MeasurementType);
        }

        [TestMethod]
        public void TestMeasurementTypeReturnsLinearFtIfDescriptionDoesContainTheWordCurb()
        {
            var target = new RestorationType {
                Description = "CURB!"
            };

            Assert.AreEqual(RestorationMeasurementTypes.LinearFt, target.MeasurementType);

            target.Description = "curb :O";
            Assert.AreEqual(RestorationMeasurementTypes.LinearFt, target.MeasurementType,
                "This should work regardless of case.");
        }

        [TestMethod]
        public void TestGetCostMultiplierForOperatingCenterReturnsExpectedValues()
        {
            var target = new RestorationType();
            MyAssert.Throws(() => target.GetCostMultiplierForOperatingCenter(null),
                "Sanity: Exception should be thrown if operating center is null.");

            var opc = new OperatingCenter();
            Assert.AreEqual(0m, target.GetCostMultiplierForOperatingCenter(opc),
                "Sanity: Should be zero if there aren't any restoration type costs.");

            var rtc = new RestorationTypeCost {Cost = 2d};
            target.RestorationTypeCosts.Add(rtc);

            Assert.AreEqual(0m, target.GetCostMultiplierForOperatingCenter(opc),
                "Should be zero if there aren't any restoration type costs with a matching op center.");

            rtc.OperatingCenter = opc;
            Assert.AreEqual(2m, target.GetCostMultiplierForOperatingCenter(opc),
                "Should return decimal version of double Cost.");

            rtc.Cost = 2.5d;
            Assert.AreEqual(2.5m, target.GetCostMultiplierForOperatingCenter(opc),
                "Conversion from double to decimal should be equal.");
        }
    }
}
