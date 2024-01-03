using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.SpoilRemovals;
using WorkOrders.Views.SpoilRemovals;

namespace _271ObjectTests.Tests.Unit.Presenters.SpoilRemovals
{
    /// <summary>
    /// Summary description for SpoilRemovalsResourcePresenterTest.
    /// </summary>
    [TestClass]
    public class SpoilRemovalResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceView<SpoilRemoval> _view;
        private IRepository<SpoilRemoval> _repository;
        private ISpoilRemovalListView _listView;
        private ISpoilRemovalSearchView _searchView;
        private TestSpoilRemovalResourcePresenter _target;

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
                new TestSpoilRemovalResourcePresenterBuilder(_view, _repository)
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
        public void TestSpoilRemovalListViewPropertyReturnsCastedListView()
        {
            var expected =
                _target.GetPropertyValueByName("SpoilRemovalListView");
            Assert.IsInstanceOfType(expected, typeof(ISpoilRemovalListView));
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSpoilRemovalSearchViewPropertyReturnsCastedSearchView()
        {
            var expected =
                _target.GetPropertyValueByName("SpoilRemovalSearchView");
            Assert.IsInstanceOfType(expected, typeof(ISpoilRemovalSearchView));
            _mocks.ReplayAll();
        }

        #endregion

        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersResourcePresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersAdminResourcePresenter<SpoilRemoval>),
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

    internal class TestSpoilRemovalResourcePresenterBuilder : TestDataBuilder<TestSpoilRemovalResourcePresenter>
    {
        #region Private Members

        private IResourceView<SpoilRemoval> _view;
        private IListView<SpoilRemoval> _listView;
        private ISearchView<SpoilRemoval> _searchView;
        private IRepository<SpoilRemoval> _repository;

        #endregion

        #region Constructors

        public TestSpoilRemovalResourcePresenterBuilder(IResourceView<SpoilRemoval> view, IRepository<SpoilRemoval> repository)
        {
            _view = view;
            _repository = repository;
        }
        
        #endregion
        
        #region Exposed Methods

        public override TestSpoilRemovalResourcePresenter Build()
        {
            var obj = new TestSpoilRemovalResourcePresenter(_view, _repository);
            if (_searchView!=null)
                obj.SearchView = _searchView;
            if (_listView != null)
                obj.ListView = _listView;
            return obj;
        }

        public TestSpoilRemovalResourcePresenterBuilder WithSearchView(ISearchView<SpoilRemoval> view)
        {
            _searchView = view;
            return this;
        }

        public TestSpoilRemovalResourcePresenterBuilder WithListView(IListView<SpoilRemoval> view)
        {
            _listView = view;
            return this;
        }
        
        #endregion
    }

    internal class TestSpoilRemovalResourcePresenter : SpoilRemovalResourcePresenter
    {
        #region Constructors

        public TestSpoilRemovalResourcePresenter(IResourceView view, IRepository<SpoilRemoval> repository) : base(view, repository)
        {
        }

        #endregion
    }
}
