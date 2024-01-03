using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.SpoilFinalProcessingLocations
{
    public interface ISpoilFinalProcessingLocationListView : IListView<SpoilFinalProcessingLocation>
    {
        #region Properties

        int OperatingCenterID { get; set; }

        #endregion
    }
}
