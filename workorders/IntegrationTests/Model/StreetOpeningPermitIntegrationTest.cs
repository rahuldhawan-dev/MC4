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
    /// Summary description for StreetOpeningPermitTestTest
    /// </summary>
    [TestClass]
    public class StreetOpeningPermitIntegrationTest : WorkOrdersTestClass<StreetOpeningPermit>, IWorkOrderDependentObjectTest
    {
        #region Exposed Static Methods

        internal static TestStreetOpeningPermitBuilder GetValidStreetOpeningPermit()
        {
            return new TestStreetOpeningPermitBuilder();
        }

        public static void DeleteStreetOpeningPermit(StreetOpeningPermit entity)
        {
            var order = entity.WorkOrder;
            StreetOpeningPermitRepository.Delete(entity);
            WorkOrderIntegrationTest.DeleteWorkOrder(order);
        }

        #endregion

        #region Private Methods

        protected override StreetOpeningPermit GetValidObject()
        {
            return GetValidStreetOpeningPermit();
        }

        protected override StreetOpeningPermit GetValidObjectFromDatabase()
        {
            var br = GetValidObject();
            StreetOpeningPermitRepository.Insert(br);
            return br;
        }

        protected override void DeleteObject(StreetOpeningPermit entity)
        {
            DeleteStreetOpeningPermit(entity);
        }

        #endregion

        [TestMethod]
        public void TestCreateNewStreetOpeningPermit()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidObject();

                MyAssert.DoesNotThrow(() => StreetOpeningPermitRepository.Insert(target));

                Assert.IsNotNull(target);
                Assert.IsInstanceOfType(target, typeof(StreetOpeningPermit));

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestCannotSaveWithoutWorkOrder()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidStreetOpeningPermit().WithWorkOrder(null);

                MyAssert.Throws(() => StreetOpeningPermitRepository.Insert(target),
                                typeof(DomainLogicException),
                                "Trying to save a StreetOpeningPermit without a linked WorkOrder should throw an exception");
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
                                "Attempting to change the WorkOrder should throw an exception for a StreetOpeningPermit object that has already been saved");

                DeleteObject(target);
            }
        }

        [TestMethod]
        public void TestCannotSaveWithoutDateRequested()
        {
            using (_simulator.SimulateRequest())
            {
                var target = GetValidStreetOpeningPermit().WithDateRequested(DateTime.MinValue);

                MyAssert.Throws(() => StreetOpeningPermitRepository.Insert(target),
                                typeof(DomainLogicException),
                                "Attempting to save a StreetOpeningPermit record without a DateRequested should throw an exception");
            }
        }
    }

    internal class TestStreetOpeningPermitBuilder : TestDataBuilder<StreetOpeningPermit>
    {
        #region Constants


        #endregion

        #region Private Members

        private WorkOrder _workOrder = WorkOrderIntegrationTest.GetValidWorkOrder();
        private string _streetOpeningPermitNumber = "test123"; 
        private DateTime _dateRequested = DateTime.Now;
        private DateTime? _dateIssued;
        private DateTime? _expirationDate;
        private string _notes;
        
        #endregion

        #region Exposed Methods

        public override StreetOpeningPermit Build()
        {
            var entity = new StreetOpeningPermit {
                DateRequested = _dateRequested,
                StreetOpeningPermitNumber = _streetOpeningPermitNumber
            };

            if (_workOrder != null)
                entity.WorkOrder = _workOrder;
            if (_dateIssued != null)
                entity.DateIssued = _dateIssued;
            if (_expirationDate != null)
                entity.ExpirationDate = _expirationDate;
            if (_notes != null)
                entity.Notes = _notes;
            return entity;
        }

        public TestStreetOpeningPermitBuilder WithDateRequested(DateTime dateRequested)
        {
            _dateRequested = dateRequested;
            return this;
        }

        public TestStreetOpeningPermitBuilder WithIssueDate(DateTime? dateIssued)
        {
            _dateIssued = dateIssued;
            return this;
        }

        public TestStreetOpeningPermitBuilder WithWorkOrder(WorkOrder workOrder)
        {
            _workOrder = workOrder;
            return this;
        }
        #endregion
    }
}