using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class MaterialTest
    {
        [TestMethod]
        public void TestToStringReturnsFormattedValueWithPartNumberAndDescription()
        {
            var target = new Material {
                PartNumber = "8675309",
                Description = "this is the description"
            };

            Assert.AreEqual(
                String.Format("{0} - {1}", target.PartNumber, target.Description),
                target.ToString());
        }

        [TestMethod]
        public void TestToStringReturnsOnlyPartNumberIfDescriptionUnset()
        {
            var target = new Material {
                PartNumber = "8675309",
            };

            Assert.AreEqual(target.PartNumber, target.ToString());
        }
    }
}
