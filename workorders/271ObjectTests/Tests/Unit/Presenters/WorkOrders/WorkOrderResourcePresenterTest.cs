using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LINQTo271;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.WorkOrders;
using WorkOrders.Views.WorkOrders;
using _271ObjectTests.Tests.Unit.Model;

namespace _271ObjectTests.Tests.Unit.Presenters.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrdersResourcePresenterTest
    /// </summary>
    [TestClass]
    public class WorkOrderResourcePresenterTest
    {
        #region Private Members

        private MockRepository _mocks;
        private TestWorkOrderResourcePresenter _target;
        private ISecurityService _securityService;
        private IRepository<WorkOrder> _repository;
        private IWorkOrderResourceView _view;
        private IWorkOrderApprovalDetailView _detailView;
        private IWorkOrderListView _listView;
        private IWorkOrderSchedulingListView _schedulingListView;
        private IWorkOrderPrePlanningListView _prePlanningListView;
        private IWorkOrderSearchView _searchView;
        private IServer _iServer;

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void WorkOrdersResourcePresenterTestInitialize()
        {
            _mocks = new MockRepository();

            _mocks
                .DynamicMock(out _repository)
                .DynamicMock(out _schedulingListView)
                .DynamicMock(out _prePlanningListView)
                .DynamicMock(out _view)
                .DynamicMock(out _detailView)
                .DynamicMock(out _listView)
                .DynamicMock(out _searchView)
                .DynamicMock(out _iServer)
                .DynamicMock(out _securityService);

            _target = new TestWorkOrderResourcePresenterBuilder(_view, _repository)
                .WithDetailView(_detailView)
                .WithListView(_listView)
                .WithSearchView(_searchView)
                .WithSecurityService(_securityService);

            SetupResult.For(_view.IServer).Return(_iServer);
        }

        [TestCleanup]
        public void WorkOrdersResourcePresenterTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        #region Event Handler Tests

        #region Page Events

        [TestMethod]
        public void TestOnViewLoadedWiresListViewAssignClickedEventHandlerWhenViewIsScheduling()
        {
            _listView = _schedulingListView;
            _target = new TestWorkOrderResourcePresenterBuilder(_view, _repository)
                .WithDetailView(_detailView)
                .WithListView(_listView)
                .WithSearchView(_searchView)
                .WithSecurityService(_securityService);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Scheduling);
                _schedulingListView.AssignClicked += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestOnViewLoadedDoesNotWireListViewAssignClickedWhenViewIsNotSchedulingOrPrePlanning()
        {
            _listView = _schedulingListView;
            _target = new TestWorkOrderResourcePresenterBuilder(_view, _repository)
                .WithDetailView(_detailView)
                .WithListView(_listView)
                .WithSearchView(_searchView)
                .WithSecurityService(_securityService);

            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phase == WorkOrderPhase.Scheduling || phase == WorkOrderPhase.PrePlanning)
                    continue;

                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(phase);
                    DoNotExpect.Call(() =>
                                     _schedulingListView.AssignClicked += null);
                    LastCall.IgnoreArguments();
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnViewInitalizedSetsViewModeToSearchByDefaultWhenViewIsPrePlanning()
        {
            SetupResult.For(_view.Phase).Return(WorkOrderPhase.PrePlanning);

            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.Search);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestOnViewInitalizedSetsViewModeToSearchByDefaultWhenViewIsPlanning()
        {
            SetupResult.For(_view.Phase).Return(WorkOrderPhase.Planning);

            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.Search);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestOnViewInitalizedSetsViewModeToSearchByDefaultWhenViewIsScheduling()
        {
            SetupResult.For(_view.Phase).Return(WorkOrderPhase.Scheduling);

            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.Search);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestOnViewInitializedSetsViewModeToSearchByDefaultWhenViewIsFinalization()
        {
            SetupResult.For(_view.Phase).Return(WorkOrderPhase.Finalization);

            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.Search);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestOnViewInitializedSetsViewModeToSearchByDefaultWhenViewIsApproval()
        {
            SetupResult.For(_view.Phase).Return(WorkOrderPhase.Approval);

            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.Search);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestOnViewInitializedSetsViewModeToSearchByDefaultWhenViewIsStockApproval()
        {
            SetupResult.For(_view.Phase).Return(WorkOrderPhase.StockApproval);

            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.Search);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestOnViewInitializedSetsViewModeToDetaliByDefaultWhenViewIsInput()
        {
            SetupResult.For(_view.Phase).Return(WorkOrderPhase.Input);

            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.Detail);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestOnViewInitalizedSetsViewModeToSearchByDefaultWhenViewIsGeneralSearchAndEdit()
        {
            SetupResult.For(_view.Phase).Return(WorkOrderPhase.General);

            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.Search);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        #endregion

        #region List View Events

        [TestMethod]
        public void TestListViewAssignCommandRedirectsViewToGeneratedCrewAssignmentLink()
        {
            _listView = _schedulingListView;
            _target = new TestWorkOrderResourcePresenterBuilder(_view, _repository)
                .WithDetailView(_detailView)
                .WithListView(_listView)
                .WithSearchView(_searchView)
                .WithSecurityService(_securityService);
            
            var date = DateTime.Now.Date;
            var crewID = 1;
            var args = new WorkOrderAssignmentEventArgs(crewID, date, null);
            var toEncode =
                String.Format(WorkOrderResourcePresenter.CREW_VIEW_DATA_FORMAT,
                    crewID, date.ToString("yyyy-MM-dd"));
            var redirectTo =
                String.Format(WorkOrderResourcePresenter.CREW_VIEW_URL_FORMAT,
                    toEncode);

            SetupResult.For(_view.Phase).Return(WorkOrderPhase.Scheduling);

            using (_mocks.Record())
            {
                _view.Redirect(redirectTo);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_listView, "AssignClicked");
                eventRaiser.Raise(null, args);
            }
        }

        [TestMethod]
        public void TestListViewOfficeAssignCommandSetsListViewData()
        {
            _listView = _prePlanningListView;
            _target = new TestWorkOrderResourcePresenterBuilder(_view, _repository)
                .WithDetailView(_detailView)
                .WithListView(_listView)
                .WithSearchView(_searchView)
                .WithSecurityService(_securityService);

            var args = new OfficeAssignmentEventArgs(1, 1, DateTime.Now);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.PrePlanning);
                // TODO: actually test the call to SetListViewData here
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_listView, "AssignClicked");
                eventRaiser.Raise(null, args);
            }
        }
        
        [TestMethod]
        public void TestListViewContractorAssignCommandSetsListViewData()
        {
            _listView = _prePlanningListView;
            _target = new TestWorkOrderResourcePresenterBuilder(_view, _repository)
                .WithDetailView(_detailView)
                .WithListView(_listView)
                .WithSearchView(_searchView)
                .WithSecurityService(_securityService);

            var args = new OfficeContractorAssignmentEventArgs(1, 1, DateTime.Now);

            using(_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.PrePlanning);
                // TODO: actually test the call to SetListViewData here
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_listView,
                    "ContractorAssignClicked");
                eventRaiser.Raise(null, args);
            }
        }

        #endregion

        #region Detail View Events

        [TestMethod]
        public void TestDetailViewDiscardChangesEventRedirectsToMenuWhenPhaseIsInputAndCurrentModeIsInsert()
        {
            SetupResult.For(_view.Phase).Return(WorkOrderPhase.Input);
            SetupResult.For(_detailView.CurrentMode).Return(
                DetailViewMode.Insert);

            using (_mocks.Record())
            {
                _view.Redirect(Configuration.MENU_URL);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_detailView,
                        "DiscardChangesClicked");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewDiscardChangesEventDoesNotRedirectWhenPhaseIsNotInput()
        {
            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phase == WorkOrderPhase.Input) continue;

                _listView = (phase == WorkOrderPhase.Scheduling)
                                ? (IWorkOrderListView)_schedulingListView
                                : _prePlanningListView;
                _target = new TestWorkOrderResourcePresenterBuilder(_view, _repository)
                    .WithDetailView(_detailView)
                    .WithListView(_listView)
                    .WithSearchView(_searchView)
                    .WithSecurityService(_securityService);
            
                // should not matter which mode
                foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
                {
                    SetupResult.For(_view.Phase).Return(phase);
                    SetupResult.For(_detailView.CurrentMode).Return(mode);

                    using (_mocks.Record())
                    {
                        DoNotExpect.Call(
                            () => _view.Redirect(Configuration.MENU_URL));
                    }

                    using (_mocks.Playback())
                    {
                        _target.OnViewLoaded();
                        IEventRaiser eventRaiser =
                            new EventRaiser((IMockedObject)_detailView,
                                "DiscardChangesClicked");

                        eventRaiser.Raise(null, EventArgs.Empty);
                    }

                    _mocks.VerifyAll();
                    _mocks.BackToRecordAll();
                }
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDetailViewDiscardChangesDoesNotRedirectWhenCurrentModeIsNotInsert()
        {
            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                if (mode == DetailViewMode.Insert) continue;

                // should not matter which phase
                foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
                {
                    _listView = (phase == WorkOrderPhase.Scheduling)
                                ? (IWorkOrderListView)_schedulingListView : _prePlanningListView;
                    _target = new TestWorkOrderResourcePresenterBuilder(_view, _repository)
                        .WithDetailView(_detailView)
                        .WithListView(_listView)
                        .WithSearchView(_searchView)
                        .WithSecurityService(_securityService);

                    using (_mocks.Record())
                    {
                        SetupResult.For(_view.Phase).Return(phase);
                        SetupResult.For(_detailView.CurrentMode).Return(mode);

                        DoNotExpect.Call(
                            () => _view.Redirect(Configuration.MENU_URL));
                    }

                    using (_mocks.Playback())
                    {
                        _target.OnViewLoaded();
                        IEventRaiser eventRaiser =
                            new EventRaiser((IMockedObject)_detailView,
                                "DiscardChangesClicked");

                        eventRaiser.Raise(null, EventArgs.Empty);
                    }

                    _mocks.VerifyAll();
                    _mocks.BackToRecordAll();
                }
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDetailViewEditEventSetsRepositoryDataKeyFromDetailViewDataKeyWhenPhaseIsInput()
        {
            var key = new object();

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(WorkOrderPhase.Input);
                SetupResult.For(_detailView.CurrentDataKey).Return(key);
                _repository.SetSelectedDataKey(key);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_detailView, "EditClicked");

                eventRaiser.Raise(null, EventArgs.Empty);
             }
        }

        [TestMethod]
        public void TestDetailViewEditEventDoesNotSetRepositoryDataKeyWhenPhaseIsNotInput()
        {
            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phase == WorkOrderPhase.Input) continue;

                _listView = (phase == WorkOrderPhase.Scheduling)
                            ? (IWorkOrderListView)_schedulingListView : _prePlanningListView;
                _target = new TestWorkOrderResourcePresenterBuilder(_view, _repository)
                    .WithDetailView(_detailView)
                    .WithListView(_listView)
                    .WithSearchView(_searchView)
                    .WithSecurityService(_securityService);

                var key = new object();

                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(phase);
                    SetupResult.For(_detailView.CurrentDataKey).Return(key);
                    DoNotExpect.Call(() => _repository.SetSelectedDataKey(key));
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    IEventRaiser eventRaiser =
                        new EventRaiser((IMockedObject)_detailView, "EditClicked");
                    eventRaiser.Raise(null, EventArgs.Empty);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDetailViewUpdatingSetsViewModeToListWhenPhaseIsStockApproval()
        {
            var order = new TestWorkOrderBuilder();
            var phase = WorkOrderPhase.StockApproval;

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(phase);
                _view.SetViewMode(ResourceViewMode.List);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                new EventRaiser((IMockedObject)_detailView, "Updating")
                    .Raise(null,
                        new EntityEventArgs<WorkOrder>(order));
            }
        }

        [TestMethod]
        public void TestDetailViewUpdatingSetsViewModeToListWhenPhaseIsOrcomOrderApproval()
        {
            var order = new TestWorkOrderBuilder();
            var phase = WorkOrderPhase.OrcomOrderApproval;

            using (_mocks.Record())
            {
                SetupResult.For(_view.Phase).Return(phase);
                _view.SetViewMode(ResourceViewMode.List);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                new EventRaiser((IMockedObject)_detailView, "Updating")
                    .Raise(null,
                        new EntityEventArgs<WorkOrder>(order));
            }
        }

        [TestMethod]
        public void TestDetailViewUpdatingDoesNotSetViewModeToListWhenPhaseIsNotStockApproval()
        {
            var order = new TestWorkOrderBuilder();
            foreach(WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phase == WorkOrderPhase.StockApproval || phase == WorkOrderPhase.OrcomOrderApproval) continue;

                _listView = (phase == WorkOrderPhase.Scheduling)
                            ? (IWorkOrderListView)_schedulingListView : _prePlanningListView;
                _target = new TestWorkOrderResourcePresenterBuilder(_view, _repository)
                    .WithDetailView(_detailView)
                    .WithListView(_listView)
                    .WithSearchView(_searchView)
                    .WithSecurityService(_securityService);

                using (_mocks.Record())
                {
                    SetupResult.For(_view.Phase).Return(phase);
                    DoNotExpect.Call(
                        () => _view.SetViewMode(ResourceViewMode.List));
                }

                using(_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    IEventRaiser eventRaiser =
                        new EventRaiser((IMockedObject)_detailView, "Updating");
                    eventRaiser.Raise(null,
                        new EntityEventArgs<WorkOrder>(order));
                }
                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDetailViewUpdatingFallsBackToListViewIfRepositoryCurrentIndexDoesNotGetSet()
        {
            var order = new TestWorkOrderBuilder();
            var phase = WorkOrderPhase.Approval;

            using (_mocks.Record())
            {
                SetupResult.For(_repository.CurrentIndex).Return(0);
                SetupResult.For(_view.Phase).Return(phase);
                DoNotExpect.Call(() => _view.SetViewMode(ResourceViewMode.List));
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                new EventRaiser((IMockedObject)_detailView, "Updating")
                    .Raise(null,
                        new EntityEventArgs<WorkOrder>(order));
            }
        }
        
        [TestMethod]
        public void TestDetailViewUpdatingDoesNotFallBackToListViewIfRepositoryCurrentIndexDoesNotGetSet()
        {
            var order = new TestWorkOrderBuilder();
            var phase = WorkOrderPhase.Approval;

            using (_mocks.Record())
            {
                SetupResult.For(_repository.CurrentIndex).Return(-1);
                SetupResult.For(_view.Phase).Return(phase);
                _view.SetViewMode(ResourceViewMode.List);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                new EventRaiser((IMockedObject)_detailView, "Updating")
                    .Raise(null,
                        new EntityEventArgs<WorkOrder>(order));
            }
        }

        #endregion

        #region Search View Events

        [TestMethod]
        public void TestSearchViewSearchClickedDisplaysErrorWhenSearchHasNoResultsAndPhaseIsNotSchedulingOrGeneral()
        {
            var data = new List<WorkOrder>();
            Expression<Func<WorkOrder, bool>> expr = w => false;

            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phase == WorkOrderPhase.Scheduling || phase == WorkOrderPhase.General) continue;

                using (_mocks.Record())
                {
                    SetupResult.For(_searchView.Phase).Return(phase);
                    SetupResult.For(_searchView.GenerateExpression())
                        .Return(expr);
                    SetupResult.For(_searchView.OperatingCenterID).Return(0);
                    SetupResult.For(_repository.GetFilteredSortedData(expr, null))
                        .Return(data);
                    _searchView
                        .DisplaySearchError(WorkOrderResourcePresenter.NO_RECORDS_FOUND_ERROR);
                }

                using (_mocks.Playback())
                {
                    _target.OnViewLoaded();
                    IEventRaiser eventRaiser =
                        new EventRaiser((IMockedObject)_searchView,
                            "SearchClicked");
                    eventRaiser.Raise(null, EventArgs.Empty);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSearchViewSearchClickedDisplaysErrorWhenSearchIsGreaterThanResultLimit()
        {
            var data = _mocks.DynamicMock<IList<WorkOrder>>();
            Expression<Func<WorkOrder, bool>> expr = w => false;

            using (_mocks.Record())
            {
                SetupResult.For(_searchView.GenerateExpression())
                    .Return(expr);
                SetupResult.For(_repository.GetFilteredSortedData(expr, null))
                    .Return(data);
                SetupResult.For(_repository.GetCountForExpression(expr)).Return(WorkOrderResourcePresenter.SEARCH_RESULT_LIMIT + 1);
                _searchView
                    .DisplaySearchError(string.Format(WorkOrderResourcePresenter.OVER_SEARCH_RESULT_LIMIT_ERROR, WorkOrderResourcePresenter.SEARCH_RESULT_LIMIT));
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_searchView,
                        "SearchClicked");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        // NOTE: When we return to jumping to a single record result, these will come in handy.
        //[TestMethod]
        //public void TestSearchViewSearchClickedDoesNotDisplayErrorWhenSearchHasNoResultAndPhaseIsSchedulingOrGeneral()
        //{
        //    var data = new List<WorkOrder>();
        //    Expression<Func<WorkOrder, bool>> expr = w => false;
        //    var phases = new[] {
        //        WorkOrderPhase.Scheduling, WorkOrderPhase.General
        //    };

        //    foreach (var phase in phases)
        //    {
        //        using (_mocks.Record())
        //        {
        //            SetupResult.For(_searchView.Phase).Return(phase);
        //            SetupResult.For(_searchView.GenerateExpression()).Return(
        //                expr);
        //            SetupResult.For(_repository.GetFilteredSortedData(expr, null))
        //                .
        //                Return(data);
        //            DoNotExpect.Call(
        //                () =>
        //                _searchView.DisplaySearchError(
        //                    WorkOrderResourcePresenter.NO_RECORDS_FOUND_ERROR));
        //        }

        //        using (_mocks.Playback())
        //        {
        //            _target.OnViewLoaded();
        //            IEventRaiser eventRaiser =
        //                new EventRaiser((IMockedObject)_searchView,
        //                    "SearchClicked");
        //            eventRaiser.Raise(null, EventArgs.Empty);
        //        }

        //        _mocks.VerifyAll();
        //        _mocks.BackToRecordAll();
        //    }
            
        //    _mocks.ReplayAll();
        //}

        //[TestMethod]
        //public void TestSearchViewSearchClickedDisplaysSignleRecordIfSearchHasSingleResultAndPhaseIsNotSchedulingOrGeneral()
        //{
        //    var expectedID = 1;
        //    var data = new List<WorkOrder> {
        //        new WorkOrder { WorkOrderID = expectedID }
        //    };
        //    Expression<Func<WorkOrder, bool>> expr = w => false;

        //    foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
        //    {
        //        if (phase == WorkOrderPhase.Scheduling || phase == WorkOrderPhase.General) continue;

        //        using (_mocks.Record())
        //        {
        //            SetupResult.For(_searchView.Phase).Return(phase);
        //            SetupResult.For(_searchView.GenerateExpression()).Return(expr);
        //            SetupResult.For(_repository.GetFilteredSortedData(expr, null)).
        //                Return(data);
        //            _repository.SetSelectedDataKey(expectedID);
        //            _view.SetViewMode(ResourceViewMode.Detail);
        //        }

        //        using (_mocks.Playback())
        //        {
        //            _target.OnViewLoaded();
        //            IEventRaiser eventRaiser =
        //                new EventRaiser((IMockedObject)_searchView, "SearchClicked");
        //            eventRaiser.Raise(null, EventArgs.Empty);
        //        }

        //        _mocks.VerifyAll();
        //        _mocks.BackToRecordAll();
        //    }

        //    _mocks.ReplayAll();
        //}

        //[TestMethod]
        //public void TestSearchViewSearchClickedDoesNotDisplaySingleRecordIfSearchHasSingleResultAndPhaseIsSchedulingOrGeneral()
        //{
        //    var expectedID = 1;
        //    var data = new List<WorkOrder> {
        //        new WorkOrder {
        //            WorkOrderID = expectedID
        //        }
        //    };
        //    Expression<Func<WorkOrder, bool>> expr = w => false;
        //    var phases = new[] {
        //        WorkOrderPhase.Scheduling, WorkOrderPhase.General
        //    };

        //    foreach (var phase in phases)
        //    {
        //        using (_mocks.Record())
        //        {
        //            SetupResult.For(_searchView.Phase).Return(phase);
        //            SetupResult.For(_searchView.GenerateExpression()).Return(
        //                expr);
        //            SetupResult.For(_repository.GetFilteredSortedData(expr, null))
        //                .Return(data);

        //            DoNotExpect.Call(
        //                () => _repository.SetSelectedDataKey(expectedID));
        //            DoNotExpect.Call(
        //                () => _view.SetViewMode(ResourceViewMode.Detail));
        //        }

        //        using (_mocks.Playback())
        //        {
        //            _target.OnViewLoaded();
        //            IEventRaiser eventRaiser =
        //                new EventRaiser((IMockedObject)_searchView,
        //                    "SearchClicked");
        //            eventRaiser.Raise(null, EventArgs.Empty);
        //        }

        //        _mocks.VerifyAll();
        //        _mocks.BackToRecordAll();
        //    }

        //    _mocks.ReplayAll();
        //}

        //[TestMethod]
        //public void TestSearchViewSearchClickedMerelySetsListViewModeToListWhenPhaseIsNotSchedulingOrGeneralAndMoreThanOneResult()
        //{
        //    var data = new List<WorkOrder> {
        //        new WorkOrder(), new WorkOrder()
        //    };

            
        //}

        #endregion

        #endregion

        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersResourcePresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersResourcePresenter<WorkOrder>),
                "ResourcePresenters in this project should inherit from WorkOrdersResourcePresenter, lest bad tings happen.");
        }

        [TestMethod]
        public void TestCheckUserSecurityThrowsExceptionForNonAdminWhenPhaseIsSchedulingOrApproval()
        {
            var user = _mocks.DynamicMock<IUser>();

            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                using (_mocks.Record())
                {
                    SetupResult.For(_securityService.CurrentUser).Return(user);
                    SetupResult.For(_securityService.UserHasAccess).Return(true);
                    SetupResult.For(_securityService.IsAdmin).Return(false);
                    SetupResult.For(_view.Phase).Return(phase);
                }
                using (_mocks.Playback())
                {
                    if (phase == WorkOrderPhase.Scheduling || phase == WorkOrderPhase.Approval || phase == WorkOrderPhase.StockApproval)
                        MyAssert.Throws(() => _target.ExposedCheckUserSecurity(), typeof(UnauthorizedAccessException));
                    else
                        MyAssert.DoesNotThrow(() => _target.ExposedCheckUserSecurity(), typeof(UnauthorizedAccessException));
                }
                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestCheckUserSecurityDoesNotThrowExceptionForAdminWhenPhaseIsSchedulingOrApproval()
        {
            var user = _mocks.DynamicMock<IUser>();

            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                using (_mocks.Record())
                {
                    SetupResult.For(_securityService.CurrentUser).Return(user);
                    SetupResult.For(_securityService.UserHasAccess).Return(true);
                    SetupResult.For(_securityService.IsAdmin).Return(true);
                    SetupResult.For(_view.Phase).Return(phase);
                }
                using (_mocks.Playback())
                {
                    MyAssert.DoesNotThrow(() => _target.ExposedCheckUserSecurity(), typeof(UnauthorizedAccessException));
                }
                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }
            _mocks.ReplayAll();
        }
    }

    internal class TestWorkOrderResourcePresenterBuilder : TestDataBuilder<TestWorkOrderResourcePresenter>
    {
        #region Private Members

        private readonly IWorkOrderResourceView _view;
        private readonly IRepository<WorkOrder> _repository;
        private IDetailView<WorkOrder> _detailView;
        private IListView<WorkOrder> _listView;
        private ISearchView<WorkOrder> _searchView;
        private ISecurityService _securityService;

        #endregion

        #region Constructors

        private TestWorkOrderResourcePresenterBuilder()
        {
        }

        public TestWorkOrderResourcePresenterBuilder(IWorkOrderResourceView view, IRepository<WorkOrder> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderResourcePresenter Build()
        {
            var presenter = new TestWorkOrderResourcePresenter(_view, _repository);
            if (_detailView != null)
                presenter.DetailView = _detailView;
            if (_listView != null)
                presenter.ListView = _listView;
            if (_searchView != null)
                presenter.SearchView = _searchView;
            if (_securityService!=null)
                presenter.SetSecurityService(_securityService);
            return presenter;
        }

        public TestWorkOrderResourcePresenterBuilder WithDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestWorkOrderResourcePresenterBuilder WithListView(IListView<WorkOrder> listView)
        {
            _listView = listView;
            return this;
        }

        public TestWorkOrderResourcePresenterBuilder WithSearchView(ISearchView<WorkOrder> searchView)
        {
            _searchView = searchView;
            return this;
        }

        public TestWorkOrderResourcePresenterBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderResourcePresenter : WorkOrderResourcePresenter
    {
        #region Constructors

        public TestWorkOrderResourcePresenter(IResourceView view, IRepository<WorkOrder> repository) : base(view, repository)
        {
        }

        #endregion

        #region Exposed Methods

        public void ExposedCheckUserSecurity()
        {
            CheckUserSecurity();
        }

        public void SetSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        #endregion
    }
}