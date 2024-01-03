using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.StockLocations
{
    public class StockLocationListPresenter : ListPresenter<StockLocation>
    {
        #region Constructors

        public StockLocationListPresenter(IListView<StockLocation> view)
            : base(view)
        {
        }

        #endregion
    }
}
