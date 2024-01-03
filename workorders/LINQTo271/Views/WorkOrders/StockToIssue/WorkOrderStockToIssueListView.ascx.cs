using System.Web.UI.WebControls;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.StockToIssue
{
    public partial class WorkOrderStockToIssueListView : WorkOrderListView
    {
        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.StockApproval; }
        }

        #endregion

        protected void gvWorkOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            WorkOrderColorHelper.ApplyColors(e.Row);
        }
    }
}
