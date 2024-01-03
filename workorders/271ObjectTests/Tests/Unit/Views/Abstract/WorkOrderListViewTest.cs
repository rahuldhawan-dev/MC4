using System;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.Abstract
{
    /// <summary>
    /// Summary description for WorkOrderListViewTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderListViewTest : EventFiringTestClass
    {
        #region Private Members

        private IListControl _listControl;
        private TestWorkOrderListView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks.DynamicMock(out _listControl);
            _target = new TestWorkOrderListViewBuilder()
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
    }

    internal class TestWorkOrderListViewBuilder : TestDataBuilder<TestWorkOrderListView>
    {
        #region Private Members

        private IListControl _listControl = new MvpGridView();

        #endregion

        #region Exposed Methods

        public override TestWorkOrderListView Build()
        {
            var obj = new TestWorkOrderListView();
            if (_listControl != null)
                obj.SetListControl(_listControl);
            return obj;
        }

        public TestWorkOrderListViewBuilder WithListControl(IListControl listControl)
        {
            _listControl = listControl;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderListView : WorkOrderListView
    {
        #region Properties

        public override WorkOrderPhase Phase
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Exposed Methods

        public void SetListControl(IListControl listControl)
        {
            gvWorkOrders = listControl;
        }

        #endregion
    }
}