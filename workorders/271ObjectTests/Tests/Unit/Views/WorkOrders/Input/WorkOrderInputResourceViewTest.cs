using System;
using LINQTo271.Views.WorkOrders.Input;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Input
{
    /// <summary>
    /// Summary description for WorkOrderInputResourceViewTest
    /// </summary>
    [TestClass]
    public class WorkOrderInputResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private IWorkOrderDetailView _detailView;
        private IListView<WorkOrder> _listView;
        private TestWorkOrderInputResourceView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _detailView)
                .DynamicMock(out _listView);

            _target = new TestWorkOrderInputResourceViewBuilder()
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
        public void TestBackToListButtonPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.BackToListButton);
        }

        [TestMethod]
        public void TestDetailViewPropertyReturnsDetailView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_detailView, _target.DetailView);
        }

        [TestMethod]
        public void TestListViewPropertyReturnsListView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_listView, _target.ListView);
        }

        [TestMethod]
        public void TestSearchViewPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.SearchView);
        }

        [TestMethod]
        public void TestPhasePropertyDenotesInput()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Input, _target.Phase);
        }

        [TestMethod]
        public void TestSetViewModeShowsListViewDetailViewAndMenuButtonWhenNewModeIsDetail()
        {
            using (_mocks.Record())
            {
                _listView.Visible = true;
                _detailView.Visible = true;
            }

            using (_mocks.Playback())
            {
                _target.SetViewMode(ResourceViewMode.Detail);
            }
        }

        [TestMethod]
        public void TestSetViewModeHidesListViewDetailViewAndMenuButtonWhenNewModeIsNotDetail()
        {
            foreach (ResourceViewMode mode in Enum.GetValues(typeof(ResourceViewMode)))
            {
                if (mode == ResourceViewMode.Detail)
                    continue;

                using (_mocks.Record())
                {
                    _listView.Visible = false;
                    _detailView.Visible = false;
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
    }

    internal class TestWorkOrderInputResourceViewBuilder : TestDataBuilder<TestWorkOrderInputResourceView>
    {
        #region Private Members

        private IWorkOrderDetailView _detailView;
        private IListView<WorkOrder> _listView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderInputResourceView Build()
        {
            var view = new TestWorkOrderInputResourceView();
            if (_detailView != null)
                view.SetDetailView(_detailView);
            if (_listView != null)
                view.SetListView(_listView);
            return view;
        }

        public TestWorkOrderInputResourceViewBuilder WithDetailView(IWorkOrderDetailView detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestWorkOrderInputResourceViewBuilder WithListView(IListView<WorkOrder> view)
        {
            _listView = view;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderInputResourceView : WorkOrderInputResourceView
    {
        #region Exposed Methods

        public void SetDetailView(IWorkOrderDetailView detailView)
        {
            wodvWorkOrder = detailView;
        }

        public void SetListView(IListView<WorkOrder> view)
        {
            wolvWorkOrder = view;
        }

        #endregion
    }
}
