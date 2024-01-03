using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.Input
{
    /// <summary>
    /// List view for the main initial input of Work Order information.  This is
    /// a special list view in that it is intended to live as a child to the detail
    /// view, and show related historical Work Orders to the user as they input new
    /// orders, based on asset information.
    /// </summary>
    public partial class WorkOrderInputListView : WorkOrderListView
    {
        #region Control Declarations

        protected IUpdatePanel upWorkOrderHistory;

        #endregion

        #region Private Members

        protected bool _hasOpenOrders;

        #endregion

        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Input; }
        }

        protected bool HasOpenOrders
        {
            get { return _hasOpenOrders; }
        }

        #endregion

        #region Private Methods

        private void OnAssetIDChanged()
        {
            ListControl.DataBind();
            upWorkOrderHistory.Update();
        }

        #endregion

        #region Event Handlers

        protected void hidAssetID_TextChanged(object sender, EventArgs e)
        {
            OnAssetIDChanged();
        }

        protected void ListControl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
                ((WorkOrder)e.Row.DataItem).DateCompleted == null)
            {
                _hasOpenOrders = true;
            }
        }

        #endregion
    }
}
