using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.SpoilStorageLocations;
using WorkOrders.Views.SpoilStorageLocations;

namespace _271ObjectTests.Tests.Unit.Presenters.SpoilStorageLocations
{
    /// <summary>
    /// Summary description for SpoilStorageLocationResourcePresenterTest.
    /// </summary>
    [TestClass]
    public class SpoilStorageLocationResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceView<SpoilStorageLocation> _view;
        private IRepository<SpoilStorageLocation> _repository;
        private ISpoilStorageLocationListView _listView;
        private ISpoilStorageLocationSearchView _searchView;

        private TestSpoilStorageLocationResourcePresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository)
                .DynamicMock(out _searchView)
                .DynamicMock(out _listView);

            _target =
                new TestSpoilStorageLocationResourcePresenterBuilder(_view, _repository)
                    .WithListView(_listView)
                    .WithSearchView(_searchView);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestSpoilStorageLocationListViewPropertyReturnsCastedListView()
        {
            var expected =
                _target.GetPropertyValueByName("SpoilStorageLocationListView");
            Assert.IsInstanceOfType(expected, typeof(ISpoilStorageLocationListView));
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSpoilStorageLocationSearchViewPropertyReturnsCastedSearchView()
        {
            var expected =
                _target.GetPropertyValueByName("SpoilStorageLocationSearchView");
            Assert.IsInstanceOfType(expected, typeof(ISpoilStorageLocationSearchView));
            _mocks.ReplayAll();
        }

        #endregion

        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersResourcePresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersAdminResourcePresenter<SpoilStorageLocation>),
                "ResourcePresenters in this project should inherit from WorkOrdersResourcePresenter, lest bad tings happen.");
        }

        [TestMethod]
        public void TestViewLoadCompleteSetsSearchViewOperatingCenterIDFromListViewOperatingCenterID()
        {
            var operatingCenterID = 10;

            using (_mocks.Record())
            {
                SetupResult.For(_listView.Visible).Return(true);
                SetupResult.For(_searchView.OperatingCenterID).Return(
                    operatingCenterID);
                _listView.OperatingCenterID = operatingCenterID;
                _listView.DataBind();
            }
            using (_mocks.Playback())
            {
                // Could not use EventRaiser here, flaw with SecurityService.
                InvokeEventByName(_target, "View_LoadComplete");
            }
        }
    }

    internal class TestSpoilStorageLocationResourcePresenterBuilder : TestDataBuilder<TestSpoilStorageLocationResourcePresenter>
    {
        #region Private Members

        private IResourceView<SpoilStorageLocation> _view;
        private IListView<SpoilStorageLocation> _listView;
        private ISearchView<SpoilStorageLocation> _searchView;
        private IRepository<SpoilStorageLocation> _repository;

        #endregion

        #region Constructors

        public TestSpoilStorageLocationResourcePresenterBuilder(IResourceView<SpoilStorageLocation> view, IRepository<SpoilStorageLocation> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestSpoilStorageLocationResourcePresenter Build()
        {
            var obj = new TestSpoilStorageLocationResourcePresenter(_view, _repository);
            if (_searchView != null)
                obj.SearchView = _searchView;
            if (_listView != null)
                obj.ListView = _listView;
            return obj;
        }

        public TestSpoilStorageLocationResourcePresenterBuilder WithSearchView(ISearchView<SpoilStorageLocation> view)
        {
            _searchView = view;
            return this;
        }

        public TestSpoilStorageLocationResourcePresenterBuilder WithListView(IListView<SpoilStorageLocation> view)
        {
            _listView = view;
            return this;
        }


        #endregion
    }

    internal class TestSpoilStorageLocationResourcePresenter : SpoilStorageLocationResourcePresenter
    {
        #region Constructor

        public TestSpoilStorageLocationResourcePresenter(IResourceView view, IRepository<SpoilStorageLocation> repository)
            : base(view, repository)
        {
        }

        #endregion
    }
}
