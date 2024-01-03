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
    /// Summary description for WorkOrderPurposeIntegrationTest
    /// </summary>
    [TestClass]
    public class WorkOrderPurposeIntegrationTest : WorkOrdersTestClass<WorkOrderPurpose>
    {
        #region Constants

        private const int MIN_EXPECTED_COUNT = 4;

        #endregion

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

        #region Additional test attributes

        [TestInitialize]
        public void WorkOrderPurposeIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void WorkOrderPurposeIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestStaticValues()
        {
            using (_simulator.SimulateRequest())
            {
                WorkOrderPurpose target, expected;
                MyAssert.IsGreaterThanOrEqualTo(
                    new WorkOrderPurposeRepository().Count(), MIN_EXPECTED_COUNT,
                    "There should be at least 4 WorkOrderPurposes in the database by default.");

                target = WorkOrderPurposeRepository.Customer;
                expected =
                    WorkOrderPurposeRepository.
                        GetEntity(
                        WorkOrderPurposeRepository.Indices.CUSTOMER);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderPurpose \"Customer\"");
                Assert.AreEqual(WorkOrderPurposeRepository.Descriptions.CUSTOMER, target.Description,
                                "Description property for WorkOrderPurpose.Customer is not as expected.");

                target = WorkOrderPurposeRepository.Complaince;
                expected =
                    WorkOrderPurposeRepository.
                        GetEntity(
                        WorkOrderPurposeRepository.Indices.COMPLIANCE);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderPurpose \"Compliance\"");
                Assert.AreEqual(WorkOrderPurposeRepository.Descriptions.COMPLIANCE, target.Description,
                                "Description property for WorkOrderPurpose.Compliance is not as expected.");

                target = WorkOrderPurposeRepository.Safety;
                expected =
                    WorkOrderPurposeRepository.
                        GetEntity(
                        WorkOrderPurposeRepository.Indices.SAFETY);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderPurpose \"Safety\"");
                Assert.AreEqual(WorkOrderPurposeRepository.Descriptions.SAFETY, target.Description,
                                "Description property for WorkOrderPurpose.Safety is not as expected.");

                target = WorkOrderPurposeRepository.LeakDetection;
                expected =
                    WorkOrderPurposeRepository.
                        GetEntity(
                        WorkOrderPurposeRepository.Indices.LEAK_DETECTION);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderPurpose \"LeakDetection\"");
                Assert.AreEqual(WorkOrderPurposeRepository.Descriptions.LEAK_DETECTION, target.Description,
                                "Description property for WorkOrderPurpose.LeakDetection is not as expected.");
            }
        }

        [TestMethod]
        public void TestCannotCreateNewWorkOrderPurpose()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new WorkOrderPurpose
                {
                    Description = "Test"
                };

                MyAssert.Throws(
                    () => WorkOrderPurposeRepository.Insert(target),
                    typeof(DomainLogicException),
                    "Attempting to create a new WorkOrderPurpose within the WorkOrdersDataContext should throw an exception.");
            }
        }

        [TestMethod]
        public void TestCannotChangeWorkOrderPurpose()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                var oldDescription = target.Description;
                target.Description = "Test";

                MyAssert.Throws(
                    () => WorkOrderPurposeRepository.Update(target),
                    "Attempting to change a WorkOrderPurpose within the WorkOrdersDataContext should throw an exception.");
                target.Description = oldDescription;
            }
        }

        [TestMethod]
        public void TestCannotDeleteWorkOrderPurpose()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(
                    () => WorkOrderPurposeRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }
    }
}