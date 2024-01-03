using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class FacilityOwnerTest
    {
        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var expected = "NJAW";
            var department = new FacilityOwner {Description = expected};

            Assert.AreEqual(expected, department.ToString());
        }
    }
}
