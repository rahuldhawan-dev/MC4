using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MMSINC.CoreTest.ClassExtensions
{
    [TestClass]
    public class ByteArrayExtensionsTest
    {
        [TestMethod]
        public void TestByteArrayEqualsThrowsExceptionIfThisIsNull()
        {
            byte[] bytes = null;
            MyAssert.Throws(() => bytes.ByteArrayEquals(new[] {new Byte()}));
        }

        [TestMethod]
        public void TestByteArrayEqualsReturnsTrueIfArraysAreNormallyEqual()
        {
            var bytes = new byte[] {1, 2, 3};
            Assert.IsTrue(bytes.ByteArrayEquals(bytes));
        }

        [TestMethod]
        public void TestByteArrayEqualsReturnsFalseIfArrayBeingComparedIsNull()
        {
            var bytes = new byte[] {1, 2, 3};
            Assert.IsFalse(bytes.ByteArrayEquals(null));
        }

        [TestMethod]
        public void TestByteArrayEqualsReturnsFalseIfArraysHaveDifferentLengths()
        {
            var bytes = new byte[] {1, 2, 3};
            Assert.IsFalse(bytes.ByteArrayEquals(new byte[] {1}));
        }

        [TestMethod]
        public void TestByteArrayEqualsReturnsFalseIfArrayHasSameValuesButDifferentOrders()
        {
            var bytes = new byte[] {1, 2, 3};
            Assert.IsFalse(bytes.ByteArrayEquals(new byte[] {3, 2, 1}));
        }

        [TestMethod]
        public void TestByteArrayEqualsReturnsTrueIfArraysHaveSameValuesInSameOrder()
        {
            var bytes = new byte[] {1, 2, 3};
            Assert.IsTrue(bytes.ByteArrayEquals(new byte[] {1, 2, 3}));
        }
    }
}
