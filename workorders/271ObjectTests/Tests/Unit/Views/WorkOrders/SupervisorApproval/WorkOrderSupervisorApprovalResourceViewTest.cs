using LINQTo271.Views.WorkOrders.SupervisorApproval;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.SupervisorApproval
{
    /// <summary>
    /// Summary description for WorkOrderSupervisorApprovalResourceViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderSupervisorApprovalResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private ISearchView<WorkOrder> _searchView;
        private IDetailView<WorkOrder> _detailView;
        private IListView<WorkOrder> _listView;
        private TestWorkOrderSupervisorApprovalResourceView _target;

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
            _target = new TestWorkOrderSupervisorApprovalResourceViewBuilder()
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
        public void TestPhasePropertyDenotesApproval()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Approval, _target.Phase);
        }
    }

    internal class TestWorkOrderSupervisorApprovalResourceViewBuilder : TestDataBuilder<TestWorkOrderSupervisorApprovalResourceView>
    {
        #region Private Members

        private ISearchView<WorkOrder> _searchView;
        private IDetailView<WorkOrder> _detailView;
        private IListView<WorkOrder> _listView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderSupervisorApprovalResourceView Build()
        {
            var obj = new TestWorkOrderSupervisorApprovalResourceView();
            if (_searchView != null)
                obj.SetSearchView(_searchView);
            if (_detailView != null)
                obj.SetDetailView(_detailView);
            if (_listView != null)
                obj.SetListView(_listView);
            return obj;
        }

        public TestWorkOrderSupervisorApprovalResourceViewBuilder WithSearchView(ISearchView<WorkOrder> searchView)
        {
            _searchView = searchView;
            return this;
        }

        public TestWorkOrderSupervisorApprovalResourceViewBuilder WithDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestWorkOrderSupervisorApprovalResourceViewBuilder WithListView(IListView<WorkOrder> listView)
        {
            _listView = listView;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderSupervisorApprovalResourceView : WorkOrderSupervisorApprovalResourceView
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
