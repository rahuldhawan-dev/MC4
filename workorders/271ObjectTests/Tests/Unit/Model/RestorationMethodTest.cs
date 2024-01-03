using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for RestorationMethodTestTest
    /// </summary>
    [TestClass]
    public class RestorationMethodTest
    {
        #region Private Members

        private TestRestorationMethod _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RestorationMethodTestInitialize()
        {
            _target = new TestRestorationMethodBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringMethodReturnsDescriptionValue()
        {
            var expected = "Test Description";
            _target = new TestRestorationMethodBuilder()
                .WithDescription(expected);

            Assert.AreEqual(expected, _target.ToString());
        }
    }

    internal class TestRestorationMethodBuilder : TestDataBuilder<TestRestorationMethod>
    {
        #region Private Members

        private string _description;

        #endregion

        #region Exposed Methods

        public override TestRestorationMethod Build()
        {
            var obj = new TestRestorationMethod();
            if (_description != null)
                obj.Description = _description;
            return obj;
        }

        public TestRestorationMethodBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        #endregion
    }

    internal class TestRestorationMethod : RestorationMethod
    {
    }
}