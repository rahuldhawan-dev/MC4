using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Views.SpoilStorageLocations;

namespace WorkOrders.Presenters.SpoilStorageLocations
{
    public class SpoilStorageLocationResourcePresenter : WorkOrdersAdminResourcePresenter<SpoilStorageLocation>
    {
        #region Properties

        protected ISpoilStorageLocationSearchView SpoilStorageLocationSearchView
        {
            get { return (ISpoilStorageLocationSearchView)SearchView; }
        }

        protected ISpoilStorageLocationListView SpoilStorageLocationListView
        {
            get { return (ISpoilStorageLocationListView)ListView; }
        }

        #endregion

        #region Constructors

        public SpoilStorageLocationResourcePresenter(IResourceView view, IRepository<SpoilStorageLocation> repository)
            : base(view, repository)
        {
        }

        #endregion

        #region Event Handlers

        protected override void View_LoadComplete(object sender, EventArgs e)
        {
            if (ListView != null && ListView.Visible)
            {
                SpoilStorageLocationListView.OperatingCenterID =
                    SpoilStorageLocationSearchView.OperatingCenterID;
                ListView.DataBind();
            }
        }

        #endregion
    }
}
