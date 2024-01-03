using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;

namespace MMSINC.CoreTest.Utilities
{
    [TestClass]
    public class TypeCacheTest
    {
        #region Init/Cleanup

        [TestCleanup]
        public void CleanupTest()
        {
            TypeCache.Clear();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRegisterTypeAlwaysReturnsTheSameGuidForTheSameType()
        {
            var expectedTypeOne = typeof(TypeOne);
            // First test that registering the same type multiple times always returns the same result.
            var result1 = TypeCache.RegisterType(expectedTypeOne);
            var result2 = TypeCache.RegisterType(expectedTypeOne);

            Assert.AreEqual(result1, result2);

            // Second test that registering a different type definitely returns a different result than the other type.
            var expectedTypeTwo = typeof(TypeTwo);
            var result3 = TypeCache.RegisterType(expectedTypeTwo);
            Assert.AreNotEqual(result1, result3, "The guids for different types must be different.");
        }

        [TestMethod]
        public void TestRegisterTypeReturnsExpectedGuidThatCanBeUsedwithTryRetrieveType()
        {
            var expectedTypeOne = typeof(TypeOne);
            var guid = TypeCache.RegisterType(expectedTypeOne);
            var result = TypeCache.TryRetrieveType(guid, out var resultType);
            Assert.IsTrue(result);
            Assert.AreSame(expectedTypeOne, resultType);
        }

        [TestMethod]
        public void TestTryRetrieveTypeReturnsFalseIfTheKeyDoesNotExistInTheCache()
        {
            var result = TypeCache.TryRetrieveType(System.Guid.Empty, out var resultType);
            Assert.IsFalse(result);
            Assert.IsNull(resultType);
        }

        #endregion

        #region Test types

        private class TypeOne { }

        private class TypeTwo { }

        #endregion
    }
}
