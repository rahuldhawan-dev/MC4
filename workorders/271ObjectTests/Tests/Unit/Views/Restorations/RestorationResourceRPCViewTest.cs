using LINQTo271.Views.Restorations;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.Restorations
{
    /// <summary>
    /// Summary description for RestorationResourceRPCViewTestTest
    /// </summary>
    [TestClass]
    public class RestorationResourceRPCViewTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailView<Restoration> _detailView;
        private TestRestorationResourceRPCView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks.DynamicMock(out _detailView);
            _target = new TestRestorationResourceRPCViewBuilder()
                .WithDetailView(_detailView);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestDetailViewPropertyReturnsDetailView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_detailView, _target.DetailView);
        }

        [TestMethod]
        public void TestListViewPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.ListView);
        }

        [TestMethod]
        public void TestSearchViewPropertyReturnsNull()
        {
            _mocks.ReplayAll();

            Assert.IsNull(_target.SearchView);
        }
    }

    internal class TestRestorationResourceRPCViewBuilder : TestDataBuilder<TestRestorationResourceRPCView>
    {
        #region Private Members

        private IDetailView<Restoration> _detailView;

        #endregion

        #region Exposed Methods

        public override TestRestorationResourceRPCView Build()
        {
            var obj = new TestRestorationResourceRPCView();
            if (_detailView != null)
                obj.SetDetailView(_detailView);
            return obj;
        }

        public TestRestorationResourceRPCViewBuilder WithDetailView(IDetailView<Restoration> detailView)
        {
            _detailView = detailView;
            return this;
        }

        #endregion
    }

    internal class TestRestorationResourceRPCView : RestorationResourceRPCView
    {
        #region Exposed Methods

        public void SetDetailView(IDetailView<Restoration> detailView)
        {
            rdvRestoration = detailView;
        }

        #endregion
    }
}