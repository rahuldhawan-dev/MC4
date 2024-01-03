using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    [TestClass]
    public class MarkoutTypeTest
    {
        #region Private Members

        private TestMarkoutType _target;
        
        #endregion

        #region Additional Test Attributes
  
        [TestInitialize]
        public void MarkoutTypeTestInitialize()
        {
            _target = new TestMarkoutType();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestToStringMethodReturnsDescriptionProperty()
        {
            var expected = "Test Description";
            _target = new TestMarkoutTypeBuilder().WithDescription(expected);

            Assert.AreEqual(expected, _target.ToString());
        }

        #endregion
    }

    internal class TestMarkoutTypeBuilder : TestDataBuilder<TestMarkoutType>
    {
        #region Private Members

        private string _description;
        private int? _order;

        #endregion

        #region Exposed Methods

        public override TestMarkoutType Build()
        {
            var obj = new TestMarkoutType();
            if (_description != null)
                obj.Description = _description;
            if (_order != null)
                obj.Order = _order.Value;
            return obj;
        }

        public TestMarkoutTypeBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public TestMarkoutTypeBuilder WithOrder(int order)
        {
            _order = order;
            return this;
        }

        #endregion
    }

    internal class TestMarkoutType : MarkoutType
    {
    }
}
