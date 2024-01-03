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
    /// Summary description for WorkOrderRequesterIntegrationTest
    /// </summary>
    [TestClass]
    public class WorkOrderRequesterIntegrationTest : WorkOrdersTestClass<WorkOrderRequester>
    {
        #region Constants

        private const int MIN_EXPECTED_COUNT = 3;

        #endregion

        #region Private Methods

        protected override WorkOrderRequester GetValidObject()
        {
            return GetValidWorkOrderRequester();
        }

        protected override WorkOrderRequester GetValidObjectFromDatabase()
        {
            return GetValidObject();
        }

        protected override void DeleteObject(WorkOrderRequester entity)
        {
            DeleteWorkOrderRequester(entity);
        }

        #endregion

        #region Exposed Static Methods

        public static WorkOrderRequester GetValidWorkOrderRequester()
        {
            return WorkOrderRequesterRepository.Customer;
        }

        public static void DeleteWorkOrderRequester(WorkOrderRequester entity)
        {
            throw new InvalidOperationException("Cannot delete WorkOrderRequester objects.");
        }

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void WorkOrderRequesterIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
        }

        [TestCleanup]
        public void WorkOrderRequesterIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestStaticValues()
        {
            using (_simulator.SimulateRequest())
            {
                WorkOrderRequester target, expected;
                MyAssert.IsGreaterThanOrEqualTo(
                    new WorkOrderRequesterRepository().Count(),
                    MIN_EXPECTED_COUNT);

                target = WorkOrderRequesterRepository.Customer;
                expected =
                    WorkOrderRequesterRepository.
                        GetEntity(
                        WorkOrderRequesterRepository.Indices.CUSTOMER);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderRequester \"Customer\"");
                Assert.AreEqual(WorkOrderRequesterRepository.Descriptions.CUSTOMER, target.Description,
                                "Description property for WorkOrderRequester.Customer is not as expected.");

                target = WorkOrderRequesterRepository.Employee;
                expected =
                    WorkOrderRequesterRepository.
                        GetEntity(
                        WorkOrderRequesterRepository.Indices.EMPLOYEE);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderRequester \"Employee\"");
                Assert.AreEqual(WorkOrderRequesterRepository.Descriptions.EMPLOYEE, target.Description,
                                "Description property for WorkOrderRequester.Employee is not as expected.");

                target = WorkOrderRequesterRepository.LocalGovernment;
                expected =
                    WorkOrderRequesterRepository.
                        GetEntity(
                        WorkOrderRequesterRepository.Indices.LOCAL_GOVERNMENT);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderRequester \"LocalGovernment\"");
                Assert.AreEqual(WorkOrderRequesterRepository.Descriptions.LOCAL_GOVERNMENT,
                                target.Description,
                                "Description property for WorkOrderRequester.LocalGovernment is not as expected.");

                target = WorkOrderRequesterRepository.CallCenter;
                expected =
                    WorkOrderRequesterRepository.
                        GetEntity(
                        WorkOrderRequesterRepository.Indices.CALL_CENTER);
                Assert.AreEqual(expected, target,
                                "Error getting static WorkOrderRequester \"LocalGovernment\"");
                Assert.AreEqual(WorkOrderRequesterRepository.Descriptions.CALL_CENTER,
                                target.Description,
                                "Description property for WorkOrderRequester.CallCenter is not as expected.");
            }
        }

        [TestMethod]
        public void TestCannotCreateNewWorkOrderRequester()
        {
            using (_simulator.SimulateRequest())
            {
                var target = new WorkOrderRequester
                {
                    Description = "Test"
                };

                MyAssert.Throws(
                    () => WorkOrderRequesterRepository.Insert(target),
                    typeof(DomainLogicException),
                    "Attempting to create a new WorkOrderRequester within the WorkOrdersDataContext should throw an exception.");
            }
        }

        [TestMethod]
        public void TestCannotChangeWorkOrderRequester()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                var oldDescription = target.Description;
                target.Description = "Test";

                MyAssert.Throws(
                    () => WorkOrderRequesterRepository.Update(target),
                    typeof(DomainLogicException),
                    "Attempting to change a WorkOrderRequester within the WorkOrdersDataContext should throw an exception.");

                target.Description = oldDescription;
            }
        }

        [TestMethod]
        public void TestCannotDeleteWorkOrderRequester()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.Throws(
                    () => WorkOrderRequesterRepository.Delete(target),
                    typeof(DomainLogicException));
            }
        }
    }
}