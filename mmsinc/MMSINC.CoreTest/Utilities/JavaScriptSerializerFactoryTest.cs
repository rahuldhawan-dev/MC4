using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class JavaScriptSerializerFactoryTest
    {
        [TestMethod]
        public void TestBuildSetsMaxJsonLengthFromConfigFile()
        {
            var expected = 123456789;

            var result = JavaScriptSerializerFactory.Build();

            Assert.AreEqual(expected, result.MaxJsonLength);
        }
    }
}
