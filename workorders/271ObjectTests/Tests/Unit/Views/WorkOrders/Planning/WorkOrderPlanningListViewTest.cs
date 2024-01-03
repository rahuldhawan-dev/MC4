using LINQTo271.Views.WorkOrders.Planning;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Planning
{
    /// <summary>
    /// Summary description for WorkOrderPlanningListViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderPlanningListViewTest
    {
        #region Private Members

        private TestWorkOrderPlanningListView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderPlanningListViewTestInitialize()
        {
            _target = new TestWorkOrderPlanningListViewBuilder();
        }

        #endregion

        [TestMethod]
        public void TestPhasePropertyDenotesPlanning()
        {
            Assert.AreEqual(WorkOrderPhase.Planning, _target.Phase);
        }
    }

    internal class TestWorkOrderPlanningListViewBuilder : TestDataBuilder<TestWorkOrderPlanningListView>
    {
        #region Exposed Methods

        public override TestWorkOrderPlanningListView Build()
        {
            var obj = new TestWorkOrderPlanningListView();
            return obj;
        }

        #endregion
    }

    internal class TestWorkOrderPlanningListView : WorkOrderPlanningListView
    {
    }
}
