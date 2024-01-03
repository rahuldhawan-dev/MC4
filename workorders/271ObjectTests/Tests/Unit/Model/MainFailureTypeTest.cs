using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for MainFailureTypeTest.
    /// </summary>
    [TestClass]
    public class MainFailureTypeTest
    {
        #region Private Members

        private TestMainFailureType _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void MainFailureTypeTestInitialize()
        {
            _target = new TestMainFailureTypeBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringMethodReflectsNameProperty()
        {
            const string expected = "foo";
            var target = new MainFailureType()
            {
                Description = expected
            };

            Assert.AreEqual(expected, target.ToString(),
                "MainFailureType#ToString() should reflect the Name property.");
        }
    }

    internal class TestMainFailureTypeBuilder : TestDataBuilder<TestMainFailureType>
    {
        #region Exposed Methods

        public override TestMainFailureType Build()
        {
            var obj = new TestMainFailureType();
            return obj;
        }

        #endregion
    }

    internal class TestMainFailureType : MainFailureType
    {
    }
}
