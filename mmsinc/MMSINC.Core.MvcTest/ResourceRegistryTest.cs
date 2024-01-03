using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest
{
    [TestClass]
    public class ResourceRegistryTest
    {
        #region Fields

        private ResourceRegistry _target;

        #endregion

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new ResourceRegistry();
        }

        [TestMethod]
        public void TestScriptsPropertyIsNotNull()
        {
            Assert.IsNotNull(_target.Scripts);
        }

        [TestMethod]
        public void TestScriptsPropertyAlwaysReturnsSameInstance()
        {
            Assert.AreSame(_target.Scripts, _target.Scripts);
        }

        [TestMethod]
        public void TestStyleSheetsPropertyIsNotNull()
        {
            Assert.IsNotNull(_target.StyleSheets);
        }

        [TestMethod]
        public void TestStyleSheetsPropertyAlwaysReturnsSameInstance()
        {
            Assert.AreSame(_target.StyleSheets, _target.StyleSheets);
        }

        [TestMethod]
        public void TestStyleSheetsAndScriptsAreDifferentInstances()
        {
            Assert.AreNotSame(_target.StyleSheets, _target.Scripts);
        }
    }
}
