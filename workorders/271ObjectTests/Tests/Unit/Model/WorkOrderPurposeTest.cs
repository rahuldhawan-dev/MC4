using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for WorkOrderPurposeTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderPurposeTest : WorkOrdersTestClass<WorkOrderPurpose>
    {
        #region Private Methods

        protected override WorkOrderPurpose GetValidObject()
        {
            return GetValidWorkOrderPurpose();
        }

        protected override WorkOrderPurpose GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(WorkOrderPurpose entity)
        {
            DeleteWorkOrderPurpose(entity);
        }

        #endregion

        #region Exposed Static Methods

        public static WorkOrderPurpose GetValidWorkOrderPurpose()
        {
            return WorkOrderPurposeRepository.Customer;
        }

        public static void DeleteWorkOrderPurpose(WorkOrderPurpose entity)
        {
            throw new InvalidOperationException("Cannot delete WorkOrderPurpose objects.");
        }

        #endregion

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestToStringMethodReflectsDescriptionProperty()
        {
            var expected = "Test";
            var target = new WorkOrderPurpose {
                Description = expected
            };

            Assert.AreEqual(expected, target.ToString());
        }
    }
}