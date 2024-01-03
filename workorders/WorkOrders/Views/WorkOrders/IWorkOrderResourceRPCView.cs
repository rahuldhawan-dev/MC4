using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.WorkOrders
{
    public interface IWorkOrderResourceRPCView : IWorkOrderView, IResourceRPCView<WorkOrder>
    {
        #region Properties

        WorkOrderPhase Phase { get; }

        #endregion
    }
}
