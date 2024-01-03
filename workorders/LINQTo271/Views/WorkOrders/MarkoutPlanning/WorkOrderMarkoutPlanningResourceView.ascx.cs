using LINQTo271.Views.Abstract;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.MarkoutPlanning
{
    public partial class WorkOrderMarkoutPlanningResourceView : WorkOrderResourceView
    {
        #region Control Declarations

        protected IListView<WorkOrder> wolvWorkOrders;
        protected ISearchView<WorkOrder> wosvWorkOrders;

        #endregion

        #region Properties

        public override IListView<WorkOrder> ListView
        {
            get { return wolvWorkOrders; }
        }

        public override IDetailView<WorkOrder> DetailView
        {
            get { return null; }
        }

        public override ISearchView<WorkOrder> SearchView
        {
            get { return wosvWorkOrders; }
        }

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.PrePlanning; }
        }

        #endregion
    }
}