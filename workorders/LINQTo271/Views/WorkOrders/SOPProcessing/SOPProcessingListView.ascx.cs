using System.Drawing;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.SOPProcessing
{
    public partial class SOPProcessingListView : WorkOrderListView
    {
        #region Constants

        public static readonly Color COMPLETED_ORDER_ROW_COLOR = Color.GreenYellow;

        #endregion

        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Finalization; }
        }
        
        #endregion

        #region Event Handlers

        protected void gvWorkOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem == null)
                return;

            if (((WorkOrder)e.Row.DataItem).WorkCompleted)
                e.Row.BackColor = COMPLETED_ORDER_ROW_COLOR;
        }

        #endregion
    }
}
