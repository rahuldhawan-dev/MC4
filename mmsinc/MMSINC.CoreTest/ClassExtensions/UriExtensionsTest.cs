using System;
using MMSINC.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.ClassExtensions
{
    /// <summary>
    /// Summary description for UriExtensionsTest
    /// </summary>
    [TestClass]
    public class UriExtensionsTest
    {
        [TestMethod]
        public void TestGetAbsolutePathWithoutQueryStringStaticMethod()
        {
            var test = new Uri("http://www.donniesdiscountgas.com/sales?today=no&yesterday=maybe");
            var expected = "http://www.donniesdiscountgas.com/sales";

            var result = test.GetAbsoluteUriWithoutQuery();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestGetAbsolutePathWithoutQueryThrowsForNulls()
        {
            Uri nullUri = null;
            MyAssert.Throws(() => nullUri.GetAbsoluteUriWithoutQuery());
        }
    }
}
