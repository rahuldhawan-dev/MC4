using MMSINC.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Controls
{
    /// <summary>
    /// Summary description for ButtonTest
    /// </summary>
    [TestClass]
    public class MvpButtonTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var target = new MvpButton();
            Assert.IsNotNull(target);
        }
    }
}
