using LINQTo271.Views.WorkOrders.General;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.General
{
    /// <summary>
    /// Summary description for WorkOrderGeneralResourceViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderGeneralResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListView<WorkOrder> _listView;
        private IDetailView<WorkOrder> _detailView;
        private ISearchView<WorkOrder> _searchView;
        private TestWorkOrderGeneralResourceView _target;

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

            _target = new TestWorkOrderGeneralResourceViewBuilder()
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

        #region Properties

        [TestMethod]
        public void TestListViewPropertyReturnsListView()
        {
            Assert.AreSame(_listView, _target.ListView);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDetailViewPropertyReturnsDetailView()
        {
            Assert.AreSame(_detailView, _target.DetailView);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSearchViewPropertyReturnsSearchView()
        {
            Assert.AreSame(_searchView, _target.SearchView);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestPhasePropertyDenotesGeneral()
        {
            Assert.AreEqual(WorkOrderPhase.General, _target.Phase);

            _mocks.ReplayAll();
        }

        #endregion
    }

    internal class TestWorkOrderGeneralResourceViewBuilder : TestDataBuilder<TestWorkOrderGeneralResourceView>
    {
        #region Private Members

        private IListView<WorkOrder> _listView;
        private IDetailView<WorkOrder> _detailView;
        private ISearchView<WorkOrder> _searchView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderGeneralResourceView Build()
        {
            var obj = new TestWorkOrderGeneralResourceView();
            if (_listView != null)
                obj.SetListView(_listView);
            if (_detailView != null)
                obj.SetDetailView(_detailView);
            if (_searchView != null)
                obj.SetSearchView(_searchView);
            return obj;
        }

        public TestWorkOrderGeneralResourceViewBuilder WithListView(IListView<WorkOrder> view)
        {
            _listView = view;
            return this;
        }

        public TestWorkOrderGeneralResourceViewBuilder WithDetailView(IDetailView<WorkOrder> view)
        {
            _detailView = view;
            return this;
        }

        public TestWorkOrderGeneralResourceViewBuilder WithSearchView(ISearchView<WorkOrder> view)
        {
            _searchView = view;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderGeneralResourceView : WorkOrderGeneralResourceView
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

        public void SetSearchView(ISearchView<WorkOrder> view)
        {
            wosvWorkOrders = view;
        }

        #endregion
    }
}
