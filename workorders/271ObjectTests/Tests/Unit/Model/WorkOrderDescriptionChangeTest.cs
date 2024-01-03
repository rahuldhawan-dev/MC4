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
    /// Summary description for WorkOrderDescriptionChangeTest.
    /// </summary>
    [TestClass]
    public class WorkOrderDescriptionChangeTest
    {
        #region Private Members

        private IRepository<WorkOrderDescriptionChange> _repository;
        private TestWorkOrderDescriptionChange _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderDescriptionChangeTestInitialize()
        {
            _repository = new MockWorkOrderDescriptionChangeRepository();
            _target = new TestWorkOrderDescriptionChangeBuilder();
        }

        #endregion

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            var description = new WorkDescription();
            _target = new TestWorkOrderDescriptionChangeBuilder()
                .WithToWorkDescription(description);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
            MyAssert.Throws<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target));
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkDescription()
        {
            var order = new WorkOrder();
            _target = new TestWorkOrderDescriptionChangeBuilder()
                .WithWorkOrder(order);

            MyAssert.Throws<DomainLogicException>(
                () => _repository.InsertNewEntity(_target));
            MyAssert.Throws<DomainLogicException>(
                () => _repository.UpdateCurrentEntity(_target));
        }
    }

    internal class TestWorkOrderDescriptionChangeBuilder : TestDataBuilder<TestWorkOrderDescriptionChange>
    {
        #region Private Members

        private WorkOrder _order;
        private WorkDescription _toDescription, _fromDescription;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderDescriptionChange Build()
        {
            var obj = new TestWorkOrderDescriptionChange();
            if (_order != null)
                obj.WorkOrder = _order;
            if (_toDescription!= null)
                obj.ToWorkDescription = _toDescription;
            if (_fromDescription != null)
                obj.FromWorkDescription = _fromDescription;
            return obj;
        }

        public TestWorkOrderDescriptionChangeBuilder WithWorkOrder(WorkOrder order)
        {
            _order = order;
            return this;
        }

        public TestWorkOrderDescriptionChangeBuilder WithToWorkDescription(WorkDescription description)
        {
            _toDescription = description;
            return this;
        }

        public TestWorkOrderDescriptionChangeBuilder WithFromWorkDescription(WorkDescription description)
        {
            _fromDescription = description;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderDescriptionChange : WorkOrderDescriptionChange
    {
    }

    internal class MockWorkOrderDescriptionChangeRepository : MockRepository<WorkOrderDescriptionChange>
    {
    }
}
