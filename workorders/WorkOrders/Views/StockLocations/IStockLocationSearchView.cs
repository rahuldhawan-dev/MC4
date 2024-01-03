using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.StockLocations
{
    public interface IStockLocationSearchView : ISearchView<StockLocation>
    {
        #region Properties

        int OperatingCenterID { get; }

        #endregion
    }
}
