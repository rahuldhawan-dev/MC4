using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINCTestImplementation.Model;
using MMSINCTestImplementation.Presenters;
using MMSINCTestImplementation.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;

namespace MMSINCTestImplementationTest.Presenters
{
    /// <summary>
    /// Summary description for EmployeeResourcePresenterTestTest
    /// </summary>
    [TestClass]
    public class EmployeeResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IEmployeeResourceView _view;
        private IListView<Employee> _listView;
        private IEmployeeDetailView _detailView;
        private ISearchView<Employee> _searchView;
        private IChildResourceView<Employee> _childResourceView;
        private IRepository<Employee> _repository;
        private TestEmployeeResourcePresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _view = _mocks.DynamicMock<IEmployeeResourceView>();
            _listView = _mocks.DynamicMock<IListView<Employee>>();
            SetupResult.For(_view.ListView).Return(_listView);
            _searchView = _mocks.DynamicMock<ISearchView<Employee>>();
            SetupResult.For(_view.SearchView).Return(_searchView);
            _detailView = _mocks.DynamicMock<IEmployeeDetailView>();
            SetupResult.For(_view.EmployeeDetailView).Return(_detailView);
            _childResourceView = _mocks.DynamicMock<IChildResourceView<Employee>>();
            SetupResult.For(_detailView.ChildResourceViews).Return(new[] {
                _childResourceView
            });
            _repository = _mocks.DynamicMock<IRepository<Employee>>();
            _target = new TestEmployeeResourcePresenterBuilder(_view,
                          _repository)
                     .WithDetailView(_detailView)
                     .WithListView(_listView)
                     .WithSearchView(_searchView);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestParentConstructorSetsEmployeeResourceViewFromArgument()
        {
            _target = new TestEmployeeResourcePresenter(_view, null);

            Assert.AreSame(_view, _target.EmployeeResourceView);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestChildConstructorSetsViewFromArgument()
        {
            var view = _mocks.DynamicMock<IChildResourceView<Employee>>();
            _target = new TestEmployeeResourcePresenter(view, null);

            Assert.AreSame(view, _target.View);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestConstructorSetsRepositoryFromArgument()
        {
            _target = new TestEmployeeResourcePresenter((IEmployeeResourceView)null, _repository);

            Assert.AreSame(_repository, _target.Repository);

            _target = new TestEmployeeResourcePresenter((IChildResourceView<Employee>)null, _repository);

            Assert.AreSame(_repository, _target.Repository);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnViewLoadedWiresEventHandlersToDetailViewAndChildResourceViews()
        {
            using (_mocks.Record())
            {
                _detailView.MenuItemClicked += null;
                LastCall.IgnoreArguments();

                _childResourceView.ChildEvent += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestChildResourceViewChangeCommandSetsRepositoryDataKeyFromListViewDataKey()
        {
            var expected = new Object();
            SetupResult.For(_listView.SelectedDataKey).Return(expected);

            using (_mocks.Record())
            {
                _repository.SetSelectedDataKey(expected);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var changeRaiser =
                    new EventRaiser((IMockedObject)_childResourceView,
                        "ChildEvent");
                changeRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewMenuItemClickedCommandSetsRepositoryDataKeyFromListView()
        {
            var expected = new Object();
            SetupResult.For(_listView.SelectedDataKey).Return(expected);

            using (_mocks.Record())
            {
                _repository.SetSelectedDataKey(expected);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var changeRaiser =
                    new EventRaiser((IMockedObject)_detailView,
                        "MenuItemClicked");
                changeRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestListViewSelectedIndexChangedCommandCallsDetailViewToggleControlWithProperArguments()
        {
            using (_mocks.Record())
            {
                _detailView.ToggleControl(
                    EmployeeDetailPresenter.Controls.DETAIL, true, true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var indexChangedRaiser =
                    new EventRaiser((IMockedObject)_listView,
                        "SelectedIndexChanged");
                indexChangedRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestListViewCreateClickedCallsDetailViewToggleControlWithProperArguments()
        {
            using (_mocks.Record())
            {
                _detailView.ToggleControl(
                    EmployeeDetailPresenter.Controls.MENU, false, false);
                _detailView.ToggleControl(
                    EmployeeDetailPresenter.Controls.DETAIL, true, true);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();

                var createClickedRaiser =
                    new EventRaiser((IMockedObject)_listView, "CreateClicked");
                createClickedRaiser.Raise(null, EventArgs.Empty);
            }
        }
    }

    internal class TestEmployeeResourcePresenterBuilder : TestDataBuilder<TestEmployeeResourcePresenter>
    {
        #region Private Members

        private readonly IEmployeeResourceView _view;
        private readonly IRepository<Employee> _repository;

        private IListView<Employee> _listView;
        private IEmployeeDetailView _detailView;
        private ISearchView<Employee> _searchView;

        #endregion

        #region Constructors

        public TestEmployeeResourcePresenterBuilder(IEmployeeResourceView view, IRepository<Employee> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestEmployeeResourcePresenter Build()
        {
            var obj = new TestEmployeeResourcePresenter(_view, _repository);
            if (_listView != null)
                obj.ListView = _listView;
            if (_detailView != null)
                obj.DetailView = _detailView;
            if (_searchView != null)
                obj.SearchView = _searchView;
            return obj;
        }

        public TestEmployeeResourcePresenterBuilder WithListView(IListView<Employee> listView)
        {
            _listView = listView;
            return this;
        }

        public TestEmployeeResourcePresenterBuilder WithDetailView(IEmployeeDetailView detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestEmployeeResourcePresenterBuilder WithSearchView(ISearchView<Employee> searchView)
        {
            _searchView = searchView;
            return this;
        }

        #endregion
    }

    internal class TestEmployeeResourcePresenter : EmployeeResourcePresenter
    {
        #region Constructors

        public TestEmployeeResourcePresenter(IChildResourceView<Employee> view, IRepository<Employee> repository)
            : base(view, repository) { }

        public TestEmployeeResourcePresenter(IEmployeeResourceView view, IRepository<Employee> repository)
            : base(view, repository) { }

        #endregion
    }
}
