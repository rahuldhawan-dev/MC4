using System;
using System.Drawing;
using System.Web.UI.WebControls;
using LINQTo271.Common;
using LINQTo271.Views.WorkOrders.General;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;
using _271ObjectTests.Tests.Unit.Model;
using _271ObjectTests.Tests.Unit.Views.WorkOrders.General;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.General
{
    /// <summary>
    /// Summary description for WorkOrderGeneralListViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderGeneralListViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestWorkOrderGeneralListView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void WorkOrderGeneralListViewTestInitialize()
        {
            _target = new TestWorkOrderGeneralListViewBuilder();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestPhasePropertyDenotesGeneral()
        {
            Assert.AreEqual(WorkOrderPhase.General, _target.Phase);

            _mocks.ReplayAll();
        }

        #endregion

        #region Event Handler Tests

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

            Assert.AreEqual(WorkOrderColorHelper.COMPLETED,
                row.BackColor);
        }

        [TestMethod]
        public void TestGVWorkOrdersRowDataBoundDoesNotSetRowColorIfRowDataShowsWorkNotCompleted()
        {
            _mocks.ReplayAll();

            WorkOrder order = new TestWorkOrderBuilder()
                .WithDateCompleted(null);
            var row = new TestGridViewRow(order);
            var args = new GridViewRowEventArgs(row);

            InvokeEventByName(_target, "gvWorkOrders_RowDataBound",
                new object[] {
                    null, args
                });

            Assert.AreNotEqual(WorkOrderGeneralListView.COMPLETED_ORDER_ROW_COLOR,
                row.BackColor);
        }

        #endregion
    }

    internal class TestWorkOrderGeneralListViewBuilder : TestDataBuilder<TestWorkOrderGeneralListView>
    {
        #region Exposed Methods

        public override TestWorkOrderGeneralListView Build()
        {
            var obj = new TestWorkOrderGeneralListView();
            return obj;
        }

        #endregion
    }

    internal class TestWorkOrderGeneralListView : WorkOrderGeneralListView
    {
    }

    public class TestGridViewRow : GridViewRow
    {
        #region Private Members

        private object _dataItem;
        private DataControlRowType _rowType;
        private DataControlRowState _rowState;

        #endregion

        #region Properties

        public override object DataItem
        {
            get { return _dataItem; }
            set { _dataItem = value; }
        }

        public override DataControlRowType RowType
        {
            get { return _rowType; }
            set { _rowType = value; }
        }

        public override DataControlRowState RowState
        {
            get { return _rowState; }
            set { _rowState = value; }
        }

        #endregion

        #region Constructors

        public TestGridViewRow(Object dataItem) : this(dataItem, DataControlRowType.DataRow)
        {
        }

        public TestGridViewRow(Object dataItem, DataControlRowType rowType) : this(dataItem, rowType, DataControlRowState.Normal)
        {
        }

        public TestGridViewRow(Object dataItem, DataControlRowType rowType, DataControlRowState rowState)
            : this(0, 0, rowType, rowState)
        {
            _dataItem = dataItem;
        }

        private TestGridViewRow(int rowIndex, int dataItemIndex, DataControlRowType rowType, DataControlRowState rowState) : base(rowIndex, dataItemIndex, rowType, rowState)
        {
            _rowType = rowType;
            _rowState = rowState;
        }

        #endregion
    }
}
