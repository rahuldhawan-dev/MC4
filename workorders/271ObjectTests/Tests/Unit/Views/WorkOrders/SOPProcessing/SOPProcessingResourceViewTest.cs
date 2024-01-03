using LINQTo271.Views.WorkOrders.SOPProcessing;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.SOPProcessing
{
    /// <summary>
    /// Summary description for SOPProcessingResourceViewTest.
    /// </summary>
    [TestClass]
    public class SOPProcessingResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestSOPProcessingResourceView _target;
        private IButton _btnBackToList;
        private IListView<WorkOrder> _listView;
        private IDetailView<WorkOrder> _detailView;
        private ISearchView<WorkOrder> _searchView;


        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _btnBackToList = _mocks.DynamicMock<IButton>();
            _listView = _mocks.DynamicMock<IListView<WorkOrder>>();
            _detailView = _mocks.DynamicMock<IDetailView<WorkOrder>>();
            _searchView = _mocks.DynamicMock<ISearchView<WorkOrder>>();
            _target =
                new TestSOPProcessingResourceViewBuilder()
                    .WithBackToListButton(_btnBackToList)
                    .WithListView(_listView)
                    .WithDetailView(_detailView)
                    .WithSearchView(_searchView);
        }

        #endregion

        [TestMethod]
        public void TestListViewPropertyReturnsListView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_listView, _target.ListView);
        }

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
        public void TestBackToListButtonPropertyReturnsBackToListButton()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_btnBackToList, _target.BackToListButton);
        }

        [TestMethod]
        public void TestPhasePropertyDenotesFinalization()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Finalization, _target.Phase);
        }

    }

    internal class TestSOPProcessingResourceViewBuilder : TestDataBuilder<TestSOPProcessingResourceView>
    {
        #region Private Members

        private IButton _btnBackToList;
        private IListView<WorkOrder> _listView;
        private IDetailView<WorkOrder> _detailView;
        private ISearchView<WorkOrder> _searchView;

        #endregion

        #region Exposed Methods

        public override TestSOPProcessingResourceView Build()
        {
            var obj = new TestSOPProcessingResourceView();
            if (_btnBackToList != null)
                obj.SetBackToListButton(_btnBackToList);
            if (_listView != null)
                obj.SetListView(_listView);
            if (_detailView != null)
                obj.SetDetailView(_detailView);
            if (_searchView != null)
                obj.SetSearchView(_searchView);
            return obj;
        }

        public TestSOPProcessingResourceViewBuilder WithBackToListButton(IButton btn)
        {
            _btnBackToList = btn;
            return this;
        }

        public TestSOPProcessingResourceViewBuilder WithListView(IListView<WorkOrder> listView)
        {
            _listView = listView;
            return this;
        }

        public TestSOPProcessingResourceViewBuilder WithDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestSOPProcessingResourceViewBuilder WithSearchView(ISearchView<WorkOrder> searchView)
        {
            _searchView = searchView;
            return this;
        }
        #endregion
    }

    internal class TestSOPProcessingResourceView : SOPProcessingResourceView
    {
        #region Exposed Methods

        public void SetBackToListButton(IButton btn)
        {
            btnBackToList = btn;
        }

        public void SetListView(IListView<WorkOrder> listView)
        {
            wolvWorkOrders = listView;
        }

        public void SetDetailView(IDetailView<WorkOrder> detailView)
        {
            wodvWorkOrder = detailView;
        }

        public void SetSearchView(ISearchView<WorkOrder> searchView)
        {
            wosvWorkOrders = searchView;
        }

        #endregion
    }
}
