using LINQTo271.Views.WorkOrders.Finalization;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Finalization
{
    /// <summary>
    /// Summary description for WorkOrderFinalizationResourceRPCViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderFinalizationResourceRPCViewTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailView<WorkOrder> _detailView;
        private TestWorkOrderFinalizationResourceRPCView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _detailView);

            _target = new TestWorkOrderFinalizationResourceRPCViewBuilder()
                .WithDetailView(_detailView);
        }

        #endregion

        [TestMethod]
        public void TestListViewPropertyReturnsNull()
        {
            Assert.IsNull(_target.ListView);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSearchViewPropertyReturnsNull()
        {
            Assert.IsNull(_target.SearchView);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDetailViewPropertyReturnsDetailView()
        {
            Assert.AreSame(_detailView, _target.DetailView);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestPhasePropertyDenotesFinalization()
        {
            Assert.AreEqual(WorkOrderPhase.Finalization, _target.Phase);

            _mocks.ReplayAll();
        }
    }

    internal class TestWorkOrderFinalizationResourceRPCViewBuilder : TestDataBuilder<TestWorkOrderFinalizationResourceRPCView>
    {
        #region Private Members

        private IDetailView<WorkOrder> _detailView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderFinalizationResourceRPCView Build()
        {
            var obj = new TestWorkOrderFinalizationResourceRPCView();
            if (_detailView != null)
                obj.SetDetailView(_detailView);
            return obj;
        }

        public TestWorkOrderFinalizationResourceRPCViewBuilder WithDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderFinalizationResourceRPCView : WorkOrderFinalizationResourceRPCView
    {
        #region Exposed Methods

        public void SetDetailView(IDetailView<WorkOrder> view)
        {
            wodvWorkOrder = view;
        }

        #endregion
    }
}
