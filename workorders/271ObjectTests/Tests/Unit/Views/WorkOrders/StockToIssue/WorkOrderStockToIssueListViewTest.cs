using LINQTo271.Views.WorkOrders.StockToIssue;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.StockToIssue
{
    /// <summary>
    /// Summary description for WorkOrderStockToIssueListViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderStockToIssueListViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListControl _listControl;
        private TestWorkOrderStockToIssueListView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks.DynamicMock(out _listControl);
            _target = new TestWorkOrderStockToIssueListViewBuilder()
                .WithListControl(_listControl);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestListControlPropertyReturnsListControl()
        {
            Assert.AreSame(_listControl, _target.ListControl);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestPhasePropertyDenotesStockApproval()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.StockApproval, _target.Phase);
        }
    }

    internal class TestWorkOrderStockToIssueListViewBuilder : TestDataBuilder<TestWorkOrderStockToIssueListView>
    {
        #region Private Members

        private IListControl _listControl = new MvpGridView();

        #endregion

        #region Exposed Methods

        public override TestWorkOrderStockToIssueListView Build()
        {
            var obj = new TestWorkOrderStockToIssueListView();
            if (_listControl != null)
                obj.SetListControl(_listControl);
            return obj;
        }

        public TestWorkOrderStockToIssueListViewBuilder WithListControl(IListControl listControl)
        {
            _listControl = listControl;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderStockToIssueListView : WorkOrderStockToIssueListView
    {
        #region Exposed Methods

        public void SetListControl(IListControl listControl)
        {
            gvWorkOrders = listControl;
        }

        #endregion
    }
}