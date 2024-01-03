using System;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.TestLibrary;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for WorkOrderPriorityIntegrationTest
    /// </summary>
    [TestClass]
    public class WorkOrderPriorityIntegrationTest : WorkOrdersTestClass<WorkOrderPriority>
    {
        #region Constants

        private const short MIN_EXPECTED_COUNT = 3;

        #endregion

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

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderPriorityIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void WorkOrderPriorityIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestStaticValues()
        {
            using (_simulator.SimulateRequest())
            {
                WorkOrderPriority target, expected;
                MyAssert.IsGreaterThanOrEqualTo(new WorkOrderPriorityRepository().Count(),
                                                MIN_EXPECTED_COUNT, "There should be at least 3 WorkOrderPriority records in the database by default.");

                target = WorkOrderPriorityRepository.Emergency;
                expected = WorkOrderPriorityRepository.GetEntity(WorkOrderPriorityRepository.Indices.EMERGENCY);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderPriority \"Emergency\"");
                Assert.AreEqual(WorkOrderPriorityRepository.Descriptions.EMERGENCY, target.Description,
                                "Description value for WorkOrderPriority.Emergency is not as expected.");

                target = WorkOrderPriorityRepository.HighPriority;
                expected = WorkOrderPriorityRepository.GetEntity(WorkOrderPriorityRepository.Indices.HIGH_PRIORITY);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderPriority \"High Priority\"");
                Assert.AreEqual(WorkOrderPriorityRepository.Descriptions.HIGH_PRIORITY, target.Description,
                                "Description value for WorkOrderPriority.HighPriority is not as expected.");

                target = WorkOrderPriorityRepository.Routine;
                expected = WorkOrderPriorityRepository.GetEntity(WorkOrderPriorityRepository.Indices.ROUTINE);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderPriority \"Routine\"");
                Assert.AreEqual(WorkOrderPriorityRepository.Descriptions.ROUTINE, target.Description,
                                "Description value for WorkOrderPriority.Routine is not as expected.");
            }
        }

        [TestMethod]
        public void TestCannotCreateNewWorkOrderPriority()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new WorkOrderPriority
                {
                    Description = "Test"
                };

                MyAssert.Throws(
                    () => WorkOrderPriorityRepository.Insert(target),
                    typeof(DomainLogicException),
                    "Attempting to create a new WorkOrderPriority within the WorkOrdersDataContext should throw an exception");
            }
        }

        [TestMethod]
        public void TestCannotChangeWorkOrderPriority()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                var oldDescription = target.Description;
                target.Description = "Test";

                MyAssert.Throws(
                    () => WorkOrderPriorityRepository.Update(target),
                    typeof(DomainLogicException),
                    "Attempting to change a WorkOrderPriority within the WorkOrdersDataContext should throw an exception");

                target.Description = oldDescription;
            }
        }

        [TestMethod]
        public void TestCannotDeleteWorkOrderPriority()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(
                    () => WorkOrderPriorityRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }
    }
}