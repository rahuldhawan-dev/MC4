using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.WorkOrders
{
    public interface IWorkOrderSearchView : ISearchView<WorkOrder>, IWorkOrderView
    {
        #region Properties

        int? OperatingCenterID { get; }

        #endregion

        #region Methods

        void DisplaySearchError(string message);

        #endregion
    }
}