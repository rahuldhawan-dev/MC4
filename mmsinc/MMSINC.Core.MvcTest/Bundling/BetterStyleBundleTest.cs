using System;
using System.Linq;
using System.Web.Optimization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Bundling;

namespace MMSINC.Core.MvcTest.Bundling
{
    [TestClass]
    public class BetterStyleBundleTest
    {
        [TestMethod]
        public void TestConstructorCssMinifierToTransforms()
        {
            var target = new BetterStyleBundle("~/blah");
            var tforms = target.Transforms.ToArray();
            Assert.IsInstanceOfType(tforms[0], typeof(CssMinify));
        }
    }
}
