using System;
using System.IO;
using System.Web;
using MMSINC.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.MvcTest.ClassExtensions
{
    [TestClass]
    public class IHtmlStringExtensionsTest
    {
        #region Tests

        [TestMethod]
        public void TestToHelperResultReturnsAResultThatRendersTheExactSameValueToTheHelperResultWriter()
        {
            var expected = "i am so expected & stuff";
            var html = new HtmlString(expected);
            using (var sw = new StringWriter())
            {
                html.ToHelperResult()(null).WriteTo(sw);
                Assert.AreEqual(expected, sw.ToString());
            }
        }

        [TestMethod]
        public void TestToHelperResultThrowsArgumentNullExceptionIfTargetIsNull()
        {
            MyAssert.Throws<ArgumentNullException>(() => IHtmlStringExtensions.ToHelperResult(null));
        }

        #endregion
    }
}
