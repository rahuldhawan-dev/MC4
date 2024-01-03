using System.Drawing;
using System.Web.UI.WebControls;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.General
{
    public partial class WorkOrderGeneralListView : WorkOrderListView
    {
        #region Constants

        public static readonly Color COMPLETED_ORDER_ROW_COLOR =
            Color.LightGreen,
            CANCELLED_ORDER_ROW_COLOR = Color.Orange;

        #endregion

        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.General; }
        }

        #endregion

        #region Event Handlers

        protected void gvWorkOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            WorkOrderColorHelper.ApplyColors(e.Row);
            //if (e.Row.DataItem != null)
            //{
            //    var backColor = e.Row.BackColor;
            //    var order = ((WorkOrder)e.Row.DataItem);

            //    if (order.CancelledAt.HasValue)
            //    {
            //        backColor = CANCELLED_ORDER_ROW_COLOR;
            //    }

            //    if (order.WorkCompleted)
            //    {
            //        backColor = COMPLETED_ORDER_ROW_COLOR;
            //    }

            //    e.Row.BackColor = backColor;
            //}
        }

        #endregion
    }
}
