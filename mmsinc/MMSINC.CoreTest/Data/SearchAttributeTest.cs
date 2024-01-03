using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace MMSINC.CoreTest.Data
{
    [TestClass]
    public class SearchAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestConstructorSetsCanMapToTrueByDefault()
        {
            var target = new SearchAttribute();
            Assert.IsTrue(target.CanMap);
        }

        #endregion
    }
}
