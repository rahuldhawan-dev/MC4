using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.SpoilFinalProcessingLocations;
using WorkOrders.Views.SpoilFinalProcessingLocations;

namespace _271ObjectTests.Tests.Unit.Presenters.SpoilFinalProcessingLocations
{
    /// <summary>
    /// Summary description for SpoilFinalProcessingLocationResourcePresenterTest.
    /// </summary>
    [TestClass]
    public class SpoilFinalProcessingLocationResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceView<SpoilFinalProcessingLocation> _view;
        private IRepository<SpoilFinalProcessingLocation> _repository;
        private ISpoilFinalProcessingLocationListView _listView;
        private ISpoilFinalProcessingLocationSearchView _searchView;

        private TestSpoilFinalProcessingLocationResourcePresenter _target;

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
                new TestSpoilFinalProcessingLocationResourcePresenterBuilder(_view, _repository)
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
        public void TestSpoilFinalProcessingLocationListViewPropertyReturnsCastedListView()
        {
            var expected =
                _target.GetPropertyValueByName("SpoilFinalProcessingLocationListView");
            Assert.IsInstanceOfType(expected, typeof(ISpoilFinalProcessingLocationListView));
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSpoilFinalProcessingLocationSearchViewPropertyReturnsCastedSearchView()
        {
            var expected =
                _target.GetPropertyValueByName("SpoilFinalProcessingLocationSearchView");
            Assert.IsInstanceOfType(expected, typeof(ISpoilFinalProcessingLocationSearchView));
            _mocks.ReplayAll();
        }

        #endregion

        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersResourcePresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersAdminResourcePresenter<SpoilFinalProcessingLocation>),
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

    internal class TestSpoilFinalProcessingLocationResourcePresenterBuilder : TestDataBuilder<TestSpoilFinalProcessingLocationResourcePresenter>
    {
        #region Private Members

        private IResourceView<SpoilFinalProcessingLocation> _view;
        private IListView<SpoilFinalProcessingLocation> _listView;
        private ISearchView<SpoilFinalProcessingLocation> _searchView;
        private IRepository<SpoilFinalProcessingLocation> _repository;

        #endregion

        #region Constructors

        public TestSpoilFinalProcessingLocationResourcePresenterBuilder(IResourceView<SpoilFinalProcessingLocation> view, IRepository<SpoilFinalProcessingLocation> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestSpoilFinalProcessingLocationResourcePresenter Build()
        {
            var obj = new TestSpoilFinalProcessingLocationResourcePresenter(_view, _repository);
            if (_searchView != null)
                obj.SearchView = _searchView;
            if (_listView != null)
                obj.ListView = _listView;
            return obj;
        }

        public TestSpoilFinalProcessingLocationResourcePresenterBuilder WithSearchView(ISearchView<SpoilFinalProcessingLocation> view)
        {
            _searchView = view;
            return this;
        }

        public TestSpoilFinalProcessingLocationResourcePresenterBuilder WithListView(IListView<SpoilFinalProcessingLocation> view)
        {
            _listView = view;
            return this;
        }


        #endregion
    }

    internal class TestSpoilFinalProcessingLocationResourcePresenter : SpoilFinalProcessingLocationResourcePresenter
    {
        #region Constructor

        public TestSpoilFinalProcessingLocationResourcePresenter(IResourceView view, IRepository<SpoilFinalProcessingLocation> repository)
            : base(view, repository)
        {
        }

        #endregion
    }
}
