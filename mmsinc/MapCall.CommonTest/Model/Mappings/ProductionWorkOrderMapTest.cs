using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using NHibernate.Linq;
using MMSINC.Utilities;
using System;
using StructureMap;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMSINC.Testing.Utilities;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class ProductionWorkOrderMapTest : InMemoryDatabaseTest<ProductionWorkOrder>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider());
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestIsLockoutFormStillOpen()
        {
            var pwo = GetFactory<ProductionWorkOrderFactory>().Create();
            var lockoutform1 = GetFactory<LockoutFormFactory>().Create(new {
                ProductionWorkOrder = pwo,
                ReturnedToServiceDateTime = (DateTime?)null
            });

            var pwo1 = GetFactory<ProductionWorkOrderFactory>().Create();
            var lockoutform2 = GetFactory<LockoutFormFactory>().Create(new {
                ProductionWorkOrder = pwo1,
                ReturnedToServiceDateTime = DateTime.Now
            });

            pwo.LockoutForms.Add(lockoutform1);
            pwo1.LockoutForms.Add(lockoutform2);
            Session.Save(pwo);
            Session.Save(pwo1);
            Session.Flush();
            Session.Evict(pwo);
            Session.Evict(pwo1);

            var pwoAgain = Session.Get<ProductionWorkOrder>(pwo.Id);
            var pwoAgain1 = Session.Get<ProductionWorkOrder>(pwo1.Id);
            Assert.AreEqual(1, pwoAgain.LockoutForms.Count);
            Assert.IsNull(pwoAgain.LockoutForms.First().ReturnedToServiceDateTime);
            Assert.IsTrue(pwoAgain.IsLockoutFormStillOpen);

            Assert.AreEqual(1, pwoAgain1.LockoutForms.Count);
            Assert.IsNotNull(pwoAgain1.LockoutForms.First().ReturnedToServiceDateTime);
            Assert.IsFalse(pwoAgain1.IsLockoutFormStillOpen);
        }

        [TestMethod]
        public void TestIsRedTagPermitStillOpen()
        {
            var pwo = GetFactory<ProductionWorkOrderFactory>().Create();

            pwo.RedTagPermit = GetFactory<RedTagPermitFactory>().Create(new { ProductionWorkOrder = pwo });

            Session.Save(pwo);
            Session.Flush();
            Session.Evict(pwo);

            pwo = Session.Get<ProductionWorkOrder>(pwo.Id);

            Assert.IsTrue(pwo.IsRedTagPermitStillOpen);

            pwo.RedTagPermit.EquipmentRestoredOn = Lambdas.GetNowDate();

            Session.Save(pwo);
            Session.Flush();
            Session.Evict(pwo);

            pwo = Session.Get<ProductionWorkOrder>(pwo.Id);
            Assert.IsFalse(pwo.IsRedTagPermitStillOpen);
        }

        [TestMethod]
        public void TestHasAssignmentsOnNonCancelledWorkOrderReturnsTrueWhenOrderIsNotCancelledAndHasEmployeeAssignment()
        {
            var pwo = GetFactory<ProductionWorkOrderFactory>().Create();
            var employeeAssignment = GetFactory<EmployeeAssignmentFactory>().Create(new {ProductionWorkOrder = pwo});
            pwo.EmployeeAssignments.Add(employeeAssignment);
            Session.Save(pwo);
            Session.Flush();
            Session.Evict(pwo);

            var pwoAgain = Session.Get<ProductionWorkOrder>(pwo.Id);
            Assert.IsTrue(pwoAgain.HasAssignmentsOnNonCancelledWorkOrder.Value);
        }

        [TestMethod]
        public void TestHasAssignmentsOnNonCancelledWorkOrderReturnsFalseWhenOrderIsNotCancelledAndHasNoEmployeeAssignment()
        {
            var pwo = GetFactory<ProductionWorkOrderFactory>().Create();
            pwo.EmployeeAssignments.Clear();
            Session.Save(pwo);
            Session.Flush();
            Session.Evict(pwo);

            var pwoAgain = Session.Get<ProductionWorkOrder>(pwo.Id);
            Assert.IsFalse(pwoAgain.HasAssignmentsOnNonCancelledWorkOrder.Value);
        }

        [TestMethod]
        public void TestHasAssignmentsOnNonCancelledWorkOrderReturnsNullWhenOrderIsCancelledAndHasEmployeeAssignment()
        {
            var pwo = GetFactory<ProductionWorkOrderFactory>().Create();
            var employeeAssignment = GetFactory<EmployeeAssignmentFactory>().Create(new {ProductionWorkOrder = pwo});
            pwo.EmployeeAssignments.Add(employeeAssignment);
            pwo.DateCancelled = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            Session.Save(pwo);
            Session.Flush();
            Session.Evict(pwo);

            var pwoAgain = Session.Get<ProductionWorkOrder>(pwo.Id);
            Assert.IsNull(pwoAgain.HasAssignmentsOnNonCancelledWorkOrder);
        }

        [TestMethod]
        public void TestAssignedOnDateReturnsLatestDate()
        {
            var earlyAssignedOnDate = new DateTime(2020, 1, 1);
            var lateAssignedOnDate = new DateTime(2020, 3, 17);

            var pwo = GetFactory<ProductionWorkOrderFactory>().Create();
            var earlyEmployeeAssignment = GetFactory<EmployeeAssignmentFactory>().Create(new {
                ProductionWorkOrder = pwo,
                AssignedFor = earlyAssignedOnDate

            });
            var lateEmployeeAssignment = GetFactory<EmployeeAssignmentFactory>().Create(new {
                ProductionWorkOrder = pwo,
                AssignedFor = lateAssignedOnDate

            });
            pwo.EmployeeAssignments.Add(earlyEmployeeAssignment);
            pwo.EmployeeAssignments.Add(lateEmployeeAssignment);
            Session.Save(pwo);
            Session.Flush();
            Session.Evict(pwo);

            var pwoAgain = Session.Get<ProductionWorkOrder>(pwo.Id);
            // Should return true
            Assert.AreEqual(lateAssignedOnDate, pwoAgain.AssignedOnDate ?? DateTime.MinValue);
        }

        #endregion
    }
}
