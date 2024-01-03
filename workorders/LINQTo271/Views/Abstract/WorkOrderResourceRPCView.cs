using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrderResourceRPCView : WorkOrdersResourceRPCView<WorkOrder>, IWorkOrderResourceRPCView
    {
        #region Abstract Properties

        public abstract WorkOrderPhase Phase { get; }

        #endregion
    }
}
