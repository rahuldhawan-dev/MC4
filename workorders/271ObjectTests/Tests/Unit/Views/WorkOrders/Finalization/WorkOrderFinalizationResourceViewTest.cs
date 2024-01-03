using LINQTo271.Views.WorkOrders.Finalization;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Finalization
{
    /// <summary>
    /// Summary description for WorkOrderFinalizationResourceViewTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderFinalizationResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private ISearchView<WorkOrder> _searchView;
        private IDetailView<WorkOrder> _detailView;
        private IListView<WorkOrder> _listView;
        private TestWorkOrderFinalizationResourceView _target;

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
            _target = new TestWorkOrderFinalizationResourceViewBuilder()
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
        public void TestPhasePropertyDenotesFinalization()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Finalization, _target.Phase);
        }
    }

    internal class TestWorkOrderFinalizationResourceViewBuilder : TestDataBuilder<TestWorkOrderFinalizationResourceView>
    {
        #region Private Members

        private ISearchView<WorkOrder> _searchView;
        private IDetailView<WorkOrder> _detailView;
        private IListView<WorkOrder> _listView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderFinalizationResourceView Build()
        {
            var obj = new TestWorkOrderFinalizationResourceView();
            if (_searchView != null)
                obj.SetSearchView(_searchView);
            if (_detailView != null)
                obj.SetDetailView(_detailView);
            if (_listView != null)
                obj.SetListView(_listView);
            return obj;
        }

        public TestWorkOrderFinalizationResourceViewBuilder WithSearchView(ISearchView<WorkOrder> searchView)
        {
            _searchView = searchView;
            return this;
        }

        public TestWorkOrderFinalizationResourceViewBuilder WithDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestWorkOrderFinalizationResourceViewBuilder WithListView(IListView<WorkOrder> listView)
        {
            _listView = listView;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderFinalizationResourceView : WorkOrderFinalizationResourceView
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