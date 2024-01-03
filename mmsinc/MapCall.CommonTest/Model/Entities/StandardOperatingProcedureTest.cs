using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class StandardOperatingProcedureTest
    {
        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var target = new StandardOperatingProcedure();
            target.Description = "Neat";
            Assert.AreEqual("Neat", target.ToString());
        }
    }
}
