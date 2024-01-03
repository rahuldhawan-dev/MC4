using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class OrderTypeTest
    {
        [TestMethod]
        public void TestToStringReturnsDisplay()
        {
            var target = new OrderType {SAPCode = "10", Description = "Foo"};

            Assert.AreEqual("10 - Foo", target.ToString());
        }
    }
}
