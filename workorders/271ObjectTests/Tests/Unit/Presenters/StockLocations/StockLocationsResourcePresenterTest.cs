using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.StockLocations;
using WorkOrders.Views.StockLocations;

namespace _271ObjectTests.Tests.Unit.Presenters.StockLocations
{
    /// <summary>
    /// Summary description for StockLocationResourcePresenterTest.
    /// </summary>
    [TestClass]
    public class StockLocationResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceView<StockLocation> _view;
        private IRepository<StockLocation> _repository;
        private IStockLocationListView _listView;
        private IStockLocationSearchView _searchView;
        private StockLocationResourcePresenter _target;

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
                new TestStockLocationResourcePresenterBuilder(_view, _repository)
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
        public void TestStockLocationListViewPropertyReturnsCastedListView()
        {
            var expected =
                _target.GetPropertyValueByName("StockLocationListView");
            Assert.IsInstanceOfType(expected, typeof(IStockLocationListView));
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestStockLocationSearchViewPropertyReturnsCastedSearchView()
        {
            var expected =
                _target.GetPropertyValueByName("StockLocationSearchView");
            Assert.IsInstanceOfType(expected, typeof(IStockLocationSearchView));
            _mocks.ReplayAll();
        }

        #endregion
        
        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersResourcePresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersAdminResourcePresenter<StockLocation>),
                "ResourcePresenters in this project should inherit from WorkOrdersResourcePresenter, lest bad tings happen.");
        }

        [TestMethod]
        public void TestViewLoadCompleteSetsSearchViewOperatingCenterIDFromListViewOperatingCenterID()
        {
            var operatingCenterID = 10;
            
            using(_mocks.Record())
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

    internal class TestStockLocationResourcePresenterBuilder : TestDataBuilder<TestStockLocationResourcePresenter>
    {
        #region Private Members

        private IResourceView<StockLocation> _view;
        private IListView<StockLocation> _listView;
        private ISearchView<StockLocation> _searchView;
        private IRepository<StockLocation> _repository;

        #endregion

        #region Constructors

        public TestStockLocationResourcePresenterBuilder(IResourceView<StockLocation> view, IRepository<StockLocation> repository)
        {
            _view = view;
            _repository = repository;
        }
        
        #endregion
        
        #region Exposed Methods

        public override TestStockLocationResourcePresenter Build()
        {
            var obj = new TestStockLocationResourcePresenter(_view, _repository);
            if (_searchView!=null)
                obj.SearchView = _searchView;
            if (_listView != null)
                obj.ListView = _listView;
            return obj;
        }

        public TestStockLocationResourcePresenterBuilder WithSearchView(ISearchView<StockLocation> view)
        {
            _searchView = view;
            return this;
        }

        public TestStockLocationResourcePresenterBuilder WithListView(IListView<StockLocation> view)
        {
            _listView = view;
            return this;
        }


        #endregion
    }

    internal class TestStockLocationResourcePresenter : StockLocationResourcePresenter
    {
        #region Constructor

        public TestStockLocationResourcePresenter(IResourceView view, IRepository<StockLocation> repository) : base(view, repository)
        {
        }

        #endregion
    }
}
