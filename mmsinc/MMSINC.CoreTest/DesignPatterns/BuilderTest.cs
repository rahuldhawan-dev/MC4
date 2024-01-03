using MMSINC.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.CoreTest.DesignPatterns
{
    /// <summary>
    /// Summary description for BuilderTest
    /// </summary>
    [TestClass]
    public class BuilderTest
    {
        [TestMethod]
        public void TestBuildMethodReturnsInstanceOfType()
        {
            var actual = new TestClassBuilder().Build();

            Assert.IsNotNull(actual);
            MyAssert.IsInstanceOfType<TestClass>(actual);
        }

        [TestMethod]
        public void TestImplicitOperator()
        {
            TestClass actual = new TestClassBuilder();

            Assert.IsNotNull(actual);
            MyAssert.IsInstanceOfType<TestClass>(actual);
        }
    }

    internal class TestClass { }

    internal class TestClassBuilder : Builder<TestClass>
    {
        #region Exposed Methods

        public override TestClass Build()
        {
            return new TestClass();
        }

        #endregion
    }
}
