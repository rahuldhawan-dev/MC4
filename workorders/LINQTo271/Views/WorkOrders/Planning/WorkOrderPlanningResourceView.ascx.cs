using LINQTo271.Views.Abstract;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.Planning
{
    public partial class WorkOrderPlanningResourceView : WorkOrderResourceView
    {
        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Planning; }
        }

        public override IListView<WorkOrder> ListView
        {
            get { return wolvWorkOrders; }
        }

        public override IDetailView<WorkOrder> DetailView
        {
            get { return wodvWorkOrder; }
        }

        public override ISearchView<WorkOrder> SearchView
        {
            get { return wosvWorkOrders;  }
        }

        #endregion
   }
}
