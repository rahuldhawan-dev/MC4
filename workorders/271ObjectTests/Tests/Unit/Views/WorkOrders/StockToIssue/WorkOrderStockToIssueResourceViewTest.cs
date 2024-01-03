using LINQTo271.Views.WorkOrders.StockToIssue;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.StockToIssue
{
    /// <summary>
    /// Summary description for WorkOrderStockToIssueResourceViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderStockToIssueResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private ISearchView<WorkOrder> _searchView;
        private IDetailView<WorkOrder> _detailView;
        private IListView<WorkOrder> _listView;
        private TestWorkOrderStockToIssueResourceView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks
                .DynamicMock(out _searchView)
                .DynamicMock(out _detailView)
                .DynamicMock(out _listView);
            _target = new TestWorkOrderStockToIssueResourceViewBuilder()
                .WithSearchView(_searchView)
                .WithDetailView(_detailView)
                .WithListView(_listView);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestDetailViewPropertyReturnsDetailView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_detailView, _target.DetailView);
        }

        [TestMethod]
        public void TestSearchViewPropertyReturnsSearchView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_searchView, _target.SearchView);
        }

        [TestMethod]
        public void TestListViewPropertyReturnsListView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_listView, _target.ListView);
        }

        [TestMethod]
        public void TestPhasePropertyDenotesStockApproval()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.StockApproval, _target.Phase);
        }
    }

    internal class TestWorkOrderStockToIssueResourceViewBuilder : TestDataBuilder<TestWorkOrderStockToIssueResourceView>
    {
        #region Private Members

        private ISearchView<WorkOrder> _searchView;
        private IDetailView<WorkOrder> _detailView;
        private IListView<WorkOrder> _listView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderStockToIssueResourceView Build()
        {
            var obj = new TestWorkOrderStockToIssueResourceView();
            if (_searchView != null)
                obj.SetSearchView(_searchView);
            if (_detailView != null)
                obj.SetDetailView(_detailView);
            if (_listView != null)
                obj.SetListView(_listView);
            return obj;
        }

        public TestWorkOrderStockToIssueResourceViewBuilder WithSearchView(ISearchView<WorkOrder> searchView)
        {
            _searchView = searchView;
            return this;
        }

        public TestWorkOrderStockToIssueResourceViewBuilder WithDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestWorkOrderStockToIssueResourceViewBuilder WithListView(IListView<WorkOrder> listView)
        {
            _listView = listView;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderStockToIssueResourceView : WorkOrderStockToIssueResourceView
    {
        #region Exposed Methods

        public void SetSearchView(ISearchView<WorkOrder> searchView)
        {
            wosvWorkOrders = searchView;
        }

        public void SetDetailView(IDetailView<WorkOrder> detailView)
        {
            wodvWorkOrder = detailView;
        }

        public void SetListView(IListView<WorkOrder> listView)
        {
            wolvWorkOrders = listView;
        }

        #endregion
    }
}