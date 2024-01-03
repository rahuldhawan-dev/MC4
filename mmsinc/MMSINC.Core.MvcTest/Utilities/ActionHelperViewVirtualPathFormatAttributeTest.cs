using System;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Utilities
{
    [TestClass]
    public class ActionHelperViewVirtualPathFormatAttributeTest
    {
        #region Tests

        [TestMethod]
        public void TestConstructorThrowsForNullEmptyWhiteSpace()
        {
            MyAssert.Throws<ArgumentNullException>(() => new ActionHelperViewVirtualPathFormatAttribute(null));
            MyAssert.Throws<ArgumentNullException>(() => new ActionHelperViewVirtualPathFormatAttribute(string.Empty));
            MyAssert.Throws<ArgumentNullException>(() => new ActionHelperViewVirtualPathFormatAttribute("   "));
        }

        [TestMethod]
        public void TestConstructorSetsVirtualPathFormatProperty()
        {
            var expected = "wow";
            var target = new ActionHelperViewVirtualPathFormatAttribute(expected);
            Assert.AreEqual(expected, target.VirtualPathFormat);
        }

        #endregion
    }
}
