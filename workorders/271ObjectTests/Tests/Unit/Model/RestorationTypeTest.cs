using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for RestorationTypeTestTest
    /// </summary>
    [TestClass]
    public class RestorationTypeTest
    {
        #region Private Members

        private TestRestorationType _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RestorationTypeTestInitialize()
        {
            _target = new TestRestorationTypeBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringMethodReturnsDescriptionProperty()
        {
            var expected = "Test Description";
            _target = new TestRestorationTypeBuilder()
                .WithDescription(expected);

            Assert.AreEqual(expected, _target.ToString());
        }
    }

    internal class TestRestorationTypeBuilder : TestDataBuilder<TestRestorationType>
    {
        #region Private Members

        private string _description;

        #endregion

        #region Exposed Methods

        public override TestRestorationType Build()
        {
            var obj = new TestRestorationType();
            if (_description != null)
                obj.Description = _description;
            return obj;
        }

        public TestRestorationTypeBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        #endregion
    }

    internal class TestRestorationType : RestorationType
    {
    }
}