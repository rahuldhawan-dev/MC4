using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.WorkOrders.SOPProcessing;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Controls;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;
using _271ObjectTests.Tests.Unit.Views.WorkOrders.General;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.SOPProcessing
{
    /// <summary>
    /// Summary description for SOPProcessingListViewTestTest.
    /// </summary>
    [TestClass]
    public class SOPProcessingListViewTest : EventFiringTestClass
    {
        #region Private Members
        private IListControl _listControl;
        private TestSOPProcessingListView _target;

        #endregion
        
        #region Additional Test Attributes

        [TestInitialize]
        public void SOPProcessingListViewTestTestInitialize()
        {
            _target = new TestSOPProcessingListViewBuilder();
            _mocks.DynamicMock(out _listControl);

            _target = new TestSOPProcessingListViewBuilder()
                .WithListControl(_listControl);
        }

        [TestMethod]
        public void TestListControlPropertyReturnsListControl()
        {
            Assert.AreSame(_listControl, _target.ListControl);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestGVWorkOrdersRowDataBoundSetsRowColorIfRowDataShowsWorkCompleted()
        {
            _mocks.ReplayAll();

            WorkOrder order = new TestWorkOrderBuilder()
                .WithDateCompleted(DateTime.Today.GetPreviousDay());
            var row = new TestGridViewRow(order);
            var args = new GridViewRowEventArgs(row);

            InvokeEventByName(_target, "gvWorkOrders_RowDataBound",
                new object[] {
                    null, args
                });

            Assert.AreEqual(SOPProcessingListView.COMPLETED_ORDER_ROW_COLOR,
                row.BackColor);
        }

        [TestMethod]
        public void TestGVWorkOrdersRowDataBoundReturnsWhenDataItemIsNull()
        {
            _mocks.ReplayAll();

            var row = new TestGridViewRow(null);
            var args = new GridViewRowEventArgs(row);

            InvokeEventByName(_target, "gvWorkOrders_RowDataBound",
                new object[] {
                    null, args
                });

            //TODO: Test something here.
        }


        [TestMethod]
        public void TestPhasePropertyDenotesFinalization()
        {
            _mocks.ReplayAll();
            Assert.AreEqual(WorkOrderPhase.Finalization, _target.Phase);
        }


        #endregion
    }

    internal class TestSOPProcessingListViewBuilder : TestDataBuilder<TestSOPProcessingListView>
    {
        #region Private Members

        private IListControl _listControl = new MvpGridView();

        #endregion

        #region Exposed Methods

        public override TestSOPProcessingListView Build()
        {
            var obj = new TestSOPProcessingListView(); 
            if (_listControl != null)
                obj.SetListControl(_listControl);
            return obj;

        }

        public TestSOPProcessingListViewBuilder WithListControl(IListControl listControl)
        {
            _listControl = listControl;
            return this;
        }

        #endregion
    }

    internal class TestSOPProcessingListView : SOPProcessingListView
    {
        public void SetListControl(IListControl listControl)
        {
            gvWorkOrders = listControl;
        }
    }
}
