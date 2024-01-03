#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Permissions;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Views;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;
#endif
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Core.WebFormsTest.Presenter
{
    /// <summary>
    /// Summary description for EntityResourcePresenterTest
    /// </summary>
    [TestClass]
    public class ResourcePresenterTest
    {
#if DEBUG

        #region Private Members

        private ISecurityService _securityService;
        private MockRepository _mocks;
        private IRepository<Employee> _repository;
        private IResourceView _view;
        private IEmployeeDetailView _detailView;
        private IDetailPresenter<Employee> _detailPresenter;
        private IListView<Employee> _listView;
        private IListPresenter<Employee> _listPresenter;
        private ISearchView<Employee> _searchView;
        private ISearchPresenter<Employee> _searchPresenter;
        private MockedResourcePresenter _target;

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public void EntityResourcePresenterTestInitialize()
        {
            _mocks = new MockRepository();
            _mocks
               .DynamicMock(out _repository)
               .DynamicMock(out _view)
               .DynamicMock(out _detailView)
               .DynamicMock(out _detailPresenter)
               .DynamicMock(out _listView)
               .DynamicMock(out _listPresenter)
               .DynamicMock(out _searchView)
               .DynamicMock(out _searchPresenter)
               .DynamicMock(out _securityService);

            SetupResult.For(_detailView.Presenter).Return(_detailPresenter);
            SetupResult.For(_listView.Presenter).Return(_listPresenter);
            SetupResult.For(_searchView.Presenter).Return(_searchPresenter);

            _target = new MockedResourcePresenter(_view, _repository) {
                ListView = _listView,
                DetailView = _detailView,
                SearchView = _searchView
            };

            _target.SetSecurityService(_securityService);
        }

        [TestCleanup]
        public void EntityResourcePresenterTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        #region Private Members

        private void AccountForOnViewLoaded()
        {
            _listView.UserControlLoaded += null;
            LastCall.IgnoreArguments();

            _listView.CreateClicked += null;
            LastCall.IgnoreArguments();

            _listView.SelectedIndexChanged += null;
            LastCall.IgnoreArguments();

            _listView.Sorting += null;
            LastCall.IgnoreArguments();

            _listView.VisibilityChanged += null;
            LastCall.IgnoreArguments();

            _detailView.EditClicked += null;
            LastCall.IgnoreArguments();

            _detailView.Updating += null;
            LastCall.IgnoreArguments();

            _detailView.UserControlLoaded += null;
            LastCall.IgnoreArguments();

            _detailView.DiscardChangesClicked += null;
            LastCall.IgnoreArguments();

            _detailView.DeleteClicked += null;
            LastCall.IgnoreArguments();

            _searchView.UserControlLoaded += null;
            LastCall.IgnoreArguments();

            _searchView.SearchClicked += null;
            LastCall.IgnoreArguments();

            _repository.CurrentEntityChanged += null;
            LastCall.IgnoreArguments();

            _view.BackToListClicked += null;
            LastCall.IgnoreArguments();
        }

        #endregion

        #region Resource View Interactions

        [TestMethod]
        public void TestOnViewInitWiresViewPageLoadCompleteEvent()
        {
            var user = _mocks.DynamicMock<IUser>();
            var iPage = _mocks.CreateMock<IPage>();
            SetupResult.For(_view.IPage).Return(iPage);

            using (_mocks.Record())
            {
                iPage.LoadComplete = null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewInit(user);
            }
        }

        [TestMethod]
        public void TestOnViewInitCallsCheckUserSecurityMethod()
        {
            var user = _mocks.CreateMock<IUser>();
            var iPage = _mocks.DynamicMock<IPage>();
            SetupResult.For(_view.IPage).Return(iPage);

            _mocks.ReplayAll();

            _target.OnViewInit(user);

            Assert.IsTrue(_target.CheckUserSecurityCalled);
        }

        [TestMethod]
        public void TestOnViewInitInitializesSecurityServiceIfPresent()
        {
            var user = _mocks.CreateMock<IUser>();
            var iPage = _mocks.DynamicMock<IPage>();
            SetupResult.For(_view.IPage).Return(iPage);

            using (_mocks.Record())
            {
                _securityService.Init(user);
            }

            using (_mocks.Playback())
            {
                _target.OnViewInit(user);
            }
        }

        [TestMethod]
        public void TestOnViewLoadedWiresUpChildViewEventHandlers()
        {
            _mocks
               .CreateMock(out _view)
               .CreateMock(out _listView)
               .CreateMock(out _searchView)
               .CreateMock(out _detailView)
               .CreateMock(out _repository);

            _target = new MockedResourcePresenter(_view, _repository) {
                ListView = _listView,
                SearchView = _searchView,
                DetailView = _detailView
            };

            using (_mocks.Record())
            {
                AccountForOnViewLoaded();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestOnViewLoadedDoesNotThrowExceptionWhenListAndDetailViewsNotPresent()
        {
            _target = new MockedResourcePresenter(_view, _repository) {
                ListView = null,
                DetailView = null
            };

            _mocks.ReplayAll();
            MyAssert.DoesNotThrow((Action)_target.OnViewLoaded,
                "Exception thrown calling OnViewLoaded");
        }

        [TestMethod]
        public void TestOnViewInitializedSetsViewModeToViewModeListWhenSearchViewNull()
        {
            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.List);
            }

            using (_mocks.Playback())
            {
                _target.SearchView = null;
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestOnViewInitializedSetsViewModeToViewModeSearch()
        {
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
        public void TestViewLoadCompleteDoesNotThrowExceptionsWhenMenuOrListViewNotPresent()
        {
            _target = new MockedResourcePresenter(_view, _repository) {
                ListView = null,
                DetailView = null
            };

            _mocks.ReplayAll();

            _target.OnViewLoaded();
            IEventRaiser eventRaiser = new EventRaiser(
                (IMockedObject)_view,
                "LoadComplete");
            MyAssert.DoesNotThrow(() => eventRaiser.Raise(null, EventArgs.Empty),
                "Exception thrown raising view.LoadComplete");
        }

        [TestMethod]
        public void TestViewLoadCompleteCausesListViewDataBind()
        {
            SetupResult.For(_listView.Visible).Return(true);

            using (_mocks.Record())
            {
                _listView.DataBind();
            }

            using (_mocks.Playback())
            {
                // LoadComplete is a strange, strange animal
                var loadCompleteRaiser =
                    typeof(ResourcePresenter<Employee>).GetMethod(
                        "View_LoadComplete",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                loadCompleteRaiser.Invoke(_target, new object[] {
                    null, null
                });
            }
        }

        [TestMethod]
        public void TestOnViewPrerenderRedirectsViewIfCurrentModeIsRedirect()
        {
            var redirectURL = "redirect url";

            using (_mocks.Record())
            {
                SetupResult.For(_view.CurrentMode)
                           .Return(ResourceViewMode.Redirect);
                SetupResult.For(_view.RedirectURL).Return(redirectURL);
                _view.Redirect(redirectURL);
            }

            using (_mocks.Playback())
            {
                _target.OnViewPrerender();
            }
        }

        #endregion

        #region List View Interactions

        [TestMethod]
        public void TestListViewUserControlLoadedSetsPresenterRepository()
        {
            using (_mocks.Record())
            {
                _listPresenter.Repository = _repository;
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_listView,
                        "UserControlLoaded");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestListViewCreateCommandPassesNewEntityToDetailViewAndSetsInsertMode()
        {
            using (_mocks.Record())
            {
                _detailView.ShowEntity(null);
                LastCall.IgnoreArguments();
                _detailView.SetViewMode(DetailViewMode.Insert);
                _view.SetViewMode(ResourceViewMode.Detail);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_listView,
                    "CreateClicked");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestListViewSortingCommandSetListViewData()
        {
            IEnumerable<Employee> data = null;
            Expression<Func<Employee, bool>> filterExpression = o => true;
            var sortExpression = "";

            using (_mocks.Record())
            {
                SetupResult.For(_searchView.GenerateExpression()).Return(
                    filterExpression);
                SetupResult.For(
                    _repository.GetFilteredSortedData(filterExpression,
                        sortExpression)).Return(data);
                _listView.SetListData(data);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_listView,
                        "Sorting");
                eventRaiser.Raise(null, new GridViewSortEventArgs("sort", SortDirection.Ascending));
            }
        }

        [TestMethod]
        public void TestListViewSortingCommandSetListViewDataWhenSearchViewNull()
        {
            IEnumerable<Employee> data = null;
            Expression<Func<Employee, bool>> filterExpression = o => true;
            var sortExpression = "";
            _target.SearchView = null;

            using (_mocks.Record())
            {
                SetupResult.For(
                    _repository.GetFilteredSortedData(filterExpression,
                        sortExpression)).Return(data);
                _listView.SetListData(data);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_listView,
                        "Sorting");
                eventRaiser.Raise(null, new GridViewSortEventArgs("sort", SortDirection.Ascending));
            }
        }

        [TestMethod]
        public void TestListViewVisibilityChangedSetsListViewDataWhenNewVisibilityIsTrue()
        {
            IEnumerable<Employee> data = null;
            Expression<Func<Employee, bool>> filterExpression = o => true;
            var sortExpression = "";
            _target.SearchView = null;

            using (_mocks.Record())
            {
                SetupResult.For(
                    _repository.GetFilteredSortedData(filterExpression,
                        sortExpression)).Return(data);
                _listView.SetListData(data);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_listView,
                        "VisibilityChanged");
                eventRaiser.Raise(null, new VisibilityChangeEventArgs(true));
            }
        }

        [TestMethod]
        public void TestListViewVisibilityChangedDoesNothingWhenNewVisibilityIsFalse()
        {
            _mocks
               .CreateMock(out _view)
               .CreateMock(out _listView)
               .CreateMock(out _searchView)
               .CreateMock(out _detailView)
               .CreateMock(out _repository);

            _target = new MockedResourcePresenter(_view, _repository) {
                ListView = _listView,
                SearchView = _searchView,
                DetailView = _detailView
            };

            using (_mocks.Record())
            {
                AccountForOnViewLoaded();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_listView,
                        "VisibilityChanged");
                eventRaiser.Raise(null, new VisibilityChangeEventArgs(false));
            }
        }

        #endregion

        #region Detail View Interactions

        [TestMethod]
        public void TestDetailViewUserControlLoadedSetsPresenterRepository()
        {
            using (_mocks.Record())
            {
                _detailPresenter.Repository = _repository;
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_detailView,
                        "UserControlLoaded");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewDeleteClickedSetsViewModeList()
        {
            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.List);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_detailView,
                    "DeleteClicked");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewEditClickedSetsRepositorySelectedDataKeyToListViewSelectedDataKey()
        {
            var expected = 1;
            SetupResult.For(_listView.SelectedDataKey).Return(expected);

            using (_mocks.Record())
            {
                _repository.SetSelectedDataKey(expected);
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
        public void TestDetailViewEditClickedDoesNotSetRepositoryDataKeyWhenListViewDataKeyIsNull()
        {
            object expected = null;
            SetupResult.For(_listView.SelectedDataKey).Return(expected);

            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _repository.SetSelectedDataKey(expected));
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
        public void TestDetailViewEditClickedDoesNotSetRepositoryDataKeyWhenMatchesListViewDataKey()
        {
            var expected = 1;
            SetupResult.For(_listView.SelectedDataKey).Return(expected);
            SetupResult.For(_repository.SelectedDataKey).Return(expected);

            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _repository.SetSelectedDataKey(expected));
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
        public void TestDetailViewUpdatingSetsRepositorySelectedDataKeyToListViewSelectedDataKey()
        {
            var expected = 1;
            SetupResult.For(_listView.SelectedDataKey).Return(expected);

            using (_mocks.Record())
            {
                _repository.SetSelectedDataKey(expected);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_detailView, "Updating");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewUpdatingDoesNotSetRepositoryDataKeyWhenListViewDataKeyIsNull()
        {
            object expected = null;
            SetupResult.For(_listView.SelectedDataKey).Return(expected);

            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _repository.SetSelectedDataKey(expected));
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_detailView, "Updating");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewUpdatingDoesNotSetRepositoryDataKeyWhenMatchesListViewDataKey()
        {
            var expected = 1;
            SetupResult.For(_listView.SelectedDataKey).Return(expected);
            SetupResult.For(_repository.SelectedDataKey).Return(expected);

            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _repository.SetSelectedDataKey(expected));
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_detailView, "Updating");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewDiscardChangesClickedSetsViewModeToListWhenDetailViewModeIsInsert()
        {
            SetupResult.For(_detailView.CurrentMode).Return(
                DetailViewMode.Insert);

            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.List);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var discardChangesRaiser = new EventRaiser(
                    (IMockedObject)_detailView, "DiscardChangesClicked");
                discardChangesRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void
            TestDetailViewDiscardChangesClickedSetsRepositoryDataKeyFromListViewDataKeyWhenDetailViewModeIsEdit()
        {
            var expected = "foo";
            SetupResult.For(_detailView.CurrentMode).Return(
                DetailViewMode.Edit);
            SetupResult.For(_listView.SelectedDataKey).Return(expected);

            using (_mocks.Record())
            {
                _repository.SetSelectedDataKey(expected);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var discardChangesRaiser = new EventRaiser(
                    (IMockedObject)_detailView, "DiscardChangesClicked");
                discardChangesRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void
            TestDetailViewDiscardChangesClickedSetsRepositoryDataKeyFromListViewDataKeyWhenDetailViewModeIsReadOnly()
        {
            var expected = "foo";
            SetupResult.For(_detailView.CurrentMode).Return(
                DetailViewMode.ReadOnly);
            SetupResult.For(_listView.SelectedDataKey).Return(expected);

            using (_mocks.Record())
            {
                _repository.SetSelectedDataKey(expected);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var discardChangesRaiser = new EventRaiser(
                    (IMockedObject)_detailView, "DiscardChangesClicked");
                discardChangesRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewDiscardChangesClickedShowsRepositoryCurrentEntityReadOnlyWhenCurrentEntityIsNotNull()
        {
            var employee = new Employee();
            SetupResult.For(_repository.CurrentEntity).Return(employee);

            using (_mocks.Record())
            {
                _detailView.SetViewMode(DetailViewMode.ReadOnly);
                _detailView.ShowEntity(employee);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var discardChangesRaiser =
                    new EventRaiser((IMockedObject)_detailView,
                        "DiscardChangesClicked");
                discardChangesRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewDiscardChangesClickedDoesNotShowRepositoryCurrentEntityWhenCurrentEntityIsNull()
        {
            using (_mocks.Record())
            {
                DoNotExpect.Call(
                    () => _detailView.SetViewMode(DetailViewMode.ReadOnly));
                DoNotExpect.Call(() => _detailView.ShowEntity(null));
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var discardChangesRaiser =
                    new EventRaiser((IMockedObject)_detailView,
                        "DiscardChangesClicked");
                discardChangesRaiser.Raise(null, EventArgs.Empty);
            }
        }

        #endregion

        #region Search View Interactions

        [TestMethod]
        public void TestSearchViewUserControlLoadedSetsPresenterRepository()
        {
            using (_mocks.Record())
            {
                _searchPresenter.Repository = _repository;
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_searchView,
                        "UserControlLoaded");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        /* TODO: this test is now invalid
        [TestMethod]
        public void TestSearchViewSearchClickedSetsListViewDataFromRepositoryFilteredBySearchViewGeneratedExpression()
        {
            IEnumerable<Employee> data = null;
            Expression<Func<Employee, bool>> fn = o => true;

            using (_mocks.Record())
            {
                SetupResult.For(_searchView.GenerateExpression()).Return(fn);
                SetupResult.For(_repository.GetFilteredData(fn)).Return(data);
                _listView.SetListData(data);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var searchClickedRaiser =
                    new EventRaiser((IMockedObject)_searchView, "SearchClicked");
                searchClickedRaiser.Raise(null, EventArgs.Empty);
            }
        }
        */

        [TestMethod]
        public void TestSearchViewClickedPutsResourceViewInSearchMode()
        {
            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.List);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var searchClickedRaiser =
                    new EventRaiser((IMockedObject)_searchView, "SearchClicked");
                searchClickedRaiser.Raise(null, EventArgs.Empty);
            }
        }

        #endregion

        #region Repository Interactions

        [TestMethod]
        public void TestRepositoryCurrentEntityChangedSetsListViewCurrentIndex()
        {
            var expected = 1;
            SetupResult.For(_repository.CurrentIndex).Return(expected);

            using (_mocks.Record())
            {
                _listView.SelectedIndex = expected;
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser =
                    new EventRaiser((IMockedObject)_repository,
                        "CurrentEntityChanged");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }

        [TestMethod]
        public void TestSetRepositoryDoesNotThrowException()
        {
            // TODO: is this test just for coverage?  there are other ways.
            _mocks.ReplayAll();

            MyAssert.DoesNotThrow(
                () => _target.Repository = new EmployeeRepository(),
                "Setting Repository threw exception");
        }

        #endregion

#else
        [TestMethod]
        public void TestRunningInDebugMode()
        {
            Assert.Fail("The tests in this class and many others in this project must be run in DEBUG mode.");
        }

#endif
    }

#if DEBUG

    internal class MockedResourcePresenter : ResourcePresenter<Employee>
    {
        #region Private Members

        private bool _checkUserSecurityCalled;

        #endregion

        #region Properties

        public bool CheckUserSecurityCalled
        {
            get { return _checkUserSecurityCalled; }
        }

        #endregion

        #region Constructors

        public MockedResourcePresenter(IResourceView view, IRepository<Employee> repository)
            : base(view, repository) { }

        #endregion

        #region Private Methods

        protected override void CheckUserSecurity()
        {
            _checkUserSecurityCalled = true;
            base.CheckUserSecurity();
        }

        #endregion

        #region Exposed Methods

        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        #endregion
    }

#endif
}
