using LINQTo271.Views.WorkOrders.Finalization;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Finalization
{
    /// <summary>
    /// Summary description for WorkOrderFinalizationListViewTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderFinalizationListViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListControl _listControl;
        private TestWorkOrderFinalizationListView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks.DynamicMock(out _listControl);
            _target = new TestWorkOrderFinalizationListViewBuilder()
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
            _mocks.ReplayAll();

            Assert.AreSame(_listControl, _target.ListControl);
        }

        [TestMethod]
        public void TestPhasePropertyDenotesFinalization()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Finalization, _target.Phase);
        }
    }

    internal class TestWorkOrderFinalizationListViewBuilder : TestDataBuilder<TestWorkOrderFinalizationListView>
    {
        #region Private Members

        private IListControl _listControl = new MvpGridView();

        #endregion

        #region Exposed Methods

        public override TestWorkOrderFinalizationListView Build()
        {
            var obj = new TestWorkOrderFinalizationListView();
            if (_listControl != null)
                obj.SetListControl(_listControl);
            return obj;
        }

        public TestWorkOrderFinalizationListViewBuilder WithListControl(IListControl listControl)
        {
            _listControl = listControl;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderFinalizationListView : WorkOrderFinalizationListView
    {
        #region Exposed Methods

        public void SetListControl(IListControl listControl)
        {
            gvWorkOrders = listControl;
        }

        #endregion
    }
}