using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Views.WorkOrders;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderAssignmentEventArgsTest.
    /// </summary>
    [TestClass]
    public class WorkOrderAssignmentEventArgsTest
    {
        #region Private Members

        private WorkOrderAssignmentEventArgs _target;

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestConstructorSetsCrewID()
        {
            var expected = 12;

            _target = new WorkOrderAssignmentEventArgs(expected, DateTime.MinValue, null);

            Assert.AreEqual(expected, _target.CrewID);
        }

        [TestMethod]
        public void TestConstructorSetsDate()
        {
            var expected = DateTime.Now;

            _target = new WorkOrderAssignmentEventArgs(0, expected, null);

            Assert.AreEqual(expected, _target.Date);
        }

        [TestMethod]
        public void TestConstructorSetsWorkOrderIDs()
        {
            var expected = new[] {
                8, 6, 7, 5, 3, 0, 9
            };

            _target = new WorkOrderAssignmentEventArgs(0, DateTime.MinValue, expected);

            Assert.AreSame(expected, _target.WorkOrderIDs);
        }

        #endregion
    }
}
