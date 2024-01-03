using LINQTo271.Views.SpoilFinalProcessingLocations;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.SpoilFinalProcessingLocations
{
    /// <summary>
    /// Summary description for SpoilFinalProcessingLocationResourceViewTest.
    /// </summary>
    [TestClass]
    public class SpoilFinalProcessingLocationResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListView<SpoilFinalProcessingLocation> _listView;
        private ISearchView<SpoilFinalProcessingLocation> _searchView;

        private TestSpoilFinalProcessingLocationResourceView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _listView)
                .DynamicMock(out _searchView);

            _target = new TestSpoilFinalProcessingLocationResourceViewBuilder()
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
        public void TestListViewPropertyReturnsListView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_listView, _target.ListView);
        }

        [TestMethod]
        public void TestDetailViewPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.DetailView);
        }

        [TestMethod]
        public void TestSearchViewPropertyReturnsSearchView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_searchView, _target.SearchView);
        }

        [TestMethod]
        public void TestBackToListButtonPropertyReturnNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.BackToListButton);
        }
    }

    internal class TestSpoilFinalProcessingLocationResourceViewBuilder : TestDataBuilder<TestSpoilFinalProcessingLocationResourceView>
    {
        private IListView<SpoilFinalProcessingLocation> _listView;
        private ISearchView<SpoilFinalProcessingLocation> _searchView;

        #region Private Members

        #endregion

        #region Exposed Methods

        public override TestSpoilFinalProcessingLocationResourceView Build()
        {
            var obj = new TestSpoilFinalProcessingLocationResourceView();
            if (_listView != null)
                obj.SetListView(_listView);
            if (_searchView != null)
                obj.SetSearchView(_searchView);
            return obj;
        }

        public TestSpoilFinalProcessingLocationResourceViewBuilder WithListView(IListView<SpoilFinalProcessingLocation> view)
        {
            _listView = view;
            return this;
        }

        public TestSpoilFinalProcessingLocationResourceViewBuilder WithSearchView(ISearchView<SpoilFinalProcessingLocation> view)
        {
            _searchView = view;
            return this;
        }

        #endregion
    }

    internal class TestSpoilFinalProcessingLocationResourceView : SpoilFinalProcessingLocationResourceView
    {
        #region Exposed Methods

        public void SetListView(IListView<SpoilFinalProcessingLocation> view)
        {
            sslListView = view;
        }

        public void SetSearchView(ISearchView<SpoilFinalProcessingLocation> view)
        {
            sslSearchView = view;
        }

        #endregion
    }
}
