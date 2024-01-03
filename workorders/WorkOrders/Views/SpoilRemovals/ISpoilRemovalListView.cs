using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.SpoilRemovals
{
    public interface ISpoilRemovalListView : IListView<SpoilRemoval>
    {
        #region Properties

        int OperatingCenterID { get; set; }

        #endregion
    }
}
