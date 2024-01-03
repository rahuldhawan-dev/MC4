using MMSINC.Data.Linq;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for DetectedLeakTestTest
    /// </summary>
    [TestClass]
    public class DetectedLeakTest
    {
        #region Private Members

        private IRepository<DetectedLeak> _repository;
        private TestDetectedLeak _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void DetectedLeakTestInitialize()
        {
            _repository = new MockDetectedLeakRepository();
            _target = new TestDetectedLeakBuilder();
        }

        #endregion

        [TestMethod]
        public void TestCreateNewDetectedLeak()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }
    }

    internal class TestDetectedLeakBuilder : TestDataBuilder<TestDetectedLeak>
    {
        #region Exposed Methods

        public override TestDetectedLeak Build()
        {
            var obj = new TestDetectedLeak();
            return obj;
        }

        #endregion
    }

    internal class TestDetectedLeak : DetectedLeak
    {
    }

    internal class MockDetectedLeakRepository : MockRepository<DetectedLeak>
    {
    }
}
