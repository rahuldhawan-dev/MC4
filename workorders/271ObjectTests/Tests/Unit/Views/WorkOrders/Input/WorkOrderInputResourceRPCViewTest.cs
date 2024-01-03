using LINQTo271.Views.WorkOrders.Input;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Input
{
    /// <summary>
    /// Summary description for WorkOrderInputResourceRPCViewTest
    /// </summary>
    [TestClass]
    public class WorkOrderInputResourceRPCViewTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailView<WorkOrder> _detailView;
        private TestWorkOrderInputResourceRPCView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks.DynamicMock(out _detailView);
            _target = new TestWorkOrderInputResourceRPCViewBuilder()
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
    
    internal class TestWorkOrderInputResourceRPCViewBuilder : TestDataBuilder<TestWorkOrderInputResourceRPCView>
    {
        #region Private Members

        private IDetailView<WorkOrder> _detailView;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderInputResourceRPCView Build()
        {
            var obj = new TestWorkOrderInputResourceRPCView();
            obj.SetDetailView(_detailView);
            return obj;
        }

        public TestWorkOrderInputResourceRPCViewBuilder WithDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderInputResourceRPCView : WorkOrderInputResourceRPCView
    {
        #region Exposed Methods

        public void SetDetailView(IDetailView<WorkOrder> detailView)
        {
            wodvWorkOrder = detailView;
        }

        #endregion
    }
}
