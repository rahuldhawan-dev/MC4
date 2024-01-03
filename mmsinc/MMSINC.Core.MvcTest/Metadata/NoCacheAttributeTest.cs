using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Metadata
{
    [TestClass]
    public class NoCacheAttributeTest
    {
        #region Fields

        private NoCacheAttribute _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new NoCacheAttribute();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestConstructorSetsDurationToZero()
        {
            Assert.AreEqual(0, _target.Duration);
        }

        [TestMethod]
        public void TestConstructorSetsLocationToNone()
        {
            Assert.AreEqual(System.Web.UI.OutputCacheLocation.None, _target.Location);
        }

        [TestMethod]
        public void TestConstructorSetsNoStoreToTrue()
        {
            Assert.IsTrue(_target.NoStore);
        }

        [TestMethod]
        public void TestConstructorSetsVaryByParamToWildcard()
        {
            Assert.AreEqual("*", _target.VaryByParam);
        }

        #endregion
    }
}
