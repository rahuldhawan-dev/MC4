using System;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Model
{
    /// <summary>
    /// Summary description for WorkOrderPriorityTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderPriorityTest : WorkOrdersTestClass<WorkOrderPriority>
    {
        #region Exposed Static Methods

        public static WorkOrderPriority GetValidWorkOrderPriority()
        {
            return WorkOrderPriorityRepository.HighPriority;
        }

        public static void DeleteWorkOrderPriority(WorkOrderPriority entity)
        {
            throw new InvalidOperationException("Cannot delete WorkOrderPriority objects.");
        }

        #endregion

        #region Private Methods

        protected override WorkOrderPriority GetValidObject()
        {
            return GetValidWorkOrderPriority();
        }

        protected override WorkOrderPriority GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(WorkOrderPriority entity)
        {
            DeleteWorkOrderPriority(entity);
        }

        #endregion

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestToStringMethodReflectsDescriptionValue()
        {
            var target = new WorkOrderPriority();
            var description = "Test";
            target.Description = description;

            Assert.AreEqual(description,
                target.ToString(), "The .ToString() method for WorkOrderPriority should merely return the Description value.");

            description = "OtherTest";
            target.Description = description;
            Assert.AreEqual(description,
                target.ToString(),
                "The .ToString() method for WorkOrderPriority should merely return the Description value.");
        }
    }

    internal class TestWorkOrderPriorityBuilder : TestDataBuilder<WorkOrderPriority>
    {
        #region Private Members

        private readonly string _description;
        private readonly int _priorityID;

        #endregion

        #region Private Static Members

        private static WorkOrderPriority revenueRelated,
                                         routine,
                                         highPriority,
                                         emergency;

        #endregion

        #region Exposed Static Properties

        public static WorkOrderPriority Routine
        {
            get
            {
                if (routine == null)
                    routine =
                        new TestWorkOrderPriorityBuilder(
                            WorkOrderPriorityRepository.Descriptions.ROUTINE,
                            WorkOrderPriorityRepository.Indices.ROUTINE);
                return routine;
            }
        }

        public static WorkOrderPriority HighPriority
        {
            get
            {
                if (highPriority == null)
                    highPriority =
                        new TestWorkOrderPriorityBuilder(
                            WorkOrderPriorityRepository.Descriptions.HIGH_PRIORITY,
                            WorkOrderPriorityRepository.Indices.HIGH_PRIORITY);
                return highPriority;
            }
        }

        public static WorkOrderPriority Emergency
        {
            get
            {
                if (emergency == null)
                    emergency =
                        new TestWorkOrderPriorityBuilder(
                            WorkOrderPriorityRepository.Descriptions.EMERGENCY,
                            WorkOrderPriorityRepository.Indices.EMERGENCY);
                return emergency;
            }
        }

        #endregion

        #region Constructors

        private TestWorkOrderPriorityBuilder(string description, int priorityID)
        {
            _description = description;
            _priorityID = priorityID;
        }

        #endregion

        #region Exposed Methods

        public override WorkOrderPriority Build()
        {
            return new WorkOrderPriority {
                Description = _description,
                WorkOrderPriorityID = _priorityID
            };
        }

        #endregion
    }
}
