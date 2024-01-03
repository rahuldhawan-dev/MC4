using LINQTo271.Views.Abstract;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.Abstract
{
    /// <summary>
    /// Summary description for WorkOrderApprovalResourceViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderApprovalResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListView<WorkOrder> _listView;
        private IDetailView<WorkOrder> _detailView;
        private ISearchView<WorkOrder> _searchView;
        private TestWorkOrderApprovalResourceView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _listView)
                .DynamicMock(out _detailView)
                .DynamicMock(out _searchView);

            _target = new TestWorkOrderApprovalResourceViewBuilder()
                .WithListView(_listView)
                .WithDetailView(_detailView)
                .WithSearchView(_searchView);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestPhasePropertyDenotesApproval()
        {
            Assert.AreEqual(WorkOrderPhase.Approval, _target.Phase);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestListViewPropertyReturnsListView()
        {
            Assert.AreSame(_listView, _target.ListView);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSearchViewPropertyReturnsSearchView()
        {
            Assert.AreSame(_searchView, _target.SearchView);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDetailViewPropertyReturnsDetailView()
        {
            Assert.AreSame(_detailView, _target.DetailView);

            _mocks.ReplayAll();
        }
    }

    internal class TestWorkOrderApprovalResourceViewBuilder : TestDataBuilder<TestWorkOrderApprovalResourceView>
    {
        #region Private Members

        private IListView<WorkOrder> _listView;
        private IDetailView<WorkOrder> _detailView;
        private ISearchView<WorkOrder> _searchView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderApprovalResourceView Build()
        {
            var obj = new TestWorkOrderApprovalResourceView();
            if (_listView != null)
                obj.SetListView(_listView);
            if (_detailView != null)
                obj.SetDetailView(_detailView);
            if (_searchView != null)
                obj.SetSearchVie(_searchView);
            return obj;
        }

        public TestWorkOrderApprovalResourceViewBuilder WithListView(IListView<WorkOrder> view)
        {
            _listView = view;
            return this;
        }

        public TestWorkOrderApprovalResourceViewBuilder WithDetailView(IDetailView<WorkOrder> view)
        {
            _detailView = view;
            return this;
        }

        public TestWorkOrderApprovalResourceViewBuilder WithSearchView(ISearchView<WorkOrder> view)
        {
            _searchView = view;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderApprovalResourceView : WorkOrderApprovalResourceView
    {
        #region Exposed Methods

        public void SetListView(IListView<WorkOrder> view)
        {
            wolvWorkOrders = view;
        }

        public void SetDetailView(IDetailView<WorkOrder> view)
        {
            wodvWorkOrder = view;
        }

        public void SetSearchVie(ISearchView<WorkOrder> view)
        {
            wosvWorkOrders = view;
        }

        #endregion
    }
}
