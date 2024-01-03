using MMSINC.ClassExtensions.EnumExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.ClassExtensions
{
    [TestClass]
    public class EnumExtensionsTest
    {
        [TestMethod]
        public void TestGetDescription()
        {
            Assert.AreEqual("foo", TestEnum.Foo.DescriptionAttr());
        }

        public enum TestEnum
        {
            [System.ComponentModel.Description("foo")]
            Foo,
            Bar,
            Baz
        }
    }
}
