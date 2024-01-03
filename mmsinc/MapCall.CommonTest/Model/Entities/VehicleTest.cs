using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class VehicleTest
    {
        #region Tests

        [TestMethod]
        public void TestDescriptionReturnsABunchOfInformation()
        {
            var v = new Vehicle {
                ARIVehicleNumber = "blah",
                VehicleIdentificationNumber = "ABCDEFGHJIKLMNOP",
                PlateNumber = "ABC-123",
                ModelYear = "1991",
                Make = "Powell",
                Model = "The Homer"
            };

            var expected = "blah : ABCDEFGHJIKLMNOP - 1991 Powell The Homer - ABC-123";
            Assert.AreEqual(expected, v.Description);
        }

        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var v = new Vehicle();
            Assert.AreEqual(v.Description, v.ToString());
        }

        #endregion
    }
}
