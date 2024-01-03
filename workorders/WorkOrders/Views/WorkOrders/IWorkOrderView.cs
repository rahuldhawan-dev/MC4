using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.WorkOrders
{
    /// <summary>
    /// Special contract for WorkOrderResourceViewObjects
    /// </summary>
    public interface IWorkOrderView
    {
        #region Properties

        /// <summary>
        /// WorkOrderResourcePhase value indicating the current phase the user
        /// is working with.
        /// </summary>
        WorkOrderPhase Phase { get; }

        #endregion
    }

    public interface IWorkOrderDetailView : IDetailView<WorkOrder>, IWorkOrderView
    {
        #region Properties

        bool ModeSet { get; }

        #endregion
    }

    public interface IWorkOrderResourceView : IWorkOrderView, IResourceView
    {
    }
}