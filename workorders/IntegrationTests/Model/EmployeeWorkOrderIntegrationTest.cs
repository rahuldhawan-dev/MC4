using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for EmployeeWorkOrderIntegrationTest
    /// </summary>
    [TestClass]
    public class EmployeeWorkOrderIntegrationTest : WorkOrdersTestClass<EmployeeWorkOrder>
    {
        #region Exposed Static Methods

        internal static TestEmployeeWorkOrderBuilder GetValidEmployeeWorkOrder()
        {
            return new TestEmployeeWorkOrderBuilder();
        }

        public static void DeleteEmployeeWorkOrder(EmployeeWorkOrder entity)
        {
            var order = entity.WorkOrder;
            EmployeeWorkOrderRepository.Delete(entity);
            WorkOrderIntegrationTest.DeleteWorkOrder(order);
        }

        #endregion

        #region Private Methods

        protected override EmployeeWorkOrder GetValidObject()
        {
            return GetValidEmployeeWorkOrder();
        }

        protected override EmployeeWorkOrder GetValidObjectFromDatabase()
        {
            var ewo = GetValidObject();
            EmployeeWorkOrderRepository.Insert(ewo);
            return ewo;
        }

        protected override void DeleteObject(EmployeeWorkOrder entity)
        {
            DeleteEmployeeWorkOrder(entity);
        }

        #endregion

        [TestMethod]
        public void TestCannotChangeWorkOrderAfterSave()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObjectFromDatabase();

                var order = new WorkOrder();
                MyAssert.Throws(() => target.WorkOrder = order,
                    typeof(DomainLogicException),
                    "Attempting to change the WorkOrder should throw an exception for an EmployeeWorkOrder that has already been saved");

                DeleteObject(target);
            }
        }
    }

    internal class TestEmployeeWorkOrderBuilder : TestDataBuilder<EmployeeWorkOrder>
    {
        #region Private Members

        private WorkOrder _workOrder = WorkOrderIntegrationTest.GetValidWorkOrder();

        #endregion

        #region Exposed Methods

        public override EmployeeWorkOrder Build()
        {
            var order = new EmployeeWorkOrder();
            if (_workOrder != null)
                order.WorkOrder = _workOrder;
            return order;
        }

        public TestEmployeeWorkOrderBuilder WithWorkOrder(WorkOrder workOrder)
        {
            _workOrder = workOrder;
            return this;
        }

        #endregion
    }
}
