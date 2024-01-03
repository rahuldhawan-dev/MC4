using System.Text;
using System.Web.Mvc;
using MMSINC.ClassExtensions.StringBuilderExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.ClassExtensions
{
    [TestClass]
    public class StringBuilderExtensionsTest
    {
        private StringBuilder _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new StringBuilder();
        }

        [TestMethod]
        public void TestToMvcHtmlStringReturnsContentsAsMvcHtmlString()
        {
            _target.Append("foo");
            _target.Append("bar");

            Assert
               .AreEqual(MvcHtmlString.Create(_target.ToString()).ToString(),
                    _target.ToMvcHtmlString().ToString());
        }
    }
}
