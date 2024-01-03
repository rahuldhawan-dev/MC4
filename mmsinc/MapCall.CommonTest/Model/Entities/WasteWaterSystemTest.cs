using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class WasteWaterSystemTest
    {
        [TestMethod]
        public void TestWasteWaterSystemIdAndDescriptionReturnFormattedValue()
        {
            var opc = new OperatingCenter {Id = 1, OperatingCenterCode = "NJ12"};
            var target = new WasteWaterSystem {
                Id = 12,
                OperatingCenter = opc,
                WasteWaterSystemName = "Mr Hankey"
            };

            string expected = "NJ12WW0012 - Mr Hankey";
            string wwsid = target.WasteWaterSystemId;
            //pattern here is OperatingCenter.OperatingCenterCode + "WW"+ Id + " - " + WasteWaterSystemName;
            //For Id it is Always 4 digits long, if Id is 12 it will have 2 leading 0s ie: 0012
            Assert.AreEqual(expected, target.WasteWaterSystemId);
            Assert.AreEqual(expected, target.Description);
        }

        [TestMethod]
        public void TestWasteWaterSystemIdAndDescriptionReturnFormattedValueWhenBusinessUnitIsNull()
        {
            var opc = new OperatingCenter {Id = 1, OperatingCenterCode = "NJ12"};
            var target = new WasteWaterSystem {
                Id = 140,
                OperatingCenter = opc,
                WasteWaterSystemName = "Mr Hankey"
            };

            string expected = "NJ12WW0140 - Mr Hankey";
            string wwsid = target.WasteWaterSystemId;
            //pattern here is OperatingCenter.OperatingCenterCode + "WW"+ Id;
            //For Id it is Always 4 digits long, if Id is 140 it will have 1 leading 0 ie: 0140
            Assert.AreEqual(expected, target.WasteWaterSystemId);
            Assert.AreEqual(expected, target.Description);
        }
    }
}
