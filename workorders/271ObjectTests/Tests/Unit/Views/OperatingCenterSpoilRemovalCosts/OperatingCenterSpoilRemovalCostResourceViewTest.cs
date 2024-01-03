using LINQTo271.Views.OperatingCenterSpoilRemovalCosts;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.OperatingCenterSpoilRemovalCosts
{
    /// <summary>
    /// Summary description for OperatingCenterSpoilRemovalCostResourceViewTest.
    /// </summary>
    [TestClass]
    public class OperatingCenterSpoilRemovalCostResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListView<OperatingCenterSpoilRemovalCost> _listView;
        private TestOperatingCenterSpoilRemovalCostResourceView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _listView);

            _target = new TestOperatingCenterSpoilRemovalCostResourceViewBuilder()
                .WithListView(_listView);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestBackToListButtonPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.BackToListButton);
        }

        [TestMethod]
        public void TestDetailViewPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.DetailView);
        }

        [TestMethod]
        public void TestListViewPropertyReturnsListView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_listView, _target.ListView);
        }

        [TestMethod]
        public void TestSearchViewPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.SearchView);
        }
    }

    internal class TestOperatingCenterSpoilRemovalCostResourceViewBuilder : TestDataBuilder<TestOperatingCenterSpoilRemovalCostResourceView>
    {
        #region Private Members

        private IListView<OperatingCenterSpoilRemovalCost> _listView;

        #endregion

        #region Exposed Methods

        public override TestOperatingCenterSpoilRemovalCostResourceView Build()
        {
            var obj = new TestOperatingCenterSpoilRemovalCostResourceView();
            if (_listView != null)
                obj.SetListView(_listView);
            return obj;
        }

        public TestOperatingCenterSpoilRemovalCostResourceViewBuilder WithListView(IListView<OperatingCenterSpoilRemovalCost> view)
        {
            _listView = view;
            return this;
        }

        #endregion
    }

    internal class TestOperatingCenterSpoilRemovalCostResourceView : OperatingCenterSpoilRemovalCostResourceView
    {
        #region Exposed Methods

        public void SetListView(IListView<OperatingCenterSpoilRemovalCost> view)
        {
            ocstListView = view;
        }

        #endregion
    }
}
