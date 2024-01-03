using System;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using Rhino.Mocks.Interfaces;

namespace MMSINC.Core.WebFormsTest.Presenter
{
    /// <summary>
    /// Summary description for ChildResourcePresenterTest
    /// </summary>
    [TestClass]
    public class ChildResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private TestChildResourcePresenter _target;
        private IRepository<Employee> _repository;
        private IChildResourceView<Employee> _view;
        private IChildDetailView<Employee> _detailview;
        private IChildListView<Employee> _listview;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
               .DynamicMock(out _repository)
               .DynamicMock(out _view)
               .DynamicMock(out _listview)
               .DynamicMock(out _detailview);
            _target = new TestChildResourcePresenterBuilder(_view, _repository)
                     .WithChildListView(_listview)
                     .WithChildDetailView(_detailview);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestChildDetailViewPropertyReturnsChildDetailView()
        {
            _mocks.ReplayAll();
            Assert.AreSame(_detailview, _target.ChildDetailView);
        }

        [TestMethod]
        public void TestChildListViewPropertyReturnsChildDetailView()
        {
            _mocks.ReplayAll();
            Assert.AreSame(_listview, _target.ChildListView);
        }

        [TestMethod]
        public void TestChildResourceViewPropertyReturnsChildResourceView()
        {
            _mocks.ReplayAll();
            Assert.AreSame(_view, _target.ChildResourceView);
        }

        [TestMethod]
        public void TestViewBackToListClickedCommandFiresResourceViewOnChildEvent()
        {
            using (_mocks.Record())
            {
                _view.OnChildEvent(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_view,
                    "BackToListClicked");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestListViewCreateCommandFiresResourceViewOnChildEvent()
        {
            using (_mocks.Record())
            {
                _view.OnChildEvent(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_listview,
                    "CreateClicked");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestListViewSelectedIndexChangedCommandFiresResourceViewOnChildEvent()
        {
            using (_mocks.Record())
            {
                _view.OnChildEvent(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_listview,
                    "SelectedIndexChanged");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewDiscardChangesClickedCommandFiresResourceViewOnChildEvent()
        {
            using (_mocks.Record())
            {
                _view.OnChildEvent(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_detailview,
                    "DiscardChangesClicked");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewEditClickedCommandFiresResourceViewOnChildEvent()
        {
            using (_mocks.Record())
            {
                _view.OnChildEvent(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_detailview,
                    "EditClicked");
                eventRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestDetailViewUpdatingCommandFiresResourceViewOnChildEvent()
        {
            using (_mocks.Record())
            {
                _view.OnChildEvent(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_detailview,
                    "Updating");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty); // need EntityEventArgs<TEntity> here.
            }
        }

        [TestMethod]
        public void TestDetailViewDeleteClickedCommandFiresResourceViewOnChildEvent()
        {
            using (_mocks.Record())
            {
                _view.OnChildEvent(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                IEventRaiser eventRaiser = new EventRaiser((IMockedObject)_detailview,
                    "DeleteClicked");
                eventRaiser.Raise(null, EntityEventArgs<Employee>.Empty);
            }
        }

        [TestMethod]
        public void TestGetNewEntityReturnsNewEntity()
        {
            _mocks.ReplayAll();
            var entity = _target.GetNewEntity();
            Assert.IsNotNull(entity);
            Assert.IsInstanceOfType(entity, typeof(Employee));
        }

        [TestMethod]
        public void TestCreateCommandCallsChildResourceViewOnChildEvent()
        {
            _target.ParentEntity = new object();
            using (_mocks.Record())
            {
                _target.ChildResourceView.OnChildEvent(null);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var createClickedRaiser =
                    new EventRaiser((IMockedObject)_listview, "CreateClicked");
                createClickedRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestCreateCommandSetsDetailViewModeToInsertAndDataBindsWhenDetailViewIsNotNull()
        {
            _target.ParentEntity = new object();
            using (_mocks.Record())
            {
                _detailview.ShowEntity(null);
                LastCall.IgnoreArguments();
                _detailview.SetViewMode(DetailViewMode.Insert);
                _detailview.DataBind();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var createClickedRaiser =
                    new EventRaiser((IMockedObject)_listview, "CreateClicked");
                createClickedRaiser.Raise(null, EventArgs.Empty);
            }
        }

        [TestMethod]
        public void TestCreateCommandSetsResourceViewModeToDetailWhenResourceViewIsNotNull()
        {
            _target.ParentEntity = new object();
            using (_mocks.Record())
            {
                _view.SetViewMode(ResourceViewMode.Detail);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
                var createClickedRaiser =
                    new EventRaiser((IMockedObject)_listview, "CreateClicked");
                createClickedRaiser.Raise(null, EventArgs.Empty);
            }
        }
    }

    internal class TestChildResourcePresenterBuilder : TestDataBuilder<TestChildResourcePresenter>
    {
        #region Private Members

        private IChildResourceView<Employee> _view;
        private IRepository<Employee> _repository;
        private IChildDetailView<Employee> _detailView;
        private IChildListView<Employee> _listview;

        #endregion

        #region Constructors

        public TestChildResourcePresenterBuilder(
            IChildResourceView<Employee> view,
            IRepository<Employee> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestChildResourcePresenter Build()
        {
            var obj = new TestChildResourcePresenter(_view, _repository);
            if (_detailView != null)
                obj.DetailView = _detailView;
            if (_listview != null)
                obj.ListView = _listview;
            return obj;
        }

        public TestChildResourcePresenterBuilder WithChildDetailView(IChildDetailView<Employee> detailview)
        {
            _detailView = detailview;
            return this;
        }

        public TestChildResourcePresenterBuilder WithChildListView(IChildListView<Employee> listview)
        {
            _listview = listview;
            return this;
        }

        #endregion
    }

    internal class TestChildResourcePresenter : ChildResourcePresenter<Employee>
    {
        #region Constructors

        public TestChildResourcePresenter(IChildResourceView<Employee> view, IRepository<Employee> repository)
            : base(view, repository) { }

        #endregion

        #region Exposed Methods

        public override void FilterListViews()
        {
            ListView.SetListData(
                Repository.GetFilteredSortedData(
                    emp => emp.ReportsTo == ParentEntity,
                    ListView.SqlSortExpression));
        }

        #endregion
    }
}
