using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.SpoilStorageLocations
{
    public interface ISpoilStorageLocationListView : IListView<SpoilStorageLocation>
    {
        #region Properties

        int OperatingCenterID { get; set; }

        #endregion
    }
}
