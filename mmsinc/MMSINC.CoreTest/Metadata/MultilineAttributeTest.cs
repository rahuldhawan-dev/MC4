using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;

namespace MMSINC.CoreTest.Metadata
{
    [TestClass]
    public class MultilineAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestConstructorSetsDataTypeToMultilineText()
        {
            var target = new MultilineAttribute();
            Assert.AreEqual(DataType.MultilineText, target.DataType);
        }

        #endregion
    }
}
