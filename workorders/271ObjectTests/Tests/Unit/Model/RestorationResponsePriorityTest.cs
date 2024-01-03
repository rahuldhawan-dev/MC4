using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for RestorationResponsePriorityTest.
    /// </summary>
    [TestClass]
    public class RestorationResponsePriorityTest
    {
        #region Private Members

        private TestRestorationResponsePriority _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void RestorationResponsePriorityTestInitialize()
        {
            _target = new TestRestorationResponsePriorityBuilder();
        }

        #endregion

        [TestMethod]
        public void TestToStringMethodReturnsDescription()
        {
            _target.Description = "This is the description";

            Assert.AreEqual(_target.Description, _target.ToString());
        }
    }

    internal class TestRestorationResponsePriorityBuilder : TestDataBuilder<TestRestorationResponsePriority>
    {
        #region Exposed Methods

        public override TestRestorationResponsePriority Build()
        {
            var obj = new TestRestorationResponsePriority();
            return obj;
        }

        #endregion
    }

    internal class TestRestorationResponsePriority : RestorationResponsePriority
    {
    }
}
