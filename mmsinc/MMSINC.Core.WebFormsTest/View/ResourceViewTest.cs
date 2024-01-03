using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.View;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementationTest.Model;
using Rhino.Mocks;
using StructureMap;
using Subtext.TestLibrary;
using System;
using System.Reflection;
using System.Web.Mvc;
using MMSINC.Utilities.StructureMap;

namespace MMSINC.Core.WebFormsTest.View
{
    /// <summary>
    /// Summary description for ResourceViewTest
    /// </summary>
    [TestClass]
    public class ResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private IUser _currentUser;
        private HttpSimulator _simulator;
        private ResourceView<Employee> _target;
        private IResourcePresenter<Employee> _presenter;

        #endregion

        #region Private Static Methods

        private static void InvokePageLoad(ResourceView<Employee> rv)
        {
            InvokeEventByName(rv, "Page_Load");
        }

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _simulator = new HttpSimulator();

            _mocks
               .DynamicMock(out _presenter)
               .DynamicMock(out _currentUser);

            _target = new TestResourceViewBuilder()
               .WithCurrentUser(_currentUser);

            _container.Inject<IRepository<Employee>>(new MockEmployeeRepository());
            _container.Inject(_presenter);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
            _simulator.Dispose();
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestThatYouAreRunningYourTestsInDebugMode()
        {
            _mocks.ReplayAll();

#if !DEBUG
            Assert.Fail("DOH! Switch to DEBUG mode and run all your tests again.");
#endif
        }

        #region Page Events

        [TestMethod]
        public void TestPageInitCallsPresenterOnViewInitWithIUser()
        {
            using (_mocks.Record())
            {
                _presenter.OnViewInit(_target.IUser);
            }

            using (_mocks.Playback())
            {
                // Page_Load wires the event handlers for us
                InvokePageLoad(_target);
                InvokeEventByName(_target, "Page_Init");
            }
        }

        [TestMethod]
        public void TestPageLoadSetsPresenterListViewWhenListViewIsNotNull()
        {
            var listView = _mocks.DynamicMock<IListView<Employee>>();
            _target = new TestResourceViewBuilder().WithListView(listView);

            using (_mocks.Record())
            {
                _presenter.ListView = listView;
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadSetsPresenterDetailViewWhenDetailViewIsNotNull()
        {
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            _target = new TestResourceViewBuilder().WithDetailView(detailView);

            using (_mocks.Record())
            {
                _presenter.DetailView = detailView;
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadSetsPresenterSearchViewWhenSearchViewIsNotNull()
        {
            var searchView = _mocks.DynamicMock<ISearchView<Employee>>();
            _target = new TestResourceViewBuilder().WithSearchView(searchView);

            using (_mocks.Record())
            {
                _presenter.SearchView = searchView;
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadCallsPresenterOnViewInitializedWhenIsNotPostBack()
        {
            _target = new TestResourceViewBuilder().WithPostBack(false);

            using (_mocks.Record())
            {
                _presenter.OnViewInitialized();
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadDoesNotCallPresenterOnViewInitializedWhenPostBack()
        {
            _target = new TestResourceViewBuilder().WithPostBack(true);

            using (_mocks.Record())
            {
                DoNotExpect.Call(_presenter.OnViewInitialized);
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadCallsPresenterOnViewLoaded()
        {
            using (_mocks.Record())
            {
                _presenter.OnViewLoaded();
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPagePrerenderCallsPresenterOnViewPrerender()
        {
            using (_mocks.Record())
            {
                _presenter.OnViewPrerender();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "Page_Prerender");
            }
        }

        #endregion

        #region Control Events

        [TestMethod]
        public void TestBtnBackToListClickFiresBackToListClickedEvent()
        {
            _target =
                new TestResourceViewBuilder().WithBackToListClickedHandler(
                    _testableHandler);

            _mocks.ReplayAll();

            InvokeEventByName(_target, "btnBackToList_Click");

            Assert.IsTrue(_called);
        }

        #endregion

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestSetDetailModeCallsSetViewModeOnDetailView()
        {
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            _target = new TestResourceViewBuilder().WithDetailView(detailView);
            var newMode = DetailViewMode.ReadOnly;

            using (_mocks.Record())
            {
                detailView.SetViewMode(newMode);
            }

            using (_mocks.Playback())
            {
                _target.SetDetailMode(newMode);
            }
        }

        [TestMethod]
        public void TestChangeModeChangesValueOfCurrentMode()
        {
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            _target = new TestResourceViewBuilder().WithDetailView(detailView);

            _mocks.ReplayAll();

            _target.SetViewMode(ResourceViewMode.List);
            Assert.AreEqual(ResourceViewMode.List, _target.CurrentMode);

            _target.SetViewMode(ResourceViewMode.Detail);
            Assert.AreEqual(ResourceViewMode.Detail, _target.CurrentMode);

            _target.SetViewMode(ResourceViewMode.Search);
            Assert.AreEqual(ResourceViewMode.Search, _target.CurrentMode);
        }

        [TestMethod]
        public void TestToggleListSetsListViewVisiblity()
        {
            var listView = _mocks.DynamicMock<IListView<Employee>>();
            _target = new TestResourceViewBuilder().WithListView(listView);

            using (_mocks.Record())
            {
                listView.Visible = false;
                listView.Visible = true;
            }

            using (_mocks.Playback())
            {
                _target.ToggleList(false);
                _target.ToggleList(true);
            }
        }

        [TestMethod]
        public void TestToggleDetailSetsDetailViewVisibility()
        {
            var detailView = _mocks.DynamicMock<IDetailView<Employee>>();
            _target = new TestResourceViewBuilder().WithDetailView(detailView);

            using (_mocks.Record())
            {
                detailView.Visible = false;
                detailView.Visible = true;
            }

            using (_mocks.Playback())
            {
                _target.ToggleDetail(false);
                _target.ToggleDetail(true);
            }
        }

        [TestMethod]
        public void TestToggleSearchSetsDetailViewVisibility()
        {
            var searchView = _mocks.DynamicMock<ISearchView<Employee>>();
            _target = new TestResourceViewBuilder().WithSearchView(searchView);

            using (_mocks.Record())
            {
                searchView.Visible = false;
                searchView.Visible = true;
            }

            using (_mocks.Playback())
            {
                _target.ToggleSearch(false);
                _target.ToggleSearch(true);
            }
        }

        [TestMethod]
        public void TestRedirectCallsResponseRedirect()
        {
            var expected = "Foo";
            var iResponse = _mocks.DynamicMock<IResponse>();
            _target = new TestResourceViewBuilder().WithResponse(iResponse);

            using (_mocks.Record())
            {
                iResponse.Redirect(expected);
            }

            using (_mocks.Playback())
            {
                _target.Redirect(expected);
            }
        }

        #endregion
    }

    internal class TestResourceViewBuilder : TestDataBuilder<MockResourceView>
    {
        #region Private Members

        //private SecurityCheckEventArgs _securityArgs;
        private IListView<Employee> _listView;
        private IDetailView<Employee> _detailView;
        private ISearchView<Employee> _searchView;
        private bool? _postBack = true;
        private EventHandler _onBackToListClicked;
        private IResponse _response;
        private IUser _iUser;

        #endregion

        #region Private Methods

        private void SetPostBack(MockResourceView rv)
        {
            var isPostBack = rv.GetType().GetField("_isMvpPostBack",
                BindingFlags.Instance |
                BindingFlags.NonPublic);
            isPostBack.SetValue(rv, _postBack.Value);
        }

        private void SetResponse(MockResourceView rv)
        {
            rv.SetResponse(_response);
        }

        #endregion

        #region Exposed Methods

        override public MockResourceView Build()
        {
            var rv = new MockResourceView();
            if (_response != null)
                SetResponse(rv);
            if (_postBack != null)
                SetPostBack(rv);
            if (_listView != null)
                rv.SetListView(_listView);
            if (_detailView != null)
                rv.SetDetailView(_detailView);
            if (_searchView != null)
                rv.SetSearchView(_searchView);
            if (_iUser != null)
                rv.SetUser(_iUser);
            //if (_securityArgs != null)
            //    rv.SetSecurityArgs(_securityArgs);
            if (_onBackToListClicked != null)
                rv.BackToListClicked += _onBackToListClicked;
            return rv;
        }

        public TestResourceViewBuilder WithListView(IListView<Employee> listView)
        {
            _listView = listView;
            return this;
        }

        public TestResourceViewBuilder WithDetailView(IDetailView<Employee> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestResourceViewBuilder WithSearchView(ISearchView<Employee> searchView)
        {
            _searchView = searchView;
            return this;
        }

        public TestResourceViewBuilder WithBackToListClickedHandler(EventHandler onBackToListClicked)
        {
            _onBackToListClicked = onBackToListClicked;
            return this;
        }

        public TestResourceViewBuilder WithPostBack(bool postBack)
        {
            _postBack = postBack;
            return this;
        }

        public TestResourceViewBuilder WithResponse(IResponse response)
        {
            _response = response;
            return this;
        }

        public MockResourceView WithCurrentUser(IUser user)
        {
            _iUser = user;
            return this;
        }

        //public ResourceView<Employee> WithSecurityEventArgs(SecurityCheckEventArgs args)
        //{
        //    _securityArgs = args;
        //    return this;
        //}

        #endregion
    }

    internal class MockResourceView : ResourceView<Employee>
    {
        #region Private Members

        private IListView<Employee> _listView;
        private IDetailView<Employee> _detailView;
        private ISearchView<Employee> _searchView;

        #endregion

        #region Properties

        public override IListView<Employee> ListView
        {
            get { return _listView; }
        }

        public override IDetailView<Employee> DetailView
        {
            get { return _detailView; }
        }

        public override ISearchView<Employee> SearchView
        {
            get { return _searchView; }
        }

        #endregion

        #region Constructors

        public MockResourceView() { }

        public MockResourceView(IListView<Employee> listView) : this(listView, null, null) { }

        public MockResourceView(IDetailView<Employee> detailView) : this(null, detailView, null) { }

        public MockResourceView(ISearchView<Employee> searchView) : this(null, null, searchView) { }

        public MockResourceView(IListView<Employee> listView, IDetailView<Employee> detailView) : this(listView,
            detailView, null) { }

        public MockResourceView(IListView<Employee> listView, IDetailView<Employee> detailView,
            ISearchView<Employee> searchView)
        {
            SetListView(listView);
            SetDetailView(detailView);
            SetSearchView(searchView);
        }

        #endregion

        #region Exposed Methods

        public void SetListView(IListView<Employee> listView)
        {
            _listView = listView;
        }

        public void SetDetailView(IDetailView<Employee> detailView)
        {
            _detailView = detailView;
        }

        public void SetSearchView(ISearchView<Employee> searchView)
        {
            _searchView = searchView;
        }

        public void SetResponse(IResponse response)
        {
            _iResponse = response;
        }

        public void SetUser(IUser user)
        {
            _iUser = user;
        }

        //public void SetSecurityArgs(SecurityCheckEventArgs args)
        //{
        //    _securityArgs = args;
        //}

        #endregion
    }
}
