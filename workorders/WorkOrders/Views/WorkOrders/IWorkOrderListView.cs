using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.WorkOrders
{
    public interface IWorkOrderListView : IListView<WorkOrder>, IWorkOrderView
    {
        #region Properties

        int? OperatingCenterID { get; set; }

        #endregion
    }
}
