using System;
using LINQTo271.Views.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.Abstract
{
    /// <summary>
    /// Summary description for WorkOrderInputResourceViewTest
    /// </summary>
    [TestClass]
    public class WorkOrdersResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private IButton _btnBackToList;
        private IDetailView<WorkOrder> _detailView;
        private IListView<WorkOrder> _listView;
        private ISearchView<WorkOrder> _searchView;
        private TestWorkOrderResourceView _target;

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
                new TestWorkOrderResourceViewBuilder()
                    .WithListView(_listView)
                    .WithDetailView(_detailView)
                    .WithSearchView(_searchView)
                    .WithBackToListButton(_btnBackToList);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestSetViewModeMakesBackToListButtonVisibleWhenModeIsNotList()
        {
            foreach (ResourceViewMode mode in Enum.GetValues(typeof(ResourceViewMode)))
            {
                if (mode == ResourceViewMode.List)
                    continue;

                using (_mocks.Record())
                {
                _btnBackToList.Visible = true;
                }

                using (_mocks.Playback())
                {
                    _target.SetViewMode(mode);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSetViewModeMakesBackToListButtonInvisibleWhenModeIsList()
        {
            using (_mocks.Record())
            {
                _btnBackToList.Visible = false;
            }

            using (_mocks.Playback())
            {
                _target.SetViewMode(ResourceViewMode.List);
            }
        }
    }

    internal class TestWorkOrderResourceViewBuilder : TestDataBuilder<TestWorkOrderResourceView>
    {
        #region Private Members

        private IButton _btnBackToList;
        private IDetailView<WorkOrder> _detailView;
        private IListView<WorkOrder> _listView;
        private ISearchView<WorkOrder> _searchView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderResourceView Build()
        {
            var view = new TestWorkOrderResourceView();
            if (_btnBackToList != null)
                view.SetbtnBackToList(_btnBackToList);
            if (_detailView != null)
                view.SetDetailView(_detailView);
            if (_listView != null)
                view.SetListView(_listView);
            if (_searchView != null)
                view.SetSearchView(_searchView);
            return view;
        }

        public TestWorkOrderResourceViewBuilder WithDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestWorkOrderResourceViewBuilder WithListView(IListView<WorkOrder> listView)
        {
            _listView = listView;
            return this;
        }

        public TestWorkOrderResourceViewBuilder WithSearchView(ISearchView<WorkOrder> searchView)
        {
            _searchView = searchView;
            return this;
        }

        public TestWorkOrderResourceViewBuilder WithBackToListButton(IButton newButton)
        {
            _btnBackToList = newButton;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderResourceView : WorkOrderResourceView
    {
        #region Private Members

        private IListView<WorkOrder> _listView;
        private IDetailView<WorkOrder> _detailView;
        private ISearchView<WorkOrder> _searchView;
        private WorkOrderPhase _phase;

        #endregion

        #region Properties

        public override IListView<WorkOrder> ListView
        {
            get { return _listView; }
        }

        public override IDetailView<WorkOrder> DetailView
        {
            get { return _detailView; }
        }

        public override ISearchView<WorkOrder> SearchView
        {
            get { return _searchView; }
        }

        public override WorkOrderPhase Phase
        {
            get { return _phase; }
        }

        #endregion

        #region Delegates

        internal delegate void OnDisposeHandler(WorkOrderResourceView view);

	    #endregion

        #region Exposed Methods

        public void SetListView(IListView<WorkOrder> listView)
        {
            _listView = listView;
        }

        public void SetDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
        }

        public void SetSearchView(ISearchView<WorkOrder> searchView)
        {
            _searchView = searchView;
        }

        public void SetbtnBackToList(IButton newButton)
        {
            btnBackToList = newButton;
        }

        public void SetPhase(WorkOrderPhase phase)
        {
            _phase = phase;
        }

        #endregion
    }
}
