using System;
using System.Linq;
using MMSINC.ClassExtensions.ReflectionExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.ClassExtensions
{
    [TestClass]
    public class ReflectionExtensionsTest
    {
        #region Fields

        private static readonly Type _someClassType = typeof(SomeClass);

        #endregion

        [TestMethod]
        public void TestGetCustomAttributesReturnsEmptyEnumerableIfNoneFound()
        {
            var result = _someClassType.GetCustomAttributes(typeof(AttributeToNotFindAttribute), true);
            Assert.IsNotNull(result, "GetCustomAttributes is not supposed to return null.");
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestGetCustomAttributesReturnsAttributesFound()
        {
            var result = _someClassType.GetCustomAttributes(typeof(AttributeToFindAttribute), true);
            Assert.IsNotNull(result, "GetCustomAttributes is not supposed to return null.");
            Assert.AreEqual(1, result.Count());
        }

        #region Test classes

        private class AttributeToFindAttribute : Attribute { }

        private class AttributeToNotFindAttribute : Attribute { }

        [AttributeToFind]
        private class SomeClass { }

        #endregion
    }
}
