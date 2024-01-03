using System;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for StockLocationTest.
    /// </summary>
    [TestClass]
    public class StockLocationTest
    {
        #region Private Members

        private IRepository<StockLocation> _repository;
        private TestStockLocation _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void StockLocationTestInitialize()
        {
            _repository = new MockStockLocationRepository();
            _target = new TestStockLocationBuilder();
        }

        #endregion

        [TestMethod]
        public void TestCreateNewStockLocation()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveStockLocationWithoutDescription()
        {
            _target = new TestStockLocationBuilder()
                .WithDescription(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));

            _target = new TestStockLocationBuilder()
                .WithDescription(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSetDescriptionToAStringTooLong()
        {
            MyAssert.Throws<DomainLogicException>(
                () => _target.Description = new String('x', 26));
        }

        [TestMethod]
        public void TestCannotSaveWithoutOperatingCenter()
        {
            _target = new TestStockLocationBuilder()
                .WithOperatingCenter(null);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestToStringMethodReturnsValueOfDescriptionProperty()
        {
            var expected = "Test Description";
            _target = new TestStockLocationBuilder()
                .WithDescription(expected);

            Assert.AreEqual(expected, _target.ToString());
        }
    }

    internal class TestStockLocationBuilder : TestDataBuilder<TestStockLocation>
    {
        #region Constants

        private const string TEST_DESCRIPTION = "Test Description";

        #endregion

        #region Private Members

        private string _description = TEST_DESCRIPTION;
        private OperatingCenter _operatingCenter = new OperatingCenter();

        #endregion

        #region Exposed Methods

        public override TestStockLocation Build()
        {
            var obj = new TestStockLocation();
            if (_description != null)
                obj.SetDescription(_description);
            if (_operatingCenter != null)
                obj.SetOperatingCenter(_operatingCenter);
            return obj;
        }

        public TestStockLocationBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public TestStockLocationBuilder WithOperatingCenter(OperatingCenter operatingCenter)
        {
            _operatingCenter = operatingCenter;
            return this;
        }

        #endregion
    }

    internal class TestStockLocation : StockLocation
    {
        #region Exposed Methods

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetOperatingCenter(OperatingCenter center)
        {
            OperatingCenter = center;
        }

        #endregion
    }

    internal class MockStockLocationRepository : MockRepository<StockLocation> { }
}
