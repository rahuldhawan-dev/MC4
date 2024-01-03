using LINQTo271.Views.Abstract;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.General
{
    public partial class WorkOrderGeneralResourceView : WorkOrderResourceView
    {
        #region Control Declarations

        protected IListView<WorkOrder> wolvWorkOrders;
        protected ISearchView<WorkOrder> wosvWorkOrders;
        protected IDetailView<WorkOrder> wodvWorkOrder;

        #endregion

        #region Properties

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
            get { return wosvWorkOrders; }
        }

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.General; }
        }

        #endregion
    }
}