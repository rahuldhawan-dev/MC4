using System.Web.Optimization;
using MMSINC.Bundling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Bundling
{
    [TestClass]
    public class BetterScriptBundleTest
    {
        [TestMethod]
        public void TestConstructorSetsConcatenationTokenTheExactSameWayScriptBundleWould()
        {
            var expected = new ScriptBundle("~/virtual").ConcatenationToken;
            var result = new BetterScriptBundle("~/virtual").ConcatenationToken;
            Assert.AreEqual(expected, result);
        }
    }
}
