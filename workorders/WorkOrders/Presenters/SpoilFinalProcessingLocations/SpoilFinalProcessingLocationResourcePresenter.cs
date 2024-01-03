using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Views.SpoilFinalProcessingLocations;

namespace WorkOrders.Presenters.SpoilFinalProcessingLocations
{
    public class SpoilFinalProcessingLocationResourcePresenter : WorkOrdersAdminResourcePresenter<SpoilFinalProcessingLocation>
    {
        #region Properties

        protected ISpoilFinalProcessingLocationSearchView SpoilFinalProcessingLocationSearchView
        {
            get { return (ISpoilFinalProcessingLocationSearchView)SearchView; }
        }

        protected ISpoilFinalProcessingLocationListView SpoilFinalProcessingLocationListView
        {
            get { return (ISpoilFinalProcessingLocationListView)ListView; }
        }

        #endregion

        #region Constructors

        public SpoilFinalProcessingLocationResourcePresenter(IResourceView view, IRepository<SpoilFinalProcessingLocation> repository)
            : base(view, repository)
        {
        }

        #endregion

        #region Event Handlers

        protected override void View_LoadComplete(object sender, EventArgs e)
        {
            if (ListView != null && ListView.Visible)
            {
                SpoilFinalProcessingLocationListView.OperatingCenterID =
                    SpoilFinalProcessingLocationSearchView.OperatingCenterID;
                ListView.DataBind();
            }
        }

        #endregion
    }
}
