using LINQTo271.Views.RestorationTypeCosts;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.RestorationTypeCosts
{
    /// <summary>
    /// Summary description for RestorationTypeCostResourceViewTestTest
    /// </summary>
    [TestClass]
    public class RestorationTypeCostResourceViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListView<RestorationTypeCost> _listView;
        private TestRestorationTypeCostResourceView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _listView);

            _target = new TestRestorationTypeCostResourceViewBuilder()
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

    internal class TestRestorationTypeCostResourceViewBuilder : TestDataBuilder<TestRestorationTypeCostResourceView>
    {
        #region Private Members

        private IListView<RestorationTypeCost> _listView;

        #endregion

        #region Exposed Methods

        public override TestRestorationTypeCostResourceView Build()
        {
            var obj = new TestRestorationTypeCostResourceView();
            if (_listView != null)
                obj.SetListView(_listView);
            return obj;
        }

        public TestRestorationTypeCostResourceViewBuilder WithListView(IListView<RestorationTypeCost> view)
        {
            _listView = view;
            return this;
        }

        #endregion
    }

    internal class TestRestorationTypeCostResourceView : RestorationTypeCostResourceView
    {
        #region Exposed Methods

        public void SetListView(IListView<RestorationTypeCost> view)
        {
            rtcListView = view;
        }

        #endregion
    }
}
