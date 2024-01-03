using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for SpoilTest.
    /// </summary>
    [TestClass]
    public class SpoilTest
    {
        #region Private Members

        private MockRepository<Spoil> _repository;
        private Spoil _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SpoilTestInitialize()
        {
            _repository = new MockRepository<Spoil>();

            _target = new TestSpoilBuilder()
                .WithWorkOrder(new WorkOrder())
                .WithSpoilStorageLocation(new SpoilStorageLocation())
                .WithQuantity(1);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewSpoil()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            _target.WorkOrder = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutStorageLocation()
        {
            _target.SpoilStorageLocation = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithQuantityOfZero()
        {
            _target.Quantity = 0;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithQuantityLessThanZero()
        {
            _target.Quantity = -1;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }
    }

    internal class TestSpoilBuilder : TestDataBuilder<Spoil>
    {
        #region Private Members

        private WorkOrder _order;
        private SpoilStorageLocation _location;
        private decimal? _quantity;

        #endregion

        #region Exposed Methods

        public override Spoil Build()
        {
            var obj = new Spoil();
            if (_order != null)
                obj.WorkOrder = _order;
            if (_location != null)
                obj.SpoilStorageLocation = _location;
            if (_quantity != null)
                obj.Quantity = _quantity.Value;
            return obj;
        }

        public TestSpoilBuilder WithWorkOrder(WorkOrder order)
        {
            _order = order;
            return this;
        }

        public TestSpoilBuilder WithSpoilStorageLocation(SpoilStorageLocation location)
        {
            _location = location;
            return this;
        }

        public TestSpoilBuilder WithQuantity(decimal quantity)
        {
            _quantity = quantity;
            return this;
        }

        #endregion
    }
}
