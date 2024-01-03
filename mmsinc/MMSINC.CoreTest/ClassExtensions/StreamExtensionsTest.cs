using System;
using System.IO;
using MMSINC.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.ClassExtensions
{
    [TestClass]
    public class StreamExtensionsTest
    {
        [TestMethod]
        public void TestWriteExtensionWritesAllBytesToStreamAtZeroOffset()
        {
            var expected = new byte[] {1, 2, 3, 4};
            using (var ms = new MemoryStream())
            {
                ms.Write(expected);
                Assert.IsTrue(expected.ByteArrayEquals(ms.ToArray()));
            }
        }

        [TestMethod]
        public void TestWriteExtensionThrowsNullExceptionIfBytesArgumentIsNull()
        {
            using (var ms = new MemoryStream())
            {
                // ReSharper disable AccessToDisposedClosure
                MyAssert.Throws<NullReferenceException>(() => ms.Write(null));
                // ReSharper restore AccessToDisposedClosure
            }
        }
    }
}
