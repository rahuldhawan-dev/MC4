using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class StreetTest
    {
        [TestMethod]
        public void TestToStringReturnsFullStName()
        {
            var expected = "Full St Name";
            var street = new Street {FullStName = expected};

            Assert.AreEqual(expected, street.ToString());
        }

        [TestMethod]
        public void TestDescriptionReturnsFullStName()
        {
            var expected = "Full St Name";
            var street = new Street {FullStName = expected};

            Assert.AreEqual(expected, street.Description);
        }
    }
}
