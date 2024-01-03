using MMSINC.ClassExtensions.TypeExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.ClassExtensions
{
    [TestClass]
    public class TypeExtensionsTest
    {
        #region HasParameterlessConstructor

        [TestMethod]
        public void TestHasParameterlessConstructorReturnsTrueIfClassHasParameterlessConstructor()
        {
            Assert.IsTrue(
                typeof(ClassWithParameterlessConstructor).HasParameterlessConstructor());
        }

        [TestMethod]
        public void TestHasParameterlessConstructorReturnsFalseIfClassDoesNotHaveParameterlessConstructor()
        {
            Assert.IsFalse(
                typeof(ClassWithoutParametlessConstructor).HasParameterlessConstructor());
        }

        [TestMethod]
        public void TestHasParameterlessConstructorReturnsFalseIfClassOnlyHasOptionalParameter()
        {
            Assert.IsFalse(
                typeof(ClassWithOptionalParameterConstructor).HasParameterlessConstructor());
        }

        #endregion

        #region Test Classes

        private class ClassWithParameterlessConstructor
        {
            public ClassWithParameterlessConstructor() { }
        }

        private class ClassWithoutParametlessConstructor
        {
            public ClassWithoutParametlessConstructor(object obj) { }
        }

        private class ClassWithOptionalParameterConstructor
        {
            public ClassWithOptionalParameterConstructor(object obj = null) { }
        }

        #endregion
    }
}
