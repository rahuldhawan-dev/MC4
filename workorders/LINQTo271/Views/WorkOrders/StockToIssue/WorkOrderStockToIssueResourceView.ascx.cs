using LINQTo271.Views.Abstract;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.StockToIssue
{
    public partial class WorkOrderStockToIssueResourceView : WorkOrderApprovalResourceView
    {
        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.StockApproval; }
        }

        #endregion
    }
}
