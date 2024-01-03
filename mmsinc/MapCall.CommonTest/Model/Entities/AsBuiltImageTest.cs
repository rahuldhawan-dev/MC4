using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class AsBuiltImageTest
    {
        [TestMethod]
        public void TestFullStreetReturnsFullStreet()
        {
            string pre = "N", street = "Main", suf = "St";
            var expected = string.Format(AsBuiltImage.STREET_FORMAT, pre, street, suf);

            var target = new AsBuiltImage {StreetPrefix = pre, Street = street, StreetSuffix = suf};

            Assert.AreEqual(expected, target.FullStreet);
        }

        [TestMethod]
        public void TestFullCrossStreetReturnsFullCrossStreet()
        {
            string pre = "N", street = "Main", suf = "St";
            var expected = string.Format(AsBuiltImage.STREET_FORMAT, pre, street, suf);

            var target = new AsBuiltImage {CrossStreetPrefix = pre, CrossStreet = street, CrossStreetSuffix = suf};

            Assert.AreEqual(expected, target.FullCrossStreet);
        }
    }
}
