
namespace WorkOrders.Views.WorkOrders
{
    public interface IWorkOrderSchedulingListView : IWorkOrderListView
    {
        #region Events

        event WorkOrderAssignmentEventHandler AssignClicked;

        #endregion
    }
}
