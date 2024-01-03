using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.StockLocations
{
    public class StockLocationSearchPresenter : SearchPresenter<StockLocation>
    {
        #region Constructors

        public StockLocationSearchPresenter(ISearchView<StockLocation> view) : base(view)
        {
        }

        #endregion
    }
}
