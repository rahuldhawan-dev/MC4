using System;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class TapImageTest
    {
        [TestMethod]
        public void TestFullStreetNameReturnsStreetPrefixNameAndSuffix()
        {
            var target = new TapImage();
            target.StreetPrefix = "S";
            target.Street = "Some";
            target.StreetSuffix = "St";

            Assert.AreEqual("S Some St", target.FullStreetName);
        }

        [TestMethod]
        public void TestFullStreetNameTrimsEndingsIfPrefixOrSuffixAreMissing()
        {
            var target = new TapImage();
            target.Street = "Some";
            Assert.AreEqual("Some", target.FullStreetName);
        }
    }
}
