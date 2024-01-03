using System;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.Linq;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for SpoilRemovalTest.
    /// </summary>
    [TestClass]
    public class SpoilRemovalTest
    {
        #region Private Members

        private MockRepository<SpoilRemoval> _repository;
        private SpoilRemoval _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SpoilRemovalTestInitialize()
        {
            _repository = new MockRepository<SpoilRemoval>();

            _target = new TestSpoilRemovalBuilder()
                .WithDateRemoved(DateTime.Now)
                .WithQuantity(1)
                .WithRemovedFrom(new SpoilStorageLocation())
                .WithFinalDestination(new SpoilFinalProcessingLocation());
        }

        #endregion

        [TestMethod]
        public void TestCreateNewSpoilRemoval()
        {
            MyAssert.DoesNotThrow(() => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutDateRemoved()
        {
            _target.DateRemoved = default(DateTime);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithQuantityOfZeroOrLess()
        {
            _target.Quantity = 0;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));

            _target.Quantity = -1;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutSpecifyingStorageLocationRemovedFrom()
        {
            _target.RemovedFrom = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutFinalDestination()
        {
            _target.FinalDestination = null;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }
    }

    internal class TestSpoilRemovalBuilder : TestDataBuilder<SpoilRemoval>
    {
        private DateTime? _dateRemoved;
        private decimal? _quantity;
        private SpoilStorageLocation _removedFrom;
        private SpoilFinalProcessingLocation _finalDestination;

        #region Private Members
        
        #endregion

        #region Exposed Methods

        public override SpoilRemoval Build()
        {
            var obj = new SpoilRemoval();
            if (_dateRemoved != null)
                obj.DateRemoved = _dateRemoved.Value;
            if (_quantity != null)
                obj.Quantity = _quantity.Value;
            if (_removedFrom != null)
                obj.RemovedFrom = _removedFrom;
            if (_finalDestination != null)
                obj.FinalDestination = _finalDestination;
            return obj;
        }

        public TestSpoilRemovalBuilder WithDateRemoved(DateTime time)
        {
            _dateRemoved = time;
            return this;
        }

        public TestSpoilRemovalBuilder WithQuantity(decimal quantity)
        {
            _quantity = quantity;
            return this;
        }

        public TestSpoilRemovalBuilder WithRemovedFrom(SpoilStorageLocation location)
        {
            _removedFrom = location;
            return this;
        }

        public TestSpoilRemovalBuilder WithFinalDestination(SpoilFinalProcessingLocation location)
        {
            _finalDestination = location;
            return this;
        }

        #endregion
    }
}
