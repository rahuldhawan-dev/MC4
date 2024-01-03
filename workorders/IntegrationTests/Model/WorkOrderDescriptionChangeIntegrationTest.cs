using System;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;
using _271ObjectTests.Tests.Unit.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for WorkOrderDescriptionChangeTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderDescriptionChangeIntegrationTest : WorkOrdersTestClass<WorkOrderDescriptionChange>
    {
        #region Exposed Static Methods

        public static WorkOrderDescriptionChange GetValidWorkOrderDescriptionChange()
        {
            return new WorkOrderDescriptionChange {
                                                      WorkOrder = WorkOrderIntegrationTest.GetValidWorkOrder(),
                                                      ToWorkDescription = WorkDescriptionTest.GetValidToWorkDescription(),
                                                      FromWorkDescription = WorkDescriptionTest.GetValidFromWorkDescription(),
                                                      ResponsibleEmployee = EmployeeIntegrationTest.GetValidEmployee(),
                                                      DateOfChange = DateTime.Now
                                                  };
        }

        public static void DeleteWorkOrderDescriptionChange(WorkOrderDescriptionChange entity)
        {
            var order = entity.WorkOrder;
            WorkOrderDescriptionChangeRepository.Delete(entity);
            WorkOrderIntegrationTest.DeleteWorkOrder(order);
        }

        #endregion

        #region Private Methods

        protected override WorkOrderDescriptionChange GetValidObject()
        {
            return GetValidWorkOrderDescriptionChange();
        }

        protected override WorkOrderDescriptionChange GetValidObjectFromDatabase()
        {
            var obj = GetValidObject();
            WorkOrderDescriptionChangeRepository.Insert(obj);
            return obj;
        }

        protected override void DeleteObject(WorkOrderDescriptionChange entity)
        {
            DeleteWorkOrderDescriptionChange(entity);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewWorkOrderDescriptionChange()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.DoesNotThrow(
                    () => WorkOrderDescriptionChangeRepository.Insert(target));
                Assert.IsNotNull(target);
                Assert.IsInstanceOfType(target, typeof(WorkOrderDescriptionChange));

                DeleteObject(target);
            }
        }
    }
}