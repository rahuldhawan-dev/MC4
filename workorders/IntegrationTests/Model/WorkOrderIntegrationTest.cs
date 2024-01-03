using _271ObjectTests;
using _271ObjectTests.Tests.Unit.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Subtext.TestLibrary;
using System.Collections.Generic;
using WorkOrders.Model;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for WorkOrdersIntegrationTest
    /// </summary>
    [TestClass]
    public class WorkOrderIntegrationTest : WorkOrdersTestClass<WorkOrder>
    {
        #region Constants

        private const int MIN_SAMPLE_COUNT = 3,
                          FIRST_SAMPLE_VALVE_ID = 184069,
                          SECOND_SAMPLE_VALVE_ID = 46604;

        #endregion

        #region Private Methods

        protected override WorkOrder GetValidObjectFromDatabase()
        {
            var order = GetValidObject();
            WorkOrderRepository.Insert(order);
            return order;
        }

        protected override WorkOrder GetValidObject()
        {
            return GetValidWorkOrder();
        }

        protected override void DeleteObject(WorkOrder entity)
        {
            DeleteWorkOrder(entity);
        }

        #endregion

        #region Exposed Static Methods

        internal static TestWorkOrderBuilder GetValidWorkOrder()
        {
            return new TestWorkOrderBuilder();
        }

        public static void DeleteWorkOrder(WorkOrder entity)
        {
            WorkOrderRepository.Delete(entity);
        }

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderIntegrationTestInitialize()
        {
            _simulator = new HttpSimulator();
            _simulator = _simulator.SimulateRequest();
        }

        [TestCleanup]
        public void WorkOrderIntegrationTestCleanup()
        {
            _simulator.Dispose();
        }

        #endregion

        [TestMethod]
        public void TestSampleData()
        {
            MyAssert.IsGreaterThanOrEqualTo(
                new WorkOrderRepository().Count(), MIN_SAMPLE_COUNT);

            TestFirstSampleWorkOrder();
            TestSecondSampleWorkOrder();
            TestThirdSampleWorkOrder();
        }

        private void TestFirstSampleWorkOrder()
        {
            WorkOrder target = null;
            MyAssert.DoesNotThrow(() => target = WorkOrderRepository.GetEntity(135450));
            Assert.IsNotNull(target);
            Assert.AreEqual(FIRST_SAMPLE_VALVE_ID, target.ValveID);
        }

        private void TestSecondSampleWorkOrder()
        {
            WorkOrder target = null;
            MyAssert.DoesNotThrow(() => target = WorkOrderRepository.GetEntity(135452));
            Assert.IsNotNull(target);
            Assert.AreEqual(FIRST_SAMPLE_VALVE_ID, target.ValveID);
        }

        private void TestThirdSampleWorkOrder()
        {
            WorkOrder target = null;
            MyAssert.DoesNotThrow(() => target = WorkOrderRepository.GetEntity(6272));
            Assert.IsNotNull(target);
            Assert.AreEqual(SECOND_SAMPLE_VALVE_ID, target.ValveID);
        }

        [TestMethod]
        public void TestGetWorkOrdersByOperatingCenterAndAssetReturnsNullIfAssetTypeIDOrAssetKeyUnset()
        {
            List<WorkOrder> orders = null;

            MyAssert.DoesNotThrow(
                () =>
                orders = WorkOrderRepository.GetWorkOrdersByOperatingCenterAndAsset(0, 0, "not null"));
            Assert.IsNull(orders);

            MyAssert.DoesNotThrow(
                () => orders = WorkOrderRepository.GetWorkOrdersByOperatingCenterAndAsset(1, 0, null));
            Assert.IsNull(orders);
        }

        [TestMethod]
        public void TestGetWorkOrdersByOperatingCenterAndAsset()
        {
            Assert.Inconclusive("TODO: Investigate why this fails on it");
            //var assetType = AssetTypeRepository.Valve;
            //var assetTypeID = assetType.AssetTypeID;
            //var valve = ValveRepository.GetEntity(FIRST_SAMPLE_VALVE_ID);
            //var assetKey = valve.ValveID;

            //var orders = WorkOrderRepository.GetWorkOrdersByOperatingCenterAndAsset(
            //    assetTypeID, 0, assetKey);

            //MyAssert.IsGreaterThan(orders.Count(), 0);
            //foreach (var order in orders)
            //    Assert.AreEqual(valve, order.Valve);
        }

        [TestMethod]
        public void TestCannotChangeCreatedOnDateAfterSave()
        {
            var target = GetValidObjectFromDatabase();

            MyAssert.Throws(
                () => target.CreatedOn = target.CreatedOn.AddHours(1),
                typeof(DomainLogicException),
                "Attempting to change the value of CreatedOn should throw an exception for a WorkOrder that has already been saved");

            DeleteObject(target);
        }

        [TestMethod]
        public void TestCannotChangeCreatedByAfterSave()
        {
            var target = GetValidObjectFromDatabase();

            MyAssert.Throws(() => target.CreatedBy = new Employee(),
                typeof(DomainLogicException),
                "Attempting to change the value of CreatedBy should throw an exception for a WorkOrder that has already been saved");

            DeleteObject(target);
        }

        [TestMethod]
        public override void TestAllStringPropertiesThrowsExceptionWhenSetTooLong()
        {
            base.TestAllStringPropertiesThrowsExceptionWhenSetTooLong();
        }

        [TestMethod]
        public void TestIsPremiseLinkedToSampleSiteReturnsTrueWhenPremiseIsLinkedToSampleSite()
        {
            var workOrder = new WorkOrder
            {
                PremiseNumber = "9180451708"
            };

            Assert.IsTrue(workOrder.IsPremiseLinkedToSampleSite);
        }

        [TestMethod]
        public void TestIsPremiseLinkedToSampleSiteReturnsFalseWhenPremiseIsNotLinkedToSampleSite()
        {
            var workOrder = new WorkOrder
            {
                PremiseNumber = "9080151702"
            };

            Assert.IsFalse(workOrder.IsPremiseLinkedToSampleSite);
        }
    }

    internal class TestWorkOrderBuilder : TestDataBuilder<WorkOrder>
    {
        #region Constants

        private const string CUSTOMER_NAME = "John Smith";
        private const string STREET_NUMBER = "123";

        #endregion

        #region Private Members

        // AssetTypeRepository.Valve
        private AssetType _assetType = AssetTypeTest.GetValidAssetType();
        //
        private Valve _valve = ValveTest.GetValidValve();
        // WorkOrderPurposeRepository.Customer
        private WorkOrderPurpose _drivenBy =
            WorkOrderPurposeTest.GetValidWorkOrderPurpose();
        // WorkOrderRequester.Customer
        private WorkOrderRequester _requestedBy =
            WorkOrderRequesterTest.GetValidWorkOrderRequester();
        // MapCall Developer
        private Employee _createdBy = EmployeeIntegrationTest.GetValidEmployee();
        // ""
        private WorkDescription _workDescription =
            WorkDescriptionTest.GetValidWorkDescription();
        // MarkoutRequirementRepository.Routine
        private MarkoutRequirement _markoutRequirement =
            MarkoutRequirementTest.GetValidMarkoutRequirement();
        // Neptune, NJ
        private Town _town = TownTest.GetValidTown();
        // High Priority
        private WorkOrderPriority _priority =
            WorkOrderPriorityTest.GetValidWorkOrderPriority();

        private OperatingCenter _operatingCenter = OperatingCenterIntegrationTest.GetValidOperatingCenter();
        private string _customerName = CUSTOMER_NAME,
                       _streetNumber = STREET_NUMBER;

        #endregion

        #region Exposed Methods

        public override WorkOrder Build()
        {
            var order = new WorkOrder();
            if (_assetType != null)
                order.AssetType = _assetType;
            if (_valve != null)
                order.Valve = _valve;
            if (_drivenBy != null)
                order.DrivenBy = _drivenBy;
            if (_requestedBy != null)
                order.RequestedBy = _requestedBy;
            if (_createdBy != null)
                order.CreatedBy = _createdBy;
            if (_customerName != null)
                order.CustomerName = _customerName;
            if (_streetNumber != null)
                order.StreetNumber = _streetNumber;
            if (_workDescription != null)
                order.WorkDescription = _workDescription;
            if (_markoutRequirement != null)
                order.MarkoutRequirement = _markoutRequirement;
            if (_town != null)
                order.Town = _town;
            if (_priority != null)
                order.Priority = _priority;
            if (_operatingCenter != null)
                order.OperatingCenter = _operatingCenter;
            return order;
        }

        public TestWorkOrderBuilder WithTown(Town town)
        {
            _town = town;
            return this;
        }

        public TestWorkOrderBuilder WithPriority(WorkOrderPriority priority)
        {
            _priority = priority;
            return this;
        }

        public TestWorkOrderBuilder WithValve(Valve valve)
        {
            _valve = valve;
            return this;
        }

        public TestWorkOrderBuilder WithCustomerName(string customerName)
        {
            _customerName = customerName;
            return this;
        }

        public TestWorkOrderBuilder WithStreetNumber(string streetNumber)
        {
            _streetNumber = streetNumber;
            return this;
        }

        public TestWorkOrderBuilder WithAssetType(AssetType assetType)
        {
            _assetType = assetType;
            return this;
        }

        public TestWorkOrderBuilder WithCreatedBy(Employee createdBy)
        {
            _createdBy = createdBy;
            return this;
        }

        public TestWorkOrderBuilder WithDrivenBy(WorkOrderPurpose drivenBy)
        {
            _drivenBy = drivenBy;
            return this;
        }

        public TestWorkOrderBuilder WithMarkoutRequirement(MarkoutRequirement markoutRequirement)
        {
            _markoutRequirement = markoutRequirement;
            return this;
        }

        public TestWorkOrderBuilder WithRequestedBy(WorkOrderRequester requestedBy)
        {
            _requestedBy = requestedBy;
            return this;
        }

        public TestWorkOrderBuilder WithWorkDescription(WorkDescription workDescription)
        {
            _workDescription = workDescription;
            return this;
        }

        public TestWorkOrderBuilder WithOperatingCenter(OperatingCenter operatingCenter)
        {
            _operatingCenter = operatingCenter;
            return this;
        }

        #endregion
    }
}
