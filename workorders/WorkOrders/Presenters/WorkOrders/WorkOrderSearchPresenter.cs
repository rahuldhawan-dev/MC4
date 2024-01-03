using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.WorkOrders
{
    public class WorkOrderSearchPresenter : SearchPresenter<WorkOrder>
    {
        #region Constructors

        public WorkOrderSearchPresenter(ISearchView<WorkOrder> view)
            : base(view)
        {
        }

        #endregion
    }
}
