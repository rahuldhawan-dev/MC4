using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Views.StockLocations;

namespace WorkOrders.Presenters.StockLocations
{
    public class StockLocationResourcePresenter : WorkOrdersAdminResourcePresenter<StockLocation>
    {
        #region Properties

        protected IStockLocationSearchView StockLocationSearchView
        {
            get { return (IStockLocationSearchView)SearchView; }
        }

        protected IStockLocationListView StockLocationListView
        {
            get { return (IStockLocationListView)ListView; }
        }

        #endregion

        #region Constructors

        public StockLocationResourcePresenter(IResourceView view, IRepository<StockLocation> repository)
            : base(view, repository)
        {
        }

        #endregion

        #region Event Handlers

        protected override void View_LoadComplete(object sender, EventArgs e)
        {
            if (ListView != null && ListView.Visible)
            {
                StockLocationListView.OperatingCenterID =
                    StockLocationSearchView.OperatingCenterID;
                ListView.DataBind();
            }
        }

        #endregion
    }
}
