using System;

namespace WorkOrders.Views.WorkOrders
{
    public interface IWorkOrderMarkoutPlanningListView : IWorkOrderListView
    {
        #region Events

        event EventHandler<MarkoutPlanningEventArgs> SaveClicked;

        #endregion
    }
}
