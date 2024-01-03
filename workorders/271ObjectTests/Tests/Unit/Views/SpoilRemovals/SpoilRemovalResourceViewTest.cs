using LINQTo271.Views.SpoilRemovals;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.SpoilRemovals
{
    /// <summary>
    /// Summary description for SpoilRemovalResourceViewTest.
    /// </summary>
    [TestClass]
    public class SpoilRemovalResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListView<SpoilRemoval> _listView;
        private ISearchView<SpoilRemoval> _searchView;
        private TestSpoilRemovalResourceView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks.DynamicMock(out _listView).DynamicMock(out _searchView);
            _target = new TestSpoilRemovalResourceViewBuilder()
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

    internal class TestSpoilRemovalResourceViewBuilder : TestDataBuilder<TestSpoilRemovalResourceView>
    {
        #region Private Members

        private IListView<SpoilRemoval> _listView;
        private ISearchView<SpoilRemoval> _searchView;

        #endregion

        #region Exposed Methods

        public override TestSpoilRemovalResourceView Build()
        {
            var obj = new TestSpoilRemovalResourceView();
            if (_listView != null)
                obj.SetListView(_listView);
            if (_searchView != null)
                obj.SetSearchView(_searchView);

            return obj;
        }

        public TestSpoilRemovalResourceViewBuilder WithListView(IListView<SpoilRemoval> listView)
        {
            _listView = listView;
            return this;
        }

        public TestSpoilRemovalResourceViewBuilder WithSearchView(ISearchView<SpoilRemoval> searchView)
        {
            _searchView = searchView;
            return this;
        }

        #endregion
    }

    internal class TestSpoilRemovalResourceView : SpoilRemovalResourceView
    {
        #region Exposed Methods

        public void SetListView(IListView<SpoilRemoval> view)
        {
            srListView = view;
        }

        public void SetSearchView(ISearchView<SpoilRemoval> view)
        {
            srSearchView = view;
        }

        #endregion
    }
}
