using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for CustomerImpactRangeTest.
    /// </summary>
    [TestClass]
    public class CustomerImpactRangeTest
    {
        #region Private Members

        private TestCustomerImpactRange _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void CustomerImpactRangeTestInitialize()
        {
            _target = new TestCustomerImpactRangeBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringReturnsDescription()
        {
            var expected = "descriptio";
            _target = new TestCustomerImpactRangeBuilder()
                .WithDescription(expected);

            Assert.AreEqual(expected, _target.ToString());
        }
    }

    internal class TestCustomerImpactRangeBuilder : TestDataBuilder<TestCustomerImpactRange>
    {
        #region Private Members

        private string _description;

        #endregion

        #region Exposed Methods

        public override TestCustomerImpactRange Build()
        {
            var obj = new TestCustomerImpactRange();
            if (_description != null)
                obj.Description = _description;
            return obj;
        }

        public TestCustomerImpactRangeBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        #endregion
    }

    internal class TestCustomerImpactRange : CustomerImpactRange
    {
    }
}
