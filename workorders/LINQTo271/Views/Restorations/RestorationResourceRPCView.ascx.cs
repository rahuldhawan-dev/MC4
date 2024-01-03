using System;
using LINQTo271.Views.Abstract;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Views.Restorations;

namespace LINQTo271.Views.Restorations
{
    public partial class RestorationResourceRPCView : WorkOrdersResourceRPCView<Restoration>, IRestorationResourceRPCView
    {
        #region Control Declarations

        protected IDetailView<Restoration> rdvRestoration;

        #endregion

        #region Properties

        public override IDetailView<Restoration> DetailView
        {
            get { return rdvRestoration; }
        }

        public override IListView<Restoration> ListView
        {
            get { return null; }
        }

        public override ISearchView<Restoration> SearchView
        {
            get { return null; }
        }

        public int WorkOrderID
        {
            get
            {
                //TODO: Return the workorderid
                return Convert.ToInt32(Argument);
            }
        }

        #endregion

        #region Exposed Methods

        public override void ShowDetailViewControls(bool show)
        {
            // noop
            // the detail view can handle toggling its controls
            // in set mode, thank you very much
        }

        #endregion
    }
}
