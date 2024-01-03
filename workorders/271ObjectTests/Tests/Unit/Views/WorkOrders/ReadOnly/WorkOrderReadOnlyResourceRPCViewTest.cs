using LINQTo271.Views.WorkOrders.ReadOnly;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.ReadOnly
{
    /// <summary>
    /// Summary description for WorkOrderReadOnlyResourceRPCViewTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderReadOnlyResourceRPCViewTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailView<WorkOrder> _detailView;
        private TestWorkOrderReadOnlyResourceRPCView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks.DynamicMock(out _detailView);
            _target = new TestWorkOrderReadOnlyResourceRPCViewBuilder()
                .WithDetailView(_detailView);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

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

        [TestMethod]
        public void TestDetailViewPropertyReturnsDetailView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_detailView, _target.DetailView);
        }
    }

    internal class TestWorkOrderReadOnlyResourceRPCViewBuilder : TestDataBuilder<TestWorkOrderReadOnlyResourceRPCView>
    {
        #region Private Members

        private IDetailView<WorkOrder> _detailView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderReadOnlyResourceRPCView Build()
        {
            var obj = new TestWorkOrderReadOnlyResourceRPCView();
            if (_detailView != null)
                obj.SetDetailView(_detailView);
            return obj;
        }

        public TestWorkOrderReadOnlyResourceRPCViewBuilder WithDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderReadOnlyResourceRPCView : WorkOrderReadOnlyResourceRPCView
    {
        #region Exposed Methods

        public void SetDetailView(IDetailView<WorkOrder> detailView)
        {
            woroWorkOrder = detailView;
        }

        #endregion
    }
}