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
    /// Summary description for StreetOpeningPermitTest.
    /// </summary>
    [TestClass]
    public class StreetOpeningPermitTest
    {
        #region Private Members

        private MockRepository<StreetOpeningPermit> _repository;
        private StreetOpeningPermit _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void StreetOpeningPermitTestInitialize()
        {
            _repository = new MockRepository<StreetOpeningPermit>();
            _target = new TestStreetOpeningPermitBuilder()
                .WithWorkOrder(new WorkOrder())
                .WithDateRequested(DateTime.Now);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewStreetOpeningPermit()
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
        public void TestCannotSaveWithoutDateRequested()
        {
            _target.DateRequested = DateTime.MinValue;

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
        }
    }

    internal class TestStreetOpeningPermitBuilder : TestDataBuilder<StreetOpeningPermit>
    {
        #region Private Members

        private DateTime? _dateIssued, _dateRequested;
        private WorkOrder _workOrder;

        #endregion

        #region Exposed Methods

        public override StreetOpeningPermit Build()
        {
            var obj = new StreetOpeningPermit();
            if (_dateIssued != null)
                obj.DateIssued = _dateIssued.Value;
            if (_dateRequested != null)
                obj.DateRequested = _dateRequested.Value;
            if (_workOrder != null)
                obj.WorkOrder = _workOrder;
            return obj;
        }

        public TestStreetOpeningPermitBuilder WithDateIssued(DateTime? dateIssued)
        {
            _dateIssued = dateIssued;
            return this;
        }

        public TestStreetOpeningPermitBuilder WithWorkOrder(WorkOrder order)
        {
            _workOrder = order;
            return this;
        }

        public StreetOpeningPermit WithDateRequested(DateTime time)
        {
            _dateRequested = time;
            return this;
        }

        #endregion
    }
}
