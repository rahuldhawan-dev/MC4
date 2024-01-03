using MMSINC.Controls;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrderListView : WorkOrdersListView<WorkOrder>, IWorkOrderView
    {
        #region Private Members

        protected IListControl gvWorkOrders;

        #endregion

        #region Properties

        public override sealed IListControl ListControl
        {
            get { return gvWorkOrders; }
        }

        #endregion

        #region Abstract Properties

        public abstract WorkOrderPhase Phase { get; }

        #endregion

        #region Exposed Methods

        public override void SetViewControlsVisible(bool visible)
        {
            // noop (for now)
        }

        #endregion
    }
}
