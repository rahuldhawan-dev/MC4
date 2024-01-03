using LINQTo271.Views.WorkOrders.SupervisorApproval;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.SupervisorApproval
{
    /// <summary>
    /// Summary description for WorkOrderSupervisorApprovalListViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderSupervisorApprovalListViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListControl _listControl;
        private TestWorkOrderSupervisorApprovalListView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks.DynamicMock(out _listControl);
            _target = new TestWorkOrderSupervisorApprovalListViewBuilder()
                .WithListControl(_listControl);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestListControlPropertyReturnsListControl()
        {
            Assert.AreSame(_listControl, _target.ListControl);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestPhasePropertyDenotesApproval()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Approval, _target.Phase);
        }
    }

    internal class TestWorkOrderSupervisorApprovalListViewBuilder : TestDataBuilder<TestWorkOrderSupervisorApprovalListView>
    {
        #region Private Members

        private IListControl _listControl = new MvpGridView();

        #endregion

        #region Exposed Methods

        public override TestWorkOrderSupervisorApprovalListView Build()
        {
            var obj = new TestWorkOrderSupervisorApprovalListView();
            if (_listControl != null)
                obj.SetListControl(_listControl);
            return obj;
        }

        public TestWorkOrderSupervisorApprovalListViewBuilder WithListControl(IListControl listControl)
        {
            _listControl = listControl;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderSupervisorApprovalListView : WorkOrderSupervisorApprovalListView
    {
        #region Exposed Methods

        public void SetListControl(IListControl listControl)
        {
            gvWorkOrders = listControl;
        }

        #endregion
    }
}
