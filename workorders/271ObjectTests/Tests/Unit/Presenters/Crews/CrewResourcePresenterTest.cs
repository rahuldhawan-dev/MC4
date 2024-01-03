using System.Collections.Generic;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.Crews;

namespace _271ObjectTests.Tests.Unit.Presenters.Crews
{
    /// <summary>
    /// Summary description for CrewResourcePresenterTestTest
    /// </summary>
    [TestClass]
    public class CrewResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceView<Crew> _view;
        private IListView<Crew> _listView;
        private IRepository<Crew> _repository;
        private TestCrewResourcePresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _listView)
                .DynamicMock(out _repository);

            _target = new TestCrewResourcePresenterBuilder(_view, _repository)
                .WithListView(_listView);

            SetupResult.For(_view.ListView).Return(_listView);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestViewLoadCompleteDoesNothingWhenListViewIsNull()
        {
            _mocks.CreateMock(out _view);
            SetupResult.For(_view.ListView).Return(null);
            _target = new TestCrewResourcePresenterBuilder(_view, _repository);

            using (_mocks.Record())
            {
                // expect nothing
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_LoadComplete");
            }
        }

        [TestMethod]
        public void TestViewLoadCompleteDoesNothingWhenListViewIsNotVisible()
        {
            _mocks
                .CreateMock(out _listView)
                .CreateMock(out _view);
            SetupResult.For(_listView.Visible).Return(false);
            SetupResult.For(_view.ListView).Return(_listView);
            _target = new TestCrewResourcePresenterBuilder(_view, _repository)
                .WithListView(_listView);

            using (_mocks.Record())
            {
                // expect nothing
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_LoadComplete");
            }
        }

        [TestMethod]
        public void TestViewLoadCompleteSetsListDataAndDataBindsListViewWhenListViewIsPresentAndVisible()
        {
            IEnumerable<Crew> result;
            _mocks
                .DynamicMock(out result)
                .CreateMock(out _listView)
                .CreateMock(out _repository)
                .CreateMock(out _view);
            SetupResult.For(_listView.Visible).Return(true);
            SetupResult.For(_view.ListView).Return(_listView);
            SetupResult.For(_listView.SqlSortExpression).Return(null);

            _target = new TestCrewResourcePresenterBuilder(_view, _repository)
                .WithListView(_listView);

            using (_mocks.Record())
            {
                SetupResult.For(_repository.GetFilteredSortedData(null, null)).Return(result);
                LastCall.IgnoreArguments();

                _listView.SetListData(result);

                _listView.DataBind();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_LoadComplete");
            }
        }

        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersAdminResourcePresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersAdminResourcePresenter<Crew>),
                "ResourcePresenters in this project should inherit from WorkOrdersResourcePresenter, lest bad tings happen.");
        }

    }

    internal class TestCrewResourcePresenterBuilder : TestDataBuilder<TestCrewResourcePresenter>
    {
        #region Private Members

        private readonly IResourceView _view;
        private readonly IRepository<Crew> _repository;
        private IListView<Crew> _listView;

        #endregion

        #region Constructors

        private TestCrewResourcePresenterBuilder()
        {
        }

        internal TestCrewResourcePresenterBuilder(IResourceView view, IRepository<Crew> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestCrewResourcePresenter Build()
        {
            var obj = new TestCrewResourcePresenter(_view, _repository);
            if (_listView != null)
                obj.ListView = _listView;
            return obj;
        }

        public TestCrewResourcePresenterBuilder WithListView(IListView<Crew> listView)
        {
            _listView = listView;
            return this;
        }

        #endregion
    }

    internal class TestCrewResourcePresenter : CrewResourcePresenter
    {
        #region Constructors

        internal TestCrewResourcePresenter(IResourceView view, IRepository<Crew> repository)
            : base(view, repository)
        {
        }

        #endregion
    }
}