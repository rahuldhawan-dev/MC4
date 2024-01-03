using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrderApprovalResourceView : WorkOrderResourceView
    {
        #region Control Declarations

        protected IListView<WorkOrder> wolvWorkOrders;
        protected ISearchView<WorkOrder> wosvWorkOrders;
        protected IDetailView<WorkOrder> wodvWorkOrder;

        #endregion

        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Approval; }
        }

        public override sealed IListView<WorkOrder> ListView
        {
            get { return wolvWorkOrders; }
        }

        public override sealed IDetailView<WorkOrder> DetailView
        {
            get { return wodvWorkOrder; }
        }

        public override sealed ISearchView<WorkOrder> SearchView
        {
            get { return wosvWorkOrders; }
        }

        #endregion
    }
}
