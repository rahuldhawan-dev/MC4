using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ValveImageTest
    {
        [TestMethod]
        public void TestFullStreetNameReturnsFullStreetName()
        {
            var target = new ValveImage {
                StreetNumber = "42",
                StreetPrefix = "N",
                Street = "Wow",
                StreetSuffix = "St"
            };

            Assert.AreEqual("42 N Wow St", target.FullStreetName);
        }
    }
}
