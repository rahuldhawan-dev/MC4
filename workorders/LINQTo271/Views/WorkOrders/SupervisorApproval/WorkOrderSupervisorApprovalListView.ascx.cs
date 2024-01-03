using System.Web.UI.WebControls;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.SupervisorApproval
{
    public partial class WorkOrderSupervisorApprovalListView : WorkOrderListView
    {
        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Approval; }
        }

        #endregion

        protected void gvWorkOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            WorkOrderColorHelper.ApplyColors(e.Row);
        }
    }
}
