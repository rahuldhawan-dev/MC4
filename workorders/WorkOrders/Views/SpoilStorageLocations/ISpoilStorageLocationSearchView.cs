using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.SpoilStorageLocations
{
    public interface ISpoilStorageLocationSearchView : ISearchView<SpoilStorageLocation>
    {
        #region Properties

        int OperatingCenterID { get; }

        #endregion
    }
}
