using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class SewerAuthorityCodeTest
    {
        [TestMethod]
        public void TestToStringReturnsCodeAndDescription()
        {
            var target = new SewerAuthorityCode {SAPCode = "AB", Description = "Abcdefg"};

            Assert.AreEqual("AB Abcdefg", target.ToString());
        }
    }
}
