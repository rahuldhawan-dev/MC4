using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for RepairTimeRangeTest.
    /// </summary>
    [TestClass]
    public class RepairTimeRangeTest
    {
        #region Private Members

        private TestRepairTimeRange _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RepairTimeRangeTestInitialize()
        {
            _target = new TestRepairTimeRangeBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var expected = "description";
            _target = new TestRepairTimeRangeBuilder()
                .WithDescription(expected);

            Assert.AreEqual(expected, _target.ToString());
        }
    }

    internal class TestRepairTimeRangeBuilder : TestDataBuilder<TestRepairTimeRange>
    {
        #region Private Methods

        private string _description;

        #endregion

        #region Exposed Methods

        public override TestRepairTimeRange Build()
        {
            var obj = new TestRepairTimeRange();
            if (_description != null)
                obj.Description = _description;
            return obj;
        }

        public TestRepairTimeRangeBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        #endregion
    }

    internal class TestRepairTimeRange : RepairTimeRange
    {
    }
}
