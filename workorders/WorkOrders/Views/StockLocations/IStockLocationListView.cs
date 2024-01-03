using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.StockLocations
{
    public interface IStockLocationListView : IListView<StockLocation>
    {
        #region Properties

        int OperatingCenterID { get; set; }

        #endregion
    }
}
