using System.Linq;
using System.Web.Optimization;
using MMSINC.Bundling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.Bundling
{
    [TestClass]
    public class DotLessStyleBundleTest
    {
        [TestMethod]
        public void TestConstructorInsertsDotLessTransformerAsFirstTransformerThenCssMinifier()
        {
            var target = new DotLessStyleBundle("~/blah");
            var tforms = target.Transforms.ToArray();
            Assert.IsInstanceOfType(tforms[0], typeof(DotLessTransform),
                "DotLessTransform must come before CssMinifier.");
            Assert.IsInstanceOfType(tforms[1], typeof(CssMinify));
        }
    }
}
