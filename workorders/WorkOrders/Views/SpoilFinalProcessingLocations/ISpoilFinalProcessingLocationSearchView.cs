using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.SpoilFinalProcessingLocations
{
    public interface ISpoilFinalProcessingLocationSearchView : ISearchView<SpoilFinalProcessingLocation>
    {
        #region Properties

        int OperatingCenterID { get; }

        #endregion
    }
}
