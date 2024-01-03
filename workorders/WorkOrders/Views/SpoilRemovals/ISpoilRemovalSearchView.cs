using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.SpoilRemovals
{
    public interface ISpoilRemovalSearchView : ISearchView<SpoilRemoval>
    {
        #region Properties

        int OperatingCenterID { get; }

        #endregion
    }
}
