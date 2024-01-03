using MMSINC.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.Data
{
    [TestClass]
    public class EntityLookupTest
    {
        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var expected = "That's one way to describe it.";
            var target = new EntityLookup {Description = expected};

            Assert.AreEqual(expected, target.ToString());
        }
    }
}
