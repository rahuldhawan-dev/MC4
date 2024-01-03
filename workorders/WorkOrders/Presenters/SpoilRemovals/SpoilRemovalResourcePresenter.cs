using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Views.SpoilRemovals;

namespace WorkOrders.Presenters.SpoilRemovals
{
    public class SpoilRemovalResourcePresenter : WorkOrdersAdminResourcePresenter<SpoilRemoval>
    {
        #region Constructors

        public SpoilRemovalResourcePresenter(IResourceView view, IRepository<SpoilRemoval> repository)
            : base(view, repository)
        {
        }

        #endregion

        #region Properties

        protected ISpoilRemovalSearchView SpoilRemovalSearchView
        {
            get { return (ISpoilRemovalSearchView)SearchView; }
        }

        protected ISpoilRemovalListView SpoilRemovalListView
        {
            get { return (ISpoilRemovalListView)ListView; }
        }
        
        #endregion

        #region Event Handlers

        protected override void View_LoadComplete(object sender, EventArgs e)
        {
            if (ListView!=null && ListView.Visible)
            {
                SpoilRemovalListView.OperatingCenterID =
                    SpoilRemovalSearchView.OperatingCenterID;
                ListView.DataBind();
            }
        }

        #endregion
    }
}
