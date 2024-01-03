using System;
using MMSINC.Exceptions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests;

namespace IntegrationTests.Model
{
    /// <summary>
    /// Summary description for SafetyMarkerIntegrationTest
    /// </summary>
    [TestClass]
    public class SafetyMarkerIntegrationTest : WorkOrdersTestClass<SafetyMarker>
    {
        #region Exposed Static Methods

        internal static TestSafetyMarkerBuilder GetValidSafetyMarker()
        {
            return new TestSafetyMarkerBuilder();
        }

        public static void DeleteSafetyMarker(SafetyMarker entity)
        {
            var order = entity.WorkOrder;
            SafetyMarkerRepository.Delete(entity);
            WorkOrderIntegrationTest.DeleteWorkOrder(order);
        }

        #endregion

        #region Private Methods

        protected override SafetyMarker GetValidObject()
        {
            return GetValidSafetyMarker();
        }

        protected override SafetyMarker GetValidObjectFromDatabase()
        {
            var marker = GetValidObject();
            SafetyMarkerRepository.Insert(marker);
            return marker;
        }

        protected override void DeleteObject(SafetyMarker entity)
        {
            DeleteSafetyMarker(entity);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewSafetyMarker()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.DoesNotThrow(
                    () => SafetyMarkerRepository.Insert(target));

                Assert.IsNotNull(target);
                Assert.IsInstanceOfType(target, typeof(SafetyMarker));

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestCannotSaveWithNoConesOrBarricades()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();
                target.ConesOnSite = 0;
                target.BaracadesOnSite = 0;

                MyAssert.Throws(() => SafetyMarkerRepository.Insert(target),
                    typeof(DomainLogicException),
                    "Attempting to save a SafetyMarker object with no Cones or Barricades specified should throw an exception");
            }
        }

        [TestMethod]
        public void TestCannotChangeNumberOfConesOrBarricadesAfterRetrievalDateSet()
        {
            var target = new SafetyMarker();
            target.ConesOnSite = target.BaracadesOnSite = 1;
            target.MarkersRetrievedOn = DateTime.Now;

            MyAssert.Throws(() => target.BaracadesOnSite = 2,
                typeof(DomainLogicException),
                "Attempting to change the number of BarricadesOnSite after retrieval date has been set should throw an exception");

            MyAssert.Throws(() => target.ConesOnSite = 2,
                typeof(DomainLogicException),
                "Attempting to change the number of ConesOnSite after retrieval date has been set should throw an exception");
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidSafetyMarker().WithWorkOrder(null);

                MyAssert.Throws(() => SafetyMarkerRepository.Insert(target),
                    typeof(DomainLogicException),
                    "Attempting to save a SafetyMarker record with no WorkOrder should throw an exception");
            }
        }

        [TestMethod]
        public void TestCannotChangeWorkOrderAfterSave()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObjectFromDatabase();
                var order = new WorkOrder();

                MyAssert.Throws(() => target.WorkOrder = order,
                    typeof(DomainLogicException),
                    "Attempting to change the WorkOrder of a SafetyMarker record after it has been set should throw an exception");

                DeleteObject(target);
            }
        }
    }

    // TODO: Should this be internal?
    internal class TestSafetyMarkerBuilder : TestDataBuilder<SafetyMarker>
    {
        #region Private Members

        private WorkOrder _workOrder = WorkOrderIntegrationTest.GetValidWorkOrder();

        #endregion

        #region Exposed Methods

        public override SafetyMarker Build()
        {
            var marker = new SafetyMarker();
            if (_workOrder != null)
                marker.WorkOrder = _workOrder;
            return marker;
        }

        public TestSafetyMarkerBuilder WithWorkOrder(WorkOrder workOrder)
        {
            _workOrder = workOrder;
            return this;
        }

        #endregion
    }
}
