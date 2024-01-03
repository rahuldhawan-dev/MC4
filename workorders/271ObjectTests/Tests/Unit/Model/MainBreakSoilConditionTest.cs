using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for MainBreakSoilConditionTest.
    /// </summary>
    [TestClass]
    public class MainBreakSoilConditionTest
    {
        #region Private Members

        private TestMainBreakSoilCondition _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void MainBreakSoilConditionTestInitialize()
        {
            _target = new TestMainBreakSoilConditionBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringMethodReflectsNameProperty()
        {
            const string expected = "foo";
            var target = new MainBreakSoilCondition()
            {
                Description = expected
            };

            Assert.AreEqual(expected, target.ToString(),
                "MainBreakSoilCondition#ToString() should reflect the Name property.");
        }
    }

    internal class TestMainBreakSoilConditionBuilder : TestDataBuilder<TestMainBreakSoilCondition>
    {
        #region Exposed Methods

        public override TestMainBreakSoilCondition Build()
        {
            var obj = new TestMainBreakSoilCondition();
            return obj;
        }

        #endregion
    }

    internal class TestMainBreakSoilCondition : MainBreakSoilCondition
    {
    }
}
