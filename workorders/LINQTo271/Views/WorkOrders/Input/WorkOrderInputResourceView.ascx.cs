using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.WorkOrders.Input
{
    /// <summary>
    /// Resource view for the main input of initial Work Order information.  This
    /// is a special view setup in that the list view is actuall a child to the
    /// detail view.
    ///
    /// When creating a new order, the save button will return the
    /// the user to the detail view in read only mode.  The edit button becomes
    /// visible and active at that point so that the user can correct any errors
    /// in the newly created order.
    ///
    /// There are no regular search or list views for this phase.
    /// </summary>
    public partial class WorkOrderInputResourceView : WorkOrderResourceView
    {
        #region Control Declarations

        protected IWorkOrderDetailView wodvWorkOrder;
        protected IListView<WorkOrder> wolvWorkOrder;
        
        #endregion

        #region Properties

        public override IButton BackToListButton
        {
            get { return null; }
        }

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Input; }
        }

        public override IListView<WorkOrder> ListView
        {
            get { return wolvWorkOrder; }
        }

        public override IDetailView<WorkOrder> DetailView
        {
            get { return wodvWorkOrder; }
        }

        public override ISearchView<WorkOrder> SearchView
        {
            get { return null; }
        }

        #endregion

        #region Exposed Methods

        public override void SetViewMode(ResourceViewMode newMode)
        {
            CurrentMode = newMode;
            var detailVisible = (newMode == ResourceViewMode.Detail);
            ToggleList(detailVisible);
            ToggleDetail(detailVisible);
        }

        #endregion
    }
}
